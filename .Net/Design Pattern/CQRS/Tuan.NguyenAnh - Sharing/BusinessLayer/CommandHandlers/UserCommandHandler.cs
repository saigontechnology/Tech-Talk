using BusinessLayer.Aggregates;
using BusinessLayer.Command;
using BusinessLayer.Helper;
using BusinessLayer.Serivces;
using BusinessLayer.Services;
using DataLayer.Service.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CommandHandlers
{
    public class UserCommandHandler
    {
        private readonly ISnapshotRepository _repository;
        private readonly IEventQueue _publisher;
        private readonly IQuerySearch _querySearch;

        public UserCommandHandler(ICommandQueue commander, IEventQueue publisher, ISnapshotRepository snapshotRepository, IQuerySearch querySearch)
        {
            _repository = snapshotRepository;
            _publisher = publisher;
            _querySearch= querySearch;

            commander.Subscribe<RegisterUser>(Handle);
            commander.Subscribe<RenameUser>(Handle);
        }

        private void Commit(UserAggregate aggregate)
        {
            var changes = _repository.Save(aggregate);
            foreach (var change in changes)
                _publisher.Publish(change);
        }

        public void Handle(RegisterUser c)
        {
            var aggregate = new UserAggregate { AggregateIdentifier = c.AggregateIdentifier };

            // Registration succeeds only if no other user has the same login name.
            var existingAggregates = _repository.GetList<UserAggregate>();

            var status = existingAggregates
                .Any(u => ((UserAggregateState)u.State).Name == c.UserName)
                ? "Failed" : "Succeeded";

            aggregate.RegisterUser(c.FirstName, c.LastName, DateTimeOffset.UtcNow, c.UserName, c.Password, status);
            Commit(aggregate);
        }

        public void Handle(RenameUser c)
        {
            var aggregate = _repository.Get<UserAggregate>(c.AggregateIdentifier);
            aggregate.RenameUser(c.FirstName, c.LastName);
            Commit(aggregate);
        }
    }
}
