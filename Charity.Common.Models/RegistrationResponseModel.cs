namespace Charity.Common.Models
{
    public class RegistrationResponseModel
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Token { get; set; }
    }
}
