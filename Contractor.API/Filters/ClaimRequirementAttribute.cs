
using Contractor.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;

namespace Contractor.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(params string[] parameters) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { parameters };
        }
    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly string[] _parameters;

        public ClaimRequirementFilter(string[] parameters)
        {
            _parameters = parameters;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Claims.Any())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasClaim = context.HttpContext.User.Claims.Any(c => _parameters.Contains(c.Value));

            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
