using System;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Helpers;
using SmallWorld.Library.Validators;
using Xunit;

namespace SmallWorld.Library.Tests.Validation.Helpers
{
    public class ValidationModelBuilderTest
    {
        public class EmptyServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType) => null;
        }

        public class IneffectiveA
        {
            public IneffectiveB B { get; }
        }

        public class IneffectiveB
        {
            public IneffectiveA A { get; }
        }

        public class WeakEffectiveA
        {
            public WeakEffectiveB B { get; }
        }

        public class WeakEffectiveB
        {
            [Required]
            public WeakEffectiveA A { get; }
        }

        public class StrongEffectiveA
        {
            [Required]
            public StrongEffectiveB B { get; }
        }

        public class StrongEffectiveB
        {
            [Required]
            public StrongEffectiveA A { get; }
        }

        public class WeakEffectiveTriangleA
        {
            public WeakEffectiveTriangleB B { get; }
        }

        public class WeakEffectiveTriangleB
        {
            public WeakEffectiveTriangleC C { get; }
        }

        public class WeakEffectiveTriangleC
        {
            [Required]
            public WeakEffectiveTriangleA A { get; }
        }

        [Fact]
        public void GetModel_Ineffective()
        {
            var builder = new ValidationModelBuilder(new IValidatorProvider[0], new EmptyServiceProvider());
            var modelA = builder.GetModel<IneffectiveA>();
            var modelB = builder.GetModel<IneffectiveB>();

            Assert.False(modelA.IsEffective);
            Assert.False(modelB.IsEffective);
        }

        [Fact]
        public void GetModel_StrongEffective()
        {
            var builder = new ValidationModelBuilder(new IValidatorProvider[0], new EmptyServiceProvider());
            var modelA = builder.GetModel<StrongEffectiveB>();
            var modelB = builder.GetModel<StrongEffectiveB>();

            Assert.True(modelA.IsEffective);
            Assert.True(modelB.IsEffective);
        }

        [Fact]
        public void GetModel_WeakEffective()
        {
            var builder = new ValidationModelBuilder(new IValidatorProvider[0], new EmptyServiceProvider());
            var modelA = builder.GetModel<WeakEffectiveA>();
            var modelB = builder.GetModel<WeakEffectiveB>();

            Assert.True(modelA.IsEffective);
            Assert.True(modelB.IsEffective);
        }

        [Fact]
        public void GetModel_WeakEffectiveTriangle()
        {
            var builder = new ValidationModelBuilder(new IValidatorProvider[0], new EmptyServiceProvider());
            var modelA = builder.GetModel<WeakEffectiveTriangleA>();
            var modelB = builder.GetModel<WeakEffectiveTriangleB>();
            var modelC = builder.GetModel<WeakEffectiveTriangleC>();

            Assert.True(modelA.IsEffective);
            Assert.True(modelB.IsEffective);
            Assert.True(modelC.IsEffective);
        }
    }
}
