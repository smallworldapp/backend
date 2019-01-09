using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmallWorld.Database;
using SmallWorld.Library;

namespace SmallWorld.Converters.ModelBased
{
    public class ModelType
    {
        public Type Type { get; }
        public Type ObjectType { get; }

        public ModelType(Type modelType)
        {
            Type = modelType;

            var node = modelType;
            while (node.GetTypeInfo().BaseType != typeof(JsonModel))
                node = node.GetTypeInfo().BaseType;

            ObjectType = node.GetGenericArguments()[0];
        }

        public JsonModel Instantiate()
        {
            return (JsonModel)Activator.CreateInstance(Type);
        }

        private int? GetRelation(Type objectType)
        {
            if (!ObjectType.IsAssignableFrom(objectType))
                return null;

            // Prioritize class models over interface models
            if (objectType.GetTypeInfo().IsInterface)
                return 100;

            var node = objectType;
            var score = 0;

            while (node != ObjectType)
            {
                node = node.GetTypeInfo().BaseType;
                score++;
            }

            return score;
        }

        public static ModelType GetForType(Type objectType)
        {
            if (TypeLookup.TryGetValue(objectType, out var modelType))
                return modelType;

            var possible = from type in ModelTypes
                           let score = type.GetRelation(objectType)
                           where score.HasValue
                           orderby score ascending
                           select type;

            modelType = possible.FirstOrDefault();

            TypeLookup.Add(objectType, modelType);

            return modelType;
        }

        private static readonly IDictionary<Type, ModelType> TypeLookup = new ConcurrentDictionary<Type, ModelType>();

        private static readonly List<ModelType> ModelTypes = (from type in typeof(JsonModel).FindTypes()
                                                              select new ModelType(type)).ToList();
    }
}
