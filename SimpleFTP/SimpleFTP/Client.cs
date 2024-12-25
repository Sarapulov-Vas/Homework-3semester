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
    /// <summary>
    /// Method for requesting a list of the contents of a directory.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>List of contents if a directory.</returns>
    public async Task<(string path, bool isDir)[]> List(string path)
    {
        using var client = new TcpClient(hostName, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync($"1 {path}");
        writer.Flush();
        return await ProcessListResponse(stream);
    }

    /// <summary>
    /// A method to retrieve the contents of a file.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>The byte array of the file.</returns>
    public async Task<byte[]> Get(string path)
    {
        using var client = new TcpClient(hostName, port);
        var stream = client.GetStream();
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync($"2 {path}");
        await writer.FlushAsync();
        return ProcessGetResponse(stream);
    }

    private async Task<(string path, bool isDir)[]> ProcessListResponse(Stream stream)
    {
        var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();
        var responseElements = data.Split(" ");
        if (int.Parse(responseElements[0]) == -1)
        {
            throw new DirectoryNotFoundException("The directory does not exist.");
        }

        var response = new (string path, bool isDir)[int.Parse(responseElements[0])];
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = (responseElements[(i * 2) + 1], bool.Parse(responseElements[(i * 2) + 2]));
        }

        return response;
    }

    private byte[] ProcessGetResponse(Stream stream)
    {
        var reader = new BinaryReader(stream);
        var length = reader.ReadInt64();
        if (length == -1)
        {
            throw new DirectoryNotFoundException("The file does not exist.");
        }

        var response = new byte[length];
        for (int i = 0; i < length; i++)
        {
            response[i] = reader.ReadByte();
        }

        return response;
    }
}
