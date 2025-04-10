using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Login;
using Imagination.Application.Features.Dashboard.Commands.CreatePost;
using Imagination.Application.Features.Dashboard.Commands.ToggleLike;
using Imagination.Application.Features.Dashboard.Queries.GetAllPosts;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Patterns.Singleton;
using Imagination.Domain.Enum;
using Imagination.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Diagnostics;

namespace Imagination.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMediator _mediator;
        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Dashboard(int pageIndex) 
        {
            var userClaim = GetUserClaims();
            List<PostDto> posts = await _mediator.Send(new GetAllPostsForCurrentUserQuery(userClaim.Result.Id));
            var paginatedPosts = PaginatedListDto<PostDto>.Create(posts, Math.Max(1, pageIndex), 5);

            ViewData["PaginatedPosts"] = paginatedPosts;

            return View("~/Views/Home/Dashboard.cshtml");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> LoadPosts(int pageIndex)
        {
            var userClaim = GetUserClaims();
            List<PostDto> posts = await _mediator.Send(new GetAllPostsForCurrentUserQuery(userClaim.Id));
             
            return Json(PaginatedListDto<PostDto>.Create(posts, Math.Max(1, pageIndex), 5));
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
            return Json(new { success = true, likeCount = response.Post.NrLikes, isLiked = response.IsLiked});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
