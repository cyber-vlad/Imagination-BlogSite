using FluentValidation;
using Imagination.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Validation.EmailAddress
{
    public class EmailAddressValidator : AbstractValidator<EmailAddressDto>
    {
        public EmailAddressValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(100).WithMessage("Email MAXIM 100 characters");

        }
    }
}
