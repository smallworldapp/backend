using System;
using System.Runtime.CompilerServices;
using SmallWorld.Database.Entities;
using Xunit;

namespace SmallWorld.Database.Tests
{
    public static class TestExtensions
    {
        public static T Init<T>(this T entity, [CallerFilePath] string src = null, [CallerLineNumber] int line = -1) where T : BaseEntity
        {
            Assert.NotNull(src);
            Assert.NotEqual(line, -1);

            var random = new Random(src.GetHashCode() + line);
            var contents = new byte[16];

            random.NextBytes(contents);
            entity.Guid = new Guid(contents);
            entity.Id = 0;

            return entity;
        }
    }
}
