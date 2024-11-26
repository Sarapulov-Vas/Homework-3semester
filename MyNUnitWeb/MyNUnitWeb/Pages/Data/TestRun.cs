namespace MyNUnitWeb.Data;

public class TestRun
{
    public int Id { get; set; }
    public int NumberPassedTests { get; set; }
    public int NumberFailedTests { get; set; }
    public int NumberIgnoredTests { get; set; }
    public int NumberTests { get; set; }
}
