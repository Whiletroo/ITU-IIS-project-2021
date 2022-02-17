//<!-- Author xkrukh00-->
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Charity.Common.Models;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSwag.Annotations;

namespace Charity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string ApiOperationBaseName = "Account";
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IMapper _mapper;
        private readonly IRepository<AdminEntity> _adminRepo;
        private readonly IRepository<ShelterAdminEntity> _shelterAdminRepo;
        private readonly IRepository<VolunteerEntity> _volunteerRepo;
        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, IMapper mapper,
            IRepository<AdminEntity> adminRepo, IRepository<ShelterAdminEntity> shelterAdminRepo, IRepository<VolunteerEntity> volunteerRepo)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _mapper = mapper;
            _adminRepo = adminRepo;
            _shelterAdminRepo = shelterAdminRepo;
            _volunteerRepo = volunteerRepo;
        }

        [HttpPost("Login")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Login))]
        public ActionResult<AuthResponseDto> Login(LoginModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user =  _userManager.FindByEmailAsync(userModel.Email).Result;

            if (user == null || !_userManager.CheckPasswordAsync(user, userModel.Password).Result)
                return Ok(new AuthResponseDto { ErrorMessage = "Incorrect email or password", IsAuthSuccessful = false});

            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("Register")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Register))]
        public ActionResult<RegistrationResponseModel> Register([FromBody] RegistrationModel userForRegistration)
        {
            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            var result = _userManager.CreateAsync(user, userForRegistration.Password).Result;
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Ok(new RegistrationResponseModel() {IsSuccessfulRegistration = false, Errors = errors });
            }

            var entity = _mapper.Map<VolunteerEntity>(userForRegistration);
            entity.Role = "Volunteer";
            var repoInsertResult = _volunteerRepo.Insert(entity);

            if (repoInsertResult == null)
            {
                return Ok(new RegistrationResponseModel()
                    {IsSuccessfulRegistration = false, Errors = new List<string>() {"Repository insert error"}});
            }

            _userManager.AddToRoleAsync(user, "Volunteer").Wait();

            return Ok(new RegistrationResponseModel { IsSuccessfulRegistration = true});
        }

        [HttpPost("RegisterShelterAdmin")]
        [OpenApiOperation(ApiOperationBaseName + nameof(RegisterShelterAdmin))]
        public ActionResult<RegistrationResponseModel> RegisterShelterAdmin([FromBody] RegistrationModel userForRegistration)
        {
            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            var result = _userManager.CreateAsync(user, userForRegistration.Password).Result;
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Ok(new RegistrationResponseModel() {IsSuccessfulRegistration = false, Errors = errors });
            }

            var entity = _mapper.Map<ShelterAdminEntity>(userForRegistration);
            entity.Role = "ShelterAdministrator";
            var repoInsertResult = _shelterAdminRepo.Insert(entity);

            if (repoInsertResult == null)
            {
                return Ok(new RegistrationResponseModel()
                    { IsSuccessfulRegistration = false, Errors = new List<string>() { "Repository insert error" } });
            }

            _userManager.AddToRoleAsync(user, "ShelterAdministrator").Wait();
            
            return Ok(new RegistrationResponseModel {IsSuccessfulRegistration = true});
        }

        [HttpPost("RegisterAdministrator")]
        [OpenApiOperation(ApiOperationBaseName + nameof(RegisterAdministrator))]
        public ActionResult<RegistrationResponseModel> RegisterAdministrator([FromBody] RegistrationModel userForRegistration)
        {
            var user = new IdentityUser { UserName = userForRegistration.Email, Email = userForRegistration.Email };

            var result = _userManager.CreateAsync(user, userForRegistration.Password).Result;
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Ok(new RegistrationResponseModel() {IsSuccessfulRegistration = false, Errors = errors });
            }

            var entity = _mapper.Map<AdminEntity>(userForRegistration);
            entity.Role = "Administrator";
            var repoInsertResult = _adminRepo.Insert(entity);

            if (repoInsertResult == null)
            {
                return Ok(new RegistrationResponseModel()
                    { IsSuccessfulRegistration = false, Errors = new List<string>() { "Repository insert error" } });
            }

            _userManager.AddToRoleAsync(user, "Administrator").Wait();

            return Ok(new RegistrationResponseModel { IsSuccessfulRegistration = true });
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private  List<Claim> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}
