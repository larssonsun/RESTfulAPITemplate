namespace RESTfulAPITemplate.Core.Interface
{
    public interface IScetiaIndentityUtil
    {
        bool ValidatePassword(string hashedPasswordFromDb, string saltFromDb, string providedPassword);
    }
}