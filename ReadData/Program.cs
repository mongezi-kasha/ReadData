using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string filePath = @"C:\Users\monge\Downloads\TestData.csv";
        FindTopScorers(filePath);
    }

    static void FindTopScorers(string filePath)
    {
        Dictionary<string, int> scores = new Dictionary<string, int>();

        // Read the CSV file and store scores in a dictionary
        using (var reader = new StreamReader(filePath))
        {
            var header = reader.ReadLine(); // Read the header row

            // Spliting by commas
            var columnNames = header.Split(',');

            
            int firstNameIndex = Array.IndexOf(columnNames, "First Name");
            int secondNameIndex = Array.IndexOf(columnNames, "Second Name");
            int scoreIndex = Array.IndexOf(columnNames, "Score");

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values.Length >= 3 && firstNameIndex >= 0 && secondNameIndex >= 0 && scoreIndex >= 0)
                {
                    string firstName = values[firstNameIndex];
                    string secondName = values[secondNameIndex];
                    string scoreStr = values[scoreIndex];

                    // Just doing some checks here 
                    if (scoreStr.All(char.IsDigit))
                    {
                        // Convert the score string to an integer
                        if (int.TryParse(scoreStr, out int score))
                        {
                            string fullName = $"{firstName} {secondName}";

                            if (scores.ContainsKey(fullName))
                            {
                                scores[fullName] = Math.Max(scores[fullName], score);
                            }
                            else
                            {
                                scores[fullName] = score;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid score format for {firstName} {secondName}. Skipping...");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid score format for {firstName} {secondName}. Skipping...");
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid line format: {line}. Skipping...");
                }
            }
        }

        // Here I am finding the highst score
        int maxScore = scores.Values.Where(value => value >= 0).DefaultIfEmpty(0).Max();

        // Find top scorers with the highest score and filter
        List<string> topScorers = scores.Where(pair => pair.Value == maxScore)
                                        .OrderBy(pair => pair.Value)
                                        .ThenBy(pair => pair.Key)
                                        .Select(pair => pair.Key)
                                        .ToList();

        // Output on Console
        foreach (var name in topScorers)
        {
            Console.WriteLine($"{name}");
            Console.WriteLine("");
        }
        Console.WriteLine($"Score: {maxScore}");
    }
}
