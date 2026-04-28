namespace PerformanceLabTest.Task4;

public static class Task4
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());

        if (args.Length != 1)
        {
            writer.WriteLine("Usage: PerformanceLabTest.exe filePath");
            return;
        }

        string filePath = args[0];

        int[] nums;
        try
        {
            nums = File.ReadAllLines(filePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => int.Parse(line.Trim()))
                .ToArray();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading file: {exception.Message}");
            return;
        }

        if (nums.Length == 0)
        {
            writer.WriteLine("File is empty or contains no valid numbers.");
            return;
        }

        Array.Sort(nums);

        int median = nums[nums.Length / 2];

        long totalMoves = nums.Sum(x => Math.Abs(x - median));

        if (totalMoves <= 20)
        {
            writer.WriteLine(totalMoves);
        }
        else
        {
            writer.WriteLine(
                "20 moves are not enough to reduce all elements of the array to the same number.");
        }

        writer.Flush();
    }
}