// <copyright file="SingleThreadLazy.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Lazy;

/// <inheritdoc/>
public class SingleThreadLazy<T>(Func<T> func) : ILazy<T>
{
    private Func<T>? supplier = func;
    private T? result;

    /// <inheritdoc/>
    public T? Get()
    {
        if (supplier != null)
        {
            result = supplier();
            supplier = null;
        }

        return result;
    }
}
