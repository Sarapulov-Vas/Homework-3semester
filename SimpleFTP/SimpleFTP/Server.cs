// <copyright file="Server.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace SimpleFTP;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Server class.
/// </summary>
/// <param name="address">Ip address.</param>
/// <param name="port">Port.</param>
public class Server(IPAddress address, int port)
{
    private readonly TcpListener tcpListener = new(address, port);

    private readonly CancellationTokenSource cancellation = new();
    private readonly List<Task> tasks = [];

    /// <summary>
    /// Server start method.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task Start()
    {
        tcpListener.Start();
        await Run();
    }

    /// <summary>
    /// A method for shutting down a server.
    /// </summary>
    /// <returns>Task.</returns>
    public async Task Shutdown()
    {
        cancellation.Cancel();
        await Task.WhenAll(tasks);
        tcpListener.Stop();
    }

    private async Task Run()
    {
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                var socket = await tcpListener.AcceptSocketAsync(cancellation.Token);
                var task = Task.Run(async () =>
                {
                    var stream = new NetworkStream(socket);
                    var reader = new StreamReader(stream);
                    var data = await reader.ReadLineAsync();
                    if (data == null)
                    {
                        return;
                    }

                    try
                    {
                        await ProcessRequest(data, stream);
                    }
                    finally
                    {
                        socket.Close();
                    }
                });
                tasks.Add(task);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task ProcessRequest(string request, Stream stream)
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
                RespondToGetRequest(elements[1], stream);
                break;
            default:
                RespondToInvalidRequest(stream);
                break;
        }
    }

    private async Task RespondToListRequest(string path, Stream stream)
    {
        var writer = new StreamWriter(stream);
        if (!Directory.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            await writer.FlushAsync();
            return;
        }

        var files = Directory.GetFileSystemEntries(path);
        StringBuilder response = new();
        response.Append(files.Length);
        foreach (var file in files)
        {
            response.Append($" {file.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)}" +
                $" {Directory.Exists(file)}");
        }

        await writer.WriteLineAsync(response.ToString());
        await writer.FlushAsync();
    }

    private void RespondToGetRequest(string path, Stream stream)
    {
        var writer = new BinaryWriter(stream);
        if (!File.Exists(path))
       {
            writer.Write(-1L);
            writer.Flush();
            return;
        }

        using var fileStream = new FileStream(path, FileMode.Open);
        writer.Write(fileStream.Length);
        for (int i = 0; i < fileStream.Length; i++)
        {
            writer.Write((byte)fileStream.ReadByte());
        }

        writer.Flush();
    }

    private void RespondToInvalidRequest(Stream stream)
    {
        var writer = new BinaryWriter(stream);
        writer.Write("-1");
        writer.Flush();
    }
}
