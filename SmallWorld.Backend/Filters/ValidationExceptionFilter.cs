using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmallWorld.Library.Validation;

namespace SmallWorld.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var e = context.Exception as ValidationException;
            if (e == null) return;

            Console.WriteLine("Validation Exception:" + e);

            context.Result = new BadRequestResult();
            context.ExceptionHandled = true;
        }
    }
}
