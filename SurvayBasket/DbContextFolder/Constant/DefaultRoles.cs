namespace SurvayBasket.DbContextFolder.Constant
{
    public static class DefaultRoles
    {
        public const string Admin = "Admin";
        public const string AdminRoleId = "D87E70CF-B177-446E-A4AE-87ECEED54A50";
        public const string AdminConcurrencyStamp = "224EC06D-1692-4784-9B84-0C388684B013";


        public const string Member = "Member";
        public const string MemberRoleId = "26B4DBD9-C25D-4CB5-BD72-DE4B793226BA";
        public const string MemberConcurrencyStamp = "584EC06D-1692-4784-9B84-0C320684B064";

        //public static IList<string> GetAllPermissions() =>
        //    typeof(DefaultRoles).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
}
