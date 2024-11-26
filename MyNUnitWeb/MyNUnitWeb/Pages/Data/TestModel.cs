namespace MyNUnitWeb.Data;

public class TestModel
{
    public int Id { get; set; }

    public int TestRunId { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Gets message to the test result.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets a value indicating whether the test has been successfully passed.
    /// </summary>
    public long Result { get; set; }

    /// <summary>
    /// Gets test execution time.
    /// </summary>
    public long Time { get; set; }

    /// <summary>
    /// Gets exception during the execution of the test.
    /// </summary>
    public string E { get; set; }
}
