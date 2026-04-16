namespace AccountService.Mappers
{
    public static class UserRoleParser
    {
        public static Entities.Enums.UserRole? ToEnum(string? role)
        {
            switch (role)
            {
                case "ADMIN": return Entities.Enums.UserRole.ADMIN;break;
                case "EMPLOYEE": return Entities.Enums.UserRole.EMPLOYEE; break;
                case "CUSTOMER": return Entities.Enums.UserRole.CUSTOMER; break;
                default: return null;
            }
        }

        public static string ToString(Entities.Enums.UserRole? role)
        {
            switch (role)
            {
                case Entities.Enums.UserRole.ADMIN: return "ADMIN"; break;
                case Entities.Enums.UserRole.EMPLOYEE: return "EMPLOYEE"; break;
                case Entities.Enums.UserRole.CUSTOMER: return "CUSTOMER"; break;
                default: return "";
            }
        }
    }
}
