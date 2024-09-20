﻿// <copyright file="Program.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

using ParallelMatrixMultiplication;
using System.Diagnostics;
using ScottPlot;

var stopwatch = new Stopwatch();
var n = 100;
List<double> xSize = [];
List<double> yTime = [];
List<double> yParallelTime = [];
List<double> yStandardDeviation = [];
List<double> yParallelStandardDeviation = [];
Plot plt = new ();
for (int i = 1; i <= 15; i++)
{
    var size = 10 * i;
    MatrixGenerator.MatrixGenerator.GenerateFile($"res/matrix{i}.txt", size, size);
    var matrix = new Matrix($"res/matrix{i}.txt");
    var times = new double[n];
    var parallelTimes = new double[n];
    for (int j = 0; j < n; j++)
    {
        stopwatch.Start();
        Matrix.Multiplication(matrix, matrix);
        stopwatch.Stop();
        times[j] = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
        stopwatch.Start();
        Matrix.ParallelMultiplication(matrix, matrix);
        stopwatch.Stop();
        parallelTimes[j] = stopwatch.ElapsedMilliseconds;
        stopwatch.Reset();
    }

    var meanValue = (double)times.Sum() / n;
    var parallelMeanValue = (double)parallelTimes.Sum() / n;
    var sum = 0.0;
    foreach (var time in times)
    {
        sum += Math.Pow(time - meanValue, 2);
    }

    var standardDeviation = Math.Sqrt(sum / n);
    var parallelSum = 0.0;
    foreach (var time in parallelTimes)
    {
        parallelSum += Math.Pow(time - parallelMeanValue, 2);
    }

    var parallelStandardDeviation = Math.Sqrt(parallelSum / n);
    xSize.Add(size);
    yTime.Add(meanValue);
    yParallelTime.Add(parallelMeanValue);
    yStandardDeviation.Add(standardDeviation);
    yParallelStandardDeviation.Add(parallelStandardDeviation);
    Console.WriteLine($"Mean value: {meanValue} ms; Parallel mean value: {parallelMeanValue} ms");
    Console.WriteLine($"Standard deviation: {standardDeviation} ms; Parallel standard deviation: {parallelStandardDeviation} ms");
}

plt.XLabel("Matrix Size");
plt.YLabel("Time (ms)");
plt.Title("Single-threaded and multi-threaded matrix multiplication.");
plt.Add.Scatter(xSize, yTime).LegendText = "Single-threaded";
plt.Add.Scatter(xSize, yParallelTime).LegendText = "Multi-threaded";
plt.Add.ErrorBar(xSize, yTime, yStandardDeviation).LegendText = "Standard deviation single-threaded";
plt.Add.ErrorBar(xSize, yParallelTime, yParallelStandardDeviation).LegendText = "Standard deviation multi-threaded";
plt.SavePng("timeСhart.png", 1200, 800);
