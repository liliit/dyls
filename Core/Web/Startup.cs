using DYLS.Common.Middleware;
using DYLS.Common.Utils;
using DYLS.IDal;
using DYLS.IDal.Wx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DYLS.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            HttpContextHelper.Env = env;
            app.UseMiddleware<InterceptTencentSafetyTeam>();
            app.UseMiddleware<AutoHttps>();
            app.UseMiddleware<PageCache>();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            DalFactory.GetInstance<IDalWxUser>().Insert(new Model.Db.Wx.WxUser {
                OpenID = "xxxxxx",
                NickName = "",
                Sex = 1,
                Province="",
                City="",
                Country="",
                HeadImgUrl="",
                UnionID=""
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseDirectoryBrowser();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{cid?}");
            });
            /*app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });*/
        }
    }
}
