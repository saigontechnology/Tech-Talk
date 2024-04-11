namespace efcore_demos.Entities;
internal class UserEntity : BaseRetrievedEntity
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTimeOffset? BirthDate { get; set; }

    public AddressEntity Address { get; set; }

    public List<DateOnly> DaysVisited { get; set; } = new();

    public List<Guid> OrderHistories { get; set; } = new();
}
