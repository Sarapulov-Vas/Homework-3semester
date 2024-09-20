// <copyright file="Philosopher.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

public class Philosopher
{
    private int number;
    private const int EatTime = 200;
    private const int ThinkTime = 500;
    private object leftFork;
    private object rightFork;
    private Thread thread;
    private bool goHome = false;

    public Philosopher(int number, object leftFork, object rightFork)
    {
        this.number = number;
        this.leftFork = leftFork;
        this.rightFork = rightFork;
        thread = new Thread(() => EatOrThink());
        thread.Start();
    }

    public void EatOrThink()
    {
        while (!goHome)
        {
            lock (leftFork)
            {
                Console.WriteLine($"Philosopher {number} get left fork.");
                lock (rightFork)
                {
                    Console.WriteLine($"Philosopher {number} eat.");
                    Thread.Sleep(EatTime);
                }
            }

            Console.WriteLine($"Philosopher {number} think.");
            Thread.Sleep(ThinkTime);
        }
    }

    public void GoHome()
    {
        goHome = true;
        Console.WriteLine($"Philosopher {number} go home.");
        thread.Join();
    }
}
