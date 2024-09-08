using ParallelMatrixMultiplication;
var firstMatrix = new Matrix("/home/sarapulov-vasilii/work/Homework-3semester/ParallelMatrixMultiplication/MatrixGenerator/firstMatrix10.txt");
var secondMatrix = new Matrix("/home/sarapulov-vasilii/work/Homework-3semester/ParallelMatrixMultiplication/MatrixGenerator/secondMatrix10.txt");
var result = Matrix.Multiplication(firstMatrix, secondMatrix);
result.Print();
