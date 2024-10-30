namespace TodoAPI.DTO
{
     public class OtpVerificationDto{
        public required string Email { get; set; }
        public required string OtpCode { get; set; }
     }
}