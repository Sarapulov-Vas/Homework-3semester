// <copyright file="ICheckSum.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace MD5Checksum;

/// <summary>
/// Interface for checksum calculation.
/// </summary>
public interface ICheckSum
{
    /// <summary>
    /// A method for calculating the checksum.
    /// </summary>
    /// <param name="path">Path.</param>
    /// <returns>Checksum.</returns>
    public byte[] CalculateCheckSum(string path);
}
