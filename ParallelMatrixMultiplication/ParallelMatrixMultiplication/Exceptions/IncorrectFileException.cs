// <copyright file="IncorrectFileException.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace ParallelMatrixMultiplication;

/// <summary>
/// Exception for an invalid file.
/// </summary>
public class IncorrectFileException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectFileException"/> class.
    /// </summary>
    public IncorrectFileException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IncorrectFileException"/> class.
    /// </summary>
    /// <param name="message">Exception Message.</param>
    public IncorrectFileException(string message)
        : base(message)
    {
    }
}
