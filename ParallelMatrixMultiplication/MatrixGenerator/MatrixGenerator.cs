// <copyright file="MatrixGenerator.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace MatrixGenerator;

using System.Text;

/// <summary>
/// A class implementing a file generator with two matrices for multiplication.
/// </summary>
public static class MatrixGenerator
{
    private static Random rand = new ();

    /// <summary>
    /// Method implementing matrix generation.
    /// </summary>
    /// <param name="path">File path.</param>
    /// <param name="size1">Matrix size1.</param>
    /// <param name="size2">Matrix size2.</param>
    public static void GenerateFile(string path, int size1, int size2)
    {
        var currentString = new StringBuilder();
        var resultStrings = new List<string>();

        for (int i = 0; i < size1; i++)
        {
            for (int j = 0; j < size2; j++)
            {
                currentString.Append($"{rand.Next(-10000, 10000)} ");
            }

            resultStrings.Add(currentString.ToString()[..^1]);
            currentString.Clear();
        }

        File.WriteAllLines(path, resultStrings);
    }

    /// <summary>
    /// Method implementing matrix generation.
    /// </summary>
    /// <param name="size1">Matrix size1.</param>
    /// <param name="size2">Matrix size2.</param>
    /// <returns>Matrix.</returns>
    public static int[,] Generate(int size1, int size2)
    {
        var matrix = new int[size1, size2];
        for (int i = 0; i < size1; i++)
        {
            for (int j = 0; j < size2; j++)
            {
                matrix[i, j] = rand.Next(-10000, 10000);
            }
        }

        return matrix;
    }
}
