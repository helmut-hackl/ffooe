using ffooe.db.context;
using ffooe.db.entities;
using ffooe.rest.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ffooe.rest.api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly FFOOEContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<ClientController> logger, FFOOEContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }
        // login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.M_Users.Find(model.Username);
            if (user == null || user.LockOut) return Unauthorized();
            if (!SecurePasswordHasher.Verify(model.Password, user.PasswordHash)) return Unauthorized();

            // generate token for user
            var token = GenerateAccessToken(model.Username);
            // return access token for user's use
            return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet("isuseravailable")]
        public IActionResult UserAvailable(string username)
        {
            return Ok(!_context.M_Users.Any(p => p.UserName == username));
        }
        [HttpGet("ispasswordstrong")]
        public IActionResult PasswordStrength(string password)
        {
            return Ok(CheckStrength(password));
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel newUser)
        {
            try
            {
                if (newUser.Username.Length < 6) return BadRequest("username needs at least 6 chars");
                if (!IsEmailValid(newUser.Email)) return BadRequest("email address is not valid");
                if (CheckStrength(newUser.Password) != PasswordScore.Strong && CheckStrength(newUser.Password) != PasswordScore.VeryStrong) return BadRequest("password complexity too weak");

                var m_user = new M_User
                {
                    UserName = newUser.Username,
                    PasswordHash = SecurePasswordHasher.Hash(newUser.Password),
                    MailAddress = newUser.Email,
                    VerifyCode = new Random().Next(100000, 999999),
                    LockOut = true
                };

                var addedUser = _context.M_Users.Add(m_user);
                _context.SaveChanges();
                return Ok(addedUser.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyModel verify)
        {
            try
            {
                var valid = Int32.TryParse(verify.VerifyCode, out int verifyCode);
                if (!valid) return BadRequest("Verifycode is not a number");

                var user = _context.M_Users.Where(p => p.UserName == verify.Username).FirstOrDefault();
                if (user == null) return BadRequest("User not found");

                if (user.VerifyCode == verifyCode)
                {
                    user.LockOut = false;
                    _context.SaveChanges();
                    return Ok(user);
                }
                return BadRequest("Verification failed, code is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private bool IsEmailValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }
        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"\d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore)score;
        }
        private JwtSecurityToken GenerateAccessToken(string userName)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "ADMIN")
                // Add additional claims as needed (e.g., roles, etc.)
            };
           
            // Create a JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}