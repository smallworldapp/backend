using System.IO;
using Newtonsoft.Json;
using SmallWorld.Library.CustomTypes;
using Xunit;

namespace SmallWorld.Library.Tests.CustomTypes
{
    public class StringTypeConverterTest : TestBase
    {
        private static JsonSerializer CreateSerializer()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringTypeConverter());

            return JsonSerializer.Create(settings);
        }

        [Fact]
        public void CanConvert_StringType()
        {
            var converter = new StringTypeConverter();

            Assert.True(converter.CanConvert(typeof(StringType)));
        }

        [Fact]
        public void CanConvert_NonStringType()
        {
            var converter = new StringTypeConverter();

            Assert.False(converter.CanConvert(typeof(int)));
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(object)));
        }

        [Fact]
        public void WriteJson()
        {
            var serializer = CreateSerializer();
            var contents = "this_is_a_test_string";
            var json = $"\"{contents}\"";

            using (var str = new StringWriter())
            using (var writer = new JsonTextWriter(str))
            {
                serializer.Serialize(writer, new FakeStringType(contents));

                Assert.Equal(json, str.ToString());
            }
        }

        [Fact]
        public void ReadJson()
        {
            var serializer = CreateSerializer();
            var contents = "this_is_a_test_string";
            var json = $"\"{contents}\"";

            using (var str = new StringReader(json))
            using (var reader = new JsonTextReader(str))
            {
                var value = serializer.Deserialize<FakeStringType>(reader);

                Assert.Equal(contents, value.Value);
            }
        }
    }
}
