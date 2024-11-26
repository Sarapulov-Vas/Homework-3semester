namespace MyNUnitWeb.Data;

using Microsoft.EntityFrameworkCore;

public class TestsDBContext : DbContext
{
    public TestsDBContext(
        DbContextOptions<TestsDBContext> options)
        : base(options)
    {
    }

    public DbSet<TestRun> TestRuns => Set<TestRun>();
    public DbSet<TestModel> Tests => Set<TestModel>();
}
