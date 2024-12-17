using clinic.auth;
using clinic.models;
using clinic.packpages;
using clinic.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static clinic.auth.JWTmanager;
using static clinic.services.EmailService;

namespace clinic.Controllers
{
    [Route("api/[controller]/[actions]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPKG_USER _package;
        private readonly EmailService _emailService;
        private readonly IjwtManager JWTmanager;


        public UserController(IPKG_USER package, EmailService emailService, IjwtManager jWTmanager)
        {
            _package = package;
            _emailService = emailService;
            JWTmanager = jWTmanager;
        }

        [HttpPost("/add_user")]
        public IActionResult Add_User([FromBody] User user)
        {
            try
            {
                bool isCodeValid = _package.ValidateVerificationCode(user.Email, user.VerificationCode);

                if (isCodeValid)
                {
                    byte[] picData = null;
                    using (var ms = new MemoryStream())
                    {
                        if (user.Profile_img != null)
                        {
                            user.Profile_img.CopyTo(ms);
                            picData = ms.ToArray();
                        }
                        else
                        {
                            picData = null;
                        }
                    }
                    _package.Add_User(user,picData);
                    return Ok("User added successfully.");
                }
                else
                {
                    return BadRequest("Invalid or expired verification code.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }

                // Return the error message as plain text
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("/get_user")]
        public List<User> Get_Users()
        {
            List<User> list = new List<User>();
            try
            {
                list = _package.Get_Users();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        [HttpPost("/send_verification_code")]
        public async Task<IActionResult> SendVerificationCode([FromBody] VerificationCodeRequest? request)
        {
            try
            {
                string GenerateVerificationCode()
                {
                    Random random = new Random();
                    return random.Next(100000, 999999).ToString();
                }
                var verificationCode = GenerateVerificationCode();
                _package.Add_v_code(request.Email, verificationCode);

                await _emailService.SendVerificationCodeAsync(request.Email, verificationCode);

                Response.ContentType = "application/json";
                return Ok("Verification code sent successfully");

            }
            catch (Exception ex)
            {
                return BadRequest($"Error sending verification code: {ex.Message}");
            }
        }
        [HttpPost("/login_user")]
        public IActionResult Authentification(Login loginData)
        {
            Token? token = null;
            User? user = null;

            try
            {
                user = _package.authentification(loginData);
                if (user == null) return Unauthorized("username or password is inccorect");

                token = JWTmanager.GetToken(user);
            }
            catch (Exception ex)
            {
               Console.WriteLine($"{ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "System error,try again");

            }
            return Ok(token);


        }
        [HttpGet("/user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = _package.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


    }
}