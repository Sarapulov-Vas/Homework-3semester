using System.Text;
var rand = new Random();
for (int i = 1; i <= 10; i++)
{
    var size = Math.Pow(2, i);
    var currentString = new StringBuilder();
    var resultStrings = new List<string>();

    for (int j = 0; j < size; j++)
    {
        for (int k = 0; k < size; k++)
        {
            currentString.Append($"{rand.Next(-10000, 10000)} ");
        }

        resultStrings.Add(currentString.ToString()[..^1]);
        currentString.Clear();
    }

    File.WriteAllLines($"firstMatrix{i}.txt", resultStrings);
    resultStrings.Clear();
    for (int j = 0; j < size; j++)
    {
        for (int k = 0; k < size; k++)
        {
            currentString.Append($"{rand.Next(-10000, 10000)} ");
        }

        resultStrings.Add(currentString.ToString()[..^1]);
        currentString.Clear();
    }

    File.WriteAllLines($"secondMatrix{i}.txt", resultStrings);
}
