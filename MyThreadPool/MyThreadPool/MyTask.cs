// <copyright file="MyTask.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyThreadPool;

/// <summary>
/// Class that implements the task interface.
/// </summary>
/// <typeparam name="TResult">The type of result of the task calculation.</typeparam>
public class MyTask<TResult> : IMyTask<TResult>
{
    private readonly Func<TResult> function;
    private readonly object lockObject = new ();
    private readonly Queue<Action> taskQueue;
    private readonly CancellationToken cancellation;
    private Exception? exception;
    private TResult? result;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTask{TResult}"/> class.
    /// </summary>
    /// <param name="func">Task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <param name="taskQueue">Task queue.</param>
    public MyTask(Func<TResult> func, CancellationToken cancellationToken, Queue<Action> taskQueue)
    {
        function = func;
        cancellation = cancellationToken;
        this.taskQueue = taskQueue;
        lock (taskQueue)
        {
            this.taskQueue.Enqueue(Task);
            Monitor.Pulse(taskQueue);
        }
    }

    /// <inheritdoc/>
    public bool IsCompleted { get; private set; }

    /// <inheritdoc/>
    public TResult Result => GetResult();

    /// <inheritdoc/>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        => new MyTask<TNewResult>(() => func(Result), cancellation, taskQueue);

    private void Task()
    {
        lock (lockObject)
        {
            if (cancellation.IsCancellationRequested)
            {
                Monitor.PulseAll(lockObject);
                return;
            }

            try
            {
                result = function();
                IsCompleted = true;
            }
            catch (Exception e)
            {
                exception = e;
            }

            Monitor.PulseAll(lockObject);
        }
    }

    private TResult GetResult()
    {
        lock (lockObject)
        {
            while (!IsCompleted && exception is null)
            {
                if (cancellation.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                Monitor.Wait(lockObject);
            }

            if (exception is not null)
            {
                throw new AggregateException(exception);
            }
            else if (result is null)
            {
                throw new ArgumentNullException("The function result was null.");
            }

            return result;
        }
    }
}
