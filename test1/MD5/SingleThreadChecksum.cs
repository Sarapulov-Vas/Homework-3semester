// <copyright file="SingleThreadChecksum.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MD5Checksum;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// A class of single-threaded checksum calculation.
/// </summary>
public class SingleThreadChecksum() : ICheckSum
{
    /// <inheritdoc/>
    public byte[] CalculateCheckSum(string path)
    {
        if (File.Exists(path))
        {
            return FileChecksum(path);
        }
        else if (Directory.Exists(path))
        {
            return DirectoryChecksum(path);
        }
        else
        {
            throw new ArgumentException("File/Directory does not exist");
        }
    }

    private static byte[] FileChecksum(string path) => MD5.HashData(new FileStream(path, FileMode.Open));

    private byte[] DirectoryChecksum(string path)
    {
        var internalFilesCheksum = Encoding.ASCII.GetBytes(Path.GetDirectoryName(path) !);
        var paths = Directory.GetFileSystemEntries(path);
        if (paths is not null)
        {
            Array.Sort(paths);
            foreach (var directoryPath in paths)
            {
                if (File.Exists(directoryPath))
                {
                    internalFilesCheksum.Concat(FileChecksum(directoryPath));
                }
                else
                {
                    internalFilesCheksum.Concat(DirectoryChecksum(directoryPath));
                }
            }
        }

        return MD5.HashData(internalFilesCheksum);
    }
}
