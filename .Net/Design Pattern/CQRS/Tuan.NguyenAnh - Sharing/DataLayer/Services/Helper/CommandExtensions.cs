using DataLayer.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Helper
{
    public static class CommandExtensions
    {
        /// <summary>
        /// Returns a deserialized command.
        /// </summary>
        public static ICommand Deserialize(this SerializedCommand x, ISerializer serializer)
        {
            var data = serializer.Deserialize<ICommand>(x.CommandData, Type.GetType(x.CommandClass));

            data.AggregateIdentifier = x.AggregateIdentifier;

            return data;
        }

        /// <summary>
        /// Returns a serialized command.
        /// </summary>
        public static SerializedCommand Serialize(this ICommand command, ISerializer serializer, Guid aggregateIdentifier)
        {
            var data = serializer.Serialize(command, new[] { "AggregateIdentifier", "AggregateVersion", "IdentityTenant", "IdentityUser", "CommandIdentifier", "SendScheduled", "SendStarted", "SendCompleted", "SendCancelled" });

            var serialized = new SerializedCommand
            {
                AggregateIdentifier = aggregateIdentifier,

                CommandClass = command.GetType().AssemblyQualifiedName,
                CommandType = command.GetType().Name,
                CommandData = data,

                CommandIdentifier = command.CommandIdentifier,
            };

            if (serialized.CommandClass.Length > 200)
                throw new OverflowException($"The assembly-qualified name for this command ({serialized.CommandClass}) exceeds the maximum character limit (200).");

            if (serialized.CommandType.Length > 100)
                throw new OverflowException($"The type name for this command ({serialized.CommandType}) exceeds the maximum character limit (100).");

            return serialized;
        }
    }
}
