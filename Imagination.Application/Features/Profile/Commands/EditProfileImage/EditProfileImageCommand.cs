using Imagination.Application.DTOs;
using Imagination.Application.Interfaces;
using Imagination.Application.Patterns.CQRS;
using Imagination.Application.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Features.Profile.Commands.EditProfileImage
{
    public sealed record EditProfileImageCommand(PhotoUserDto model) : ICommand<BaseResponse>;
    internal sealed class EditProfileImageCommandHandler : ICommandHandler<EditProfileImageCommand, BaseResponse>
    {
        private readonly IProfileService _profileService;
        public EditProfileImageCommandHandler(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<BaseResponse> Handle(EditProfileImageCommand command, CancellationToken cancellationToken)
        {
            return await _profileService.EditProfileImageAsync(command.model);
        }
    }
}
