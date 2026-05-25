public interface IPool
{
    bool IsFull { get; }
    int ActiveCount { get; }
    void ReturnAll();
}