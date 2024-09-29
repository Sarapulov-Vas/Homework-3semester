// <copyright file="MyThreadPool.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyThreadPool;

/// <summary>
/// Class that implements thread pool.
/// </summary>
public class MyThreadPool
{
    private readonly CancellationTokenSource cancellation = new ();

    private readonly Queue<Action> taskQueue = new ();

    private readonly Thread[] threads;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyThreadPool"/> class.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    public MyThreadPool(int numberOfThreads)
    {
        if (numberOfThreads <= 0)
        {
            throw new ArgumentOutOfRangeException("The number of streams must be positive.");
        }

        threads = new Thread[numberOfThreads];
        for (int i = 0; i < numberOfThreads; i++)
        {
            threads[i] = new Thread(СompleteTask);
            threads[i].Start();
        }
    }

    /// <summary>
    /// Method to add a task to the thread pool.
    /// </summary>
    /// <typeparam name="TResult">Task result type.</typeparam>
    /// <param name="func">Function to calculate.</param>
    /// <returns>Task result.</returns>
    public IMyTask<TResult> Submit<TResult>(Func<TResult> func)
        => new MyTask<TResult>(func, cancellation.Token, taskQueue);

    /// <summary>
    /// The method that closes threads.
    /// </summary>
    public void Shutdown()
    {
        cancellation.Cancel();
        lock (taskQueue)
        {
            Monitor.PulseAll(taskQueue);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private void СompleteTask()
    {
        while (!cancellation.IsCancellationRequested || taskQueue.Count > 0)
        {
            Action task;
            lock (taskQueue)
            {
                while (taskQueue.Count == 0)
                {
                    Monitor.Wait(taskQueue);
                }

                task = taskQueue.Dequeue();
            }

            task();
        }
    }
}
