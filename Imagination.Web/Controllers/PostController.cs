﻿using Azure;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Dashboard.Commands.CreatePost;
using Imagination.Application.Features.Dashboard.Commands.ToggleLike;
using Imagination.Application.Features.Post.Commands.CreateComment;
using Imagination.Application.Features.Post.Commands.DeletePost;
using Imagination.Application.Features.Post.Commands.EditPost;
using Imagination.Application.Features.Post.Queries.GetAuthorPostById;
using Imagination.Application.Features.Post.Queries.GetPostById;
using Imagination.Application.Features.Post.Queries.GetPostsByAuthorId;
using Imagination.Application.Patterns.Mediator;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Domain.Entities;
using Imagination.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imagination.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int postId)
        {
            var responsePost = await _mediator.Send(new GetPostDetailsByIdQuery(postId, GetUserClaims().Result.Id));

            if (responsePost is null)
            {
                TempData["ErrorMessage"] = "Something is wrong with this post";
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Post/Details.cshtml", responsePost);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePost()
        {
            return PartialView("~/Views/Home/Modals/CreatePostModal.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto model)
        {
            model.AuthorId = GetUserClaims().Result.Id;

            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Home/Modals/CreatePostModal.cshtml", model);
            }

            var response = await _mediator.Send(new CreatePostCommand(model));

            switch (response.ErrorCode)
            {
                case ErrorCode.NoError:
                    TempData["SuccessMessage"] = "Post created successfully";
                    return Json(new { success = true });
                case ErrorCode.Create_post_failed:
                    TempData["ErrorMessage"] = response.ErrorMessage; break;
            }
         
            return PartialView("~/Views/Home/Modals/CreatePostModal.cshtml", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var response = await _mediator.Send(new DeletePostCommand(postId));
            
            if(response.ErrorCode == ErrorCode.Delete_post_failed)
                return Json(new { success = false });
            
            return Json(new { success = true });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditPost(int postId)
        {
            var post = await _mediator.Send(new GetAuthorPostByIdQuery(postId, GetUserClaims().Result.Id));
            if(post is not null) 
                return View("~/Views/Post/EditPost.cshtml", post);
            else
                return RedirectToAction("ViewPosts", "Profile");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost(EditPostDto model)
        {
            var response = await _mediator.Send(new EditPostCommand(model));
            if(response.ErrorCode == ErrorCode.NoError)
            {
                return RedirectToAction("ViewPosts", "Profile");
            }
            return RedirectToAction("ViewPosts", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLike(int postId)
        {
            var model = new ToggleLikeDto { PostId = postId, UserId = GetUserClaims().Result.Id };
            var response = await _mediator.Send(new ToggleLikeCommand(model));

            if (response.ErrorCode == ErrorCode.Internal_error)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true, likeCount = response.NrLikes, isLiked = response.IsLiked });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Views/Home/_CreateCommentPartial.cshtml", model);
            }

            model.UserId = GetUserClaims().Result.Id;
            var response = await _mediator.Send(new CreateCommentCommand(model));
            
            if(response.ErrorCode == ErrorCode.NoError)
            {
                return Json(new { success = true, commentCount = response.NrComments });
            }
            else
            {
                return Json(new { success = false});
            }
        }
    }
}
