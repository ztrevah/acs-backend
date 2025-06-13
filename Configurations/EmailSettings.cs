namespace SystemBackend.Configurations
{
    public class EmailSettings
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SmtpEmail { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool EnableSsl { get; set; }
    }
}
