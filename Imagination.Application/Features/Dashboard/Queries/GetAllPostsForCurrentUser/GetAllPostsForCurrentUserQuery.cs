using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Dashboard.Queries.GetAllPosts
{
    public sealed record GetAllPostsForCurrentUserQuery(int currentUserId) : IQuery<List<PostDto>>;

    public class GetAllPostsQueryHandler : IQueryHandler<GetAllPostsForCurrentUserQuery, List<PostDto>>
    {
        private readonly IPostService _postService;

        public GetAllPostsQueryHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<List<PostDto>> Handle(GetAllPostsForCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return await _postService.GetAllPostsForCurrentUserAsync(query.currentUserId);
        }
    }
}
