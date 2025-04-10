using FluentValidation;
using FluentValidation.Validators;
using Imagination.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Validation.CreatePost
{
    public class CreatePostValidator : AbstractValidator<CreatePostDto>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(2).WithMessage("Title MINIM 2 characters")
                .MaximumLength(60).WithMessage("Title MAXIM 60 characters");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(4000).WithMessage("Content MAXIM 4000 characters");
        }
    }
}
