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
    private readonly CancellationTokenSource cancellation = new();
    private readonly Queue<Action> taskQueue = new();
    private readonly Thread[] threads;
    private readonly AutoResetEvent queueWaitHandler = new(true);
    private readonly AutoResetEvent taskQueueEvent = new(false);
    private readonly ManualResetEvent shutdownEvent = new(false);

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
            threads[i] = new Thread(CompleteTask);
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
    {
        var task = new MyTask<TResult>(func, this);
        AddTask(task.RunTask);
        return task;
    }

    /// <summary>
    /// The method that shutdown threads.
    /// </summary>
    public void Shutdown()
    {
        cancellation.Cancel();
        shutdownEvent.Set();
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private void AddTask(Action runTask)
    {
        queueWaitHandler.WaitOne();
        taskQueue.Enqueue(runTask);
        queueWaitHandler.Set();
        taskQueueEvent.Set();
    }

    private void CompleteTask()
    {
        while (!cancellation.IsCancellationRequested || taskQueue.Count > 0)
        {
            queueWaitHandler.WaitOne();
            if (taskQueue.Count == 0)
            {
                queueWaitHandler.Set();
                WaitHandle.WaitAny([taskQueueEvent, shutdownEvent]);
                continue;
            }

            var task = taskQueue.Dequeue();
            queueWaitHandler.Set();
            task();
        }
    }

    /// <summary>
    /// Class that implements the task interface.
    /// </summary>
    /// <typeparam name="TResult">The type of result of the task calculation.</typeparam>
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private readonly MyThreadPool threadPool;
        private readonly Func<TResult> function;
        private readonly ManualResetEvent resultWaitHandler = new(false);
        private readonly AutoResetEvent continueWithHandler = new(true);
        private readonly List<Action> continueTaskList = new();
        private readonly CancellationToken cancellation;
        private Exception? exception;
        private TResult? result;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyTask{TResult}"/> class.
        /// </summary>
        /// <param name="func">Task.</param>
        /// <param name="threadPool">Thread pool.</param>
        public MyTask(Func<TResult> func, MyThreadPool threadPool)
        {
            function = func;
            cancellation = threadPool.cancellation.Token;
            this.threadPool = threadPool;
        }

        /// <inheritdoc/>
        public bool IsCompleted { get; private set; }

        /// <inheritdoc/>
        public TResult Result => GetResult();

        /// <inheritdoc/>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            continueWithHandler.WaitOne();
            if (IsCompleted)
            {
                continueWithHandler.Set();
                return threadPool.Submit(() => func(Result));
            }

            var task = new MyTask<TNewResult>(() => func(Result), threadPool);
            continueTaskList.Add(task.RunTask);
            continueWithHandler.Set();
            return task;
        }

        /// <summary>
        /// Task Start Method.
        /// </summary>
        public void RunTask()
        {
            if (cancellation.IsCancellationRequested)
            {
                resultWaitHandler.Set();
                return;
            }

            try
            {
                result = function();
            }
            catch (Exception e)
            {
                exception = e;
            }

            IsCompleted = true;
            resultWaitHandler.Set();
            continueWithHandler.WaitOne();
            foreach (var continueTask in continueTaskList)
            {
                threadPool.AddTask(continueTask);
            }

            continueWithHandler.Set();
        }

        private TResult GetResult()
        {
            if (!IsCompleted && cancellation.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            if (!IsCompleted)
            {
                resultWaitHandler.WaitOne();
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
