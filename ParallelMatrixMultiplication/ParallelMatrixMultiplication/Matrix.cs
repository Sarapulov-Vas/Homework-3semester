using System.Text;

namespace ParallelMatrixMultiplication;

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
            throw new IncorrectFileException();
        }

        foreach (var line in fileText)
        {
            var splitLine = line.Split(' ');
            List<int> buffer = [];
            foreach (var num in splitLine)
            {
                if (int.TryParse(num, out int number))
                {
                    buffer.Add(number);
                }
                else
                {
                    throw new IncorrectFileException();
                }
            }

            this.matrix.Add(buffer.ToArray());
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
        get
        {
            return this.matrix[i][j];
        }

        set
        {
            this.matrix[i][j] = value;
        }
    }

    /// <summary>
    /// Method implementing matrix multiplication.
    /// </summary>
    /// <param name="firstMatrix">First matrix.</param>
    /// <param name="secondMatrix">Second matrix.</param>
    /// <returns>The result of matrix multiplication.</returns>
    public static Matrix Multiplication(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (!MatrixCheck(firstMatrix, secondMatrix))
        {
            throw new ArgumentException("The number of columns of the first matrix is not equal to the number of rows of the second matrix.");
        }

        var currentMatrix = new int[firstMatrix.GetRowsCount(), secondMatrix.GetColumnsCount()];

        for (int i = 0; i < firstMatrix.GetRowsCount(); i++)
        {
            for (int j = 0; j < secondMatrix.GetColumnsCount(); j++)
            {
                for (int k = 0; k < secondMatrix.GetRowsCount(); k++)
                {
                    currentMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                }
            }
        }

        var result = new Matrix(currentMatrix);
        result.ExportToFile("result.txt");
        return result;
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
    /// Method to get the number of rows.
    /// </summary>
    /// <returns>Number of rows in the matrix.</returns>
    public int GetRowsCount() => this.matrix.Count;

    /// <summary>
    /// Method to get the number of columns.
    /// </summary>
    /// <returns>Number of columns.</returns>
    public int GetColumnsCount() => this.matrix[0].Length;

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

    /// <summary>
    /// A method of checking matrices for the possibility of multiplication.
    /// </summary>
    /// <param name="firstMatrix">First matrix.</param>
    /// <param name="secondMatrix">Second matrix.</param>
    /// <returns>Is it possible to multiply matrices.</returns>
    private static bool MatrixCheck(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.GetColumnsCount() == secondMatrix.GetRowsCount();
    }
}