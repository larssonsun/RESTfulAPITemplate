namespace RESTfulAPITemplate.Core.Entity
{
    public abstract class BaseEntity<T>
    {
        public virtual T Id { get; protected set; }
    }
}