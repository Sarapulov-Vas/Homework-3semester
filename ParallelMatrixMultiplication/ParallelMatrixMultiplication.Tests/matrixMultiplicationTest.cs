namespace ParallelMatrixMultiplication.Tests;

using MatrixGenerator;

/// <summary>
/// Test class.
/// </summary>
public class Tests
{
    /// <summary>
    /// Indexer testing.
    /// </summary>
    [Test]
    public void TestIndexer()
    {
        var matrix = new Matrix("../../../TestFiles/matrix1.txt");
        int k = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Assert.That(matrix[i, j], Is.EqualTo(k));
                k++;
            }
        }
    }

    /// <summary>
    /// Matrix equivalence test.
    /// </summary>
    /// <param name="inputFilePath">Path to the matrix file.</param>
    [TestCase("../../../TestFiles/matrix1.txt")]
    [TestCase("../../../TestFiles/matrix2.txt")]
    [TestCase("../../../TestFiles/matrix3.txt")]
    public void TestEquals(string inputFilePath)
    {
        Assert.IsTrue(Matrix.Equals(new Matrix(inputFilePath), new Matrix(inputFilePath)));
    }

    /// <summary>
    /// Matrix equivalence test.
    /// </summary>
    /// <param name="inputFilePath1">Path to the first matrix file.</param>
    /// <param name="inputFilePath2">Path to the second matrix file.</param>
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix2.txt")]
    [TestCase("../../../TestFiles/matrix2.txt", "../../../TestFiles/matrix3.txt")]
    [TestCase("../../../TestFiles/matrix3.txt", "../../../TestFiles/matrix1.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix4.txt")]
    public void TestNotEquals(string inputFilePath1, string inputFilePath2)
    {
        Assert.IsFalse(Matrix.Equals(new Matrix(inputFilePath1), new Matrix(inputFilePath2)));
    }

    /// <summary>
    /// Matrices multiplication test.
    /// </summary>
    /// <param name="inputFilePath1">Path to the first matrix file.</param>
    /// <param name="inputFilePath2">Path to the second matrix file.</param>
    /// <param name="expectedResult">Path to a file with the expected result.</param>
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix1.txt", "../../../TestFiles/expectedMatrix1_1.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix2.txt", "../../../TestFiles/expectedMatrix1_2.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix3.txt", "../../../TestFiles/expectedMatrix1_3.txt")]
    public void TestMultiplication(string inputFilePath1, string inputFilePath2, string expectedResult)
    {
        var firstMatrix = new Matrix(inputFilePath1);
        var secondMatrix = new Matrix(inputFilePath2);
        var result = Matrix.Multiplication(firstMatrix, secondMatrix);
        Assert.IsTrue(Matrix.Equals(result, new Matrix(expectedResult)));
    }

    /// <summary>
    /// Matrices multiplication test.
    /// </summary>
    /// <param name="inputFilePath1">Path to the first matrix file.</param>
    /// <param name="inputFilePath2">Path to the second matrix file.</param>
    /// <param name="expectedResult">Path to a file with the expected result.</param>
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix1.txt", "../../../TestFiles/expectedMatrix1_1.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix2.txt", "../../../TestFiles/expectedMatrix1_2.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix3.txt", "../../../TestFiles/expectedMatrix1_3.txt")]
    public void TestParallelMultiplication(string inputFilePath1, string inputFilePath2, string expectedResult)
    {
        var firstMatrix = new Matrix(inputFilePath1);
        var secondMatrix = new Matrix(inputFilePath2);
        var result = Matrix.ParallelMultiplication(firstMatrix, secondMatrix);
        Assert.IsTrue(Matrix.Equals(result, new Matrix(expectedResult)));
    }

    /// <summary>
    /// Test verifying the method of column counting.
    /// </summary>
    /// <param name="path">Path to the matrix file.</param>
    /// <param name="number">Expected number of columns.</param>
    [TestCase("../../../TestFiles/matrix1.txt", 3)]
    [TestCase("../../../TestFiles/matrix2.txt", 1)]
    [TestCase("../../../TestFiles/matrix3.txt", 5)]
    [TestCase("../../../TestFiles/matrix4.txt", 3)]
    public void TestGetColumnsCount(string path, int number)
    {
        var matrix = new Matrix(path);
        Assert.That(matrix.GetColumnsCount(), Is.EqualTo(number));
    }

    /// <summary>
    /// Test verifying the method of row counting.
    /// </summary>
    /// <param name="path">Path to the matrix file.</param>
    /// <param name="number">Expected number of rows.</param>
    [TestCase("../../../TestFiles/matrix1.txt", 3)]
    [TestCase("../../../TestFiles/matrix2.txt", 3)]
    [TestCase("../../../TestFiles/matrix3.txt", 3)]
    [TestCase("../../../TestFiles/matrix4.txt", 3)]
    public void TestGetRowsCount(string path, int number)
    {
        var matrix = new Matrix(path);
        Assert.That(matrix.GetRowsCount(), Is.EqualTo(number));
    }

    /// <summary>
    /// Exclusion test when matrices cannot be multiplied.
    /// </summary>
    [Test]
    public void TestThrowException()
    {
        var firstMatrix = new Matrix("../../../TestFiles/matrix2.txt");
        var secondMatrix = new Matrix("../../../TestFiles/matrix3.txt");
        Assert.Throws<ArgumentException>(() => Matrix.Multiplication(firstMatrix, secondMatrix));
    }

    /// <summary>
    /// Testing incorrect files.
    /// </summary>
    /// <param name="path">File path.</param>
    [TestCase("../../../TestFiles/incorrectFile1.txt")]
    [TestCase("../../../TestFiles/incorrectFile2.txt")]
    public void TestIncorrectFile(string path)
    {
        var firstMatrix = new Matrix("../../../TestFiles/matrix2.txt");
        Assert.Throws<IncorrectFileException>(() => new Matrix(path));
    }

    /// <summary>
    /// Test the method of exporting a matrix to a file.
    /// </summary>
    /// <param name="path">Path to the matrix file.</param>
    [TestCase("../../../TestFiles/matrix1.txt")]
    [TestCase("../../../TestFiles/matrix2.txt")]
    [TestCase("../../../TestFiles/matrix3.txt")]
    [TestCase("../../../TestFiles/matrix4.txt")]
    public void TestExport(string path)
    {
        var matrix = new Matrix(path);
        matrix.ExportToFile("../../../TestFiles/exportMatrix.txt");
        var exportMatrix = new Matrix("../../../TestFiles/exportMatrix.txt");
        Assert.IsTrue(Matrix.Equals(matrix, exportMatrix));
        File.Delete("../../../TestFiles/exportMatrix.txt");
    }

    /// <summary>
    /// Testing multithreaded matrix multiplication, using random matrix generation.
    /// </summary>
    [Test]
    public void RandomTestParallelMultiplication()
    {
        var size = 2;
        for (int i = 1; i <= 10; i++)
        {
            var firstMatrix = new Matrix(MatrixGenerator.Generate(size, size));
            var secondMatrix = new Matrix(MatrixGenerator.Generate(size, size));
            var expectedResult = Matrix.Multiplication(firstMatrix, secondMatrix);
            var result = Matrix.ParallelMultiplication(firstMatrix, secondMatrix);
            Assert.IsTrue(Matrix.Equals(result, expectedResult));
            size *= 2;
        }
    }
}
