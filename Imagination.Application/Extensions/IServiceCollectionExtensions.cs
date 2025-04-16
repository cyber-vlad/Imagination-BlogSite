using FluentValidation;
using FluentValidation.AspNetCore;
using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Login;
using Imagination.Application.Features.Account.Commands.Logout;
using Imagination.Application.Features.Account.Commands.Register;
using Imagination.Application.Features.Account.Queries.CheckUserExistsByEmail;
using Imagination.Application.Features.Dashboard.Commands.CreatePost;
using Imagination.Application.Features.Dashboard.Commands.ToggleLike;
using Imagination.Application.Features.Dashboard.Queries.GetAllPosts;
using Imagination.Application.Features.Post.Commands.CreateComment;
using Imagination.Application.Features.Post.Queries.GetPostById;
using Imagination.Application.Features.Profile.Commands.EditProfileImage;
using Imagination.Application.Interfaces;
using Imagination.Application.Interfaces.Repositories;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Patterns.Mediator;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Patterns.Proxy;
using Imagination.Application.Patterns.Singleton;
using Imagination.Application.Responses;
using Imagination.Application.Validation.CreateComment;
using Imagination.Application.Validation.CreatePost;
using Imagination.Application.Validation.EmailAddress;
using Imagination.Application.Validation.Login;
using Imagination.Application.Validation.Register;
using Imagination.Application.Validation.ResetPassword;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddLoggerSingleton();
            services.AddAutoMapper();
            services.AddMediator();
            services.AddCommandsQueries();
            services.AddValidators();
        }

        private static void AddLoggerSingleton(this IServiceCollection services)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); 
            });

            var loggerSingleton = LoggerSingleton.GetInstance(loggerFactory);

            services.AddSingleton(loggerSingleton);
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void AddCommandsQueries(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<LoginCommand, BaseResponse>, LoginCommandHandler>();
            services.AddTransient<ICommandHandler<RegisterCommand, BaseResponse>, RegisterCommandHandler>();
            services.AddTransient<ICommandHandler<LogoutCommand, BaseResponse>, LogoutCommandHandler>();
            services.AddTransient<ICommandHandler<EditProfileImageCommand, BaseResponse>, EditProfileImageCommandHandler>();
            services.AddTransient<ICommandHandler<CreatePostCommand, BaseResponse>, CreatePostCommandHandler>();
            services.AddTransient<ICommandHandler<ToggleLikeCommand, ToggledLikeResponse>, ToggleLikeCommandHandler>();
            services.AddTransient<ICommandHandler<CreateCommentCommand, CreatedCommentResponse>, CreateCommentCommandHandler>();
            
            services.AddTransient<IQueryHandler<CheckUserExistsByEmailQuery, BaseResponse>, CheckUserExistsByEmailQueryHandler>();
            services.AddTransient<IQueryHandler<GetAllPostsForCurrentUserQuery, List<PostDto>>, GetAllPostsForCurrentUserQueryHandler>();
            services.AddTransient<IQueryHandler<GetPostDetailsByIdQuery, PostDto>, GetPostDetailsByIdQueryHandler>();
        }

        private static void AddMediator(this IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
        }

        private static void AddValidators(this IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation();

            services.AddTransient<IValidator<RegisterDto>, RegisterValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginValidator>();
            services.AddTransient<IValidator<ResetPasswordDto>, ResetPasswordValidator>();
            services.AddTransient<IValidator<EmailAddressDto>, EmailAddressValidator>();
            services.AddTransient<IValidator<CreatePostDto>, CreatePostValidator>();
            services.AddTransient<IValidator<CreateCommentDto>, CreateCommentValidator>();
        }
    }
}
