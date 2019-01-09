using System;
using Newtonsoft.Json;
using SmallWorld.Library.CustomTypes;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public abstract class BaseEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public Guid Guid { get; set; }

        public void CreateIds()
        {
            Guid = Token.Generate();
            Id = 0;
        }


        protected static Name WrapName(string str) => str == null ? null : new Name(str);
        protected static Identifier WrapIdentifier(string str) => str == null ? null : new Identifier(str);
        protected static EmailAddress WrapEmailAddress(string str) => str == null ? null : new EmailAddress(str);

        protected static string Unwrap<T>(T t) where T : StringType => t?.Value;
    }
}