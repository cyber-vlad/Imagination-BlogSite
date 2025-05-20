using Imagination.Application.DTOs;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Post.Queries.GetAuthorPostById
{

    public sealed record GetAuthorPostByIdQuery(int postId, int authorId) : IQuery<PostDto>;

    public class GetAuthorPostByIdQueryHandler : IQueryHandler<GetAuthorPostByIdQuery, PostDto>
    {
        private readonly IPostFacade _postFacade;

        public GetAuthorPostByIdQueryHandler(IPostFacade postFacade)
        {
            _postFacade = postFacade;
        }

        public async Task<PostDto> Handle(GetAuthorPostByIdQuery query, CancellationToken cancellationToken)
        {
            return await _postFacade.GetAuthorPostByIdAsync(query.postId, query.authorId);
        }
    }
}
