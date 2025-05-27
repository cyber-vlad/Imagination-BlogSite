using Imagination.Application.Interfaces.Repositories;
using Imagination.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Imagination.Infrastructure.Services.Repositories.CachedPostRepository
{
    public class CachedPostRepository : IPostRepository
    {
        private readonly IPostRepository _postRepository;
        private readonly IDistributedCache _distributedCache;

        public CachedPostRepository(IPostRepository postRepository, IDistributedCache distributedCache)
        {
            _postRepository = postRepository;
            _distributedCache = distributedCache;
        }

        public async Task AddPostAsync(Post post)
        {
            await _postRepository.AddPostAsync(post);
        }

        public async Task DeletePostAsync(int postId)
        {
            await _postRepository.DeletePostAsync(postId);
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _postRepository.GetAllPostsAsync();
        }

        public async Task<List<Post>> GetAllPostsByAuthorId(int authorId)
        {
            return await _postRepository.GetAllPostsByAuthorId(authorId);
        }

        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            string key = $"post-{postId}";
            
            string? cachedPost = await _distributedCache.GetStringAsync(key);

            Post? post;
            if (string.IsNullOrEmpty(cachedPost))
            {
                post = await _postRepository.GetPostByIdAsync(postId);
                
                if(post is null)
                {
                    return post;
                }
                
                await _distributedCache.SetStringAsync(key,JsonConvert.SerializeObject(post, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

                return post;
            }

            post = JsonConvert.DeserializeObject<Post>(cachedPost);
            
            return post;
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _postRepository.UpdatePostAsync(post);
        }
    }
}
