using Cartamentis.Application.ViewModels;

namespace Cartamentis.Application.Services;

/// <summary>
/// Service responsible for calculating the visual layout of mind map topics.
/// Implements a circular layout algorithm where child topics are arranged in a circle around their parent.
/// </summary>
public class LayoutService
{
    /// <summary>
    /// The default radius for the first level of children (distance from parent).
    /// </summary>
    private const double DefaultRadius = 150;

    /// <summary>
    /// The spacing between levels (distance between parent level and child level).
    /// </summary>
    private const double LevelSpacing = 150;

    /// <summary>
    /// Calculates the circular layout for a topic and all its descendants.
    /// </summary>
    /// <param name="topic">The topic to position.</param>
    /// <param name="centerX">The X coordinate of the center point.</param>
    /// <param name="centerY">The Y coordinate of the center point.</param>
    /// <param name="radius">The radius at which to place this topic.</param>
    /// <param name="startAngle">The starting angle in radians for child placement.</param>
    /// <param name="endAngle">The ending angle in radians for child placement.</param>
    public void CalculateCircularLayout(
        TopicViewModel topic,
        double centerX,
        double centerY,
        double radius,
        double startAngle,
        double endAngle)
    {
        // Position the current topic
        topic.X = centerX + radius * Math.Cos(startAngle);
        topic.Y = centerY + radius * Math.Sin(startAngle);

        // If no children, we're done
        if (topic.Children.Count == 0)
        {
            return;
        }

        // Calculate the angle step between children
        double angleStep = (endAngle - startAngle) / topic.Children.Count;
        double currentAngle = startAngle;

        // Position each child at a new radius (further from center)
        double childRadius = radius + LevelSpacing;

        foreach (var child in topic.Children)
        {
            CalculateCircularLayout(
                child,
                centerX,
                centerY,
                childRadius,
                currentAngle,
                currentAngle + angleStep);

            currentAngle += angleStep;
        }
    }

    /// <summary>
    /// Calculates the layout for the entire mind map starting from the root.
    /// </summary>
    /// <param name="root">The root topic of the mind map.</param>
    /// <param name="canvasWidth">The width of the canvas.</param>
    /// <param name="canvasHeight">The height of the canvas.</param>
    public void CalculateLayout(TopicViewModel root, double canvasWidth, double canvasHeight)
    {
        // Center the root in the canvas
        double centerX = canvasWidth / 2;
        double centerY = canvasHeight / 2;

        // Start with radius 0 for the root (it will be at the center)
        CalculateCircularLayout(root, centerX, centerY, 0, 0, Math.PI * 2);
    }
}
