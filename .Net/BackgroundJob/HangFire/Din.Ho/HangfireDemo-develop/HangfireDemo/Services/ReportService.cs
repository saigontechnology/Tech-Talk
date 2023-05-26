namespace HangfireDemo.Services
{
    public class ReportService
    {
        private readonly EmailSender _emailSender;

        public ReportService(EmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task GenerateReport()
        {
            // a task processing for 5 seconds.
            return Task.Delay(5000);
        }

        public async Task GenerateReportNoWait()
        {
            // a task processing for 5 seconds.
            await Task.Delay(5000);

            // send notification email
            var message = "This is an email for notification purpose.";
            var subject = "Dummy Subject";
            await _emailSender.SendEmailAsync("din.ho@saigontechnology.com", subject, message);
        }
    }
}
