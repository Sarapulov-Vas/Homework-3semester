namespace ParallelMatrixMultiplication;

public class IncorrectFileException : Exception
{
    public IncorrectFileException()
    {
    }

    public IncorrectFileException(string message)
        : base(message)
    {
    }
}
