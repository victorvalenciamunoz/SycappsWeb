namespace SycappsWeb.Server.Data;

public interface IUnitOfWork : IDisposable
{
    ITrekiRepository TrekiRepository { get; }
    IActivityRepository ActivityRepository { get; }

    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}