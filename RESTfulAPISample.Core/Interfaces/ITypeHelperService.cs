namespace RESTfulAPISample.Core.Interface
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}