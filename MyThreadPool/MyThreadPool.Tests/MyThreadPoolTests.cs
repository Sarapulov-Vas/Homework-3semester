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
    [Test]
    public void TestArgumentOutOfRangeExceptionWhenCreatingPool()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MyThreadPool(-1));
    }

    /// <summary>
    /// Thread pool test.
    /// </summary>
    /// <param name="numberOfThreads">Number of Threads.</param>
    /// <param name="numberOfTasks">Number of tasks.</param>
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
    [TestCase(2, 2, 1)]
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
    [Test]
    public void TestThreadPoolDifferentTresult()
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
    /// Exception test when an exception occurs in a task.
    /// </summary>
    /// <exception cref="DivideByZeroException">Test exception.</exception>
    [Test]
    public void TestThreadPoolAggregateException()
    {
        int numberOfThreads = 4;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit<int?>(() => throw new DivideByZeroException());
        var exception = Assert.Throws<AggregateException>(() => { var a = task.Result; });
        Assert.That(exception.InnerException.GetType(), Is.EqualTo(typeof(DivideByZeroException)));
        Assert.IsFalse(task.IsCompleted);
    }

    /// <summary>
    /// Test an exception when null is returned in a task.
    /// </summary>
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
    [Test]
    public void TestShutdown()
    {
        int numberOfThreads = 4;
        var pool = new MyThreadPool(numberOfThreads);
        var task = pool.Submit(() => 1 + 2);
        pool.Shutdown();
        Assert.Throws<TaskCanceledException>(() => { var a = task.Result; });
    }

    /// <summary>
    /// Exception test at ContinueWith after shutdown.
    /// </summary>
    [Test]
    public void TestContinueWithAfterShutdown()
    {
        int numberOfThreads = 4;
        var pool = new MyThreadPool(numberOfThreads);
        var task1 = pool.Submit(() => 1 + 2);
        pool.Shutdown();
        var task2 = task1.ContinueWith((x) => x.ToString());
        Assert.Throws<TaskCanceledException>(() => { var a = task2.Result; });
    }

    private void AssertTasks<T>(IMyTask<T>[] tasks, T expeecdeResult)
    {
        foreach (var task in tasks)
        {
            Assert.That(task.Result, Is.EqualTo(expeecdeResult));
            Assert.IsTrue(task.IsCompleted);
        }
    }
}