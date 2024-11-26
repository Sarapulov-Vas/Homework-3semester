namespace MyNUnitWeb.Pages;

using MyNUnitWeb.Data;

[BindProperties]
public class TestRuns(TestsDBContext context) : PageModel
{
    public IList<TestRun> Launches { get; private set; } = [];

    public void OnGet()
    {
        Launches = context.TestRuns.OrderBy(run => run.Id).ToList();
    }

    public async Task<IActionResult> OnPostDelete(int testRunId)
    {
        var testRun = context.TestRuns.Find(testRunId);
        var testsToDelete = context.Tests
            .Where(test => test.TestRunId == testRun.Id)
            .ToList();
        context.Tests.RemoveRange(testsToDelete);
        context.TestRuns.Remove(testRun);
        await context.SaveChangesAsync();
        Launches = context.TestRuns.OrderBy(run => run.Id).ToList();
        return Page();
    }
}
