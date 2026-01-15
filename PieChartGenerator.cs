using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace EmployeeTimeTracker;

public class PieChartGenerator
{
    private class EmployeeChartData
    {
        public EmployeeSummary Employee { get; set; } = null!;
        public double Percentage { get; set; }
    }

    private const int ImageWidth = 1000;
    private const int ImageHeight = 800;
    private const int ChartSize = 500;
    private const int ChartX = 50;
    private const int ChartY = 100;
    private const int LegendX = 600;
    private const int LegendY = 150;
    private const int LegendItemHeight = 30;

    // Color palette - distinct colors for good visibility
    private static readonly SKColor[] ColorPalette = new SKColor[]
    {
        new SKColor(31, 119, 180),   // Blue
        new SKColor(255, 127, 14),   // Orange
        new SKColor(44, 160, 44),    // Green
        new SKColor(214, 39, 40),    // Red
        new SKColor(148, 103, 189),  // Purple
        new SKColor(140, 86, 75),    // Brown
        new SKColor(227, 119, 194),  // Pink
        new SKColor(127, 127, 127),  // Gray
        new SKColor(188, 189, 34),   // Olive
        new SKColor(23, 190, 207),   // Cyan
        new SKColor(174, 199, 232),  // Light Blue
        new SKColor(255, 187, 120),  // Light Orange
    };

    public void GeneratePieChart(List<EmployeeSummary> employees, string outputPath = "employees.png")
    {
        if (employees == null || employees.Count == 0)
        {
            throw new ArgumentException("Employee list cannot be null or empty.");
        }

        var totalHours = employees.Sum(e => e.TotalHours);
        if (totalHours == 0)
        {
            throw new InvalidOperationException("Total hours cannot be zero.");
        }

        // Calculate percentages
        var employeeData = employees.Select(e => new EmployeeChartData
        {
            Employee = e,
            Percentage = (e.TotalHours / totalHours) * 100
        }).ToList();

        using var surface = SKSurface.Create(new SKImageInfo(ImageWidth, ImageHeight));
        var canvas = surface.Canvas;

        // Fill background
        canvas.Clear(SKColors.White);

        // Draw title
        DrawTitle(canvas);

        // Draw pie chart
        DrawPieChart(canvas, employeeData);

        // Draw legend
        DrawLegend(canvas, employeeData);

        // Save image
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var stream = System.IO.File.OpenWrite(outputPath);
        data.SaveTo(stream);
    }

    private void DrawTitle(SKCanvas canvas)
    {
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 24,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
            TextAlign = SKTextAlign.Center
        };

        var titleText = "Employee Time Distribution";
        canvas.DrawText(titleText, ImageWidth / 2, 50, paint);
    }

    private void DrawPieChart(SKCanvas canvas, List<EmployeeChartData> employeeData)
    {
        var rect = new SKRect(ChartX, ChartY, ChartX + ChartSize, ChartY + ChartSize);
        float startAngle = -90; // Start at top

        for (int i = 0; i < employeeData.Count; i++)
        {
            var data = employeeData[i];
            var sweepAngle = (float)(data.Percentage * 360 / 100);
            var color = ColorPalette[i % ColorPalette.Length];

            // Draw pie slice
            using var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            using var path = new SKPath();
            path.AddArc(rect, startAngle, sweepAngle);
            path.LineTo(rect.MidX, rect.MidY);
            path.Close();
            canvas.DrawPath(path, paint);

            // Draw slice border
            using var borderPaint = new SKPaint
            {
                Color = SKColors.White,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2
            };
            canvas.DrawPath(path, borderPaint);

            // Draw percentage label on slice if percentage is large enough
            if (data.Percentage >= 5) // Only show label if slice is >= 5%
            {
                DrawSliceLabel(canvas, rect, startAngle, sweepAngle, data.Percentage);
            }

            startAngle += sweepAngle;
        }
    }

    private void DrawSliceLabel(SKCanvas canvas, SKRect rect, float startAngle, float sweepAngle, double percentage)
    {
        // Calculate label position (middle of slice)
        var labelAngle = (startAngle + sweepAngle / 2) * Math.PI / 180;
        var centerX = rect.MidX;
        var centerY = rect.MidY;
        var radius = rect.Width / 2 * 0.7f; // Position label at 70% of radius

        var labelX = centerX + (float)(radius * Math.Cos(labelAngle));
        var labelY = centerY + (float)(radius * Math.Sin(labelAngle));

        var labelText = $"{percentage:F1}%";

        // Draw text shadow
        using var shadowPaint = new SKPaint
        {
            Color = new SKColor(128, 0, 0, 0),
            TextSize = 12,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
            TextAlign = SKTextAlign.Center
        };
        canvas.DrawText(labelText, labelX + 1, labelY + 1, shadowPaint);

        // Draw text
        using var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 12,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold),
            TextAlign = SKTextAlign.Center
        };
        canvas.DrawText(labelText, labelX, labelY, textPaint);
    }

    private void DrawLegend(SKCanvas canvas, List<EmployeeChartData> employeeData)
    {
        var y = LegendY;
        var boxSize = 20;
        var boxSpacing = 5;

        using var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 11,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
        };

        for (int i = 0; i < employeeData.Count; i++)
        {
            var data = employeeData[i];
            var color = ColorPalette[i % ColorPalette.Length];

            // Draw color box
            using var boxPaint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            var boxRect = new SKRect(LegendX, y, LegendX + boxSize, y + boxSize);
            canvas.DrawRect(boxRect, boxPaint);

            // Draw box border
            using var borderPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };
            canvas.DrawRect(boxRect, borderPaint);

            // Draw employee name and percentage
            var legendText = $"{data.Employee.Name}: {data.Percentage:F2}%";
            canvas.DrawText(legendText, LegendX + boxSize + boxSpacing, y + boxSize - 5, textPaint);

            y += LegendItemHeight;
        }
    }
}
