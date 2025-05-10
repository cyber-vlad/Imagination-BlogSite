using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
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

    public class GetAllPostsForCurrentUserQueryHandler : IQueryHandler<GetAllPostsForCurrentUserQuery, List<PostDto>>
    {
        private readonly IPostFacade _postFacade;

        public GetAllPostsForCurrentUserQueryHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<List<PostDto>> Handle(GetAllPostsForCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return await _postFacade.GetAllPostsForCurrentUserAsync(query.currentUserId);
        }
    }
}
