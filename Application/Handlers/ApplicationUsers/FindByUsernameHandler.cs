﻿using Application.Repository;
using Common.Dtos;
using Common.Entities;
using Common.Mappings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ApplicationUsers
{
    public class FindByUsernameHandler : IRequestHandler<FindByUsernameRequest, ApplicationUserResponseDto>
    {
        private readonly ILogger<FindByUsernameHandler> logger;
        private readonly IApplicationUserRepository repository;
        private readonly IRepository<ApplicationUserDetails> detailsRepository;
        private readonly SharedMapping sharedMapping;

        public FindByUsernameHandler(
            ILogger<FindByUsernameHandler> logger, 
            IApplicationUserRepository repository,
            IRepository<ApplicationUserDetails> detailsRepository,
            SharedMapping sharedMapping)
        {
            this.logger = logger;
            this.repository = repository;
            this.sharedMapping = sharedMapping;
            this.detailsRepository = detailsRepository;
        }

        public Task<ApplicationUserResponseDto> Handle(FindByUsernameRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Getting user data for {request.Username}");
            var user = repository.Find(u => u.UserName == request.Username).FirstOrDefault();
            ApplicationUserResponseDto responseDto = new ApplicationUserResponseDto();
            if (user != null)
            {
                sharedMapping.Map(user, responseDto);
            }
            else
            {
                responseDto.SetError($"User not found by username {request.Username}");
            }
            return Task.FromResult(responseDto);
        }
    }
}
