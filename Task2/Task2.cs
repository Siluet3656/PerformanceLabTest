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

            if (!double.TryParse(numbers[0], out double cx) ||
                !double.TryParse(numbers[1], out double cy) ||
                !double.TryParse(numbers[2], out double rx) ||
                !double.TryParse(numbers[3], out double ry))
            {
                writer.WriteLine("Invalid number format in ellipse file.");
                return;
            }

            rx = Math.Abs(rx);
            ry = Math.Abs(ry);

            ellipse = new Ellipse(cx, cy, rx, ry);
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

                if (!double.TryParse(numbers[0], out double x) ||
                    !double.TryParse(numbers[1], out double y))
                {
                    writer.WriteLine("Invalid number in points file.");
                    continue;
                }

                points.Add(new PointDouble(x, y));
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading points file: {exception.Message}");
            return;
        }

        foreach (PointDouble point in points)
        {
            int result = CheckPoint(ellipse, point);
            writer.WriteLine(result);
        }

        writer.Flush();
    }

    private struct Ellipse(double xCenter, double yCenter, double xCoordinate, double yCoordinate)
    {
        public readonly double CenterX = xCenter;
        public readonly double CenterY = yCenter;
        public readonly double RadiusX = xCoordinate;
        public readonly double RadiusY = yCoordinate;
    }

    private struct PointDouble(double x, double y)
    {
        public readonly double X = x;
        public readonly double Y = y;
    }

    private static int CheckPoint(Ellipse ellipse, PointDouble point)
    {
        double a = ellipse.RadiusX;
        double b = ellipse.RadiusY;
        double dx = point.X - ellipse.CenterX;
        double dy = point.Y - ellipse.CenterY;
        const double eps = 1e-6;

        if (a == 0.0 && b == 0.0)
        {
            double dist = Math.Sqrt(dx * dx + dy * dy);
            return dist <= eps ? 0 : 2;
        }

        if (a == 0.0)
        {
            double tol = eps * Math.Max(b, 1.0);
            if (Math.Abs(dx) > tol)
                return 2;
            double absDy = Math.Abs(dy);
            if (Math.Abs(absDy - b) <= tol)
                return 0;
            else if (absDy < b)
                return 1;
            else
                return 2;
        }

        if (b == 0.0)
        {
            double tol = eps * Math.Max(a, 1.0);
            if (Math.Abs(dy) > tol)
                return 2;
            double absDx = Math.Abs(dx);
            if (Math.Abs(absDx - a) <= tol)
                return 0;
            else if (absDx < a)
                return 1;
            else
                return 2;
        }

        double left = dx * dx * b * b + dy * dy * a * a;
        double right = a * a * b * b;
        double diff = left - right;
        double tolerance = eps * right;

        if (Math.Abs(diff) <= tolerance)
            return 0;
        else if (diff < 0)
            return 1;
        else
            return 2;
    }
}