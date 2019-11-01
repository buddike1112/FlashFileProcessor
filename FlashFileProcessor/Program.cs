using FlashFileProcessor.Service;
using FlashFileProcessor.Service.Helpers;
using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Topshelf.Extensions.Hosting;

namespace FlashFileProcessor
{
   class Program
   {
      static void Main(string[] args)
      {
         var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

         var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
               services.AddOptions();

               services.Configure<FilesOptions>(config.GetSection("files"));
               services.Configure<ProfilesOptions>(config.GetSection("profiles"));
               services.AddTransient<IRuleProcessor, RuleProcessor>();
               services.AddTransient<IValidator, Validator>();
               services.AddTransient<IFileHelper, FileHelper>();
               services.AddTransient<IFileProcessorService, FileProcessorService>();
               services.AddHostedService<FileProcessorHostedService>();
               services.AddMvcCore();
            });

         builder.RunAsTopshelfService(hc =>
         {
            hc.SetServiceName("Flash File Processor Service");
            hc.SetDisplayName("Flash File Processor Service");
            hc.SetDescription("This service will run and validate some dataset and generate files");
         });
      }
   }
}
