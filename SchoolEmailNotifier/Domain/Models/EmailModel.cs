namespace SchoolEmailNotifier.Domain.Models
{
    public class EmailModel
    {
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string EmailsCopy { get; set; }
        public string Password { get; set; }

        public EmailModel(string emailFrom, string emailTo, string emailsCopy, string password)
        {
            EmailFrom = emailFrom;
            EmailTo = emailTo;
            EmailsCopy = emailsCopy;
            Password = password;
        }
    }
}
