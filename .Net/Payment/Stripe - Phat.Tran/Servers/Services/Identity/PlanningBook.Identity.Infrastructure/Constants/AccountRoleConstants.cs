namespace PlanningBook.Identity.Infrastructure.Constants
{
    public static class AccountRoleConstants
    {
        #region Role Names
        public const string SuperAdminRoleName = "Super Admin";
        public const string AdminRoleName = "Admin";
        public const string UserRoleName = "User";
        #endregion Role Names

        #region Role Ids
        public static readonly Guid SuperAdminRoleId = new Guid("5dae3b58-d3bb-4a9d-8efb-71518621bfc6");
        public static readonly Guid AdminRoleId = new Guid("dd0d2565-b017-4d96-9abf-c981366b6d62");
        public static readonly Guid UserRoleId = new Guid("f7846fe7-3c67-4da4-9185-843143948afa");
        #endregion Role Ids

        #region Helper Methods
        /// <summary>
        /// Get Role Id by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns>Role Id as Guid</returns>
        public static Guid? GetRoleId(string roleName)
        {
            return roleName switch
            {
                SuperAdminRoleName => SuperAdminRoleId,
                AdminRoleName => AdminRoleId,
                UserRoleName => UserRoleId,
                _ => null
            };
        }

        /// <summary>
        /// Get Role Name by role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns>Role Name as string</returns>
        public static string? GetRoleName(Guid roleId)
        {
            return roleId switch
            {
                _ when roleId == SuperAdminRoleId => SuperAdminRoleName,
                _ when roleId == AdminRoleId => AdminRoleName,
                _ when roleId == UserRoleId => UserRoleName,
                _ => null
            };
        }
        #endregion Helper Methods
    }
}
