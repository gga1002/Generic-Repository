namespace GenericRepository
{
    public interface IUnitOfWork
    {
        void Complete();
        Repository<T> Repository<T>() where T : class;
    }
}