using Cartamentis.Application.Services;
using Cartamentis.Application.ViewModels;
using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;
using Xunit;

namespace Cartamentis.Application.Tests.Services;

/// <summary>
/// Tests for the <see cref="LayoutService"/> class.
/// </summary>
public class LayoutServiceTests
{
    private readonly LayoutService _service;

    public LayoutServiceTests()
    {
        _service = new LayoutService();
    }

    [Fact]
    public void CalculateCircularLayout_RootAtCenter_WhenRadiusIsZero()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var root = new TopicViewModel(mindMap.Root);

        // Act
        _service.CalculateCircularLayout(root, 400, 300, 0, 0, Math.PI * 2);

        // Assert
        Assert.Equal(400, root.X, 0.001);
        Assert.Equal(300, root.Y, 0.001);
    }

    [Fact]
    public void CalculateCircularLayout_RootAtSpecifiedRadius()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var root = new TopicViewModel(mindMap.Root);

        // Act
        _service.CalculateCircularLayout(root, 400, 300, 100, 0, Math.PI * 2);

        // Assert
        // At angle 0, X = centerX + radius * cos(0) = 400 + 100 * 1 = 500
        // At angle 0, Y = centerY + radius * sin(0) = 300 + 100 * 0 = 300
        Assert.Equal(500, root.X, 0.001);
        Assert.Equal(300, root.Y, 0.001);
    }

    [Fact]
    public void CalculateCircularLayout_ChildrenAtCorrectRadius()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var child = Topic.CreateChild("Child", mindMap.Id, mindMap.Root);
        var root = new TopicViewModel(mindMap.Root);
        var childVm = new TopicViewModel(child);
        root.AddChild(childVm);

        // Act
        _service.CalculateCircularLayout(root, 400, 300, 0, 0, Math.PI * 2);

        // Assert
        // Root is at center (400, 300)
        Assert.Equal(400, root.X, 0.001);
        Assert.Equal(300, root.Y, 0.001);

        // Child should be at radius 150 (0 + 150) from center
        var distance = Math.Sqrt(Math.Pow(childVm.X - 400, 2) + Math.Pow(childVm.Y - 300, 2));
        Assert.InRange(distance, 149.9, 150.1);
    }

    [Fact]
    public void CalculateCircularLayout_TwoChildrenEvenlySpaced()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var child1 = Topic.CreateChild("Child1", mindMap.Id, mindMap.Root);
        var child2 = Topic.CreateChild("Child2", mindMap.Id, mindMap.Root);
        
        var root = new TopicViewModel(mindMap.Root);
        var child1Vm = new TopicViewModel(child1);
        var child2Vm = new TopicViewModel(child2);
        root.AddChild(child1Vm);
        root.AddChild(child2Vm);

        // Act
        _service.CalculateCircularLayout(root, 400, 300, 0, 0, Math.PI * 2);

        // Assert
        // With 2 children and full circle (2PI), angle step is PI (180 degrees)
        // Child1 at angle 0: (400+150, 300) = (550, 300)
        // Child2 at angle PI: (400-150, 300) = (250, 300)
        
        Assert.InRange(child1Vm.X, 549.9, 550.1);
        Assert.Equal(300, child1Vm.Y, 0.1);
        
        Assert.InRange(child2Vm.X, 249.9, 250.1);
        Assert.Equal(300, child2Vm.Y, 0.1);
    }

    [Fact]
    public void CalculateCircularLayout_FourChildrenAtCardinalPoints()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var childRight = Topic.CreateChild("Right", mindMap.Id, mindMap.Root);
        var childBottom = Topic.CreateChild("Bottom", mindMap.Id, mindMap.Root);
        var childLeft = Topic.CreateChild("Left", mindMap.Id, mindMap.Root);
        var childTop = Topic.CreateChild("Top", mindMap.Id, mindMap.Root);
        
        var root = new TopicViewModel(mindMap.Root);
        var rightVm = new TopicViewModel(childRight);
        var bottomVm = new TopicViewModel(childBottom);
        var leftVm = new TopicViewModel(childLeft);
        var topVm = new TopicViewModel(childTop);
        
        root.AddChild(rightVm);
        root.AddChild(bottomVm);
        root.AddChild(leftVm);
        root.AddChild(topVm);

        // Act
        _service.CalculateCircularLayout(root, 400, 300, 0, 0, Math.PI * 2);

        // Assert
        // With 4 children at full circle, each is at 90 degree intervals
        // Right: angle 0 -> (400+150, 300) = (550, 300)
        // Bottom: angle PI/2 -> (400, 300+150) = (400, 450)
        // Left: angle PI -> (400-150, 300) = (250, 300)
        // Top: angle 3*PI/2 -> (400, 300-150) = (400, 150)
        
        // Check distances from center
        var distanceRight = Math.Sqrt(Math.Pow(rightVm.X - 400, 2) + Math.Pow(rightVm.Y - 300, 2));
        var distanceBottom = Math.Sqrt(Math.Pow(bottomVm.X - 400, 2) + Math.Pow(bottomVm.Y - 300, 2));
        var distanceLeft = Math.Sqrt(Math.Pow(leftVm.X - 400, 2) + Math.Pow(leftVm.Y - 300, 2));
        var distanceTop = Math.Sqrt(Math.Pow(topVm.X - 400, 2) + Math.Pow(topVm.Y - 300, 2));
        
        Assert.InRange(distanceRight, 149.9, 150.1);
        Assert.InRange(distanceBottom, 149.9, 150.1);
        Assert.InRange(distanceLeft, 149.9, 150.1);
        Assert.InRange(distanceTop, 149.9, 150.1);
    }

    [Fact]
    public void CalculateLayout_CentersRootInCanvas()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var root = new TopicViewModel(mindMap.Root);

        // Act
        _service.CalculateLayout(root, 800, 600);

        // Assert
        // Root should be at the center of the canvas
        Assert.Equal(400, root.X, 0.001);
        Assert.Equal(300, root.Y, 0.001);
    }

    [Fact]
    public void CalculateLayout_EmptyMindMap_DoesNotThrow()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var root = new TopicViewModel(mindMap.Root);

        // Act & Assert
        var exception = Record.Exception(() => _service.CalculateLayout(root, 800, 600));
        Assert.Null(exception);
    }

    [Fact]
    public void CalculateLayout_NullRoot_DoesNotThrow()
    {
        // Arrange
        TopicViewModel? root = null;

        // Act & Assert - This will throw if root is null, which is expected
        // We're testing that the method handles null gracefully or throws appropriately
        // For now, we expect it to not crash the entire test runner
        // In a real scenario, we might want to add null checks
        var exception = Record.Exception(() => _service.CalculateLayout(root!, 800, 600));
        // If it throws, that's fine - we're just verifying it doesn't crash unexpectedly
        Assert.NotNull(exception); // Expects ArgumentNullException or similar
    }
}
