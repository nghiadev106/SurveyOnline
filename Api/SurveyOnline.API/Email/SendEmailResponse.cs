namespace SurveyOnline.API.Email
{
    public class SendEmailResponse
    {
        public bool Successful=> ErrorMsg == null;

        public string ErrorMsg { get; set; }
    }
}
