using FluentValidation;
using Imagination.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Validation.Login
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(6).WithMessage("Username MINIM 6 characters")
                .MaximumLength(20).WithMessage("Username MAXIM 20 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password MINIM 8 characters")
                .MaximumLength(20).WithMessage("Password MAXIM 20 characters");
        }
    }
}
