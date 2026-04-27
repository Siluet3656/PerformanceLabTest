namespace PerformanceLabTest;

public static class Program
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());
        
        if (args.Length > 0)
        {
            
            foreach (string variable in args)
            {
                writer.WriteLine(variable);
            }
        }
        else
        {
            writer.WriteLine("No arguments provided..");
            writer.WriteLine("Usage: PerformanceLabTest.exe arg1 arg2 arg3");
            return;
        }
        
        writer.Flush();
    }
}