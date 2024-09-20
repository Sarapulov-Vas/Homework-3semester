// <copyright file="Matrix.cs" company="Sarapulov Vasilii">
// Copyright (c) Sarapulov Vasilii. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the repository root for license information.
// https://github.com/Sarapulov-Vas/Homework-3semester/blob/main/LICENSE
// </copyright>

namespace ParallelMatrixMultiplication;
using System.Text;

/// <summary>
/// A class that implements a matrix.
/// </summary>
public class Matrix
{
    private readonly List<int[]> matrix = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="path">The path to the matrix file.</param>
    public Matrix(string path)
    {
        var fileText = File.ReadAllLines(path);
        if (fileText.Length == 0)
        {
            throw new IncorrectFileException("Empty file.");
        }

        List<int> buffer = [];
        foreach (var line in fileText)
        {
            var splitLine = line.Split(' ');
            if (this.matrix.Count != 0 && this.matrix[0].Length != splitLine.Length)
            {
                throw new IncorrectFileException("Incorrect matrix.");
            }

            foreach (var num in splitLine)
            {
                if (int.TryParse(num, out int number))
                {
                    buffer.Add(number);
                }
                else
                {
                    throw new IncorrectFileException("File contains foreign characters.");
                }
            }

            this.matrix.Add(buffer.ToArray());
            buffer.Clear();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// </summary>
    /// <param name="matrix">Matrix.</param>
    public Matrix(int[,] matrix)
    {
        List<int> buffer = [];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                buffer.Add(matrix[i, j]);
            }

            this.matrix.Add(buffer.ToArray());
            buffer.Clear();
        }
    }

    /// <summary>
    /// Indexer to access the elements of a matrix.
    /// </summary>
    /// <param name="i">First index.</param>
    /// <param name="j">Second index.</param>
    /// <returns>Value of a matrix element.</returns>
    public int this[int i, int j]
    {
        get => this.matrix[i][j];
        set => this.matrix[i][j] = value;
    }

    /// <summary>
    /// Method implementing matrix multiplication.
    /// </summary>
    /// <param name="firstMatrix">First matrix.</param>
    /// <param name="secondMatrix">Second matrix.</param>
    /// <returns>The result of matrix multiplication.</returns>
    /// <exception cref="ArgumentException">Exception if matrices cannot be multiplied.</exception>
    public static Matrix Multiplication(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.ColumnsCount != secondMatrix.RowsCount)
        {
            throw new ArgumentException("The number of columns of the first matrix is not equal to the number of rows of the second matrix.");
        }

        var currentMatrix = new int[firstMatrix.RowsCount, secondMatrix.ColumnsCount];

        for (int i = 0; i < firstMatrix.RowsCount; i++)
        {
            for (int j = 0; j < secondMatrix.ColumnsCount; j++)
            {
                for (int k = 0; k < secondMatrix.RowsCount; k++)
                {
                    currentMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        var result = new Matrix(currentMatrix);
        return result;
    }

    /// <summary>
    /// Method implementing multithreaded matrix multiplication.
    /// </summary>
    /// <param name="firstMatrix">First matrix.</param>
    /// <param name="secondMatrix">Second matrix.</param>
    /// <returns>The result of matrix multiplication.</returns>
    /// <exception cref="ArgumentException">Exception if matrices cannot be multiplied.</exception>
    public static Matrix ParallelMultiplication(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (firstMatrix.ColumnsCount != secondMatrix.RowsCount)
        {
            throw new ArgumentException("The number of columns of the first matrix is not equal to the number of rows of the second matrix.");
        }

        var threadCount = Math.Min(Environment.ProcessorCount, firstMatrix.RowsCount);
        var threads = new Thread[threadCount];
        var chunkSize = (firstMatrix.RowsCount / threads.Length) + 1;

        var currentMatrix = new int[firstMatrix.RowsCount, secondMatrix.ColumnsCount];

        for (int i = 0; i < threads.Length; i++)
        {
            var local = i;
            threads[i] = new Thread(() =>
            {
                for (int j = local * chunkSize;
                    j < (local + 1) * chunkSize && j < firstMatrix.RowsCount; j++)
                {
                    for (int k = 0; k < secondMatrix.ColumnsCount; k++)
                    {
                        for (int p = 0; p < secondMatrix.RowsCount; p++)
                        {
                            currentMatrix[j, k] += firstMatrix[j, p] * secondMatrix[p, k];
                        }
                    }
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return new Matrix(currentMatrix);
    }

    /// <summary>
    /// A method for checking the equivalence of matrices.
    /// </summary>
    /// <param name="first">first matrix.</param>
    /// <param name="second">second matrix.</param>
    /// <returns>Are matrices equivalent.</returns>
    public static bool Equals(Matrix first, Matrix second)
    {
        if (first.RowsCount != second.RowsCount ||
            first.ColumnsCount != second.ColumnsCount)
        {
            return false;
        }

        for (int i = 0; i < first.RowsCount; i++)
        {
            for (int j = 0; j < first.ColumnsCount; j++)
            {
                if (first[i, j] != second[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Matrix printing method.
    /// </summary>
    public void Print()
    {
        foreach (var line in this.matrix)
        {
            foreach (var num in line)
            {
                Console.Write($"{num} ");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Gets rows count.
    /// </summary>
    public int RowsCount => this.matrix.Count;

    /// <summary>
    /// Gets number of columns.
    /// </summary>
    /// <returns>Number of columns.</returns>
    public int ColumnsCount => this.matrix[0].Length;

    /// <summary>
    /// Method of unloading the matrix to a file.
    /// </summary>
    /// <param name="path">File path.</param>
    public void ExportToFile(string path)
    {
        var resultStrings = new List<string>();
        var currentString = new StringBuilder();
        foreach (var line in this.matrix)
        {
            foreach (var element in line)
            {
                currentString.Append($"{element} ");
            }

            resultStrings.Add(currentString.ToString()[..^1]);
            currentString.Clear();
        }

        File.WriteAllLines(path, resultStrings);
    }
}
