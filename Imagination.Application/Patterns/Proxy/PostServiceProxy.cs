using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.Singleton;
using Imagination.Application.Responses;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Patterns.Proxy
{
    public class PostServiceProxy //: IPostService
    {
        //private readonly IPostService _postService;
        //private readonly IMemoryCache _cache;
        //private readonly ILogger _logger;
        //private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        //public PostServiceProxy(IPostService postService, IMemoryCache cache, LoggerSingleton loggerSingleton)
        //{
        //    _postService = postService;
        //    _cache = cache;
        //    _logger = loggerSingleton.GetLogger();
        //}

        //public async Task<List<PostDto>> GetAllPostsAsync()
        //{
        //    if (_cache.TryGetValue("AllPosts", out List<PostDto> cachedPosts))
        //    {
        //        _logger.LogInformation("Returning posts from cache.");
        //        return cachedPosts;
        //    }

        //    _logger.LogInformation("Loading posts from database.");

        //    var posts = await _postService.GetAllPostsAsync();

        //    _cache.Set("AllPosts", posts, _cacheDuration);

        //    return posts;
        //}

        //public async Task<BaseResponse> CreatePostAsync(CreatePostDto model)
        //{
        //    _logger.LogInformation("Creating a new post and invalidating cache.");

        //    _cache.Remove("AllPosts");

        //    return await _postService.CreatePostAsync(model);
        //}
    }
}
