namespace Elite.Model
{
    public class TempUserData
    {
        public string Email { get; set; }
        public string OtpCode { get; set; }
        public DateTime OtpGeneratedAt { get; set; }
        public bool IsOtpVerified { get; set; }

        public string Bvn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public bool IsPinConfirmed { get; set; }

        public string Username => (FirstName + LastName).ToLower();

        public bool IsFullyRegistered =>
            IsOtpVerified && !string.IsNullOrEmpty(Bvn) &&
            !string.IsNullOrEmpty(Password) && IsPinConfirmed;
    }
}
