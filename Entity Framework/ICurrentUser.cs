namespace Core
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        string FullName { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}