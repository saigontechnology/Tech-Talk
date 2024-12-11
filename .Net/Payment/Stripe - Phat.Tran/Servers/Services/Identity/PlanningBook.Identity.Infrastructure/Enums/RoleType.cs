namespace PlanningBook.Identity.Infrastructure.Enums
{
    // Any changes should sync with PersonRole & AccountRole in our Frameworks->PlanningBook.Enums
    public enum RoleType
    {
        #region Account Roles
        SysAdmin = 1,
        User = 1001,
        #endregion Account Roles

        #region Person Roles
        Staff = 3000,
        EndUser = 4001
        #endregion Person Roles
    }
}
