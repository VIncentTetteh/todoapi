namespace TodoAPI.Utils
{
    public class OTPGen{

        public static string GenerateOtp()
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString(); // 6-digit OTP
            return otp;
        }

    }
}