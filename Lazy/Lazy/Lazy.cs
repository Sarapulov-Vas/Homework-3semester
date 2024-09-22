// <copyright file="Lazy.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Lazy;

/// <inheritdoc/>
public class Lazy<T> : ILazy<T>
{
    private Func<T> supplier;
    private T? result;
    private bool hasResult;

    /// <summary>
    /// Initializes a new instance of the <see cref="Lazy{T}"/> class.
    /// </summary>
    /// <param name="func">Function.</param>
    public Lazy(Func<T> func)
    {
        supplier = func;
    }

    /// <inheritdoc/>
    public T Get()
    {
        if (!hasResult)
        {
            hasResult = true;
            result = supplier();
        }

        if (result != null)
        {
            return result;
        }
        else
        {
            throw new NullReferenceException("The calculated value was null.");
        }
    }
}
