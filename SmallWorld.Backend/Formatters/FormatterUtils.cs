using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmallWorld.Formatters
{
    public class FormatterUtils
    {
        public static readonly MediaTypeHeaderValue ApplicationJson
            = MediaTypeHeaderValue.Parse("application/json").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue TextJson
            = MediaTypeHeaderValue.Parse("text/json").CopyAsReadOnly();

        public static readonly MediaTypeHeaderValue ApplicationAnyJsonSyntax
            = MediaTypeHeaderValue.Parse("application/*+json").CopyAsReadOnly();

        public static JsonSerializerSettings GetSettings(IServiceProvider provider)
        {
            var settings = new JsonSerializerSettings {
                MaxDepth = 32,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            settings.Converters.Add(new StringEnumConverter(true));

            var converters = provider.GetServices<JsonConverter>();
            foreach (var converter in converters)
                settings.Converters.Add(converter);

            return settings;
        }
    }
}
