using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeTimeTracker;

public class DataProcessor
{
    public List<EmployeeSummary> ProcessTimeEntries(List<TimeEntry> timeEntries)
    {
        // Filter out deleted entries and entries with invalid dates
        var validEntries = timeEntries
            .Where(entry => entry.DeletedOn == null 
                && entry.StarTimeUtc.HasValue 
                && entry.EndTimeUtc.HasValue
                && !string.IsNullOrWhiteSpace(entry.EmployeeName))
            .ToList();

        // Group by employee name and calculate total hours
        var employeeGroups = validEntries
            .GroupBy(entry => entry.EmployeeName)
            .Select(group =>
            {
                var totalHours = group.Sum(entry =>
                {
                    var startTime = entry.StarTimeUtc!.Value;
                    var endTime = entry.EndTimeUtc!.Value;
                    
                    // Handle case where end time might be before start time (data issue)
                    if (endTime < startTime)
                    {
                        return 0.0;
                    }
                    
                    var timeSpan = endTime - startTime;
                    return timeSpan.TotalHours;
                });

                return new EmployeeSummary
                {
                    Name = group.Key,
                    TotalHours = totalHours
                };
            })
            .OrderByDescending(emp => emp.TotalHours)
            .ToList();

        return employeeGroups;
    }
}
