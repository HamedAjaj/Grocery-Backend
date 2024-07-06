using AutoMapper;
using Grocery.Domain.Entities.Identity;
using Grocery.Service.Dtos;
using Grocery.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Grocery.Extensions;
using Grocery.Helpers.Attributes;
using Grocery.Domain.IServices.ITokenServices;
using Grocery.Domain.IServices.MailServices;
using Grocery.Service.Dtos.OTP;
using Grocery.Domain.GroceryMetaData.Routing;
using Grocery.Service.Dtos.UserAccount;

namespace Grocery.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AccountController(
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

        //[HttpPost(ApiRouter.AccountRoutes.Login)]
        //public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        //{
        //    var user = await _userManager.FindByEmailAsync(loginDto.Email);
        //    if (user == null) return Unauthorized(new ApiResponse(401));
        //    var result = await _signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
        //    if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        //    return Ok(new UserDto()
        //    {
        //        DisplayName = user.DisplayName,
        //        Email = user.Email,
        //        Token = await _tokenService.CreateTokenAsync(user, _userManager)
        //    });
        //}

        //[HttpPost(ApiRouter.AccountRoutes.Register)]
        //public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        //{
        //    if(await _userManager.FindByEmailAsync(registerDto.Email) != null)
        //    {
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = new[] { "Email address is in use" } });
        //    }
        //    var user = new AppUser()
        //    {
        //        DisplayName = registerDto.DisplayName,
        //        Email = registerDto.Email,
        //        UserName = registerDto.Email.Split('@')[0],
        //        PhoneNumber = registerDto.PhoneNumber,

        //    };

        //    var result = await _userManager.CreateAsync(user, registerDto.Password);
        //    if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        //    await  _mailService.SendOtp(registerDto.Email);

        //    return Ok(new ApiResponse(200));
        //}


        //[HttpPost(ApiRouter.AccountRoutes.SendOtp)]
        //public async Task<IActionResult> SendOtp(SendOtpRequestDto otpRequest) =>
        //    await _mailService.SendOtp(otpRequest.Email) ? Ok(new ApiResponse(200))
        //    : BadRequest(new ApiResponse(404, "User Not found"));


        //[HttpPost(ApiRouter.AccountRoutes.ActivateUser)]
        //public  async Task<IActionResult> ActivateAccount(VerifyOTPDto verifyOTP)
        //{
        //    if (verifyOTP == null) return BadRequest(new ApiResponse(400));
        //    bool verify = await _mailService.VerifyOTPActivateAccountAsync(verifyOTP.Email, verifyOTP.otp);
        //    return verify ? Ok(new ApiResponse(200)) : BadRequest("Invalid code");
        //}


        //[HttpPatch(ApiRouter.AccountRoutes.UpdatePassword)]
        //public async Task<IActionResult> UpdatePassword(UpdatePasswordDto passwordDto)
        //{
        //    var user = await _userManager.FindByEmailAsync(passwordDto.Email);
        //    if (user == null) return BadRequest(400);
        //    var result =await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.Password);
        //    return result.Succeeded ? Ok(new ApiResponse(200)): BadRequest(new ApiResponse(400));
        //}


        [Authorize]
        [Cache(1000)]
        [HttpGet(ApiRouter.AccountRoutes.Get)]
        public async Task<ActionResult<AuthorizedUserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new AuthorizedUserDto()
            {
                Email = email,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                IsVerified = user.IsEmailVerified

            });
        }


        [Authorize]
        [Cache(1000)]
        [HttpGet(ApiRouter.AccountRoutes.GetAddress)]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        }


        [Authorize]
        [HttpPut(ApiRouter.AccountRoutes.UpdateAddress)]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto newAddress)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            user.Address = _mapper.Map<AddressDto, Address>(newAddress);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = new[] { "An error occured during updating the address" } });
            return Ok(newAddress);
        }
    }
}
