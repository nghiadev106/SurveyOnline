namespace SurveyOnline.API.Helpers
{
    public class AppSettings
    {
        // Properties for JWT Token Signature
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
        public string ReturnUrl { get; set; }
        // Sendgrind
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
        public string SendGridEmailAddress { get; set; }
    }
}
