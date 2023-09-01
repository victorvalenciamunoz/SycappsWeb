using Microsoft.EntityFrameworkCore;

namespace SycappsWeb.Server.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    public ITrekiRepository TrekiRepository { get; private set; }
    public IActivityRepository ActivityRepository { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
        TrekiRepository = new TrekiRepository(context);
        ActivityRepository = new ActivityRepository(context);
    }

    public void Commit() => context.SaveChanges();

    public async Task CommitAsync() => await context.SaveChangesAsync();

    public void Rollback() => context.Dispose();

    public async Task RollbackAsync() => await context.DisposeAsync();

    public void Dispose()
    {
        context.Dispose();
    }
}
