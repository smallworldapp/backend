namespace SmallWorld
{
    public class SmallWorldOptions
    {
        public int Port { get; set; }

        public string Host { get; set; }
        public string[] Origins { get; set; }
        public UrlConfig Urls { get; set; }

        public SmtpConfig Smtp { get; set; }
        public AdminCredentials AdminLogin { get; set; }
    }

    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class UrlConfig
    {
        public string Admin { get; set; }
        public string Reset { get; set; }

        public string Join { get; set; }
        public string Verify { get; set; }
        public string OptOut { get; set; }

        public string Feedback { get; set; }
    }

    public class AdminCredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
