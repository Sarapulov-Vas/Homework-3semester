// <copyright file="TestsIgnored.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace TestFiles;

public class TestsIgnored
{
    [Test("WIP")]
    public void TestIgnored1()
    {
        Console.WriteLine("ignored");
    }

    [Test("")]
    public void TestIgnored2()
    {
        throw new DivideByZeroException();
    }

    [Test]
    public void TestIgnored3(int num)
    {
    }
}
