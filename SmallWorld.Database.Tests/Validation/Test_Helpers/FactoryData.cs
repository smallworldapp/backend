using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace SmallWorld.Database.Tests.Validation.Test_Helpers
{
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public class FactoryDataAttribute : DataAttribute
    {
        public Type FactoryType { get; }
        public bool Valid { get; }

        public FactoryDataAttribute(Type factoryType, bool valid)
        {
            FactoryType = factoryType;
            Valid = valid;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var factory = (ISource)Activator.CreateInstance(FactoryType);
            var src = Valid ? factory.Valid() : factory.Invalid();

            var i = 0;
            foreach (var value in src)
            {
                i++;
                yield return new[] { value };
            }
        }
    }
}
