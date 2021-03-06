﻿using FlashFileProcessor.Service;
using FlashFileProcessor.Service.Helpers;
using FlashFileProcessor.Service.Interfaces;
using FlashFileProcessor.Service.Options;
using FlashFileProcessor.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Topshelf.Extensions.Hosting;
using Topshelf.Runtime;

namespace FlashFileProcessor
{
   internal class Program
   {
      /// <summary>Defines the entry point of the application.</summary>
      /// <param name="args">The arguments.</param>
      private static void Main(string[] args)
      {
         //Acquiring the environment
         var hostingEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
         string settingsFile = string.Empty;

         if (hostingEnvironment.Equals(Environments.Production))
         {
            settingsFile = $"appsettings.prod.json";
         }
         else if (hostingEnvironment.Equals(Environments.Staging))
         {
            settingsFile = $"appsettings.stage.json";
         }
         else
         {
            settingsFile = $"appsettings.json";
         }

         // Use appsettings json file from root to read configurations
         var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: true)
                .Build();

         // Inject dependencies we are using withing the hosted service
         var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
               services.AddOptions();
               services.AddLogging(logging =>
               {
                  logging.ClearProviders();
                  logging.AddConsole();
               });

               services.Configure<CustomersOptions>(options => config.GetSection("customers").Bind(options));
               services.Configure<ProfilesOptions>(config.GetSection("profiles"));
               services.AddTransient<IRuleProcessor, RuleProcessor>();
               services.AddTransient<IValidator, Validator>();
               services.AddTransient<IFileHelper, FileHelper>();
               services.AddTransient<IFileProcessorService, FileProcessorService>();
               services.AddHostedService<FileProcessorHostedService>();
               services.AddMvcCore();
            });

         // Pump service details to the service using Topshelf configurations
         builder.RunAsTopshelfService(hc =>
         {
            hc.SetServiceName("Flash File Processor Service");
            hc.SetDisplayName("Flash File Processor Service");
            hc.SetDescription("This service will run and validate some dataset and generate files");
         });
      }
   }
}