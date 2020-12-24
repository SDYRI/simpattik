using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUser.DALS;
using TasikmalayaKota.Simpatik.Web.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.DALS;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.DALS;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces;
using TasikmalayaKota.Simpatik.Web.Middlewares;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.DALS;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.DALS;
using Microsoft.AspNetCore.HttpOverrides;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.DALS;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.DALS;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.DALS;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.DALS;

namespace TasikmalayaKota.Simpatik.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => {
              options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });
            services.AddScoped<Simpat1kContext>();
            services.AddCors();

            services.AddMvc().AddNewtonsoftJson(options =>
            options.SerializerSettings.ContractResolver =
            new DefaultContractResolver());
            services.AddControllersWithViews();

            //services.AddControllersWithViews().
            //AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //});

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddSingleton<ITempDataProvider, CookieTempDataProvider>()
                .AddScoped<IUrlHelper>(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            services
                .AddTransient<ImUser, mUserDAL>()
                .AddTransient<ImOpd, mOpdDAL>()
                .AddTransient<ImTahunAnggaran, mTahunAnggaranDAL>()
                .AddTransient<IMiddlewares, MiddlewareDAL>()
                .AddTransient<ItsIdentifikasi, tsIdentifikasiDAL>()
                .AddTransient<ImProgram, mProgramDAL>()
                .AddTransient<ImKodeRekening, mKodeRekeningDAL>()
                .AddTransient<ImUrusan, mUrusanDAL>()
                .AddTransient<ItsPaket, tsPaketDAL>()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseMiddleware<AuthMiddleware>();           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion")))
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"scripts", @"ej2.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2", @"ej2.min.js"));
                }

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"bootstrap4.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap4.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"material.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"material.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"highcontrast.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"highcontrast.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2-js-es5", @"styles", @"fabric.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"fabric.css"));
                }
            }
            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"vue")))
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"vue")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"vue"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"vue", @"dist", @"vue.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"vue", @"vue.min.js"));
                }
            }
        }
    }
}
