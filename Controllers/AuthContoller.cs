using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.DTO;
using TodoAPI.Models;
using TodoAPI.Utils;

namespace TodoAPI.Auth
{
    public class AuthController: ControllerBase {
        private readonly TokenService _tokenService;
        private readonly ApplicationContext _context;

        public AuthController(TokenService tokenService, ApplicationContext context){
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("api/register")]
        public async Task<IActionResult> Register(UserDTO userDto)
        {

            var userEmailExists = await _context.Users.AnyAsync(x => x.Email == userDto.Email);
            if (userEmailExists)
                return BadRequest(new { message = "User already exists" });

            var userPhoneExists = await _context.Users.AnyAsync(x => x.PhoneNumber == userDto.PhoneNumber);
            if (userPhoneExists)
                return BadRequest(new { message = "User with this phone number already exists" });


            var user = new User { Username = userDto.Username, Email = userDto.Email, Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),PhoneNumber = userDto.PhoneNumber };

            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login(LoginDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if( user == null)
                return Unauthorized(new { message = "User does not exist" });
            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                return Unauthorized(new { message = "Password Invalid" });

            // var token = _tokenService.GenerateToken(user);
            // return Ok(new { token });

            // Generate and save OTP
            var otp = OTPGen.GenerateOtp();
            user.OtpCode = otp;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5); // OTP valid for 5 minutes
            await _context.SaveChangesAsync();

            Console.WriteLine(otp);

            // Send OTP to user's email
            // await _emailService.SendOtpAsync(user.Email, otp);

            return Ok("OTP sent to your email.");
        }

        [HttpPost("api/verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationDto otpDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == otpDto.Email);

            if (user == null || user.OtpCode != otpDto.OtpCode || user.OtpExpiryTime < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired OTP.");
            }

            var tokenString = _tokenService.GenerateToken(user);

            // Clear OTP fields once verified
            user.OtpCode = "";
            user.OtpExpiryTime = DateTime.MinValue;
            await _context.SaveChangesAsync();

            return Ok(new { Token = tokenString });
        }


    }


}