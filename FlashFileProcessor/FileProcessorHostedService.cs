using FlashFileProcessor.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service
{
   public class FileProcessorHostedService : IHostedService, IDisposable
   {
      private Timer _timer;
      private IFileProcessorService fileProcessorService { get; set; }

      public FileProcessorHostedService(IFileProcessorService fileProcessorInstance)
      {
         fileProcessorService = fileProcessorInstance;
      }      

      public Task StartAsync(CancellationToken cancellationToken)
      {
         _timer = new Timer(
                 async (e) => await fileProcessorService.ProcessFilesAsync(),
                 null,
                 TimeSpan.Zero,
                 TimeSpan.FromSeconds(10));


         return Task.CompletedTask;
      }

      public Task StopAsync(CancellationToken cancellationToken)
      {
         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
