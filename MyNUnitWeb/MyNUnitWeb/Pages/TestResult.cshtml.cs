// <copyright file="TestResult.cshtml.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Pages;

using MyNUnitWeb.Data;

/// <summary>
/// A model to display the results of the run.
/// </summary>
/// <param name="context">DB context.</param>
[BindProperties]
public class TestResult(TestsDBContext context) : PageModel
{
    /// <summary>
    /// Gets list of tests.
    /// </summary>
    public IList<TestModel> Tests { get; private set; } = [];

    /// <summary>
    /// A method for processing a POST request.
    /// </summary>
    /// <param name="id">Primary test run key.</param>
    public void OnPost(int id)
    {
        Tests = context.Tests.OrderBy(test => test.Id).Where(test => test.TestRunId == id).ToList();
    }

    /// <summary>
    /// A method for processing a GET request.
    /// </summary>
    /// <param name="id">Primary test run key.</param>
    public void OnGet(int id)
    {
        Tests = context.Tests.OrderBy(test => test.Id).Where(test => test.TestRunId == id).ToList();
    }
}
