using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TasikmalayaKota.Simpatik.Web.Helpers;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate Next;
        private IMiddlewares Middleware;
        private IConfiguration Configuration { get; }

        public AuthMiddleware(RequestDelegate next, IMiddlewares middleware, IConfiguration configuration)
        {
            Next = next;
            Middleware = middleware;
            Configuration = configuration;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            //httpContext.Response.Redirect("http://www.bing.com", false);

            //httpContext.Response.Redirect("/simpattik/login");

            //var currentPath = httpContext.Request.Path.ToString().Split("/")[1].ToLower();

            var path = httpContext.Request.Path;

            ////var currentPathAksi = httpContext.Request.Path.ToString().Replace("/" + currentPath + "/", "");


            if (path.HasValue && !path.Value.StartsWith("/login"))
            {
                if (httpContext.Session.GetString("IDAkun") == null)
                {
                    string slug = "/" + Configuration.GetValue<string>("SimpatikSettings:WebsiteSLUG");
                    if (slug != "/")
                    {
                        httpContext.Response.Redirect(slug + "/login");
                    }
                    else
                    {
                        httpContext.Response.Redirect("/login");
                    }
                }
                else
                {
                    if ((httpContext.Request.Path.ToString() == "/") && httpContext.Session.GetString("IDAkun") != null)
                    {
                        httpContext.Response.Redirect("/home/index");
                    }
                    //else if ((httpContext.Request.Path.ToString() == "/") && httpContext.Session.GetString("IDAkun") == null)
                    //{
                    //    httpContext.Response.Redirect("/simpattik/login");
                    //}
                    //else
                    //{
                    //    var currentPath = httpContext.Request.Path.ToString().Split("/")[1].ToLower();

                    //    if (currentPath != "css" && currentPath != "js" && currentPath != "images" && currentPath != "login" && currentPath != "logout" && currentPath != "error")
                    //    {
                    //        var currentPathAksi = httpContext.Request.Path.ToString().Split("/")[2].ToLower();
                    //        var additionalAksi = httpContext.Request.Path.ToString().Split("/").Length > 3 ? httpContext.Request.Path.ToString().Split("/")[3].ToLower() : "";
                    //        var UserId = httpContext.Session.GetString("UserIdUsman");
                    //        //var _getData = Middleware.GetControllerIndex(currentPath);
                    //        //if (!string.IsNullOrEmpty(_getData.NamaIndex))
                    //        //{
                    //        //    var _getNextData = Middleware.GetUserController((int)httpContext.Session.GetInt32("IDAkun"), path.ToString().ToLower());

                    //        //    if (string.IsNullOrEmpty(_getNextData.LinkUrl))
                    //        //    {
                    //        //        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    //        //        return;
                    //        //    }
                    //        //}
                    //        if (!Middleware.GetAksesAksiadditional(currentPath, currentPathAksi, additionalAksi, UserId))
                    //        {
                    //            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    //            return;
                    //        }
                    //    }
                    //}
                }
            }
            else if (path.Value.StartsWith("/login") && httpContext.Session.GetString("IDAkun") != null && !httpContext.Request.IsAjaxRequest())
            {
                httpContext.Response.Redirect("/home/index");
            }
            else if (path.Value.StartsWith("/login") && httpContext.Session.GetString("IDAkun") != null && httpContext.Request.IsAjaxRequest())
            {
                httpContext.Response.Redirect("/login/get-data-login");
            }

            await Next(httpContext);

            /* if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    //the user is authenticated, yet we are returning a 401
                    //let's return a 403 instead
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                }
            } */
        }
    }
}
