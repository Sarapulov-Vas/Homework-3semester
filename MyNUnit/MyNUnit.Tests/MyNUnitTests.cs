// <copyright file="MyNUnitTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit.Tests;

using System.Reflection;
using MyNUnit;
using NUnit.Framework.Api;

/// <summary>
/// Class for tests.
/// </summary>
public class MyNUnitTests
{
    private readonly Dictionary<string, (int, Type?, string)> expectedResult = new()
    {
        { "TestFailed1", (0, typeof(IndexOutOfRangeException), string.Empty) },
        { "TestIgnored1", (-1, null, "WIP") },
        { "TestIgnored2", (-1, null, string.Empty) },
        { "TestIgnored3", (-1, typeof(TargetParameterCountException), "Exception in TestIgnored3; Message:\n Parameter count mismatch.") },
        { "TestPassed1", (1, null, string.Empty) },
        { "TestPassed2", (1, typeof(DivideByZeroException), string.Empty) },
    };

    /// <summary>
    /// Test run tests by path in the directory.
    /// </summary>
    [NUnit.Framework.Test]
    public async Task TestOfTests_PathToDirectory()
    {
        var result = await UnitTest.RunTests("../../../../TestFiles/Tests/bin/Debug/net9.0/");
        foreach (var testResult in result)
        {
            Assert.That(testResult.Value!.Result, Is.EqualTo(expectedResult[testResult.Key.Name].Item1));
            if (testResult.Value!.Exception is not null)
            {
                if (testResult.Value!.Exception.InnerException is null)
                {
                    Assert.That(testResult.Value!.Exception.GetType(), Is.EqualTo(expectedResult[testResult.Key.Name].Item2));
                }
                else
                {
                    Assert.That(testResult.Value!.Exception.InnerException.GetType(), Is.EqualTo(expectedResult[testResult.Key.Name].Item2));
                }
            }

            Assert.That(testResult.Value!.Messages, Is.EqualTo(expectedResult[testResult.Key.Name].Item3));
        }
    }

    /// <summary>
    /// Test run tests by file path.
    /// </summary>
    [NUnit.Framework.Test]
    public async Task TestOfTests_PathToFile()
    {
        var result = await UnitTest.RunTests("../../../../TestFiles/Tests/bin/Debug/net9.0/Tests.dll");
        foreach (var testResult in result)
        {
            Assert.That(testResult.Value!.Result, Is.EqualTo(expectedResult[testResult.Key.Name].Item1));
        }
    }

    /// <summary>
    /// Test messages for test errors.
    /// </summary>
    [NUnit.Framework.Test]
    public async Task TestError()
    {
        var result = await UnitTest.RunTests("../../../../TestFiles/TestsError/bin/Debug/net9.0");
        foreach (var message in result.GetMessages)
        {
            Assert.True(message.StartsWith("Exception"));
        }

        foreach (var testResult in result)
        {
            if (testResult.Value is not null && testResult.Value.Messages != string.Empty)
            {
                Assert.True(testResult.Value.Messages.StartsWith("Exception"));
                Assert.That(testResult.Value.Result, Is.EqualTo(-1));
            }
        }
    }
}
