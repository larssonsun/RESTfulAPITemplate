using System.Collections.Generic;
using RESTfulAPISample.Core.Interface;

namespace RESTfulAPISample.Core
{
    public class RESTfulAPIHelperOptions
    {
        public IList<IPropertyMapping> PropertyMappings { get; } = new List<IPropertyMapping>();
        public void Register<T>() where T : IPropertyMapping, new()
        {
            PropertyMappings.Add(new T());
        }
    }
}