namespace MyNUnitWeb.Pages;
using MyNUnit;

[BindProperties]
public class IndexModel(TestsDBContext context) : PageModel
{
    public string TestsPath { get; private set; } = Directory.GetCurrentDirectory() + "/wwwroot/Assemblies";

    public async Task<IActionResult> OnPostLoadAsync(IFormFile file)
    {
        if (file is not null)
        {
            var filePath = Path.Combine(TestsPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        return Page();
    }

    public IActionResult OnPostDelete(string filePath)
    {
        System.IO.File.Delete(filePath);
        return Page();
    }

    public async Task<IActionResult> OnPostRun()
    {
        var testsResult = await UnitTest.RunTests(TestsPath);
        var testRun = new TestRun
            {
                NumberFailedTests = testsResult.NumberFailedTests,
                NumberPassedTests = testsResult.NumberPassedTests,
                NumberIgnoredTests = testsResult.NumberIgnoredTests,
                NumberTests = testsResult.GetNumberTests,
            };
        context.TestRuns.Add(testRun);
        await context.SaveChangesAsync();
        foreach (var test in testsResult)
        {
            if (test.Value is not null)
            {
                var testResult = new TestModel
                    {
                        TestRunId = testRun.Id,
                        Name = test.Key.Name,
                        Message = test.Value.Messages,
                        Result = test.Value.Result,
                        Time = test.Value.Time,
                        E = test.Value.E is not null ?
                            test.Value.E.InnerException is not null ?
                             test.Value.E.InnerException.Message : string.Empty : string.Empty,
                    };
                context.Tests.Add(testResult);
            }
        }

        await context.SaveChangesAsync();

        return RedirectToPage("./TestResult", new { testRun.Id });
    }
}
