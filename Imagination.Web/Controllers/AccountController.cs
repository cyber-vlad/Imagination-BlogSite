using FluentValidation;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Register;
using Imagination.Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Imagination.Domain.Enum;
using Imagination.Application.Features.Account.Commands.Login;
using Microsoft.AspNetCore.Authentication;
using Imagination.Application.Features.Account.Commands.Logout;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Features.Account.Queries.CheckUserExistsByEmail;
using Imagination.Application.Patterns.Singleton;
using Azure;

namespace Imagination.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AccountController(IMediator mediator, LoggerSingleton loggerSingleton)
        {
            _mediator = mediator;
            _logger = loggerSingleton.GetLogger();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login failed due to invalid model state");
                return View(model); 
            }

            var response = await _mediator.Send(new LoginCommand(model));

            switch(response.ErrorCode)
            {
                case ErrorCode.NoError:
                    _logger.LogInformation($"User {model.Username} logged in successfully");
                    return RedirectToAction("Dashboard", "Home");
                case ErrorCode.Username_or_password_is_incorrect:
                    _logger.LogWarning($"Failed login attempt for {model.Username}: {response.ErrorMessage}");
                    TempData["ErrorMessage"] = response.ErrorMessage; break;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Registration failed due to invalid model state");
                return View(model);
            }
            
            var response = await _mediator.Send(new RegisterCommand(model));

            switch(response.ErrorCode)
            {
                case ErrorCode.NoError:
                    _logger.LogInformation($"User {model.Email} registered in successfully");
                    return RedirectToAction("Login", "Account");
                case ErrorCode.Email_already_used:
                    _logger.LogWarning($"Registration failed: Email {model.Email} is already in use.");
                    TempData["ErrorMessage"] = response.ErrorMessage; break;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logging out");

            await _mediator.Send(new LogoutCommand());

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View("~/Views/Account/ForgotPassword.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(EmailAddressDto model)
        {
           if (!ModelState.IsValid)
            {
                _logger.LogWarning("Forgot password request failed due to invalid model state");
                return View(model);
            }

            var result = await _mediator.Send(new CheckUserExistsByEmailQuery(model));

            if (result.ErrorCode == ErrorCode.User_not_found)
            {
                _logger.LogWarning($"Password reset failed: Email {model.Email} not found.");
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                _logger.LogInformation($"Password reset request processed for email: {model.Email}");
            }

            return View("~/Views/Account/ForgotPassword.cshtml");
        }
    }
}
