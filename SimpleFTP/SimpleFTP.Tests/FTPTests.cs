// <copyright file="FTPTests.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace SimpleFTP.Tests;

using System.Net;

/// <summary>
/// Tests for FTP server.
/// </summary>
public class FTPTests
{
    private const int Port = 8888;

    private const string Host = "localhost";

    private static readonly object[] TestCase =
    [
        new object[]
        {
            "../../../TestFiles/TestFolder",
            new (string path, bool isDir)[]
            {
                ("../../../TestFiles/TestFolder/testFile2.txt", false),
                ("../../../TestFiles/TestFolder/testFile1.txt", false),
            },
        },
        new object[]
        {
            "../../../TestFiles",
            new (string path, bool isDir)[]
            {
                ("../../../TestFiles/test.bmp", false),
                ("../../../TestFiles/TestFolder", true),
            },
        },
    ];

    private Server server;

    /// <summary>
    /// Method for server start.
    /// </summary>
    [SetUp]
    public void StartServer()
    {
        server = new Server(IPAddress.Any, Port);
        server.Start();
    }

    /// <summary>
    /// Method for server shutdown.
    /// </summary>
    [TearDown]
    public void ShutdownServer() => server.Shutdown();

    /// <summary>
    /// Test list request.
    /// </summary>
    /// <param name="path">Path to get list of files.</param>
    /// <param name="expectedResult">Expected result.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    [TestCaseSource(nameof(TestCase))]
    public async Task TestList(string path, (string path, bool isDir)[] expectedResult)
    {
        var client = new Client(Host, Port);
        var response = await client.List(path);
        Assert.That(response.Length, Is.EqualTo(expectedResult.Length));
        foreach (var element in response)
        {
            Assert.IsTrue(expectedResult.Contains(element));
        }
    }

    /// <summary>
    /// Test get request.
    /// </summary>
    /// <param name="path">Path to file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [TestCase("../../../TestFiles/test.bmp")]
    [TestCase("../../../TestFiles/TestFolder/testFile1.txt")]
    public async Task TestGet(string path)
    {
        var expectedResult = File.ReadAllBytes(path);
        var client = new Client(Host, Port);
        var response = await client.Get(path);
        Assert.That(response, Is.EqualTo(expectedResult));
    }

    /// <summary>
    /// Test retrieving a list of files from a directory that does not exist.
    /// </summary>
    [Test]
    public void TestListNonExistentDirectory()
    {
        var client = new Client(Host, Port);
        Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await client.List("../../../TestFolder"));
    }

    /// <summary>
    /// Test retrieving the contents of a file on a path that does not exist.
    /// </summary>
    [Test]
    public void TestGetNonExistentDirectory()
    {
        var client = new Client(Host, Port);
        Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await client.Get("../../../TestFiles/testFile.txt"));
    }
}
