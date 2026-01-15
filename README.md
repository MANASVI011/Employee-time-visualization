# Employee Time Tracker

A C# console application that visualizes employee time entry data from a REST API endpoint. The application generates both an HTML table and a PNG pie chart showing employee work time distribution.

## Features

### Task A: HTML Table Visualization
- Fetches employee time entries from REST API
- Displays employees in a formatted HTML table
- Orders employees by total time worked (descending)
- Highlights rows in red for employees with less than 100 hours
- Shows employee name and total time worked

### Task B: PNG Pie Chart Visualization
- Generates a PNG image file with a pie chart
- Shows percentage distribution of total time worked per employee
- Displays percentage labels on chart slices
- Includes a legend with employee names and percentages

## Requirements

- .NET 10.0 SDK or later
- SkiaSharp NuGet package (for cross-platform image generation)

## Setup

1. Clone the repository:
```bash
git clone https://github.com/MANASVI011/Employee-time-visualization.git
cd Employee-time-visualization
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run the application:
```bash
dotnet run
```

## Output

The application generates two files:
- `employees.html` - HTML table with employee time data
- `employees.png` - Pie chart showing time distribution percentages

## Project Structure

```
EmployeeTimeTracker/
├── Program.cs              # Main entry point
├── ApiService.cs           # HTTP client for API calls
├── DataProcessor.cs        # Processes and aggregates time entries
├── HtmlGenerator.cs        # Generates HTML table
├── PieChartGenerator.cs    # Generates PNG pie chart
├── TimeEntry.cs            # Data model for API response
├── EmployeeSummary.cs      # Data model for processed results
└── EmployeeTimeTracker.csproj
```

## API Endpoint

The application fetches data from:
```
https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={key}
```

## Technologies Used

- C# (.NET 10.0)
- SkiaSharp (for cross-platform image generation)
- System.Text.Json (for JSON parsing)
- System.Net.Http (for HTTP requests)
