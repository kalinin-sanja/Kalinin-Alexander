using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace PhonebookApi.Mappers
{
    public interface IAutoMapper
    {
        T2 Map<T1, T2>(T1 inEntry, T2 outEntry = null, string methodName = "Map") where T1 : new() where T2 : class, new();
        T2 MapFull<T1, T2>(T1 inEntry, T2 outEntry = null, string methodName = "Map") where T1 : new() where T2 : class, new();
        T2 MapEntry<T1, T2>(T1 inEntry, T2 outEntry, IDictionary<PropertyInfo, PropertyInfo> properties,
            string methodName = "Map");
        IEnumerable<PropertyInfo> GetProporties(Type type, bool withEnumerables);
        bool IsGenericEnumerable(Type type);

        IDictionary<PropertyInfo, PropertyInfo> MapProperty(
            ICollection<PropertyInfo> property1,
            ICollection<PropertyInfo> property2);

        T2 MapAfterEditing<T1, T2>(T1 inEntry, T2 outEntry, string methodName = "Map");
    }

    public class AutoMapper : IAutoMapper
    {
        public AutoMapper(IMapperUoW mapperUoW)
        {
            MapperUoW = mapperUoW;
        }

        protected IMapperUoW MapperUoW { get; }

        public T2 Map<T1, T2>(T1 inEntry, T2 outEntry = null, string methodName = "Map") where T1 : new() where T2 : class, new()
        {
            var inProperties = GetProporties(typeof(T1), false).ToList();
            var outProperties = GetProporties(typeof(T2), false).ToList();
            var properties = MapProperty(inProperties, outProperties);
            outEntry = outEntry ?? new T2();
            return MapEntry(inEntry, outEntry, properties, methodName);
        }

        public T2 MapFull<T1, T2>(T1 inEntry, T2 outEntry = null, string methodName = "Map") where T1 : new() where T2 : class, new()
        {
            outEntry = Map(inEntry, outEntry, methodName);
            var inProperties = GetProporties(typeof(T1), true).ToList();
            var outProperties = GetProporties(typeof(T2), true).ToList();
            var properties = MapProperty(inProperties, outProperties);
            return MapEntry(inEntry, outEntry, properties, methodName);
        }

        public T2 MapEntry<T1, T2>(T1 inEntry, T2 outEntry, IDictionary<PropertyInfo, PropertyInfo> properties, string methodName = "Map")
        {
            foreach (var pair in properties)
            {
                var inProp = pair.Key;
                var outProp = pair.Value;

                object value = null;
                if (inProp.PropertyType == outProp.PropertyType)
                    value = inProp.GetMethod.Invoke(inEntry, null);
                else if (!IsGenericEnumerable(inProp.PropertyType) || !IsGenericEnumerable(outProp.PropertyType))
                {
                    var inType = inProp.PropertyType;
                    var outType = outProp.PropertyType;

                    var method = typeof(IMapperUoW)
                        .GetMethod("GetFullMapper")
                        .MakeGenericMethod(inType, outType);
                    if (methodName.Contains("Inverse"))
                    {
                        method = typeof(IMapperUoW)
                            .GetMethod("GetFullMapper")
                            .MakeGenericMethod(outType, inType);
                    }

                    var mapper = method.Invoke(MapperUoW, null);
                    var mapMethod = mapper.GetType().GetMethod(methodName);

                    var inValue = inProp.GetMethod.Invoke(inEntry, null);
                    value = inValue == null ? null : mapMethod.Invoke(mapper, new[] { inValue });
                }
                else
                {
                    var inType = inProp.PropertyType.GenericTypeArguments[0];
                    var outType = outProp.PropertyType.GenericTypeArguments[0];

                    var method = typeof(IMapperUoW)
                        .GetMethod("GetFullMapper")
                        .MakeGenericMethod(inType, outType);
                    if (methodName.Contains("Inverse"))
                    {
                        method = typeof(IMapperUoW)
                            .GetMethod("GetFullMapper")
                            .MakeGenericMethod(outType, inType);
                    }

                    var mapper = method.Invoke(MapperUoW, null);
                    var mapMethod = mapper.GetType().GetMethod(methodName);

                    var inRange = (IEnumerable)inProp.GetMethod.Invoke(inEntry, null);

                    var outCollectionType = typeof(Collection<>).MakeGenericType(outType);
                    var addMethod = outCollectionType.GetMethod("Add");
                    var outCollection = Activator.CreateInstance(outCollectionType);

                    foreach (var inItem in inRange)
                    {
                        var outItem = mapMethod.Invoke(mapper, new[] { inItem });
                        addMethod.Invoke(outCollection, new[] { outItem });
                    }

                    value = outCollection;
                }

                outProp.SetMethod.Invoke(outEntry, new[] { value });
            }
            return outEntry;
        }

        public IEnumerable<PropertyInfo> GetProporties(Type type, bool withEnumerables)
        {
            var props = type.GetProperties().Where(x => x.CanRead && x.CanWrite);
            props = withEnumerables
                ? props.Where(x => IsGenericEnumerable(x.PropertyType))
                : props.Where(x => !IsGenericEnumerable(x.PropertyType));
            return props;
        }

        public bool IsGenericEnumerable(Type type)
        {
            return type.IsGenericType && typeof(IEnumerable<object>).IsAssignableFrom(type);
        }

        public IDictionary<PropertyInfo, PropertyInfo> MapProperty(
            ICollection<PropertyInfo> property1,
            ICollection<PropertyInfo> property2)
        {
            return property1
                .Select(x =>
                {
                    var prop = property2.FirstOrDefault(p => p.Name == x.Name);
                    return Tuple.Create(x, prop);
                })
                .Where(x => x.Item2 != null)
                .ToDictionary(x => x.Item1, x => x.Item2);
        }

        public T2 MapAfterEditing<T1, T2>(T1 inEntry, T2 outEntry, string methodName = "Map")
        {
            var inProperties = GetProporties(typeof(T1), false)
                .Concat(GetProporties(typeof(T1), true))
                .ToList();
            var outProperties = GetProporties(typeof(T2), false)
                .Concat(GetProporties(typeof(T2), true))
                .ToList();
            //var includeProps = new[]
            //{
            //    nameof(IIdentityBase.Deleted),
            //    nameof(IIdentityBase.CreatingTime),
            //    nameof(IIdentityBase.LastEditingTime)
            //};
            var properties = MapProperty(inProperties, outProperties)
                .Where(x => x.Key.PropertyType != x.Value.PropertyType/* || includeProps.Contains(x.Key.Name)*/)
                .ToDictionary(x => x.Key, x => x.Value);
            return MapEntry(inEntry, outEntry, properties, methodName);
        }
    }
}