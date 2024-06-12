namespace BASE.Model.IdentityAccess
{
    public class ResetPasswordModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }

    public class ResetPasswordAdminModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
