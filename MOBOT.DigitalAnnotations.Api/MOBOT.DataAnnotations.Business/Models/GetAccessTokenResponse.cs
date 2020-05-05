using System;

namespace MOBOT.DigitalAnnotations.Business.Models
{
    public class GetAccessTokenResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
