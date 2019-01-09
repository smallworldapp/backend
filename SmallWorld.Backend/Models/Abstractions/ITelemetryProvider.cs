using System;
using System.Collections.Generic;
using System.Text;
using SharpRaven.Data;

namespace SmallWorld.Models.Abstractions
{
    public interface ITelemetryProvider
    {
        void HandleException(Exception x);
        void Send(SentryEvent e);
    }
}
