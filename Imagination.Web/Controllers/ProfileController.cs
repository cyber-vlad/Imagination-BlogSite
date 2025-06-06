﻿using Azure;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Post.Queries.GetPostsByAuthorId;
using Imagination.Application.Features.Profile.Commands.EditProfileImage;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Responses;
using Imagination.Domain.Enum;
using Imagination.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imagination.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Profile/Index.cshtml");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewPosts()
        {
            var posts = await _mediator.Send(new GetAllPostsByAuthorIdQuery(GetUserClaims().Result.Id));
            return View("~/Views/Profile/_ViewPosts.cshtml", posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfileImage(PhotoUserDto model)
        {
            model.IdUser = GetUserClaims().Result.Id;
            var response = await _mediator.Send(new EditProfileImageCommand(model));

            switch (response.ErrorCode)
            {
                case ErrorCode.Upload_image_failed:
                    TempData["ErrorMessage"] = response.ErrorMessage; break;
                case ErrorCode.NoError:
                    TempData["SuccessMessage"] = response.ErrorMessage; break;
            }

            return View("~/Views/Profile/Index.cshtml");
        }
    }
}
