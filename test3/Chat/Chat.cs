// <copyright file="Chat.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace Chat;

using System.Net;
using System.Net.Sockets;

/// <summary>
/// Class that implements network chat.
/// </summary>
public class Chat()
{
    private static readonly CancellationTokenSource Cts = new();

    /// <summary>
    /// Server Start Method.
    /// </summary>
    /// <param name="port">Port.</param>
    /// <returns>Task.</returns>
    public static async Task StartServer(int port)
    {
        var listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine("Server is run!");
        while (!Cts.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync();
            Writer(client.GetStream());
            Reader(client.GetStream());
        }

        listener.Stop();
    }

    /// <summary>
    /// Client Start Method.
    /// </summary>
    /// <param name="ip">IP.</param>
    /// <param name="port">Port.</param>
    public static void StartClient(string ip, int port)
    {
        using (var client = new TcpClient(ip, port))
        {
            var stream = client.GetStream();
            Console.WriteLine("Connect!");
            Reader(stream);
            while (!Cts.IsCancellationRequested)
            {
                var writer = new StreamWriter(stream) { AutoFlush = true };
                Console.Write(">");
                var data = Console.ReadLine();
                if (data == "exit")
                {
                    Cts.Cancel();
                }

                writer.Write(data + "\n");
            }
        }
    }

    private static void Writer(NetworkStream stream)
    {
        Task.Run(async () =>
        {
            var writer = new StreamWriter(stream) { AutoFlush = true };
            while (!Cts.IsCancellationRequested)
            {
                Console.Write(">");
                var data = Console.ReadLine();
                if (data == "exit")
                {
                    Cts.Cancel();
                }

                await writer.WriteAsync(data + "\n");
            }
        });
    }

    private static void Reader(NetworkStream stream)
    {
        Task.Run(async () =>
        {
            var reader = new StreamReader(stream);
            while (!Cts.IsCancellationRequested)
            {
                var data = await reader.ReadToEndAsync();
                if (data == "exit")
                {
                    Cts.Cancel();
                }

                Console.WriteLine(data);
            }
        });
    }
}
