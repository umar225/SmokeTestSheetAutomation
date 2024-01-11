using Coursewise.Api.Utility;

namespace Coursewise.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsHaveClaims(this HttpContext context)
        {
            return context.User.Claims.Any();
        }
        public static string GetCustomerWordpressToken(this HttpContext context)
        {
            if (context.IsHaveClaims())
            {
                try
                {
                    var customerId = context.User.Claims.Single(c => c.Type == ClaimValues.BoardwiseWordpressToken).Value;
                    return customerId;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        public static Guid GetCustomerId(this HttpContext context)
        {
            if (context.IsHaveClaims())
            {
                try
                {
                    var customerId = context.User.Claims.Single(c => c.Type == ClaimValues.CustomerId).Value;
                    return Guid.Parse(customerId);
                }
                catch (Exception)
                {
                    return Guid.Empty;
                }
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
