using DataLayer.WriteModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    public class SnapshotStore : ISnapshotStore
    {
        private string DatabaseConnectionString { get; set; }

        public SnapshotStore(string databaseConnectionString)
        {
            DatabaseConnectionString = databaseConnectionString;
        }

        public List<Snapshot> GetList()
        {
            const string text = @"
                SELECT 
                    AggregateIdentifier,
                    AggregateVersion,
                    AggregateState
                FROM 
                    logs.Snapshot 
            ";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(text, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Snapshot>();

                        while (reader.Read())
                        {
                            var tmp = new Snapshot
                            {
                                AggregateIdentifier = reader.GetGuid(0),
                                AggregateVersion = reader.GetInt32(1),
                                AggregateState = reader.GetString(2),
                            };
                            list.Add(tmp);
                        }

                        return list;
                    }
                }
            }
        }

        public Snapshot Get(Guid id)
        {
            const string text = @"
                SELECT 
                    AggregateIdentifier,
                    AggregateVersion,
                    AggregateState
                FROM 
                    logs.Snapshot 
                WHERE
                    AggregateIdentifier = @AggregateIdentifier
            ";

            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(text, connection))
                {
                    command.Parameters.AddWithValue("AggregateIdentifier", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Snapshot
                            {
                                AggregateIdentifier = reader.GetGuid(0),
                                AggregateVersion = reader.GetInt32(1),
                                AggregateState = reader.GetString(2),
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public void Save(Snapshot snapshot)
        {

            //check if this aggregate already existed a snapshot -> update it
            const string query = @"
            IF NOT EXISTS(SELECT TOP 1 1 FROM logs.Snapshot WHERE AggregateIdentifier = @AggregateIdentifier)
              INSERT INTO logs.Snapshot (AggregateIdentifier, AggregateVersion, AggregateState) VALUES (@AggregateIdentifier, @AggregateVersion, @AggregateState);
            ELSE
              UPDATE logs.Snapshot SET AggregateVersion = @AggregateVersion, AggregateState = @AggregateState WHERE AggregateIdentifier = @AggregateIdentifier;
            ";
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                connection.Open();

                using (var insert = new SqlCommand(query, connection))
                {
                    insert.Parameters.AddWithValue("AggregateIdentifier", snapshot.AggregateIdentifier);
                    insert.Parameters.AddWithValue("AggregateVersion", snapshot.AggregateVersion);
                    insert.Parameters.AddWithValue("AggregateState", snapshot.AggregateState);

                    insert.ExecuteNonQuery();
                }
            }

            // 
        }
    }
}
