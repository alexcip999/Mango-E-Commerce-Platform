using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = 
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto.Message);
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem { Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            ResponseDto result = await _authService.RegisterAsync(registerRequestDto);
            ResponseDto assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registerRequestDto.Role))
                {
                    registerRequestDto.Role = SD.RoleCustomer;
                }
                
                assignRole = await _authService.AssignRoleAsync(registerRequestDto);

                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "User registered successfully";
                    return RedirectToAction(nameof(Login));
                }
            }

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text=SD.RoleAdmin, Value=SD.RoleAdmin},
                new SelectListItem { Text=SD.RoleCustomer, Value=SD.RoleCustomer}
            };

            ViewBag.RoleList = roleList;

            return View(registerRequestDto);
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
