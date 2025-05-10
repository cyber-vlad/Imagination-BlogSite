using Imagination.Domain.Common.Interfaces;
using Imagination.Domain.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagination.Application.Interfaces;
using Imagination.Infrastructure.Services;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Infrastructure.Services.Repositories;
using Imagination.Application.Patterns.Proxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Imagination.Application.Patterns.Singleton;
using Imagination.Infrastructure.Services.UnitOfWork;
using Imagination.Application.Patterns.ChainOfResponsability;
using Imagination.Infrastructure.Handlers.Comments;
using Imagination.Application.Patterns.Facade;

namespace Imagination.Infrastructure.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddServices();
            services.AddChainOfResponsability();
            //services.AddProxy();
        }

        //private static void AddProxy(this IServiceCollection services)
        //{
        //    services.AddMemoryCache();
        //    services.AddScoped<IPostService>(provider =>
        //    {
        //        var postService = new PostService(
        //            provider.GetRequiredService<IPostRepository>(),
        //            provider.GetRequiredService<IUserRepository>()
        //        );
        //        var cache = provider.GetRequiredService<IMemoryCache>();
        //        var loggerSingleton = provider.GetRequiredService<LoggerSingleton>();
        //        return new PostServiceProxy(postService, cache, loggerSingleton);
        //    });
        //}

        private static void AddServices(this IServiceCollection services)
        {
            services
                .AddTransient<IMediator, Mediator>()
                .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<ILikeRepository, LikeRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IPostFacade, PostFacade>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
                //.AddScoped<ICommentHandler, CommentHandler>();


        }

        private static void AddChainOfResponsability(this IServiceCollection services)
        {
            services.AddTransient<ParentCommentHandler>();
            services.AddTransient<ReplyCommentHandler>();

            services.AddTransient<ICommentHandler>(sp =>
            {
                var parentHandler = sp.GetRequiredService<ParentCommentHandler>();
                var replyHandler = sp.GetRequiredService<ReplyCommentHandler>();

                parentHandler.SetSuccessor(replyHandler);

                return parentHandler;
            });
        }
    }
}
