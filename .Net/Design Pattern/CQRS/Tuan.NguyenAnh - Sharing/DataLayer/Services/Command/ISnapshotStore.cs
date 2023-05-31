using DataLayer.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Service.Command
{
    /// <summary>
    /// Defines the methods needed from the snapshot store.
    /// </summary>
    public interface ISnapshotStore
    {

        /// <summary>
        /// Gets list of snapshots from the store (should add the aggregate type as a parameter).
        /// </summary>
        List<Snapshot> GetList();

        /// <summary>
        /// Gets a snapshot from the store.
        /// </summary>
        Snapshot Get(Guid id);

        /// <summary>
        /// Saves a snapshot to the store.
        /// </summary>
        void Save(Snapshot snapshot);
    }
}
