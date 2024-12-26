// <copyright file="Queue`1.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MultithreadQueue;

/// <summary>
/// Class implementing thread-safe queue with priorities.
/// </summary>
/// <typeparam name="T">Item Type.
/// </typeparam>
public class Queue<T>
{
    private PriorityQueue<T, int> queue = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));

    /// <summary>
    /// Method for adding an item to the queue.
    /// </summary>
    /// <param name="value">Item value.</param>
    /// <param name="priority">Item priority.</param>
    public void Enqueue(T value, int priority)
    {
        lock (queue)
        {
            queue.Enqueue(value, priority);
            Monitor.PulseAll(queue);
        }
    }

    /// <summary>
    /// A method for retrieving an element from a queue.
    /// </summary>
    /// <returns>The element with the highest priority.</returns>
    public T Dequeue()
    {
        lock (queue)
        {
            while (queue.Count == 0)
            {
                Monitor.Wait(queue);
            }

            return queue.Dequeue();
        }
    }

    /// <summary>
    /// Method to get the number of elements in the queue.
    /// </summary>
    /// <returns>Number of items in the queue.</returns>
    public int Size()
    {
        lock (queue)
        {
            return queue.Count;
        }
    }
}
