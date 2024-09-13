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
            try
            {
                var httpContext = _contextAccessor.HttpContext;
                var emailClaim = httpContext.User.FindFirst(ClaimTypes.Email);
                
                return emailClaim.Value;
            }
            
            catch (Exception)
            {
                throw new InvalidOperationException("Email claim not found.");
            }
         
        }

        public string GetCurrentUserId()
        {
            try
            {
                var httpContext = _contextAccessor.HttpContext;
                var emailClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                return emailClaim.Value;
            }

            catch (Exception)
            {
                throw new InvalidOperationException("Id claim not found.");
            }
        }
    }
}
