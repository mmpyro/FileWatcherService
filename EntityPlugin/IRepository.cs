namespace EntityPlugin
{
    public interface IRepository<in T> where T : class
    {
        void Add(T item);
        void Delete(T item);
        void Update(T item);
    }
}