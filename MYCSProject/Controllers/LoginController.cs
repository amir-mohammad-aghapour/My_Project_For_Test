using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MYCSProject.Models;

namespace MYCSProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly object user;


        public LoginController(IConfiguration configuraton)
        {
            _configuration = configuraton;
        }

        public string HmacSha256 { get; private set; }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Username.Equals("admin") && user.Password.Equals("password"))
            {
                //user.Id = Guid.NewGuid().ToString();
               var temp = Guid.NewGuid().ToString();
                int Amount = Convert.ToInt32(temp);
                user.Id = Amount;
                var token = GenerateJwtToken(user);
                return Ok(token);
            }
            return BadRequest("InValid User");
        }

        private string GenerateJwtToken(User user)
        {

            var securitykey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securitykey), SecurityAlgorithms.HmacSha256);

            var token =  new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}