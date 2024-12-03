// <copyright file="TestsDBContext.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnitWeb.Data;

using Microsoft.EntityFrameworkCore;

/// <inheritdoc/>
public class TestsDBContext(
    DbContextOptions<TestsDBContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets test run table.
    /// </summary>
    public DbSet<TestRun> TestRuns => Set<TestRun>();

    /// <summary>
    /// Gets table of tests.
    /// </summary>
    public DbSet<TestModel> Tests => Set<TestModel>();
}
