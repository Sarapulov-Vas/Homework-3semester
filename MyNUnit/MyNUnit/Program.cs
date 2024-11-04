// <copyright file="Program.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

using MyNUnit;

if (args.Length != 1)
{
    Console.WriteLine("Wrong number of input arguments.");
    Console.WriteLine("The path to an assembly or directory with assemblies was expected.");
    return;
}

if (!Path.Exists(args[0]))
{
    Console.WriteLine("Path dose not exist.");
    return;
}

Console.WriteLine("Start test execution; wait ...");
var testsResult = UnitTest.RunTests(args[0]);
if (testsResult.GetMessages().Length != 0)
{
     foreach (var message in testsResult.GetMessages())
     {
        Console.WriteLine(message);
     }
}
else
{
    foreach (var test in testsResult)
    {
        Console.Write($"Test: {test.Key.Name} ");
        if (test.Value!.Result == -1)
        {
            Console.Write($"Ignored; Reason: {test.Value.Messages}; ");
        }
        else if (test.Value.Result == 1)
        {
            Console.Write("Passed; ");
        }
        else
        {
            Console.Write($"Failed; Exception: {test.Value.E!.InnerException!.ToString()}; ");
        }

        Console.WriteLine($"Time(ms): {test.Value.Time};");
    }

    Console.Write(testsResult.NumberFailedTests == 0 ? "Passed!  " : "Failed!  ");
    Console.WriteLine($"failed  {testsResult.NumberFailedTests}; passed  {testsResult.NumberPassedTests}; " +
        $"ignored  {testsResult.NumberIgnoredTests}; total  {testsResult.GetNumberTests()}");
}
