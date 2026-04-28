namespace PerformanceLabTest;

public static class Task1
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());
        
        if (args.Length == 4)
        {
            int n1 = int.Parse(args[0]);
            int m1 = int.Parse(args[1]);
            int n2 = int.Parse(args[2]);
            int m2 = int.Parse(args[3]);

            if (n1 > 0 && m1 > 0 && n2 > 0 && m2 > 0)
            {
                int[] array1 = new int[n1];
                int[] array2 = new int[n2];

                for (int i = 0; i < n1; i++)
                {
                    array1[i] = i + 1;
                }

                for (int i = 0; i < n2; i++)
                {
                    array2[i] = i + 1;
                }

                int c = 0;
                bool isEnd = false;
                while (isEnd == false)
                {
                    for (int i = 0; i < m1; i++)
                    {
                        int cycleIndex = CalculateCycleIndex(c, n1);

                        if (i == 0)
                        {
                           writer.Write(array1[cycleIndex]);
                        }
                        
                        if (i == m1 - 1)
                        {
                            if (array1[cycleIndex] == 1)
                            {
                                isEnd = true;
                            }
                            break;
                        }
                        
                        c++;
                    }
                }
                
                c = 0;
                isEnd = false;
                while (isEnd == false)
                {
                    for (int i = 0; i < m2; i++)
                    {
                        int cycleIndex = CalculateCycleIndex(c, n2);

                        if (i == 0)
                        {
                            writer.Write(array2[cycleIndex]);
                        }
                        
                        if (i == m2 - 1)
                        {
                            if (array2[cycleIndex] == 1)
                            {
                                isEnd = true;
                            }
                            break;
                        }
                        
                        c++;
                    }
                }
            }
            else
            {
                writer.WriteLine("All arguments MUST be positive..");
                writer.WriteLine("Usage: PerformanceLabTest.exe n1 m1 n2 m2");
            }
        }
        else
        {
            writer.WriteLine("No arguments provided..");
            writer.WriteLine("Usage: PerformanceLabTest.exe n1 m1 n2 m2");
        }
        
        writer.Flush();
    }

    private static int CalculateCycleIndex(int index, int size)
    {
        return index % size;
    }
}