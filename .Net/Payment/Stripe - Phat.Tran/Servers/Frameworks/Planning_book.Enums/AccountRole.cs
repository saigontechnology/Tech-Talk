namespace PlanningBook.Enums
{
    // Any changes should sync with PersonRole & RoleType(IdentityService)
    // Start from 1 -> 2000
    public enum AccountRole
    {
        // Internal start from 1 -> 1000
        SysAdmin = 1,

        // External start from 1001 -> 2000
        User = 1001
    }
}
