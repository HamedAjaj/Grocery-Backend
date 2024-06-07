using AutoMapper;
using Grocery.Domain.Entities.Identity;
using Grocery.Dtos;
using Grocery.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Grocery.Extensions;
using Grocery.Helpers.Attributes;
using Grocery.Domain.IServices.ITokenServices;

namespace Grocery.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManger,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManger = signInManger;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("login")]
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

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.FindByEmailAsync(registerDto.Email) != null)
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

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email
            });
        }


        [Authorize]
        [Cache(1000)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                Email = email,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [Cache(1000)]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            return Ok(_mapper.Map<Address, AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("address")]
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
