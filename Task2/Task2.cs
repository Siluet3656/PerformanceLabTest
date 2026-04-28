namespace PerformanceLabTest.Task2;

public static class Task2
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());

        if (args.Length != 2)
        {
            writer.WriteLine("Wrong arguments provided..");
            writer.WriteLine("Usage: PerformanceLabTest.exe ellipseFilePath pointsFilePath");
            return;
        }

        string ellipseFile = args[0];
        string pointsFile = args[1];

        Ellipse ellipse;

        try
        {
            string ellipseContent = File.ReadAllText(ellipseFile);

            string[] numbers = ellipseContent.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            ellipse = new Ellipse(double.Parse(numbers[0]),
                double.Parse(numbers[1]),
                double.Parse(numbers[2]),
                double.Parse(numbers[3]));
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading ellipse file: {exception.Message}");
            return;
        }

        List<PointDouble> points = new List<PointDouble>();

        try
        {
            string pointsContent = File.ReadAllText(pointsFile);

            string[] pointsCoordinates = pointsContent.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (string coordinate in pointsCoordinates)
            {
                string[] numbers = coordinate.Split([' '], StringSplitOptions.RemoveEmptyEntries);

                if (numbers.Length != 2)
                {
                    continue;
                }

                points.Add(new PointDouble(double.Parse(numbers[0]), double.Parse(numbers[1])));
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading points file: {exception.Message}");
            return;
        }

        foreach (PointDouble point in points)
        {
            int result =CheckPoint(ellipse, point);
            writer.WriteLine(result);
        }

        writer.Flush();
    }

    private struct Ellipse(double xCenter, double yCenter, double xCoordinate, double yCoordinate)
    {
        public double XCenter = xCenter;
        public double YCenter = yCenter;
        public double XCoordinate = xCoordinate;
        public double YCoordinate = yCoordinate;
    }

    private struct PointDouble(double x, double y)
    {
        public double X = x;
        public double Y = y;
    }

    private static int CheckPoint(Ellipse ellipse, PointDouble point)
    {
        double a = Math.Abs(ellipse.XCoordinate - ellipse.XCenter);
        double b = Math.Abs(ellipse.YCoordinate - ellipse.YCenter);
        double dx = point.X - ellipse.XCenter;
        double dy = point.Y - ellipse.YCenter;

        if (a == 0.0 && b == 0.0)
        {
            if (dx == 0.0 && dy == 0.0) return 0;
            return 2;
        }
        if (a == 0.0)
        {
            if (dx != 0.0) return 2;
            double absDy = Math.Abs(dy);
            if (absDy < b) return 1;
            if (absDy == b) return 0;
            return 2;
        }
        if (b == 0.0)
        {
            if (dy != 0.0) return 2;
            double absDx = Math.Abs(dx);
            if (absDx < a) return 1;
            if (absDx == a) return 0;
            return 2;
        }
        
        double left = dx * dx * b * b + dy * dy * a * a;
        double right = a * a * b * b;
        double diff = left - right;
        const double eps = 1e-12;
        double tolerance = eps * right;

        if (Math.Abs(diff) <= tolerance)
            return 0;
        else if (diff < 0)
            return 1;
        else
            return 2;
    }
}