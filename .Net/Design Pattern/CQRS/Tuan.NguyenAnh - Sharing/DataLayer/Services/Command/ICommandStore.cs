using DataLayer.WriteModels;
using DataLayer.Service.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    public interface ICommandStore
    {
        /// <summary>
        /// Utility for serializing and deserializing commands.
        /// </summary>
        ISerializer Serializer { get; }

        /// <summary>
        /// Returns true if a command exists.
        /// </summary>
        bool Exists(Guid command);

        /// <summary>
        /// Gets the serialized version of specific command.
        /// </summary>
        SerializedCommand Get(Guid command);

        /// <summary>
        /// Saves a serialized command.
        /// </summary>
        void Save(SerializedCommand command, bool isNew);

        /// <summary>
        /// Returns the serialized version of a command.
        /// </summary>
        SerializedCommand Serialize(ICommand command);
    }
}
