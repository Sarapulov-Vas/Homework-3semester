// <copyright file="TestRun.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Data;

/// <summary>
/// The test run data model.
/// </summary>
public class TestRun
{
    /// <summary>
    /// Gets or sets primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets number of tests passed.
    /// </summary>
    public int NumberPassedTests { get; set; }

    /// <summary>
    /// Gets or sets number of tests failed.
    /// </summary>
    public int NumberFailedTests { get; set; }

    /// <summary>
    /// Gets or sets number of tests ignored.
    /// </summary>
    public int NumberIgnoredTests { get; set; }

    /// <summary>
    /// Gets or sets total number of tests.
    /// </summary>v
    public int NumberTests { get; set; }
}
