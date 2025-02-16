// <copyright file="ILazy.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Lazy;

/// <summary>
/// Initializes a new instance of the <see cref="Lazy"/> class.
/// </summary>
/// <typeparam name="T">Function return type.</typeparam>
public interface ILazy<out T>
{
    /// <summary>
    /// A method for obtaining the result of a function.
    /// </summary>
    /// <returns>Function result.</returns>
    public T? Get();
}
