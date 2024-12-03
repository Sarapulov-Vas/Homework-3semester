// <copyright file="Index.cshtml.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Pages;
using MyNUnit;

/// <summary>
/// Home page model.
/// </summary>
/// <param name="context">DB context.</param>
[BindProperties]
public class IndexModel(TestsDBContext context) : PageModel
{
    /// <summary>
    /// Gets path to assemblies.
    /// </summary>
    public string TestsPath { get; private set; } = Directory.GetCurrentDirectory() + "/wwwroot/Assemblies";

    /// <summary>
    /// Processing a POST request to upload a file.
    /// </summary>
    /// <param name="file">File.</param>
    /// <returns>The PageResult.</returns>
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

    /// <summary>
    /// Processing a POST request to delete a file.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <returns>The PageResult.</returns>
    public IActionResult OnPostDelete(string filePath)
    {
        System.IO.File.Delete(filePath);
        return Page();
    }

    /// <summary>
    /// Processing a POST request to run tests.
    /// </summary>
    /// <returns>The PageResult.</returns>
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
