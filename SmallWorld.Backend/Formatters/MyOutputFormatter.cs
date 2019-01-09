using System;
using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace SmallWorld.Formatters
{
    public class MyOutputFormatter : TextOutputFormatter
    {
        public MyOutputFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(FormatterUtils.ApplicationJson);
            SupportedMediaTypes.Add(FormatterUtils.TextJson);
            SupportedMediaTypes.Add(FormatterUtils.ApplicationAnyJsonSyntax);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var settings = FormatterUtils.GetSettings(context.HttpContext.RequestServices);

            var formatter = new JsonOutputFormatter(settings, ArrayPool<char>.Shared);

#if DEBUG
            var timer = new Stopwatch();
            timer.Start();
#endif
            await formatter.WriteResponseBodyAsync(context, selectedEncoding);
#if DEBUG
            timer.Stop();
            Console.WriteLine($"Write response: {timer.Elapsed.TotalMilliseconds}");
#endif
        }
    }
}
