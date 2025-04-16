using FluentValidation;
using Imagination.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Validation.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MinimumLength(1).WithMessage("Content MINIM 1 character")
                .MaximumLength(1000).WithMessage("Content MAXIM 1000 characters");
        }
    }
}
