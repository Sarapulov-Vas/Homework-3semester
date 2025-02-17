// <copyright file="ReflectorTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Reflector.Tests;
using System.Reflection;

/// <summary>
/// Test class.
/// </summary>
public class ReflectorTests
{
    /// <summary>
    /// Print structure test.
    /// </summary>
    [Test]
    public void ReflectorPrintStructureTest()
    {
        var assembly = Assembly.LoadFrom("../../../../TestClass/bin/Debug/net9.0/TestClass.dll");
        Reflector.PrintStructure(assembly.GetTypes()[0]);
    }

    /// <summary>
    /// Diff classes test.
    /// </summary>
    [Test]
    public void ReflectorDiffClassesTest()
    {
        var assembly = Assembly.LoadFrom("../../../../TestClass/bin/Debug/net9.0/TestClass.dll");
        Assert.That(Reflector.DiffClasses(assembly.GetTypes()[0], assembly.GetTypes()[0]).Length, Is.EqualTo(0));
    }
}
