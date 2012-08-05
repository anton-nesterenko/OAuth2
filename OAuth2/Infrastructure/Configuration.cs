using System;
using System.Linq;

namespace OAuth2.Infrastructure
{
    /// <summary>
    /// Base class for configuration implementation based on key-value storage.
    /// </summary>
    public abstract class Configuration : IConfiguration
    {
        /// <summary>
        /// Returns configuration section by name.
        /// </summary>
        /// <param name="name">The section name.</param>
        /// <param name="allowInheritance">Allows read values from parent section if true.</param>
        public abstract IConfiguration GetSection(string name, bool allowInheritance = true);

        /// <summary>
        /// Returns configuration section for given type (uses type name as section name).
        /// </summary>
        /// <param name="allowInheritance">Allows read values from parent section if true.</param>
        public IConfiguration GetSection<T>(bool allowInheritance = true)
        {
            return GetSection(typeof (T), allowInheritance);
        }

        /// <summary>
        /// Returns configuration section for given type (uses type name as section name).
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="allowInheritance">Allows read values from parent section if true.</param>
        /// <returns></returns>
        public IConfiguration GetSection(Type type, bool allowInheritance = true)
        {
            return GetSection(type.Name, allowInheritance);
        }

        /// <summary>
        /// Returns value by key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string Get(string key);

        /// <summary>
        /// Returns instance with properties initialized from configuration values.
        /// </summary>
        public T Get<T>()
        {
            var instance = Activator.CreateInstance<T>();
            typeof (T).GetProperties()
                .Where(x => x.CanWrite)
                .ForEach(x => x.SetValue(instance, Get(x.Name, x.PropertyType), null));
            return instance;
        }

        /// <summary>
        /// Returns strongly typed value by key.
        /// </summary>
        public T Get<T>(string key)
        {
            return (T) Get(key, typeof (T));
        }

        private object Get(string key, Type valueType)
        {
            return Convert.ChangeType(Get(key), valueType);
        }
    }
}