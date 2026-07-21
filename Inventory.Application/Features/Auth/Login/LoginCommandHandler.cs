using Inventory.Application.Abstractions.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Inventory.Application.Features.Auth.Login
{
    public sealed class LoginCommandHandler
        : IRequestHandler<LoginCommand, string>
    {
        private readonly IJwtConfiguration _jwtConfiguration;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IJwtConfiguration jwtConfiguration,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtConfiguration = jwtConfiguration;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public Task<string> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var options = _jwtConfiguration.Current;

            if (request.UserName != options.UserName ||
                request.Password != options.Password)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.UserName),
                new(ClaimTypes.Role, "Administrator")
            };

            var token = _jwtTokenGenerator.GenerateToken(
                request.UserName,
                claims);

            return Task.FromResult(token);
        }
    }
}