// <copyright file="BeforeTestError.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace TestsError;

public class BeforeTestError
{
    [Before]
    public void BeforeTest (int param)
    {
    }

    [Test]
    public void Test (int param)
    {
    }
}
