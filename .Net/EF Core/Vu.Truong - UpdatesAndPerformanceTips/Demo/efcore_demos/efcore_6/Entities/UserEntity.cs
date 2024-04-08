namespace efcore_demos.Entities;
internal class UserEntity : BaseEntity
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
}
