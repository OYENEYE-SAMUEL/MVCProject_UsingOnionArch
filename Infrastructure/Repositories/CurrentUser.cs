using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string GetCurrentUser()
        {
            var httpContext = _contextAccessor.HttpContext;
            if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("No authenticated user found");
            }
            var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                throw new InvalidOperationException("Email claim not found.");
            }
            return emailClaim.Value;
        }
    }
}
