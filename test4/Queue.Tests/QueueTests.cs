// <copyright file="QueueTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Queue.Tests;

/// <summary>
/// Test class.
/// </summary>
public class QueueTests
{
    /// <summary>
    /// Single-threaded queue test.
    /// </summary>
    [Test]
    public void SingleThreadTest()
    {
        var queue = new MultithreadQueue.Queue<int>();
        queue.Enqueue(1, 1);
        queue.Enqueue(2, 2);
        queue.Enqueue(3, 2);
        Assert.That(queue.Size, Is.EqualTo(3));
        Assert.That(queue.Dequeue, Is.EqualTo(2));
        Assert.That(queue.Dequeue, Is.EqualTo(3));
        Assert.That(queue.Dequeue, Is.EqualTo(1));
        Assert.That(queue.Size, Is.EqualTo(0));
    }

    /// <summary>
    /// A multi-threaded test of adding items to a queue.
    /// </summary>
    [Test]
    public void MultiThreadEnqueueTest()
    {
        ManualResetEvent resetEvent = new(false);
        List<(int, int)[]> testData =
        [
            [(3, 3), (2, 2), (1, 1)],
            [(7, 7), (6, 6), (5, 5), (4, 4), (3, 3)],
            [(13, 13), (12, 12), (11, 11), (10, 10), (9, 9)]
        ];
        Thread[] threads = new Thread[3];
        var queue = new MultithreadQueue.Queue<int>();
        for (int i = 0; i < 3; i++)
        {
            var num = i;
            threads[i] = new Thread(() =>
            {
                resetEvent.WaitOne();
                foreach (var element in testData[num])
                {
                    queue.Enqueue(element.Item1, element.Item2);
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        resetEvent.Set();
        foreach (var thread in threads)
        {
            thread.Join();
        }

        Assert.That(queue.Size, Is.EqualTo(13));
        foreach (var test in testData.ToArray().Reverse())
        {
            foreach (var item in test)
            {
                Assert.That(queue.Dequeue, Is.EqualTo(item.Item1));
            }
        }

        Assert.That(queue.Size, Is.EqualTo(0));
    }

    /// <summary>
    /// Multithreaded test of element extraction from the queue.
    /// </summary>
    [Test]
    public void MultiThreadDequeueTest()
    {
        var queue = new MultithreadQueue.Queue<int>();
        var thread = new Thread(() =>
        {
            for (int i = 7; i >= 1; i--)
            {
                Assert.That(queue.Dequeue(), Is.EqualTo(i));
            }
        });
        (int, int)[] testData = { (7, 7), (6, 6), (5, 5), (4, 4), (3, 3), (2, 2), (1, 1) };
        thread.Start();
        foreach (var element in testData)
        {
            queue.Enqueue(element.Item1, element.Item2);
        }

        thread.Join();
    }
}
