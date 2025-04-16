using Azure;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Dashboard.Commands.CreatePost;
using Imagination.Application.Features.Dashboard.Commands.ToggleLike;
using Imagination.Application.Features.Post.Commands.CreateComment;
using Imagination.Application.Features.Post.Queries.GetPostById;
using Imagination.Application.Patterns.Mediator;
using Imagination.Application.Patterns.Mediator.Interfaces;
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
            PostDto responsePost = await _mediator.Send(new GetPostDetailsByIdQuery(postId));
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
        public async Task<IActionResult> ToggleLike(int postId, int userId)
        {
            var model = new ToggleLikeDto { PostId = postId, UserId = userId };
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
            var result = await _mediator.Send(new CreateCommentCommand(model));
            return Json(new { success = true });
        }
    }
}
