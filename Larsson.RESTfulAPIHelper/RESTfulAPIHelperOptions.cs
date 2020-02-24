using System.Collections.Generic;
using Larsson.RESTfulAPIHelper.Interface;

namespace Larsson.RESTfulAPIHelper
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