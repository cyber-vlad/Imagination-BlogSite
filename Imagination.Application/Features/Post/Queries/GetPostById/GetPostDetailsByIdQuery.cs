using Imagination.Application.DTOs;
using Imagination.Application.Features.Dashboard.Queries.GetAllPosts;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Post.Queries.GetPostById
{
    public sealed record GetPostDetailsByIdQuery(int postId, int currentUserId) : IQuery<PostDto>;

    public class GetPostDetailsByIdQueryHandler : IQueryHandler<GetPostDetailsByIdQuery, PostDto>
    {
        private readonly IPostService _postService;

        public GetPostDetailsByIdQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<PostDto> Handle(GetPostDetailsByIdQuery query, CancellationToken cancellationToken)
        {
            return await _postService.GetPostDetailsByIdAsync(query.postId, query.currentUserId);
        }
    }
}
