using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpRaven.Data;
using SmallWorld.Auth;
using SmallWorld.Library.Validation;
using SmallWorld.Models.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Filters
{
    public class TelemetryExceptionFilter : IExceptionFilter
    {
        private readonly ITelemetryProvider telemetry;
        private readonly IServiceProvider services;
        private readonly AuthProvider authProvider;

        public TelemetryExceptionFilter(ITelemetryProvider telemetry, AuthProvider authProvider, IServiceProvider services)
        {
            this.telemetry = telemetry;
            this.authProvider = authProvider;
            this.services = services;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
                return;

            var auth = authProvider.GetSession(context.HttpContext);

            var actionName = context.ActionDescriptor.DisplayName;
            if (context.ActionDescriptor is ControllerActionDescriptor action)
                actionName = action.ControllerName + "." + action.MethodInfo.Name;

            var sessionName = "Unknown";
            switch (auth)
            {
                case null:
                    sessionName = "None";
                    break;
                case AccountSession session:
                    var account = session.GetAccount(services);
                    sessionName = "Account: " + account.Email;
                    break;
                case AdminSession _:
                    sessionName = "Admin";
                    break;
            }

            JToken requestBody = "";
            if (context.HttpContext.Request.Body.CanSeek)
            {
                context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                using (var tmp = new MemoryStream())
                {
                    context.HttpContext.Request.Body.CopyTo(tmp);
                    if (tmp.TryGetBuffer(out var buffer))
                    {
                        var rawRequest = new UTF8Encoding().GetString(buffer.Array, buffer.Offset, buffer.Count);

                        try
                        {
                            var json = JsonConvert.DeserializeObject(rawRequest);
                            requestBody = (JToken) json;
                        }
                        catch
                        {
                            requestBody = rawRequest;
                        }
                    }
                }
            }

            var error = new JObject {
                ["action"] = actionName,
                ["session"] = sessionName,
                ["request"] = new JObject {
                    ["method"] = context.HttpContext.Request.Method,
                    ["path"] = context.HttpContext.Request.Path.ToString(),
                    ["query"] = context.HttpContext.Request.QueryString.ToString(),
                    ["body"] = requestBody,
                },
                ["exception"] = context.Exception.Message,
            };

            var msg = error.ToString(Formatting.Indented);

            var e = new SentryEvent(context.Exception) {
                Message = new SentryMessage(msg)
            };

            telemetry?.Send(e);
        }
    }
}
