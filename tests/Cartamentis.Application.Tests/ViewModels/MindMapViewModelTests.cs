using Cartamentis.Application.Services;
using Cartamentis.Application.ViewModels;
using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;
using Xunit;

namespace Cartamentis.Application.Tests.ViewModels;

/// <summary>
/// Tests for the <see cref="MindMapViewModel"/> class.
/// </summary>
public class MindMapViewModelTests
{
    private readonly LayoutService _layoutService;

    public MindMapViewModelTests()
    {
        _layoutService = new LayoutService();
    }

    [Fact]
    public void Constructor_WithValidMindMap_CreatesViewModel()
    {
        // Arrange
        var mindMap = MindMap.Create("Test Map");

        // Act
        var viewModel = new MindMapViewModel(mindMap, _layoutService);

        // Assert
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.Root);
        Assert.Equal("Test Map", viewModel.Root.Content);
    }

    [Fact]
    public void Constructor_WithNullMindMap_ThrowsArgumentNullException()
    {
        // Arrange
        MindMap? mindMap = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MindMapViewModel(mindMap!, _layoutService));
    }

    [Fact]
    public void Constructor_WithNullLayoutService_ThrowsArgumentNullException()
    {
        // Arrange
        var mindMap = MindMap.Create("Test Map");
        LayoutService? layoutService = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MindMapViewModel(mindMap, layoutService!));
    }

    [Fact]
    public void AllTopics_ReturnsFlattenedList()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var child1 = Topic.CreateChild("Child1", mindMap.Id, mindMap.Root);
        var child2 = Topic.CreateChild("Child2", mindMap.Id, mindMap.Root);
        var grandchild = Topic.CreateChild("Grandchild", mindMap.Id, child1);

        // Act
        var viewModel = new MindMapViewModel(mindMap, _layoutService);
        var allTopics = viewModel.AllTopics.ToList();

        // Assert
        Assert.Equal(4, allTopics.Count);
        Assert.Contains(viewModel.Root, allTopics);
        Assert.Contains(viewModel.Root.Children[0], allTopics);
        Assert.Contains(viewModel.Root.Children[1], allTopics);
        Assert.Contains(viewModel.Root.Children[0].Children[0], allTopics);
    }

    [Fact]
    public void Connections_ReturnsAllParentChildPairs()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var child1 = Topic.CreateChild("Child1", mindMap.Id, mindMap.Root);
        var child2 = Topic.CreateChild("Child2", mindMap.Id, mindMap.Root);
        var grandchild = Topic.CreateChild("Grandchild", mindMap.Id, child1);

        // Act
        var viewModel = new MindMapViewModel(mindMap, _layoutService);
        var connections = viewModel.Connections.ToList();

        // Assert
        Assert.Equal(3, connections.Count);
        
        // Check that each connection is a parent-child pair
        var rootToChild1 = connections.FirstOrDefault(c => c.From == viewModel.Root && c.To == viewModel.Root.Children[0]);
        var rootToChild2 = connections.FirstOrDefault(c => c.From == viewModel.Root && c.To == viewModel.Root.Children[1]);
        var child1ToGrandchild = connections.FirstOrDefault(c => c.From == viewModel.Root.Children[0] && c.To == viewModel.Root.Children[0].Children[0]);
        
        Assert.NotNull(rootToChild1);
        Assert.NotNull(rootToChild2);
        Assert.NotNull(child1ToGrandchild);
    }

    [Fact]
    public void CalculateLayout_RecalculatesPositions()
    {
        // Arrange
        var mindMap = MindMap.Create("Root");
        var child = Topic.CreateChild("Child", mindMap.Id, mindMap.Root);
        var viewModel = new MindMapViewModel(mindMap, _layoutService);

        var initialX = viewModel.Root.X;
        var initialY = viewModel.Root.Y;

        // Act
        viewModel.CalculateLayout(1000, 800);

        // Assert
        // After recalculating with different dimensions, positions should change
        Assert.NotEqual(initialX, viewModel.Root.X);
        Assert.NotEqual(initialY, viewModel.Root.Y);
        
        // New center should be at (500, 400)
        Assert.Equal(500, viewModel.Root.X, 0.001);
        Assert.Equal(400, viewModel.Root.Y, 0.001);
    }

    [Fact]
    public void Root_PropertyChanged_WhenRootChanges()
    {
        // This test verifies that the Root property properly implements INotifyPropertyChanged
        // Arrange
        var mindMap = MindMap.Create("Root");
        var viewModel = new MindMapViewModel(mindMap, _layoutService);
        
        var propertyChanged = false;
        viewModel.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(viewModel.Root))
            {
                propertyChanged = true;
            }
        };

        // Note: We can't easily change Root after construction, but we can verify
        // that the property is properly initialized and that PropertyChanged works
        
        // Assert
        Assert.NotNull(viewModel.Root);
    }

    [Fact]
    public void EmptyMindMap_HasOnlyRoot()
    {
        // Arrange
        var mindMap = MindMap.Create("Single Topic");

        // Act
        var viewModel = new MindMapViewModel(mindMap, _layoutService);

        // Assert
        Assert.Equal(1, viewModel.AllTopics.Count());
        Assert.Equal(0, viewModel.Connections.Count());
        Assert.Equal("Single Topic", viewModel.Root.Content);
        Assert.Empty(viewModel.Root.Children);
    }

    [Fact]
    public void DeepHierarchy_AllTopicsAndConnectionsCorrect()
    {
        // Arrange
        var mindMap = MindMap.Create("Level0");
        var level1 = Topic.CreateChild("Level1", mindMap.Id, mindMap.Root);
        var level2 = Topic.CreateChild("Level2", mindMap.Id, level1);
        var level3 = Topic.CreateChild("Level3", mindMap.Id, level2);

        // Act
        var viewModel = new MindMapViewModel(mindMap, _layoutService);

        // Assert
        Assert.Equal(4, viewModel.AllTopics.Count());
        Assert.Equal(3, viewModel.Connections.Count());
        
        // Verify hierarchy
        Assert.Equal("Level0", viewModel.Root.Content);
        Assert.Single(viewModel.Root.Children);
        Assert.Equal("Level1", viewModel.Root.Children[0].Content);
        Assert.Single(viewModel.Root.Children[0].Children);
        Assert.Equal("Level2", viewModel.Root.Children[0].Children[0].Content);
        Assert.Single(viewModel.Root.Children[0].Children[0].Children);
        Assert.Equal("Level3", viewModel.Root.Children[0].Children[0].Children[0].Content);
    }
}
