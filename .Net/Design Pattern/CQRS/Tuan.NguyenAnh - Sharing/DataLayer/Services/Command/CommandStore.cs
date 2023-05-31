using DataLayer.WriteModels;
using System;
using DataLayer.Service.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DataLayer.Service.Command
{
    public class CommandStore : ICommandStore
    {
        private string DatabaseConnectionString { get; set; }

        public ISerializer Serializer { get; private set; }

        public CommandStore(ISerializer serializer, string databaseConnectionString)
        {
            Serializer = serializer;
            DatabaseConnectionString = databaseConnectionString;
        }

        public void Delete(Guid id)
        {
            using (var db = new LogDbContext(DatabaseConnectionString))
            {
                var command = db.SerializedCommand.FirstOrDefault(x => x.CommandIdentifier == id);
                if (command != null)
                {
                    db.SerializedCommand.Remove(command);
                    db.SaveChanges();
                }
            }
        }

        public bool Exists(Guid command)
        {
            using (var db = new LogDbContext(DatabaseConnectionString))
            {
                return db.SerializedCommand
                    .AsNoTracking()
                    .Any(x => x.CommandIdentifier == command);
            }
        }

        public SerializedCommand Get(Guid command)
        {
            using (var db = new LogDbContext(DatabaseConnectionString))
            {
                var entity = db.SerializedCommand
                    .AsNoTracking()
                    .Where(x => x.CommandIdentifier == command)
                    .FirstOrDefault();

                return entity ?? throw new NotImplementedException("This command is not exist");
            }
        }

        public void Save(SerializedCommand command, bool isNew)
        {
            using (var connection = new SqlConnection(DatabaseConnectionString))
            {
                if (isNew)
                    InsertCommand(command, connection);
                else
                    UpdateCommand(command, connection);
            }
        }

        public SerializedCommand Serialize(ICommand command)
        {
            return command.Serialize(Serializer, command.AggregateIdentifier);
        }

        #region Methods (insert and update)

        private void InsertCommand(SerializedCommand c, SqlConnection connection)
        {
            const string query = @"
INSERT INTO logs.Command
(
    AggregateIdentifier,
    CommandIdentifier, CommandClass, CommandType, CommandData,
    SendStarted, SendCompleted, SendStatus
)
VALUES
( @AggregateIdentifier, @CommandIdentifier, @CommandClass, @CommandType, @CommandData, @SendStarted, @SendCompleted, @SendStatus )";

            using (var command = new SqlCommand(query, connection))
            {
                var parameters = command.Parameters;

                parameters.AddWithValue("AggregateIdentifier", c.AggregateIdentifier);

                parameters.AddWithValue("CommandIdentifier", c.CommandIdentifier);
                parameters.AddWithValue("CommandClass", c.CommandClass);
                parameters.AddWithValue("CommandType", c.CommandType);
                parameters.AddWithValue("CommandData", c.CommandData);

                parameters.AddWithValue("SendStarted", (object)c.SendStarted ?? DBNull.Value);
                parameters.AddWithValue("SendCompleted", (object)c.SendCompleted ?? DBNull.Value);

                parameters.AddWithValue("SendStatus", c.SendStatus);

                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex) { throw new Exception("Error when trying to save command"); }
            }
        }

        private void UpdateCommand(SerializedCommand c, SqlConnection connection)
        {
            const string query = @"
UPDATE logs.Command
SET SendStarted = @SendStarted, SendCompleted = @SendCompleted,
    SendStatus = @SendStatus
WHERE CommandIdentifier = @CommandIdentifier
";

            using (var command = new SqlCommand(query, connection))
            {
                var parameters = command.Parameters;

                parameters.AddWithValue("CommandIdentifier", c.CommandIdentifier);

                parameters.AddWithValue("SendStarted", (object)c.SendStarted ?? DBNull.Value);
                parameters.AddWithValue("SendCompleted", (object)c.SendCompleted ?? DBNull.Value);

                parameters.AddWithValue("SendStatus", (object)c.SendStatus ?? DBNull.Value);

                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception("Error when trying to save command"); }
            }
        }
        #endregion
    }
}
