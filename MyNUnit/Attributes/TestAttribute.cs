// <copyright file="TestAttribute.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace TestAttributes;

/// <summary>
/// Test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    public TestAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    /// <param name="argument">Test argument.</param>
    /// <param name="message">Ignore message.</param>
    public TestAttribute(string ignore)
    {
        Ignore = ignore;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    public TestAttribute(Type E)
    {
        Expected = E;
    }

    /// <summary>
    /// Gets test argument.
    /// </summary>
    public Type? Expected { get; private set; } = null;

    /// <summary>
    ///  Gets ignore message.
    /// </summary>
    public string? Ignore { get; private set; } = null;
}
