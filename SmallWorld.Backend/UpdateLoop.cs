using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace SmallWorld
{
    public class UpdateLoop
    {
        private const int tickRate = 60000;
        private readonly object _lock = new object();

        private readonly IServiceProvider provider;

        private bool isAlive;

        public UpdateLoop(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Start()
        {
            isAlive = true;
            var thread = new Thread(Loop) {
                IsBackground = true,
                Name = "UpdateLoop"
            };
            thread.Start();
        }

        public void Stop()
        {
            isAlive = false;
            lock (_lock)
            {
                Monitor.PulseAll(_lock);
            }
        }

        private void Loop()
        {
            var timer = new Stopwatch();

            while (true)
            {
                timer.Restart();

                Tick();

                timer.Stop();

                var diff = tickRate - timer.ElapsedMilliseconds;

                lock (_lock)
                {
                    if (!isAlive) break;

                    if (diff > 0) Monitor.Wait(_lock, (int)diff);

                    if (!isAlive) break;
                }
            }
        }

        private void Tick()
        {
            using (var scope = provider.CreateScope())
            {
                var scopeProvider = scope.ServiceProvider;

                var runner = ActivatorUtilities.CreateInstance<UpdateRunner>(scopeProvider);

                runner.Run(tickRate).Wait();
            }
        }
    }
}