using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Imagination.Application.DTOs;
namespace Imagination.Application.Validation.Register
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("Username MAXIM 20 characters")
                .MinimumLength(6).WithMessage("Username MINIM 6 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid")
                .MaximumLength(100).WithMessage("Email MAXIM 100 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password MINIM 8 characters")
                .MaximumLength(20).WithMessage("Password MAXIM 20 characters");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords are not match!");
        }
    }
}
