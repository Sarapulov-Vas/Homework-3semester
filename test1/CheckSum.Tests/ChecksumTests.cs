// <copyright file="ChecksumTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

using MD5Checksum;

namespace CheckSum.Tests;

/// <summary>
/// Tests for checksum classes.
/// </summary>
public class ChecksumTests
{
    /// <summary>
    /// Test for singlethread checksum.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <param name="expectedResult">Expected result.</param>
    [TestCase("../../../TestFiles/TestFile1.txt", new byte[] { 87, 207, 162, 71, 59, 104, 15, 72, 167, 12, 53, 12, 9, 199, 197, 110 })]
    [TestCase("../../../TestFiles/TestFolder/test.bmp", new byte[] { 51, 17, 240, 157, 199, 50, 247, 111, 210, 73, 176, 132, 179, 166, 66, 36 })]
    [TestCase("../../../TestFiles/TestFolder", new byte[] { 196, 166, 240, 76, 30, 128, 65, 70, 103, 189, 203, 168, 181, 191, 132, 201 })]
    [TestCase("../../../TestFiles", new byte[] { 108, 25, 101, 19, 131, 143, 172, 200, 168, 80, 187, 21, 237, 14, 75, 81 })]
    public void SingleThreadChecksumTest(string path, byte[] expectedResult)
    {
        var checksum = new SingleThreadChecksum();
        Assert.That(checksum.CalculateCheckSum(path), Is.EqualTo(expectedResult));
    }

    /// <summary>
    /// Test for multithread checksum.
    /// </summary>
    /// <param name="path">Path.</param>
    [TestCase("../../../TestFiles/TestFolder/test.bmp")]
    [TestCase("../../../TestFiles/TestFile1.txt")]
    [TestCase("../../../TestFiles/TestFolder")]
    [TestCase("../../../TestFiles")]
    public void MultiThreadChecksumTest(string path)
    {
        var singleThreadChecksum = new SingleThreadChecksum();
        var multiThreadChecksum = new SingleThreadChecksum();

        Assert.That(singleThreadChecksum.CalculateCheckSum(path), Is.EqualTo(multiThreadChecksum.CalculateCheckSum(path)));
    }

    /// <summary>
    /// Test for throw exception with dose not exist directory/file.
    /// </summary>
    [Test]
    public void NotExistDirectoryTest()
    {
        var singleThreadChecksum = new SingleThreadChecksum();
        Assert.Throws<ArgumentException>(() => singleThreadChecksum.CalculateCheckSum("123"));

        var multiThreadCheckSum = new MultiThreadCheckSum();
        Assert.Throws<ArgumentException>(() => multiThreadCheckSum.CalculateCheckSum("123"));
    }
}
