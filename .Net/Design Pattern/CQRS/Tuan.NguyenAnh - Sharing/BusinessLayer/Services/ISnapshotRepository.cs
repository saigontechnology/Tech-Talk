using DataLayer;
using DataLayer.Service.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface ISnapshotRepository
    {

        /// <summary>
        /// Returns a list of aggregate (should add the aggregate type as a parameter).
        /// </summary>
        List<T> GetList<T>() where T : AggregateRoot;

        /// <summary>
        /// Returns the aggregate identified by the specified id.
        /// </summary>
        T Get<T>(Guid id) where T : AggregateRoot;

        /// <summary>
        /// Saves an aggregate.
        /// </summary>
        IEvent[] Save<T>(T aggregate, int? version = null) where T : AggregateRoot;
    }
}
