// <copyright file="MyThreadPoolTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyThreadPool.Tests;

/// <summary>
/// Tests for thread pool.
/// </summary>
public class MyThreadPoolTests
{
    /// <summary>
    /// Test of throwing an exception when the number of threads is incorrect.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestArgumentOutOfRangeExceptionWhenCreatingPool() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new MyThreadPool(-1));

    /// <summary>
    /// Thread pool test.
    /// </summary>
    /// <param name="numberOfThreads">Number of Threads.</param>
    /// <param name="numberOfTasks">Number of tasks.</param>
    [Timeout(5000)]
    [TestCase(2, 2)]
    [TestCase(5, 10)]
    [TestCase(10, 5)]
    public void TestThreadPool(int numberOfThreads, int numberOfTasks)
    {
        var tasks = new IMyTask<int>[numberOfTasks];
        var pool = new MyThreadPool(numberOfThreads);
        for (int i = 0; i < numberOfTasks; i++)
        {
            tasks[i] = pool.Submit(() => 1 + 1);
        }

        foreach (var task in tasks)
        {
            Assert.That(task.Result, Is.EqualTo(2));
            Assert.IsTrue(task.IsCompleted);
        }
    }

    /// <summary>
    /// The ContinueWith test.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    /// <param name="numberOfTasks">Number of tasks.</param>
    /// <param name="numberOfContinue">Number of ContinueWith.</param>
    [Timeout(5000)]
    [TestCase(2, 2, 1)]
    [TestCase(5, 2, 2)]
    [TestCase(5, 10, 5)]
    [TestCase(10, 5, 5)]
    public void TestThreadPoolContinueWith(int numberOfThreads, int numberOfTasks, int numberOfContinue)
    {
        var tasks = new IMyTask<int>[numberOfTasks];
        var pool = new MyThreadPool(numberOfThreads);
        for (int i = 0; i < numberOfTasks; i++)
        {
            tasks[i] = pool.Submit(() => 1 + 1);
        }

        AssertTasks(tasks, 2);

        var newTasks = new IMyTask<string>[numberOfContinue];

        for (int i = 0; i < numberOfContinue; i++)
        {
            newTasks[i] = tasks[i].ContinueWith((x) => x.ToString());
        }

        AssertTasks(newTasks, "2");
    }

    /// <summary>
    /// Test thread pool on tasks with different return types.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestThreadPoolDifferentResult()
    {
        int numberOfThreads = 2;
        var pool = new MyThreadPool(numberOfThreads);
        var task1 = pool.Submit(() => 2 * 3);
        var task2 = pool.Submit(() => "Hello," + " World!");
        var task3 = pool.Submit(() => 1 != 2);
        var task4 = pool.Submit(() =>
        {
            float[] arr = [1.2f, 3.4f, 5.2f];
            return arr.Sum();
        }).ContinueWith((x) => x * 10);

        Assert.That(task1.Result, Is.EqualTo(6));
        Assert.IsTrue(task1.IsCompleted);
        Assert.That(task2.Result, Is.EqualTo("Hello, World!"));
        Assert.IsTrue(task2.IsCompleted);
        Assert.That(task3.Result, Is.EqualTo(true));
        Assert.IsTrue(task3.IsCompleted);
        Assert.That(task4.Result, Is.EqualTo(98));
        Assert.IsTrue(task4.IsCompleted);
    }

