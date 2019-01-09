namespace SmallWorld.Database.Model
{
//    public sealed class MyTypeMapper : SqliteTypeMapper
//    {
//        public MyTypeMapper(RelationalTypeMapperDependencies dependencies) : base(dependencies)
//        {
//            var mappings = GetClrTypeMappings() as Dictionary<Type, RelationalTypeMapping>;
//
//            if (mappings == null)
//                throw new Exception();
//
//            var types = typeof(StringType<>).FindTypes();
//
//            foreach (var type in types)
//            {
//                if (type.BaseType.GetGenericTypeDefinition() == typeof(StringType<>))
//                    mappings[type.AsType()] = new MyStringTypeMapping();
//
//                else
//                    Debugger.Break();
//            }
//        }
//
//        public override RelationalTypeMapping FindMapping(IProperty property)
//        {
//            var x = base.FindMapping(property);
//            return x;
//        }
//
//        protected override RelationalTypeMapping FindCustomMapping(IProperty property)
//        {
//            var x = base.FindCustomMapping(property);
//            return x;
//        }
//    }
//
//    public class MyStringTypeMapping : RelationalTypeMapping
//    {
//        public MyStringTypeMapping() : base("TEXT", typeof(StringType)) { }
//
//        public override RelationalTypeMapping Clone(string storeType, int? size)
//        {
//            throw new NotImplementedException();
//        }
//    }
}
