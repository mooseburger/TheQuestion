using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TheQuestion.CAPTCHA
{
    public interface ICaptchaService
    {
        Task<bool> CaptchaPassed(string token);
    }

    public class CaptchaService : ICaptchaService
    {
        private readonly CaptchaConfiguration _configuration;

        public CaptchaService(IOptions<CaptchaConfiguration> options)
        {
            _configuration = options.Value;
        }

        public async Task<bool> CaptchaPassed(string token)
        {
            HttpClient httpClient = new HttpClient();

            string captchaUrl = "https://www.google.com/recaptcha/api/siteverify";
            var requestBody = new Dictionary<string, string>();
            requestBody["secret"] = _configuration.SecretKey;
            requestBody["response"] = token;
            
            var request = new FormUrlEncodedContent(requestBody);

            var response = await httpClient.PostAsync(captchaUrl, request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            string responseString = await response.Content.ReadAsStringAsync();
            dynamic responseObj = JObject.Parse(responseString);

            if (responseObj.success != true || responseObj.score <= _configuration.MinScore)
            {
                return false;
            }

            return true;
        }
    }
}
