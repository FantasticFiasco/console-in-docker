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
            var done = new ManualResetEventSlim(false);
            
            void Shutdown()
            {
                done.Set();
            };

            AppDomain.CurrentDomain.ProcessExit += (sender, e) => Shutdown();
            
            Console.CancelKeyPress += (sender, e) =>
            {
                Shutdown();
                
                // Don't terminate the process immediately, wait for the main thread to exit gracefully
                e.Cancel = true;
            };

            done.Wait();
        }
    }
}
