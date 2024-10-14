// <copyright file="Client.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace SimpleFTP;

using System.Net.Sockets;

/// <summary>
/// Client class.
/// </summary>
/// <param name="hostName">Host name.</param>
/// <param name="port">Port.</param>
public class Client(string hostName, int port)
{
    private readonly string hostName = hostName;

    private readonly int port = port;

    /// <summary>
    /// Method for requesting a list of the contents of a directory.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>List of contents if a directory.</returns>
    public async Task<(string path, bool isDir)[]?> List(string path)
    {
    using var client = new TcpClient(hostName, port);
    var stream = client.GetStream();
    var writer = new StreamWriter(stream);
    writer.WriteLineAsync($"1 {path}");
    writer.Flush();
    return await ProcessListResponse(stream);
    }

    /// <summary>
    /// A method to retrieve the contents of a file.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>The byte array of the file.</returns>
    public async Task<byte[]?> Get(string path)
    {
    using var client = new TcpClient(hostName, port);
    var stream = client.GetStream();
    var writer = new StreamWriter(stream);
    writer.WriteLineAsync($"2 {path}");
    writer.Flush();
    return await ProcessGetResponse(stream);
    }

    private async Task<(string path, bool isDir)[]?> ProcessListResponse(Stream stream)
    {
        var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();
        var elements = data.Split(" ");
        if (elements[0] == "-1")
        {
            return null;
        }

        var response = new (string path, bool isDir)[int.Parse(elements[0])];
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = (elements[(i * 2) + 1], bool.Parse(elements[(i * 2) + 2]));
        }

        return response;
    }

    private async Task<byte[]?> ProcessGetResponse(Stream stream)
    {
        var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();
        var elements = data.Split(" ");
        if (elements[0] == "-1")
        {
            return null;
        }

        var response = new byte[int.Parse(elements[0])];
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = byte.Parse(elements[i + 1]);
        }

        return response;
    }
}
