// <copyright file="TestFiles.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace TestFiles;

public class TestsFailed
{
    [Test]
    public void TestFailed1()
    {
        throw new IndexOutOfRangeException();
    }
}