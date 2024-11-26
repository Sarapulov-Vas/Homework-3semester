namespace MyNUnitWeb.Pages;

using MyNUnitWeb.Data;

[BindProperties]
public class TestResult(TestsDBContext context) : PageModel
{
    public IList<TestModel> Tests { get; private set; } = [];

    public void OnPost(int Id)
    {
        Tests = context.Tests.OrderBy(test => test.Id).Where(test => test.TestRunId == Id).ToList();
    }

    public void OnGet(int Id)
    {
        Tests = context.Tests.OrderBy(test => test.Id).Where(test => test.TestRunId == Id).ToList();
    }
}
