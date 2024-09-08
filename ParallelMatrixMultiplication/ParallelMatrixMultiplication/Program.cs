using ParallelMatrixMultiplication;
var firstMatrix = new Matrix("/home/sarapulov-vasilii/work/Homework-3semester/ParallelMatrixMultiplication/test1.txt");
var secondMatrix = new Matrix("/home/sarapulov-vasilii/work/Homework-3semester/ParallelMatrixMultiplication/test2.txt");
var result = Matrix.Multiplication(firstMatrix, secondMatrix);
result.Print();
