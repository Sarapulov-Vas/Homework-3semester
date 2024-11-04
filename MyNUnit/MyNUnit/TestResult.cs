// <copyright file="TestResult.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit;

/// <summary>
/// Container for storing test results.
/// </summary>
/// <param name="testName">Test name.</param>
/// <param name="result">Test result.</param>
/// -1 - test ignored.
/// 0 - test failed.
/// 1 - test passed.
/// <param name="message">Test message.</param>
public class TestResult(string testName, int result, string message, long time, Exception? exception)
{
    /// <summary>
    /// Gets message to the test result.
    /// </summary>
    public string Messages { get; private set; } = message;

    /// <summary>
    /// Gets a value indicating whether the test has been successfully passed.
    /// </summary>
    public long Result { get; private set; } = result;

    /// <summary>
    /// Gets test Name.
    /// </summary>
    public string TestName { get; private set; } = testName;

    /// <summary>
    /// Gets test execution time.
    /// </summary>
    public long Time { get; private set; } = time;

    /// <summary>
    /// Gets exception during the execution of the test.
    /// </summary>
    public Exception? E { get; private set; } = exception;
}
