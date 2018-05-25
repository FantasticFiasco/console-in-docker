using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleInDocker
{
    /// <summary>
    /// Class capable of blocking a console application in a Docker container.
    /// </summary>
    public static class Wait
    {
        /// <summary>
        /// Block the calling thread until shutdown is triggered via Ctrl+C or SIGTERM.
        /// </summary>
        public static void ForShutdown()
        {
            ForShutdownAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns a <see cref="Task"/> that completes when shutdown is triggered via the given token, Ctrl+C or
        /// SIGTERM.
        /// </summary>
        /// <param name="token">The token to trigger shutdown.</param>
        public static Task ForShutdownAsync(CancellationToken token = default(CancellationToken))
        {
            var done = new ManualResetEventSlim(false);
            
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                AttachCtrlcSigtermShutdown(cts, done);

                done.Set();
            }
        }

        private static void AttachCtrlcSigtermShutdown(
            CancellationTokenSource cts,
            ManualResetEventSlim resetEvent)
        {
            void Shutdown()
            {
                if (!cts.IsCancellationRequested)
                {
                    try
                    {
                        cts.Cancel();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }

                // Wait on the given reset event
                resetEvent.Wait();
            }

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => Shutdown();
            
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Shutdown();
                
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
        }
    }
}
