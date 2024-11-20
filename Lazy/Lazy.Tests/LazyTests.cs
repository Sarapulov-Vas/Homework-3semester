// <copyright file="LazyTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Lazy.Tests;

/// <summary>
/// Tests of lazy calculations.
/// </summary>
public class LazyTests
{
    /// <summary>
    /// Single thread suppliers call count test.
    /// </summary>
    [Test]
    public void TestSingleThreadSuppliersCallCount()
    {
        int numberOfEvaluations = 0;
        var lazy = new SingleThreadLazy<int?>(() => Interlocked.Increment(ref numberOfEvaluations));
        Assert.That(lazy.Get(), Is.EqualTo(1));
        for (int i = 0; i < 10; i++)
        {
            Assert.That(lazy.Get(), Is.EqualTo(1));
        }

        Assert.That(lazy.Get(), Is.EqualTo(1));
    }

    /// <summary>
    /// Multi thread suppliers call count test.
    /// </summary>
    [Test]
    public void TestMultiThreadSuppliersCallCount()
    {
        int numberOfEvaluations = 0;
        var lazy = new MultiThreadLazy<int?>(() => Interlocked.Increment(ref numberOfEvaluations));
        StartThreads(
            () => ThreadAction(lazy, 10, 1), 5);
    } 

    /// <summary>
    /// Test of correct operation in single-threaded mode.
    /// </summary>
    /// <typeparam name="T">Function return type.</typeparam>
    /// <param name="function">Function for lazy calculation.</param>
    /// <param name="expectedResult">Expected result.</param>
    /// <param name="numberOfGet">Number of Get calls.</param>
    [TestCaseSource(typeof(TestData), nameof(TestData.SingleThreadCases))]
    public void TestSingleThread<T>(Func<T> function, object expectedResult, int numberOfGet)
    {
        var testLazy = new SingleThreadLazy<T>(function);

        for (int i = 1; i <= numberOfGet; i++)
        {
            Assert.That(testLazy.Get(), Is.EqualTo(expectedResult));
        }
    }

    /// <summary>
    /// Test of correct operation in multi-threaded mode.
    /// </summary>
    /// <typeparam name="T">Function return type.</typeparam>
    /// <param name="function">Function for lazy calculation.</param>
    /// <param name="expectedResult">Expected result.</param>
    /// <param name="numberOfGet">Number of Get calls.</param>
    /// <param name="numberOfThreads">Number of threads.</param>
    [TestCaseSource(typeof(TestData), nameof(TestData.MultiThreadCases))]
    public void TestMultiThread<T>(Func<T> function, object expectedResult, int numberOfGet, int numberOfThreads)
    {
        var testLazy = new MultiThreadLazy<T>(function);
        StartThreads(
            () => ThreadAction(testLazy, numberOfGet, expectedResult),
            numberOfThreads);
    }

    private static void StartThreads(Action threadAction, int numberOfThreads)
    {
        var threads = new Thread[numberOfThreads];
        for (int i = 0; i < numberOfThreads; i++)
        {
            threads[i] = new Thread(() => threadAction());
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private static void ThreadAction<T>(ILazy<T> testLazy, int numberOfGet, object expectedResult)
    {
        for (int i = 0; i < numberOfGet; i++)
        {
            Assert.That(testLazy.Get(), Is.EqualTo(expectedResult));
        }
    }

    private static void ThreadActionNullException<T>(ILazy<T> testLazy, int numberOfGet)
    {
        for (int i = 0; i < numberOfGet; i++)
        {
            Assert.Throws<ArgumentNullException>(() => testLazy.Get());
        }
    }

    /// <summary>
    /// Class of test data.
    /// </summary>
    public static class TestData
    {
        private const int NumberOfGets = 5;
        private static readonly Delegate[] Functions = [
            () => 1 + 2,
            () => "1" + "2",
            TestFunction];

        private static readonly object[] ExpectedResult = [
            3, "12", new float[] {0.1f, 0.4f, 1.5f, 6}];

        private static readonly int[] NumberOfThreads = [1, 5, 10];
        
        /// <summary>
        /// Cases for single-threaded lazy computation.
        /// </summary>
        /// <returns>Test case.</returns>
        public static IEnumerable<TestCaseData> SingleThreadCases()
        {
            for (int numberOfGet = 1; numberOfGet <= NumberOfGets; numberOfGet++)
            {
                for (int j = 0; j < 3; j++)
                {
                    yield return new TestCaseData(Functions[j], ExpectedResult[j], numberOfGet);
                }
            }
        }

        /// <summary>
        /// Cases for multi-threaded lazy computation.
        /// </summary>
        /// <returns>Test case.</returns>
        public static IEnumerable<TestCaseData> MultiThreadCases()
        {
            foreach (var number in NumberOfThreads)
            {
                for (int numberOfGet = 1; numberOfGet <= NumberOfGets; numberOfGet++)
                {
                    for (int j = 0; j < Functions.Length; j++)
                    {
                        yield return new TestCaseData(Functions[j], ExpectedResult[j], numberOfGet, number);
                    }
                }
            }
        }

        /// <summary>
        /// Cases of multithreaded lazy computation when null is returned.
        /// </summary>
        /// <returns>Test case.</returns>
        public static IEnumerable<TestCaseData> MultiThreadNullCases()
        {
            foreach (var number in NumberOfThreads)
            {
                for (int numberOfGet = 1; numberOfGet <= NumberOfGets; numberOfGet++)
                {
                    yield return new TestCaseData(numberOfGet, number);
                }
            }
        }

        private static float[] TestFunction()
        {
            int[] array1 = {1, 2, 3, 4};
            float[] array2 = {0.1f, 0.2f, 0.5f, 1.5f};
            var resultArray = new float[4];
            for (int i = 0; i < array1.Length; i++)
            {
                resultArray[i] = array1[i] * array2[i];
            }

            return resultArray;
        }
    }
}