namespace ExpenseManagementMVC.Security
{
    public static class EncryptionUtil
    {
        public static string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool IsValidPasswd(string passwd, string encrypted)
        {
            return BCrypt.Net.BCrypt.Verify(passwd, encrypted);
        }
    }
}
