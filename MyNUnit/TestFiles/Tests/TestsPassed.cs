// <copyright file="TestsPassed.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace TestFiles;
public class TestsPassed
{
    private int status;
    [Before]
    public void Before()
    {
        status = 1;
    }

    [After]
    public void After()
    {
        Thread.Sleep(100);
        status = 2;
    }

    [BeforeClass]
    public static void BeforeClass()
    {
        Thread.Sleep(100);
    }

    [AfterClass]
    public static void AfterClass()
    {
        Thread.Sleep(200);
    }

    [Test]
    public void TestPassed1()
    {
        status = 3;
        Thread.Sleep(200);
    }

    [Test(typeof(DivideByZeroException))]
    public void TestPassed2()
    {
        throw new DivideByZeroException();
    }
}
