using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Post.Queries.GetPostsByAuthorId
{
    public sealed record GetAllPostsByAuthorIdQuery(int authorId) : IQuery<List<PostDto>>;

    public class GetAllPostsByAuthorIdQueryHandler : IQueryHandler<GetAllPostsByAuthorIdQuery, List<PostDto>>
    {
        private readonly IPostFacade _postFacade;

        public GetAllPostsByAuthorIdQueryHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<List<PostDto>> Handle(GetAllPostsByAuthorIdQuery query, CancellationToken cancellationToken)
        {
            return await _postFacade.GetAllPostsByAuthorIdAsync(query.authorId);
        }
    }
}
