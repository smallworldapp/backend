using SmallWorld.Library.Model;
using Xunit;

namespace SmallWorld.Library.Tests.Model
{
    public class PropertyPathTest : TestBase
    {
        private class Root
        {
            public Branch Branch { get; set; }
        }

        private class Branch
        {
            public object Value { get; set; }
        }

        [Fact]
        public void Parse()
        {
            var path1 = PropertyPath<Root>.Parse("Branch.Value");
            var path2 = PropertyPath<Root>.Create(r => r.Branch.Value);

            Assert.Equal(path1.Properties, path2.Properties);
            Assert.Equal(path1.PropertyType, path2.PropertyType);

            var prop1 = typeof(Root).GetProperty(nameof(Root.Branch));
            var prop2 = typeof(Branch).GetProperty(nameof(Branch.Value));

            Assert.Equal(path1.Properties, new[] { prop1, prop2 });
            Assert.Equal(typeof(object), path1.PropertyType);
        }

        [Fact]
        public void Test_ToString()
        {
            var path = PropertyPath<Root>.Create(r => r.Branch.Value);

            Assert.Equal("Object Branch.Value", path.ToString());
        }

        [Fact]
        public void TrySetValue_NullBranch()
        {
            var path = PropertyPath<Root>.Create(r => r.Branch.Value);
            var root = new Root();

            Assert.False(path.TrySetValue(root, new object()));
        }

        [Fact]
        public void TrySetValue_Branch()
        {
            var path = PropertyPath<Root>.Create(r => r.Branch.Value);
            var root = new Root {
                Branch = new Branch()
            };

            Assert.Null(root.Branch.Value);
            Assert.True(path.TrySetValue(root, new object()));
            Assert.NotNull(root.Branch.Value);
        }
    }
}
