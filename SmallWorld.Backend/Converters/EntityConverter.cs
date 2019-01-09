using System;
using SmallWorld.Converters.ModelBased;
using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters
{
    public abstract class EntityConverter<T> : JsonModel<T> where T : BaseEntity, new()
    {
        public Guid id
        {
            get => Value.Guid;
            set => Value.Guid = value;
        }
    }
}
