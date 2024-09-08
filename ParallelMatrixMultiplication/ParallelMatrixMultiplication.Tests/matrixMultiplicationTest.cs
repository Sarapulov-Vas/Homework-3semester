using ParallelMatrixMultiplication;
namespace ParallelMatrixMultiplication.Tests;

public class Tests
{
    [Test]
    public void TestIterator()
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

    [TestCase("../../../TestFiles/matrix1.txt")]
    [TestCase("../../../TestFiles/matrix2.txt")]
    [TestCase("../../../TestFiles/matrix3.txt")]
    public void TestEquals(string inputFilePath)
    {
        Assert.IsTrue(Matrix.Equals(new Matrix(inputFilePath), new Matrix(inputFilePath)));
    }

    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix2.txt")]
    [TestCase("../../../TestFiles/matrix2.txt", "../../../TestFiles/matrix3.txt")]
    [TestCase("../../../TestFiles/matrix3.txt", "../../../TestFiles/matrix1.txt")]
    [TestCase("../../../TestFiles/matrix1.txt", "../../../TestFiles/matrix4.txt")]
    public void TestNotEquals(string inputFilePath1, string inputFilePath2)
    {
        Assert.IsFalse(Matrix.Equals(new Matrix(inputFilePath1), new Matrix(inputFilePath2)));
    }

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
}
