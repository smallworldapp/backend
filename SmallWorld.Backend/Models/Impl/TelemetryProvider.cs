using System;
using SharpRaven;
using SharpRaven.Data;
using SmallWorld.Models.Abstractions;

namespace SmallWorld.Models.Impl
{
    public class TelemetryProvider : ITelemetryProvider
    {
        private readonly RavenClient raven;

        public TelemetryProvider(RavenClient raven)
        {
            this.raven = raven;
        }

        public void HandleException(Exception x)
        {
            var e = new SentryEvent(x);
            Send(e);
        }

        public void Send(SentryEvent e)
        {
            raven.Capture(e);
        }
    }
}
