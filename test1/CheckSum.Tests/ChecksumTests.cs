// <copyright file="ChecksumTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace CheckSum.Tests;
using MD5Checksum;

/// <summary>
/// Tests for checksum classes.
/// </summary>
public class ChecksumTests
{
    /// <summary>
    /// Test for multithread checksum.
    /// </summary>
    /// <param name="path">Path.</param>
    [TestCase("../../../TestFiles/TestFolder/test.bmp")]
    [TestCase("../../../TestFiles/TestFile1.txt")]
    [TestCase("../../../TestFiles/TestFolder")]
    [TestCase("../../../TestFiles")]
    public async Task MultiThreadChecksumTest(string path)
    {
        var singleThreadChecksum = new SingleThreadChecksum();
        var singleThreadResult = singleThreadChecksum.CalculateCheckSum(path).Result;
        var multiThreadChecksum = new MultiThreadChecksum();
        var multiThreadResult = await multiThreadChecksum.CalculateCheckSum(path);

        Assert.That(singleThreadResult, Is.EqualTo(multiThreadResult));
    }

    /// <summary>
    /// Test for throw exception with dose not exist directory/file.
    /// </summary>
    [Test]
    public void NotExistDirectoryTest()
    {
        var singleThreadChecksum = new SingleThreadChecksum();
        Assert.Throws<ArgumentException>(() => singleThreadChecksum.CalculateCheckSum("123"));

        var multiThreadChecksum = new MultiThreadChecksum();
        Assert.ThrowsAsync<ArgumentException>(async () => await multiThreadChecksum.CalculateCheckSum("123"));
    }
}
