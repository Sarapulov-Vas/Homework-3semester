// <copyright file="MyNUnitTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit.Tests;
using MyNUnit;

public class MyNUnitTests
{
    private Dictionary<string, int> expectedResult = new ()
    {
        { "TestFailed1", 0 },
        { "TestIgnored1", -1 },
        { "TestIgnored2", -1 },
        { "TestIgnored3", -1 },
        { "TestPassed1", 1 },
        { "TestPassed2", 1 },
    };

    [NUnit.Framework.Test]
    public void TestOfTests_PathToDirectory()
    {
        var result = UnitTest.RunTests("../../../../TestFiles/Tests/bin/Debug/net8.0/");
        foreach (var testResult in result)
        {
            Assert.That(testResult.Value!.Result, Is.EqualTo(expectedResult[testResult.Key.Name]));
        }
    }

    [NUnit.Framework.Test]
    public void TestOfTests_PathToFile()
    {
        var result = UnitTest.RunTests("../../../../TestFiles/Tests/bin/Debug/net8.0/Tests.dll");
        foreach (var testResult in result)
        {
            Assert.That(testResult.Value!.Result, Is.EqualTo(expectedResult[testResult.Key.Name]));
        }
    }

    [NUnit.Framework.Test]
    public void TestError()
    {
        var result = UnitTest.RunTests("../../../../TestFiles/TestsError/bin/Debug/net8.0");
        foreach (var message in result.GetMessages())
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