    /// <summary>
    /// Test a task with several ContinueWith.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestSeveralContinueWith()
    {
        int numberOfThreads = 2;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() =>
        {
            float[] arr = [1.2f, 3.4f, 5.2f];
            return arr.Sum();
        }).ContinueWith((x) => x * 10).ContinueWith((x) => x.ToString());
        Assert.That(task.Result, Is.EqualTo("98"));
        Assert.IsTrue(task.IsCompleted);
    }

    /// <summary>
    /// Test continueWith after task completion.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    [Timeout(5000)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    [TestCase(7)]
    [TestCase(10)]
    public void TestDifferentContinueWith(int numberOfThreads)
    {
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() => 1 + 2);
        var taskContinue1 = task.ContinueWith((x) => x * 10);
        Assert.That(task.Result, Is.EqualTo(3));
        Assert.IsTrue(task.IsCompleted);
        var taskContinue2 = task.ContinueWith((x) => x + 1);
        var taskContinue3 = task.ContinueWith((x) => x.ToString());
        Assert.That(taskContinue1.Result, Is.EqualTo(30));
        Assert.IsTrue(taskContinue1.IsCompleted);
        Assert.That(taskContinue2.Result, Is.EqualTo(4));
        Assert.IsTrue(taskContinue2.IsCompleted);
        Assert.That(taskContinue3.Result, Is.EqualTo("3"));
        Assert.IsTrue(taskContinue3.IsCompleted);
    }

    /// <summary>
    /// Test continueWith after task completion.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    [Timeout(5000)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void TestContinueWithAfterTaskCompletion(int numberOfThreads)
    {
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() => 1 + 2);
        Thread.Sleep(100);
        var taskContinue = task.ContinueWith((x) => x * 10);
        Assert.That(taskContinue.Result, Is.EqualTo(30));
        Assert.IsTrue(taskContinue.IsCompleted);
    }

    /// <summary>
    /// Test continueWith before task completion.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    [Timeout(5000)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void TestContinueWithBeforeTaskCompletion(int numberOfThreads)
    {
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() =>
        {
            Thread.Sleep(200);
            return 1;
        });
        var taskContinue = task.ContinueWith((x) => x * 10);
        Assert.That(taskContinue.Result, Is.EqualTo(10));
        Assert.IsTrue(taskContinue.IsCompleted);
    }

    /// <summary>
    /// Exception test when an exception occurs in a task.
    /// </summary>
    /// <exception cref="DivideByZeroException">Test exception.</exception>
    [Timeout(5000)]
    [Test]
    public void TestThreadPoolAggregateException()
    {
        int numberOfThreads = 4;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit<int?>(() => throw new DivideByZeroException());
        var exception = Assert.Throws<AggregateException>(() => { var a = task.Result; });
        Assert.That(exception!.InnerException!.GetType(), Is.EqualTo(typeof(DivideByZeroException)));
        Assert.IsTrue(task.IsCompleted);
    }

    /// <summary>
    /// Test an exception when null is returned in a task.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestThreadPoolNullReturn()
    {
        int numberOfThreads = 4;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit<int?>(() => null);
        Assert.Throws<ArgumentNullException>(() => { var a = task.Result; });
        Assert.IsTrue(task.IsCompleted);
    }

    /// <summary>
    /// Test of the thread termination method.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestShutdown()
    {
        int numberOfThreads = 1;
        var pool = new MyThreadPool(numberOfThreads);
        var task1 = pool.Submit(() =>
        {
            Thread.Sleep(100);
            return 1;
        });
        Thread.Sleep(100);
        var task2 = pool.Submit(() => 1 + 1);
        pool.Shutdown();
        var task3 = pool.Submit(() => 1 + 1);
        Assert.That(task1.Result, Is.EqualTo(1));
        Assert.True(task1.IsCompleted);
        Assert.Throws<TaskCanceledException>(() => { var a = task2.Result; });
        Assert.False(task2.IsCompleted);
        Assert.Throws<TaskCanceledException>(() => { var a = task3.Result; });
        Assert.False(task3.IsCompleted);
    }

    /// <summary>
    /// Exception test at ContinueWith after shutdown.
    /// </summary>
    /// <param name="numberOfThreads">Number of threads.</param>
    [Timeout(5000)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void TestContinueWithAfterShutdown(int numberOfThreads)
    {
        var pool = new MyThreadPool(numberOfThreads);
        var task1 = pool.Submit(() => 1 + 2);
        pool.Shutdown();
        var task2 = task1.ContinueWith((x) => x.ToString());
        Assert.Throws<TaskCanceledException>(() => { var a = task2.Result; });
    }

    /// <summary>
    /// Test of parallel access to thread pool.
    /// </summary>
    [Timeout(5000)]
    [Test]
    public void TestParallelAccess()
    {
        int numberOfThreads = 1;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() =>
        {
            Thread.Sleep(1000);
            return 1;
        });

        var threads = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            threads[i] = new Thread(() => Assert.That(task.Result, Is.EqualTo(1)));
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private void AssertTasks<T>(IMyTask<T>[] tasks, T expectedResult)
    {
        foreach (var task in tasks)
        {
            Assert.That(task.Result, Is.EqualTo(expectedResult));
            Assert.IsTrue(task.IsCompleted);
        }
    }
}
