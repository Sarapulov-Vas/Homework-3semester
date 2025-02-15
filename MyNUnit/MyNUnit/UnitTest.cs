// <copyright file="UnitTest.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MyNUnit;

using System.Diagnostics;
using System.Reflection;
using TestAttributes;

/// <summary>
/// A class that implements running unit tests.
/// </summary>
public static class UnitTest
{
    /// <summary>
    /// Test Run Method.
    /// </summary>
    /// <param name="path">Path to the tests.</param>
    /// <returns>Tests result.</returns>
    public static async Task<TestsInfo> RunTests(string path)
    {
        TestsInfo testsResult = new();
        List<Task<TestsInfo>> taskList = new();
        if (File.Exists(path))
        {
            taskList.Add(Task.Run(() => Run(path)));
        }
        else
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
            {
                taskList.Add(Task.Run(() => Run(file)));
            }
        }

        foreach (var task in taskList)
        {
            var result = await task;
            testsResult.LoadTestsResults(result);
        }

        return testsResult;
    }

    private static async Task<TestsInfo> Run(string path)
    {
        TestsInfo testsResult = new();
        var assembly = Assembly.LoadFrom(path);
        List<Task<TestsInfo>> taskList = new();
        foreach (var type in assembly.GetExportedTypes())
        {
            taskList.Add(Task.Run(() =>
            {
                var testsInfo = GetTestsInfo(type);
                try
                {
                    StartTests(testsInfo, type);
                }
                catch
                {
                    // We do not do anything because the error message is in testInfo.
                }

                return testsInfo;
            }));
        }

        foreach (var task in taskList)
        {
            var result = await task;
            testsResult.LoadTestsResults(result);
        }

        return testsResult;
    }

    private static TestsInfo GetTestsInfo(Type classType)
    {
        TestsInfo testsInfo = new();
        foreach (var methodInfo in classType.GetMethods())
        {
            var attrs = Attribute.GetCustomAttributes(methodInfo);
            if (attrs.Length == 1)
            {
                if (attrs[0].GetType() == typeof(TestAttribute))
                {
                    testsInfo.AddTest(methodInfo, null);
                }
                else if (attrs[0].GetType() == typeof(BeforeAttribute))
                {
                    testsInfo.BeforeTest = methodInfo;
                }
                else if (attrs[0].GetType() == typeof(AfterAttribute))
                {
                    testsInfo.AfterTest = methodInfo;
                }
                else if (attrs[0].GetType() == typeof(BeforeClassAttribute))
                {
                    testsInfo.BeforeClass = methodInfo;
                }
                else if (attrs[0].GetType() == typeof(AfterClassAttribute))
                {
                    testsInfo.AfterClass = methodInfo;
                }
            }
        }

        return testsInfo;
    }

    private static void StartTests(TestsInfo testsInfo, Type classType)
    {
        if (testsInfo.GetNumberTests == 0)
        {
            return;
        }

        if (testsInfo.BeforeClass is not null)
        {
            try
            {
                testsInfo.BeforeClass.Invoke(classType, null);
            }
            catch (TargetInvocationException e)
            {
                testsInfo.AddMessage($"Exception in {testsInfo.BeforeClass.Name}; Message:\n {e.Message}");
                throw;
            }
        }

        var instance = Activator.CreateInstance(classType);
        foreach (var test in testsInfo)
        {
            if (testsInfo.BeforeTest is not null)
            {
                try
                {
                    testsInfo.BeforeTest.Invoke(instance, null);
                }
                catch (TargetInvocationException e)
                {
                    testsInfo.AddMessage($"Exception in {testsInfo.BeforeTest.Name}; Message:\n {e.Message}");
                    continue;
                }
            }

            var attributeArguments = (TestAttribute)Attribute.GetCustomAttributes(test.Key)[0];
            if (attributeArguments.Ignore != null)
            {
                testsInfo[test.Key] = new TestResult(-1, attributeArguments.Ignore, 0, null);
            }
            else
            {
                var stopwatch = new Stopwatch();
                try
                {
                    stopwatch.Start();
                    test.Key.Invoke(instance, null);
                    stopwatch.Stop();
                    testsInfo[test.Key] = new TestResult(1, string.Empty, stopwatch.ElapsedMilliseconds, null);
                }
                catch (TargetParameterCountException e)
                {
                    stopwatch.Stop();
                    testsInfo[test.Key] = new TestResult(-1, $"Exception in {test.Key.Name}; Message:\n {e.Message}", stopwatch.ElapsedMilliseconds, e);
                    continue;
                }
                catch (Exception e)
                {
                    if (attributeArguments.Expected is not null && e.InnerException is not null
                        && attributeArguments.Expected.Equals(e.InnerException.GetType()))
                    {
                        stopwatch.Stop();
                        testsInfo[test.Key] = new TestResult(1, string.Empty, stopwatch.ElapsedMilliseconds, e);
                    }
                    else
                    {
                        stopwatch.Stop();
                        testsInfo[test.Key] = new TestResult(0, string.Empty, stopwatch.ElapsedMilliseconds, e);
                    }
                }
            }

            if (testsInfo.AfterTest is not null)
            {
                try
                {
                    testsInfo.AfterTest.Invoke(instance, null);
                }
                catch (Exception e)
                {
                    testsInfo.AddMessage($"Exception in {testsInfo.AfterTest.Name}; Message:\n {e.Message}");
                    continue;
                }
            }
        }

        if (testsInfo.AfterClass is not null)
        {
            try
            {
                testsInfo.AfterClass.Invoke(classType, null);
            }
            catch (Exception e)
            {
                testsInfo.AddMessage($"Exception in {testsInfo.AfterClass.Name}; Message:\n {e.Message}");
            }
        }
    }
}
