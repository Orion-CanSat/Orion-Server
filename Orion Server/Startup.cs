using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionServer
{
    public class Startup
    {
        /// <summary>
        /// This function will create and load all WWWData files
        /// </summary>
        public static void CreateUpdateWWWData()
        {
            // Checks if wwwdata folder exists and if it does not it creates it.
            {
                try
                {
                    if (!Directory.Exists(Constants.WWWDataFolder))
                        Directory.CreateDirectory(Constants.WWWDataFolder);
                }
                catch (UnauthorizedAccessException e)
                {
                    Utilities.ExceptionConsoleWriter<UnauthorizedAccessException>
                        .ShowException(e, "Orion Server does not have the right permission to create a wwwdata folder.", true, 1);
                }
                catch (Exception e)
                {
                    Utilities.ExceptionConsoleWriter<Exception>
                        .ShowException(e, "Orion Server encountered a fatal exception while trying to create wwwdata folder.", true, 1);
                }
            }

            // Checks if modules folder exists and if it does not it creates it.
            {
                try
                {
                    if (!Directory.Exists(Constants.ModulesFolder))
                        Directory.CreateDirectory(Constants.ModulesFolder);
                }
                catch (UnauthorizedAccessException e)
                {
                    Utilities.ExceptionConsoleWriter<UnauthorizedAccessException>
                        .ShowException(e, "Orion Server does not have the right permission to create a modules folder.", true, 1);
                }
                catch (Exception e)
                {
                    Utilities.ExceptionConsoleWriter<Exception>
                        .ShowException(e, "Orion Server encountered a fatal exception while trying to create modules folder.", true, 1);
                }
            }

            // Checks if data folder exists and if it does not it creates it.
            {
                try
                {
                    if (!Directory.Exists(Constants.DataFolder))
                        Directory.CreateDirectory(Constants.DataFolder);
                    else
                    {
                        for (int i = 0; ; i++)
                        {
                            if (!Directory.Exists($"{Constants.DataFolder}/old{i}"))
                            {
                                String[] files = Directory.GetFiles(Constants.DataFolder);
                                Directory.CreateDirectory($"{Constants.DataFolder}/old{i}");
                                foreach (string file in files)
                                    Directory.Move(file, $"{Constants.DataFolder}/old{i}/{Path.GetFileName(file)}");
                                break;
                            }
                        }
                    }    
                }
                catch (UnauthorizedAccessException e)
                {
                    Utilities.ExceptionConsoleWriter<UnauthorizedAccessException>
                        .ShowException(e, "Orion Server does not have the right permission to create a wwwdata folder.", true, 1);
                }
                catch (Exception e)
                {
                    Utilities.ExceptionConsoleWriter<Exception>
                        .ShowException(e, "Orion Server encountered a fatal exception while trying to create wwwdata folder.", true, 1);
                }
            }

            // Checks if previous authentication keys are still present and deletes them.
            // Then if creates new ones.
            {
                try
                {
                    if (File.Exists(Constants.AuthenticationKeysFile))
                        File.Delete(Constants.AuthenticationKeysFile);
                    string authorizedKey = Utilities.StringUtilities.GenerateRandomString(28);
                    File.WriteAllLines(
                        Constants.AuthenticationKeysFile,
                        new string[]
                        {
                            "{",
                            "\t\"keys\": [",
                            "\t\t\"" + authorizedKey + "\"",
                            "\t]",
                            "}"
                        },
                        Encoding.UTF8
                    );

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Authorized Key]: ");
                    Console.ResetColor();
                    Console.WriteLine(authorizedKey);
                }
                catch (DirectoryNotFoundException e)
                {
                    Utilities.ExceptionConsoleWriter<DirectoryNotFoundException>
                        .ShowException(e, "Orion Server can not locate wwwdata folder", true, 1);
                }
                catch (UnauthorizedAccessException e)
                {
                    Utilities.ExceptionConsoleWriter<UnauthorizedAccessException>
                        .ShowException(e, "Orion Server does not have the right permission to create a delete/write the authentication keys.", true, 1);
                }
                catch (IOException e)
                {
                    Utilities.ExceptionConsoleWriter<IOException>
                        .ShowException(e, "Orion Server does can not delete previous authentication files because they are still used by an other proccess.", true, 1);
                }
                catch (Exception e)
                {
                    Utilities.ExceptionConsoleWriter<Exception>
                        .ShowException(e, "Orion Server encountered a fatal exception while trying to create authentication keys.", true, 1);
                }
            }

            // Load Authorized keys
            Utilities.Authenticator.LoadAuthenticationKeys();
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/Home/Error";
                        await next();
                    }
                } catch { }
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller}/{dt?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
