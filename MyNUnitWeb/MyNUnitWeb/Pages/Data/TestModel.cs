// <copyright file="TestModel.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Data;

/// <summary>
/// The data model for the test.
/// </summary>
public class TestModel
{
    /// <summary>
    /// Gets or sets primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the primary startup key in which the test was run.
    /// </summary>
    public int TestRunId { get; set; }

    /// <summary>
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets message to the test result.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets  a value indicating whether the test has been successfully passed.
    /// </summary>
    public long Result { get; set; }

    /// <summary>
    /// Gets or sets  test execution time.
    /// </summary>
    public long Time { get; set; }

    /// <summary>
    /// Gets or sets exception during the execution of the test.
    /// </summary>
    public string E { get; set; } = string.Empty;
}
