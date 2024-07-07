using AutoMapper;
using Grocery.Domain.Entities.Identity;
using Grocery.Domain.GroceryMetaData.Routing;
using Grocery.Domain.IServices.ITokenServices;
using Grocery.Errors;
using Grocery.Service.Dtos.OTP;
using Grocery.Service.Dtos.UserAccount;
using Grocery.Service.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Grocery.Domain.IServices.MailServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            ITokenService tokenService,
            IMailService mailService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManger = signInManger;
            _tokenService = tokenService;
            _mailService = mailService;
            _mapper = mapper;
        }









        [HttpPost(ApiRouter.AuthRoutes.AuthWithGoogle)]
        public async Task<IActionResult> GoogleSignIn([FromBody] ExternalAuthDto token)
        {
            var userInfo = await _tokenService.ValidateGoogleToken(token.Token);
            if (userInfo == null) return BadRequest(new ApiResponse(400,"Invalid token."));

            if (await _userManager.FindByEmailAsync(userInfo?.Email) == null)
            {
                var createUserResult = await _userManager.CreateAsync(userInfo);
                if (!createUserResult.Succeeded) return BadRequest(new ApiResponse(400, createUserResult.Errors.ToString()));
            }
            return Ok(token);
        }


        [HttpPost(ApiRouter.AuthRoutes.AuthWithFacebook)]
        public async Task<IActionResult> FacebookSignIn([FromBody] string token)
        {
            var userInfo = await _tokenService.ValidateFacebookToken(token);
            if (userInfo == null) return BadRequest(new ApiResponse(400, "Invalid token."));

            if (await _userManager.FindByEmailAsync(userInfo?.Email) == null)
            {
                var createUserResult = await _userManager.CreateAsync(userInfo);
                if (!createUserResult.Succeeded) return BadRequest(new ApiResponse(400, createUserResult.Errors.ToString()));
            }
            return Ok(token);
        }


        [HttpPost(ApiRouter.AuthRoutes.Login)]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpPost(ApiRouter.AuthRoutes.Register)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = new[] { "Email address is in use" } });
            }
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            await _mailService.SendOtp(registerDto.Email);

            return Ok(new ApiResponse(200));
        }


        [HttpPost(ApiRouter.AuthRoutes.SendOtp)]
        public async Task<IActionResult> SendOtp(SendOtpRequestDto otpRequest) =>
            await _mailService.SendOtp(otpRequest.Email) ? Ok(new ApiResponse(200))
            : BadRequest(new ApiResponse(404, "User Not found"));


        [HttpPost(ApiRouter.AuthRoutes.ActivateUser)]
        public async Task<IActionResult> ActivateAccount(VerifyOTPDto verifyOTP)
        {
            if (verifyOTP == null) return BadRequest(new ApiResponse(400));
            bool verify = await _mailService.VerifyOTPActivateAccountAsync(verifyOTP.Email, verifyOTP.otp);
            return verify ? Ok(new ApiResponse(200)) : BadRequest("Invalid code");
        }


        [HttpPatch(ApiRouter.AuthRoutes.UpdatePassword)]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto passwordDto)
        {
            var user = await _userManager.FindByEmailAsync(passwordDto.Email);
            if (user == null) return BadRequest(400);
            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.Password);
            return result.Succeeded ? Ok(new ApiResponse(200)) : BadRequest(new ApiResponse(400));
        }

    }



}
