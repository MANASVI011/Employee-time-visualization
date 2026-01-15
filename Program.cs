using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeTimeTracker;

namespace EmployeeTimeTracker;

class Program
{
    static async Task Main(string[] args)
    {
        ApiService? apiService = null;
        
        try
        {
            Console.WriteLine("Fetching employee time entries from API...");
            
            apiService = new ApiService();
            var timeEntries = await apiService.GetTimeEntriesAsync();
            
            Console.WriteLine($"Retrieved {timeEntries.Count} time entries.");
            
            Console.WriteLine("Processing data...");
            var processor = new DataProcessor();
            var employeeSummaries = processor.ProcessTimeEntries(timeEntries);
            
            Console.WriteLine($"Processed {employeeSummaries.Count} employees.");
            
            Console.WriteLine("Generating HTML report...");
            var htmlGenerator = new HtmlGenerator();
            var html = htmlGenerator.GenerateHtml(employeeSummaries);
            
            var outputFile = "employees.html";
            await File.WriteAllTextAsync(outputFile, html);
            
            Console.WriteLine($"HTML report generated successfully: {outputFile}");
            
            Console.WriteLine("Generating Pie Chart...");
            var pieChartGenerator = new PieChartGenerator();
            var chartOutputFile = "employees.png";
            pieChartGenerator.GeneratePieChart(employeeSummaries, chartOutputFile);
            
            Console.WriteLine($"Pie chart generated successfully: {chartOutputFile}");
            Console.WriteLine($"Total employees: {employeeSummaries.Count}");
            Console.WriteLine($"Employees with less than 100 hours: {employeeSummaries.Count(e => e.TotalHours < 100)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            Environment.Exit(1);
        }
        finally
        {
            apiService?.Dispose();
        }
    }
}
