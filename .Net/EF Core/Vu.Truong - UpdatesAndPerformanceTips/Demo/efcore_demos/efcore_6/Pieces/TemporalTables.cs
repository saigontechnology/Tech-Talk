using Microsoft.EntityFrameworkCore;

namespace efcore_demos.Pieces;
internal class TemporalTables : IExample
{
    private readonly DemoDbContext _dbContext;

    const string email = "vu.test@gmail.com";
    const string username = "vu.test";

    public TemporalTables(DemoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task IExample.ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var user = _dbContext.Users.Add(new UserEntity
        {
            BirthDate = DateTime.Now,
            Id = Guid.NewGuid(),
            Email = email,
            Password = "password",
            UserName = username
        }).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        for (var i = 0; i < 3; i++)
        {
            user.UserName = $"{username}{i}";
            _dbContext.Update(user);
            await Task.Delay(1000);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        for (var i = 0; i < 3; i++)
        {
            await Task.Delay(1000);

            user.UserName = $"{username}{i + 10}";
            _dbContext.Update(user);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        await Task.Delay(1000);
        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userHistories = await _dbContext.Users
            .TemporalAll()
            .OrderBy(user => EF.Property<DateTime>(user, "PeriodStart"))
            .Where(user => user.Email == email)
            .Select(user => new
            {
                Email = user.Email,
                UserName = user.UserName,
                PeriodStart = EF.Property<DateTime?>(user, "PeriodStart"),
                PeriodEnd = EF.Property<DateTime?>(user, "PeriodEnd")
            })
            .ToListAsync(cancellationToken);

        var j = 0;

        while(j < userHistories.Count)
        {
            var prev = j == 0 ? null : userHistories[j - 1];
            var current = userHistories[j];
            var next = j == userHistories.Count - 1 ? null : userHistories[j + 1];

            if (prev is null || current.PeriodStart != prev.PeriodEnd)
            {
                Console.WriteLine($"Added: {current.PeriodStart}, -> {current.UserName}");
            }
            else
            {
                Console.WriteLine($"Edited: {current.PeriodStart}, changed: {prev.UserName} -> {current.UserName}");
            }

            if (next is not null && next.PeriodStart != current.PeriodEnd)
            {
                Console.WriteLine($"Removed: {current.PeriodEnd}");
            }
            else if (next is null && current.PeriodEnd != DateTime.MaxValue)
            {
                Console.WriteLine($"Removed: {current.PeriodEnd}");
            }

            j++;
        }
    }
}
