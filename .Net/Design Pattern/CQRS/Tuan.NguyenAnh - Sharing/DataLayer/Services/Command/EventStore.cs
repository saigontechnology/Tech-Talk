
using DataLayer.Service.Helper;
using DataLayer.WriteModels;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DataLayer.Service.Command
{
    public class EventStore : IEventStore
    {
        private string DatabaseConnectionString { get; set; }

        public ISerializer Serializer { get; private set; }

        public EventStore( ISerializer serializer, string databaseConnectionString)
        {
            Serializer = serializer;
            DatabaseConnectionString = databaseConnectionString;
        }

        public bool Exists(Guid aggregate)
        {
            const string query = @"SELECT TOP 1 1 FROM logs.Aggregate WHERE AggregateIdentifier = @AggregateIdentifier";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var select = new SqlCommand(query, connection))
                {
                    select.Parameters.AddWithValue("AggregateIdentifier", aggregate);
                    object o = select.ExecuteScalar();
                    return o != DBNull.Value;
                }
            }
        }

        public bool Exists(Guid aggregate, int version)
        {
            const string query = @"SELECT TOP 1 1 FROM logs.Aggregate WHERE AggregateIdentifier = @AggregateIdentifier AND AggregateVersion = @AggregateVersion";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var select = new SqlCommand(query, connection))
                {
                    select.Parameters.AddWithValue("AggregateIdentifier", aggregate);
                    select.Parameters.AddWithValue("AggregateVersion", version);
                    object o = select.ExecuteScalar();
                    return o != DBNull.Value;
                }
            }
        }

        public IEnumerable<IEvent> Get(Guid aggregate, int fromVersion, string aggregateType)
        {
            return GetSerialized(aggregate, fromVersion, aggregateType)
                .Select(x => x.Deserialize(Serializer))
                .ToList()
                .AsEnumerable();
        }

        private IEnumerable<SerializedEvent> GetSerialized(Guid aggregate, int fromVersion, string aggregateType)
        {
            const string text = @"
SELECT 
    e.AggregateIdentifier,
    e.AggregateVersion,
    e.EventTime,
    e.EventClass,
    e.EventType,
    e.EventData
FROM 
    logs.Event e
JOIN
    logs.Aggregate a
ON
    e.AggregateIdentifier = a.AggregateIdentifier
WHERE
    a.AggregateIdentifier = @AggregateIdentifier AND e.AggregateVersion > @AggregateVersion AND a.AggregateType = @AggregateType
ORDER BY
    AggregateVersion";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(text, connection))
                {
                    command.Parameters.AddWithValue("AggregateIdentifier", aggregate);
                    command.Parameters.AddWithValue("AggregateVersion", fromVersion);
                    command.Parameters.AddWithValue("AggregateType", aggregateType);

                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<SerializedEvent>();

                        while (reader.Read())
                        {
                            var item = new SerializedEvent
                            {
                                AggregateIdentifier = reader.GetGuid(0),
                                AggregateVersion = reader.GetInt32(1),
                                EventTime = reader.GetDateTimeOffset(2),
                                EventClass = reader.GetString(3),
                                EventType = reader.GetString(4),
                                EventData = reader.GetString(5),
                            };
                            list.Add(item);
                        }

                        return list;
                    }
                }
            }
        }

        public void Save(AggregateRoot aggregate, IEnumerable<IEvent> events)
        {
            var list = new List<SerializedEvent>();

            foreach (var e in events)
            {
                var item = e.Serialize(Serializer, aggregate.AggregateIdentifier, e.AggregateVersion);

                list.Add(item);
            }

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    EnsureAggregateExists(aggregate.AggregateIdentifier, aggregate.GetType().Name.Replace("Aggregate", string.Empty), aggregate.GetType().FullName, connection, transaction);

                    if (list.Count > 1)
                        InsertEvents(list, connection, transaction);
                    else
                        InsertEvent(list[0], connection, transaction);

                    transaction.Commit();
                }
            }
        }

        #region Methods (insert, update, delete)

        private int Delete(Guid aggregate)
        {
            const string query = @"
DELETE FROM logs.Aggregate WHERE AggregateIdentifier = @AggregateIdentifier;
DELETE FROM logs.Event WHERE AggregateIdentifier = @AggregateIdentifier;
";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("AggregateIdentifier", aggregate);
                    return command.ExecuteNonQuery();
                }
            }
        }

        private void InsertEvent(SerializedEvent e, SqlConnection connection, SqlTransaction transaction)
        {
            const string query = @"
INSERT INTO logs.Event
(
    AggregateIdentifier, AggregateVersion,
    EventClass, EventType, EventData,
    EventTime
)
VALUES
( @AggregateIdentifier, @AggregateVersion, @EventClass, @EventType, @EventData, @EventTime )";

            using (var command = new SqlCommand(query, connection, transaction))
            {
                var parameters = command.Parameters;

                parameters.AddWithValue("AggregateIdentifier", e.AggregateIdentifier);
                parameters.AddWithValue("AggregateVersion", e.AggregateVersion);

                parameters.AddWithValue("EventClass", e.EventClass);
                parameters.AddWithValue("EventType", e.EventType);
                parameters.AddWithValue("EventData", e.EventData);

                parameters.AddWithValue("EventTime", e.EventTime);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex) { throw new Exception("Error when trying to save event"); }
            }
        }

        private void InsertEvents(List<SerializedEvent> events, SqlConnection connection, SqlTransaction transaction)
        {
            var table = new DataTable();

            table.Columns.Add("AggregateIdentifier", typeof(Guid));
            table.Columns.Add("AggregateVersion", typeof(int));

            table.Columns.Add("IdentityTenant", typeof(Guid));
            table.Columns.Add("IdentityUser", typeof(Guid));

            table.Columns.Add("EventTime", typeof(DateTimeOffset));
            table.Columns.Add("EventClass", typeof(string));
            table.Columns.Add("EventType", typeof(string));
            table.Columns.Add("EventData", typeof(string));

            foreach (var e in events)
            {
                var row = table.NewRow();
                row["AggregateIdentifier"] = e.AggregateIdentifier;
                row["AggregateVersion"] = e.AggregateVersion;;
                row["EventTime"] = e.EventTime;
                row["EventClass"] = e.EventClass;
                row["EventType"] = e.EventType;
                row["EventData"] = e.EventData;
                table.Rows.Add(row);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.BatchSize = 5000;
                bulkCopy.DestinationTableName = "logs.Event";
                bulkCopy.WriteToServer(table);
            }
        }

        #endregion

        #region Methods (lookup)

        private void EnsureAggregateExists(Guid aggregate, string name, string type, SqlConnection connection, SqlTransaction transaction)
        {
            const string query = @"
IF NOT EXISTS(SELECT TOP 1 1 FROM logs.Aggregate WHERE AggregateIdentifier = @AggregateIdentifier)
  BEGIN
    INSERT INTO logs.Aggregate (AggregateIdentifier, AggregateType, AggregateClass) VALUES (@AggregateIdentifier, @AggregateType, @AggregateClass);
  END";

            using (var insert = new SqlCommand(query, connection, transaction))
            {
                insert.Parameters.AddWithValue("AggregateIdentifier", aggregate);
                insert.Parameters.AddWithValue("AggregateType", name);
                insert.Parameters.AddWithValue("AggregateClass", type);

                insert.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
