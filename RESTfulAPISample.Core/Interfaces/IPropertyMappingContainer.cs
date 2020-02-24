namespace RESTfulAPISample.Core.Interface
{
    public interface IPropertyMappingContainer
    {
        IPropertyMapping Resolve<TSource, TDestination>() where TDestination : IEntity;
        bool ValidMappingExistsFor<TSource, TDestination>(string fields) where TDestination : IEntity;
    }
}