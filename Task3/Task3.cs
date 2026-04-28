using System.Text.Json;
using System.Text.Json.Nodes;

namespace PerformanceLabTest.Task3;

public static class Task3
{
    static void Main(string[] args)
    {
        using var writer = new StreamWriter(Console.OpenStandardOutput());

        if (args.Length != 3)
        {
            writer.WriteLine("Usage: PerformanceLabTest.exe valuesPath testsPath reportPath");
            return;
        }

        string valuesPath = args[0];
        string testsPath = args[1];
        string reportPath = args[2];

        Dictionary<int, JsonNode?> lookup;
        JsonNode? structureToProcess;
        JsonNode? testsRoot;

        try
        {
            lookup = new Dictionary<int, JsonNode?>();
            JsonNode? valuesRoot = JsonNode.Parse(File.ReadAllText(valuesPath));

            JsonArray? valuesArray = null;
            if (valuesRoot is JsonArray array)
            {
                valuesArray = array;
            }
            else if (valuesRoot is JsonObject obj && obj.TryGetPropertyValue("values", out JsonNode? valsNode) &&
                     valsNode is JsonArray valsArray)
            {
                valuesArray = valsArray;
            }

            if (valuesArray != null)
            {
                foreach (JsonNode? item in valuesArray)
                {
                    if (item is JsonObject objItem &&
                        objItem.TryGetPropertyValue("id", out JsonNode? idNode) && idNode != null &&
                        objItem.TryGetPropertyValue("value", out JsonNode? valueNode))
                    {
                        int id = idNode.GetValue<int>();
                        lookup[id] = valueNode?.DeepClone();
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading values file: {exception.Message}");
            return;
        }

        try
        {
            testsRoot = JsonNode.Parse(File.ReadAllText(testsPath));

            structureToProcess = testsRoot;
            if (testsRoot is JsonObject rootObj && rootObj.TryGetPropertyValue("tests", out JsonNode? testsNode) &&
                testsNode is JsonArray)
            {
                structureToProcess = testsNode;
            }
            else if (testsRoot is JsonArray)
            {
                structureToProcess = testsRoot;
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error reading tests file: {exception.Message}");
            return;
        }

        if (structureToProcess != null && lookup.Count > 0)
        {
            FillValues(structureToProcess, lookup);
        }

        try
        {
            JsonNode? reportRoot = testsRoot;
            if (testsRoot is JsonObject rootObjWithTests && rootObjWithTests.TryGetPropertyValue("tests", out _))
            {
                reportRoot = testsRoot;
            }
            else if (structureToProcess is JsonArray)
            {
                reportRoot = structureToProcess;
            }

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string reportJson = reportRoot?.ToJsonString(options) ?? "{}";
            File.WriteAllText(reportPath, reportJson);

            Console.WriteLine("Report saved in: " + reportPath);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Error in report file: {exception.Message}");
            return;
        }

        writer.Flush();
    }

    private static void FillValues(JsonNode? node, Dictionary<int, JsonNode?> lookup)
    {
        if (node == null) return;

        switch (node)
        {
            case JsonObject obj:
                if (obj.TryGetPropertyValue("id", out var idNode)
                    && idNode != null
                    && lookup.TryGetValue(idNode.GetValue<int>(), out var newValue))
                {
                    obj["value"] = newValue?.DeepClone();
                }

                foreach (var property in obj)
                {
                    if (property.Value is JsonObject or JsonArray)
                        FillValues(property.Value, lookup);
                }

                break;

            case JsonArray arr:
                foreach (JsonNode? item in arr)
                {
                    FillValues(item, lookup);
                }

                break;
        }
    }
}