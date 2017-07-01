using System.Linq;
using PhonebookApi.Models;

namespace PhonebookApi.Mappers
{
    public class AutoDbMapper
    {
        public T MapForDb<T>(T fromEntry, T inEntry) where T : IIdentified
        {
            var baseProperties = typeof(IIdentified).GetProperties().Select(x => x.Name).ToList();
            var properties = typeof(T).GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .Where(x => !baseProperties.Contains(x.Name));

            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetMethod.Invoke(fromEntry, null);
                if (value != null)
                    propertyInfo.SetMethod.Invoke(inEntry, new[] { value });
            }

            return inEntry;
        }
        public T Map<T>(T fromEntry, T inEntry)
        {
            var properties = typeof(T).GetProperties()
                .Where(x => x.CanRead && x.CanWrite);

            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetMethod.Invoke(fromEntry, null);
                propertyInfo.SetMethod.Invoke(inEntry, new[] { value });
            }

            return inEntry;
        }
    }
}