// <copyright file="Program.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

using System.Diagnostics;
using MD5Checksum;

var singleThreadChecksum = new SingleThreadChecksum();
var multiThreadChecksum = new MultiThreadChecksum();
var stopwatch = new Stopwatch();
stopwatch.Start();
await singleThreadChecksum.CalculateCheckSum("../CheckSum.Tests/TestFiles");
stopwatch.Stop();
Console.WriteLine($"Singlethread time(ms): {stopwatch.ElapsedMilliseconds}");
stopwatch.Reset();
stopwatch.Start();
await multiThreadChecksum.CalculateCheckSum("../CheckSum.Tests/TestFiles");
Console.WriteLine($"Multithread time(ms): {stopwatch.ElapsedMilliseconds}");
