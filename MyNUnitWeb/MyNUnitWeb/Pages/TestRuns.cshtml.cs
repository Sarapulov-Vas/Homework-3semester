// <copyright file="TestRuns.cshtml.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Pages;

using MyNUnitWeb.Data;

/// <summary>
/// Model page for displaying the history of launches.
/// </summary>
/// <param name="context">DB context.</param>
[BindProperties]
public class TestRuns(TestsDBContext context) : PageModel
{
    /// <summary>
    /// Gets launch list.
    /// </summary>
    public IList<TestRun> Launches { get; private set; } = [];

    /// <summary>
    /// A method for processing a GET request.
    /// </summary>
    public void OnGet()
    {
        Launches = context.TestRuns.OrderBy(run => run.Id).ToList();
    }

    /// <summary>
    /// A method for processing a startup deletion request.
    /// </summary>
    /// <param name="testRunId">Primary launch key.</param>
    /// <returns>The PageResult.</returns>
    public async Task<IActionResult> OnPostDelete(int testRunId)
    {
        var testRun = context.TestRuns.Find(testRunId);
        if (testRun is not null)
        {
            var testsToDelete = context.Tests
                .Where(test => test.TestRunId == testRun.Id)
                .ToList();
            context.Tests.RemoveRange(testsToDelete);
            context.TestRuns.Remove(testRun);
            await context.SaveChangesAsync();
            Launches = context.TestRuns.OrderBy(run => run.Id).ToList();
        }

        return Page();
    }
}
