namespace PerformanceLabTest.Task1;

public static class Task1
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());

        if (args.Length != 4)
        {
            writer.WriteLine("Wrong arguments provided..");
            writer.WriteLine("Usage: PerformanceLabTest.exe n1 m1 n2 m2");
            writer.Flush();
            return;
        }
        
        if (!int.TryParse(args[0], out int n1) || !int.TryParse(args[1], out int m1) ||
            !int.TryParse(args[2], out int n2) || !int.TryParse(args[3], out int m2))
        {
            writer.WriteLine("All arguments must be valid integers..");
            writer.WriteLine("Usage: PerformanceLabTest.exe n1 m1 n2 m2");
            writer.Flush();
            return;
        }

        if (n1 <= 0 || m1 <= 0 || n2 <= 0 || m2 <= 0)
        {
            writer.WriteLine("All arguments MUST be positive..");
            writer.WriteLine("Usage: PerformanceLabTest.exe n1 m1 n2 m2");
            writer.Flush();
            return;
        }

        Task<string> task1 = Task.Run(() => CalculatePath(n1, m1));
        Task<string> task2 = Task.Run(() => CalculatePath(n2, m2));

        Task.WaitAll(task1, task2);
        
        writer.Write(task1.Result + task2.Result);

        writer.Flush();
    }

    private static string CalculatePath(int n, int m)
    {
        string result = "";
        int current = 1;
        do
        {
            result += current;
            int next = (current + m - 1) % n;
            if (next == 0) next = n;
            current = next;
        } while (current != 1);

        return result;
    }
}