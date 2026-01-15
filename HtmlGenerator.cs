using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeTimeTracker;

public class HtmlGenerator
{
    public string GenerateHtml(List<EmployeeSummary> employees)
    {
        var html = new StringBuilder();
        
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("    <meta charset=\"UTF-8\">");
        html.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine("    <title>Employee Time Report</title>");
        html.AppendLine("    <style>");
        html.AppendLine("        body {");
        html.AppendLine("            font-family: Arial, sans-serif;");
        html.AppendLine("            margin: 20px;");
        html.AppendLine("            background-color: #f5f5f5;");
        html.AppendLine("        }");
        html.AppendLine("        h1 {");
        html.AppendLine("            color: #333;");
        html.AppendLine("        }");
        html.AppendLine("        table {");
        html.AppendLine("            border-collapse: collapse;");
        html.AppendLine("            width: 100%;");
        html.AppendLine("            max-width: 800px;");
        html.AppendLine("            background-color: white;");
        html.AppendLine("            box-shadow: 0 2px 4px rgba(0,0,0,0.1);");
        html.AppendLine("        }");
        html.AppendLine("        th, td {");
        html.AppendLine("            border: 1px solid #ddd;");
        html.AppendLine("            padding: 12px;");
        html.AppendLine("            text-align: left;");
        html.AppendLine("        }");
        html.AppendLine("        th {");
        html.AppendLine("            background-color: #4CAF50;");
        html.AppendLine("            color: white;");
        html.AppendLine("            font-weight: bold;");
        html.AppendLine("        }");
        html.AppendLine("        tr.low-hours {");
        html.AppendLine("            background-color: #ffcccc;");
        html.AppendLine("        }");
        html.AppendLine("        tr:hover:not(.low-hours) {");
        html.AppendLine("            background-color: #f5f5f5;");
        html.AppendLine("        }");
        html.AppendLine("        td:last-child {");
        html.AppendLine("            text-align: right;");
        html.AppendLine("            font-weight: bold;");
        html.AppendLine("        }");
        html.AppendLine("    </style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("    <h1>Employee Time Report</h1>");
        html.AppendLine("    <table>");
        html.AppendLine("        <thead>");
        html.AppendLine("            <tr>");
        html.AppendLine("                <th>Name</th>");
        html.AppendLine("                <th>Total Time Worked</th>");
        html.AppendLine("            </tr>");
        html.AppendLine("        </thead>");
        html.AppendLine("        <tbody>");

        foreach (var employee in employees)
        {
            var rowClass = employee.TotalHours < 100 ? " class=\"low-hours\"" : "";
            var formattedHours = FormatHours(employee.TotalHours);
            
            html.AppendLine($"            <tr{rowClass}>");
            html.AppendLine($"                <td>{EscapeHtml(employee.Name)}</td>");
            html.AppendLine($"                <td>{formattedHours}</td>");
            html.AppendLine("            </tr>");
        }

        html.AppendLine("        </tbody>");
        html.AppendLine("    </table>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    private string FormatHours(double totalHours)
    {
        var hours = (int)totalHours;
        var minutes = (int)((totalHours - hours) * 60);
        
        if (minutes == 0)
        {
            return $"{hours} hours";
        }
        else if (hours == 0)
        {
            return $"{minutes} minutes";
        }
        else
        {
            return $"{hours}h {minutes}m";
        }
    }

    private string EscapeHtml(string text)
    {
        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }
}
