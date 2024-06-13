namespace BlazorAuthentication.Service.Authentication
{
    public static class CookieHelper
    {
        public static void SetCookie(IHttpContextAccessor httpContextAccessor, string key, string value, int? expireTime)
        {
            var context = httpContextAccessor.HttpContext;
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMinutes(30);

            context.Response.Cookies.Append(key, value, option);
        }

        public static string GetCookie(IHttpContextAccessor httpContextAccessor, string key)
        {
            var context = httpContextAccessor.HttpContext;
            return context.Request.Cookies[key];
        }

        public static void RemoveCookie(IHttpContextAccessor httpContextAccessor, string key)
        {
            var context = httpContextAccessor.HttpContext;
            context.Response.Cookies.Delete(key);
        }
    }
}
