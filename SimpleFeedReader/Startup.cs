using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleFeedReader.Services;

namespace SimpleFeedReader
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<NewsService>();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
              
            }

            // Set security headers
            app.UseXContentTypeOptions();
            app.UseXfo(options => options.SameOrigin());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseReferrerPolicy(options => options.StrictOrigin());
            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());

            app.UseCsp(options => options
                .BlockAllMixedContent()
                .StyleSources(s => s.Self().UnsafeInline())
                .StyleSources(s => s.UnsafeInline())
                .FontSources(s => s.Self())
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ImageSources(s => s.Self())
                .ScriptSources(s => s.UnsafeInline())
            );
            
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
