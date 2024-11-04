// <copyright file="TestAttribute.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit;

/// <summary>
/// Test attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    /// <param name="argument">Test argument.</param>
    public TestAttribute(TestArgument argument)
    {
        Argument = argument;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    /// <param name="argument">Test argument.</param>
    /// <param name="message">Ignore message.</param>
    public TestAttribute(TestArgument argument, string message)
    {
        Argument = argument;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestAttribute"/> class.
    /// </summary>
    public TestAttribute()
    {
    }

    /// <summary>
    /// Gets test argument.
    /// </summary>
    public TestArgument? Argument { get; private set; } = null;

    /// <summary>
    ///  Gets ignore message.
    /// </summary>
    public string Message { get; private set; } = string.Empty;
}
