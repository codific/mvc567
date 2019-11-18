namespace Codific.Mvc567.CommonCore
{
    public abstract class ReCaptchaResponse
    {
        public bool Success { get; set; }

        public string ChallengeTs { get; set; }

        public string Hostname { get; set; }

        public string[] Errorcodes { get; set; }
    }
}