using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string UserName,
        string Password)
        : IRequest<string>;
}