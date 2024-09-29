// <copyright file="IMyTask.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyThreadPool;

/// <summary>
/// Task Interface.
/// </summary>
/// <typeparam name="TResult">Type of result of function calculation.</typeparam>
public interface IMyTask<TResult>
{
    /// <summary>
    /// Gets a value indicating whether status of the task completion.
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    /// Gets calculation result.
    /// </summary>
    public TResult Result { get; }

    /// <summary>
    /// Creating a new task based on the results of the past task.
    /// </summary>
    /// <typeparam name="TNewResult">Type of result of the new task.</typeparam>
    /// <param name="func">New task.</param>
    /// <returns>The result of calculating a new task.</returns>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
}
