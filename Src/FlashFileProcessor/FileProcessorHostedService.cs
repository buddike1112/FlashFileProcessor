using FlashFileProcessor.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlashFileProcessor.Service
{
   /// <summary>
   /// File Processor Hosted Service class
   /// </summary>
   /// <seealso cref="Microsoft.Extensions.Hosting.IHostedService" />
   /// <seealso cref="System.IDisposable" />
   public class FileProcessorHostedService : IHostedService, IDisposable
   {
      /// <summary>
      /// The timer object
      /// </summary>
      private Timer _timer;

      /// <summary>
      /// Gets or sets the file processor service.
      /// </summary>
      /// <value>
      /// The file processor service.
      /// </value>
      private IFileProcessorService fileProcessorService { get; set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="FileProcessorHostedService"/> class.
      /// </summary>
      /// <param name="fileProcessorInstance">The file processor instance.</param>
      public FileProcessorHostedService(IFileProcessorService fileProcessorInstance)
      {
         fileProcessorService = fileProcessorInstance;
      }

      /// <summary>
      /// Triggered when the application host is ready to start the service.
      /// </summary>
      /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
      /// <returns></returns>
      public Task StartAsync(CancellationToken cancellationToken)
      {
         _timer = new Timer(
                 async (e) => await fileProcessorService.ProcessFilesAsync(),
                 null,
                 TimeSpan.Zero,
                 TimeSpan.FromSeconds(10));


         return Task.CompletedTask;
      }

      /// <summary>
      /// Triggered when the application host is performing a graceful shutdown.
      /// </summary>
      /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
      /// <returns></returns>
      public Task StopAsync(CancellationToken cancellationToken)
      {
         _timer?.Change(Timeout.Infinite, 0);

         return Task.CompletedTask;
      }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         _timer?.Dispose();
      }
   }
}
