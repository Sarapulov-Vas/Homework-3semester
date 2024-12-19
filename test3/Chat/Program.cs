// <copyright file="Program.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

if (args.Length == 1)
{
    if (int.TryParse(args[0], out int port))
    {
        await Chat.Chat.StartServer(port);
    }
    else
    {
        Console.WriteLine("Invalid argument");
    }
}
else if (args.Length == 2)
{
    if (int.TryParse(args[1], out int port))
    {
        Chat.Chat.StartClient(args[0], port);
    }
    else
    {
        Console.WriteLine("Invalid argument");
    }
}
else
{
    Console.WriteLine("Invalid argument");
}
