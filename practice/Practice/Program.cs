// <copyright file="Program.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

int number = 5;
var forks = new object[number];
var philosophers = new Philosopher[number];

for (var i = 0; i < number; i++)
{
    forks[i] = new object();
}

philosophers[0] = new Philosopher(0, forks[0], forks[number - 1]);
for (int i = 1; i < number; i++)
{
    philosophers[i] = new Philosopher(i, forks[(i + 1) % number], forks[i]);
}

Console.ReadLine();
foreach (var philosopher in philosophers)
{
    philosopher.GoHome();
}
