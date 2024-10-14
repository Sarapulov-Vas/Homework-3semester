// <copyright file="Server.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace SimpleFTP;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// Server class.
/// </summary>
/// <param name="address">Ip address.</param>
/// <param name="port">Port.</param>
public class Server(IPAddress address, int port)
{
    private readonly TcpListener tcpListener = new (address, port);

    /// <summary>
    /// Server start method.
    /// </summary>
    public void Start()
    {
        tcpListener.Start();
        Run();
    }

    /// <summary>
    /// A method for shutting down a server.
    /// </summary>
    public void Shutdown()
    {
        tcpListener.Stop();
    }

    private async Task Run()
    {
        while (true)
        {
            var socket = await tcpListener.AcceptSocketAsync();
            Task.Run(async () =>
            {
                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                var data = await reader.ReadLineAsync();
                if (data == null)
                {
                    return;
                }

                ProcessRequest(data, stream);
                socket.Close();
            });
        }
    }

    private async void ProcessRequest(string request, Stream stream)
    {
        var elements = request.Split(' ');
        if (elements.Length != 2)
        {
            return;
        }

        switch (elements[0])
        {
            case "1":
                await RespondToListRequest(elements[1], stream);
                break;
            case "2":
                await RespondToGetRequest(elements[1], stream);
                break;
            default:
                throw new ArgumentException("Invalid request type.");
        }
    }

    private async Task RespondToListRequest(string path, Stream stream)
    {
        var writer = new StreamWriter(stream);
        if (!Directory.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }

        var files = Directory.GetFileSystemEntries(path);
        await writer.WriteAsync(files.Length.ToString());
        foreach (var file in files)
        {
            await writer.WriteAsync($" {file} {Directory.Exists(file)}");
        }

        await writer.WriteLineAsync();
    }

    private async Task RespondToGetRequest(string path, Stream stream)
    {
        var writer = new StreamWriter(stream);
        if (!File.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }

        using var fileStream = new FileStream(path, FileMode.Open);
        await writer.WriteAsync(fileStream.Length.ToString());
        for (int i = 0; i < fileStream.Length; i++)
        {
            await writer.WriteAsync($" {fileStream.ReadByte()}");
        }

        await writer.WriteLineAsync();
    }
}
