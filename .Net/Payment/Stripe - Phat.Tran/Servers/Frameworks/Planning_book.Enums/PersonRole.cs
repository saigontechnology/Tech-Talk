namespace PlanningBook.Enums
{
    // Any changes should sync with AccountRole & RoleType(IdentityService)
    // Start from 3000 -> 5000
    public enum PersonRole
    {
        // Internal start from 3000 -> 4000
        Staff = 3000,
        // External start from 4001 -> 5000
        EndUser = 4001
    }
}
