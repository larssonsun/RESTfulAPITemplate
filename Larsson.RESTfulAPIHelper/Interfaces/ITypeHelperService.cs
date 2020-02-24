namespace Larsson.RESTfulAPIHelper.Interface
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}