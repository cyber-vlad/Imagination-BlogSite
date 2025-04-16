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
            List<PostDto> posts = await _mediator.Send(new GetAllPostsForCurrentUserQuery(GetUserClaims().Result.Id));

            return View("~/Views/Home/Dashboard.cshtml", PaginatedListDto<PostDto>.Create(posts, Math.Max(1, pageIndex), 5));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
