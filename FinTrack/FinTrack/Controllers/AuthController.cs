using FinTrack.Data;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FinTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            // Validate the incoming model based on data annotations in LoginDTO
            if (!ModelState.IsValid)
            {
                // If the model is invalid, return a 400 Bad Request with validation errors
                return BadRequest(ModelState);
            }

            var client = _context.Clients.FirstOrDefault(x => x.ClientId == loginDTO.ClientId);

            if (client == null)
            {
                return Unauthorized("Invalid client creds");
            }

            var user = _context.Users.Include(x => x.UserRole).ThenInclude(x => x.Role).FirstOrDefault(x => x.UserName.ToLower() == loginDTO.UserName.ToLower());

            if (user == null)
            {
                return Unauthorized("Inavalid User Name");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Inavalid Password!");
            }

            //As of now password is validated
            var token = GenerateJWTToken(user, client);

            return Ok(new
            {
                Token = token,
                UserID = user.Id
            });
        }

        private string GenerateJWTToken(User user, Client client)
        {
            var signingKey = _context.SigningKeys.FirstOrDefault(k => k.IsActive);

            // If no active signing key is found, throw an exception
            if (signingKey == null)
            {
                throw new Exception("No active signing key available.");
            }
            // Convert the Base64-encoded private key string back to a byte array
            var privateKeyBytes = Convert.FromBase64String(signingKey.PrivateKey);
            // Create a new RSA instance for cryptographic operations
            var rsa = RSA.Create();
            // Import the RSA private key into the RSA instance
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            // Create a new RsaSecurityKey using the RSA instance
            var rsaSecurityKey = new RsaSecurityKey(rsa)
            {
                // Assign the Key ID to link the JWT with the correct public key
                KeyId = signingKey.KeyId
            };

            // Define the signing credentials using the RSA security key and specifying the algorithm
            var creds = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

            // Initialize a list of claims to include in the JWT
            var claims = new List<Claim>
            {
                // Subject (sub) claim with the user's ID
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                // JWT ID (jti) claim with a unique identifier for the token
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                // Name claim with the user's first name
                new Claim(ClaimTypes.Name, user.FirstName),

                // NameIdentifier claim with the user's email
                new Claim(ClaimTypes.NameIdentifier, user.Email),

                // Email claim with the user's email
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Iterate through the user's roles and add each as a Role claim
            foreach (var userRole in user.UserRole)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            // Define the JWT token's properties, including issuer, audience, claims, expiration, and signing credentials
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // The token issuer, typically your application's URL
                audience: client.ClientURL, // The intended recipient of the token, typically the client's URL
                claims: claims, // The list of claims to include in the token
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time set to 1 hour from now
                signingCredentials: creds // The credentials used to sign the token
            );

            // Create a JWT token handler to serialize the token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Serialize the token to a string
            var token = tokenHandler.WriteToken(tokenDescriptor);

            // Return the serialized JWT token
            return token;
        }
    }
}
