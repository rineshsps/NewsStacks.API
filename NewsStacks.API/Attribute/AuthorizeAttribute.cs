using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsStacks.API.Attribute
{
    public enum AuthenticationParams
    {
        AdminRequired = 1
    }
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public string? Roles { get; set; }
        public AuthorizeAttribute(params AuthenticationParams[] authParams) : base(typeof(AuthenticationFilter))
        {
            Arguments = new object[] { authParams };
        }
    }

    public class AuthenticationFilter : IAuthorizationFilter
    {
        public AuthenticationFilter(AuthenticationParams[] authParams)
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["Name"];
            var permissionResult = false;

            var authorizationToken = context.HttpContext.Request.Headers["Authorization"][0].Split("Bearer ");
            if (authorizationToken != null && authorizationToken.Length > 1)
            {
                var token = authorizationToken[1];
                var validated = ValidateJWT(token, out permissionResult);

                if (!permissionResult)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private ClaimsPrincipal ValidateJWT(string jwtToken, out bool permissionResult)
        {
            ClaimsPrincipal principal = null;
            var errorMessage = "No valid cookie found";
            permissionResult = false;
            if (!String.IsNullOrEmpty(jwtToken))
            {
                try
                {
                    string rawToken = jwtToken.Trim();

                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    JwtSecurityToken token = (JwtSecurityToken)tokenHandler.ReadToken(rawToken);

                    SecurityToken validatedToken;
                    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("passkeywordsdfgdsfgfdsg"));
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,
                        RequireExpirationTime = false,
                    };

                    principal = tokenHandler.ValidateToken(token.RawData, tokenValidationParameters, out validatedToken);
                    ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;

                    //if (!authorizedToken)
                    //{
                    //    Claim DataExtractionClaim = identity.FindFirst("dataextraction");
                    //    if (DataExtractionClaim != null)
                    //    {
                    //        var canExtract = JsonSerializer.Deserialize<PermissionToken>(DataExtractionClaim.Value);
                    //        if (canExtract.can_schedule_and_extract)
                    //        {
                    //            validated = true;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    validated = true;
                    //}
                    permissionResult = true;
                    errorMessage = "";
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }

            return principal;
        }
    }
}
