namespace TheQuestion.CAPTCHA
{
    public class CaptchaConfiguration
    {
        public string SiteKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public decimal MinScore { get; set; }
    }
}
