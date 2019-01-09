using System;
using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;

namespace SmallWorld.Formatters
{
    public class MyInputFormatter : TextInputFormatter
    {
        public MyInputFormatter()
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add(FormatterUtils.ApplicationJson);
            SupportedMediaTypes.Add(FormatterUtils.TextJson);
            SupportedMediaTypes.Add(FormatterUtils.ApplicationAnyJsonSyntax);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var settings = FormatterUtils.GetSettings(context.HttpContext.RequestServices);
            var logger = context.HttpContext.RequestServices.GetService<ILogger<JsonInputFormatter>>();

#if DEBUG
            var timer = new Stopwatch();
            timer.Start();
#endif
            var formatter = new JsonInputFormatter(logger, settings, ArrayPool<char>.Shared, new DefaultObjectPoolProvider());

            var result = await formatter.ReadRequestBodyAsync(context, encoding);
#if DEBUG
            timer.Stop();
            Console.WriteLine($"Read body: {timer.Elapsed.TotalMilliseconds}");
#endif

            return result;
        }
    }
}
