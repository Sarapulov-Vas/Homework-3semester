// <copyright file="TestsInfo.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit;

using System.Collections;
using System.Reflection;

/// <summary>
/// Container for storing tests information.
/// </summary>
public class TestsInfo : IEnumerable<KeyValuePair<MethodInfo, TestResult?>>
{
    private readonly Dictionary<MethodInfo, TestResult?> tests = new();

    private readonly List<string> messages = new();

    /// <summary>
    /// Gets or sets method to execute before running the test.
    /// </summary>
    public MethodInfo? BeforeTest { get; set; }

    /// <summary>
    /// Gets or sets method to execute after running the test.
    /// </summary>
    public MethodInfo? AfterTest { get; set; }

    /// <summary>
    /// Gets or sets method to execute before running class tests.
    /// </summary>
    public MethodInfo? BeforeClass { get; set; }

    /// <summary>
    /// Gets or sets method to execute after running class tests.
    /// </summary>
    public MethodInfo? AfterClass { get; set; }

    /// <summary>
    /// Gets number of tests passed.
    /// </summary>
    public int NumberPassedTests { get; private set; }

    /// <summary>
    /// Gets number of tests failed.
    /// </summary>
    public int NumberFailedTests { get; private set; }

    /// <summary>
    /// Gets number of tests ignored.
    /// </summary>
    public int NumberIgnoredTests { get; private set; }

    /// <summary>
    /// A method for retrieving messages that occurred during test execution.
    /// </summary>
    /// <returns>Messages.</returns>
    public string[] GetMessages => messages.ToArray();

    /// <summary>
    /// A method for obtaining the number of tests.
    /// </summary>
    /// <returns>Number of tests.</returns>
    public int GetNumberTests => tests.Count;

    /// <summary>
    /// Indexer for accessing tests.
    /// </summary>
    /// <param name="test">Test.</param>
    /// <returns>Test result.</returns>
    public TestResult? this[MethodInfo test]
    {
        get => tests[test];
        set
        {
            tests[test] = value;
            if (value is not null)
            {
                if (value.Result == 1)
                {
                    NumberPassedTests++;
                }
                else if (value.Result == -1)
                {
                    NumberIgnoredTests++;
                }
                else
                {
                    NumberFailedTests++;
                }
            }
        }
    }

    /// <summary>
    /// Method to add a test.
    /// </summary>
    /// <param name="test">Test method.</param>
    /// <param name="result">The result of performing the test.</param>
    public void AddTest(MethodInfo test, TestResult? result)
    {
        tests.Add(test, result);
        if (result is not null)
        {
            if (result.Result == 1)
            {
                NumberPassedTests++;
            }
            else if (result.Result == -1)
            {
                NumberIgnoredTests++;
            }
            else
            {
                NumberFailedTests++;
            }
        }
    }

    /// <summary>
    /// A method for adding a message when testing is performed.
    /// </summary>
    /// <param name="message">Message.</param>
    public void AddMessage(string message) => messages.Add(message);

    /// <summary>
    /// Method for unloading tests from other classes.
    /// </summary>
    /// <param name="testsInfo">Loaded tests info.</param>
    public void LoadTestsResults(TestsInfo testsInfo)
    {
        foreach (var test in testsInfo)
        {
            AddTest(test.Key, test.Value);
        }

        foreach (var message in testsInfo.GetMessages)
        {
            AddMessage(message);
        }
    }

    /// <summary>
    /// A method of obtaining an enumerator.
    /// </summary>
    /// <returns>Tests enumerator.</returns>
    public IEnumerator<KeyValuePair<MethodInfo, TestResult?>> GetEnumerator() => tests.GetEnumerator();

    /// <summary>
    /// A method of obtaining an enumerator.
    /// </summary>
    /// <returns>Tests enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
