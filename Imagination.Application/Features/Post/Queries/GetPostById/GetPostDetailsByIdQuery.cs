using Imagination.Application.DTOs;
using Imagination.Application.Features.Dashboard.Queries.GetAllPosts;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
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
        private readonly IPostFacade _postFacade;

        public GetPostDetailsByIdQueryHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<PostDto> Handle(GetPostDetailsByIdQuery query, CancellationToken cancellationToken)
        {
            return await _postFacade.GetPostDetailsByIdAsync(query.postId, query.currentUserId);
        }
    }
}
