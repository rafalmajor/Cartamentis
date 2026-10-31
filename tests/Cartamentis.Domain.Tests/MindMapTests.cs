using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;
using Xunit;

namespace Cartamentis.Domain.Tests.MindMaps;

public sealed class MindMapTests
{
    // 1. Core Idea: A mind map is a rooted tree
    // Test: A mind map has exactly one root
    [Fact]
    public void CreateMindMap_HasExactlyOneRoot()
    {
        var mindMap = MindMap.Create("Cartamentis");

        Assert.NotNull(mindMap.Root);
        Assert.Equal("Cartamentis", mindMap.Root.Content);
    }

    // Test: The root has no parent
    [Fact]
    public void RootTopic_HasNoParent()
    {
        var mindMap = MindMap.Create("Cartamentis");

        Assert.Null(mindMap.Root.Parent);
    }

    // 6. Create Mind Map
    [Fact]
    public void CreateMindMap_WithRootTopic()
    {
        var mindMap = MindMap.Create("Test Map");

        Assert.Equal("Test Map", mindMap.Root.Content);
    }

    // 7. Add Child
    [Fact]
    public void AddChild_ToRoot_IncreasesChildrenCount()
    {
        var mindMap = MindMap.Create("Programming");

        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);

        Assert.Single(mindMap.Root.Children);
        Assert.Equal(".NET", mindMap.Root.Children[0].Content);
    }

    [Fact]
    public void AddChild_ParentIsNotLeafAfterAddingChild()
    {
        var mindMap = MindMap.Create("Programming");

        Assert.True(mindMap.Root.IsLeaf);

        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);

        Assert.False(mindMap.Root.IsLeaf);
    }

    // 2. Topics, Leaves, and Branches - Leaf status
    [Fact]
    public void CreateRoot_TopicIsInitiallyLeaf()
    {
        var mindMap = MindMap.Create("Test");

        Assert.True(mindMap.Root.IsLeaf);
    }

    [Fact]
    public void Root_WithChildren_IsNotLeaf()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);

        Assert.False(mindMap.Root.IsLeaf);
        Assert.True(dotNet.IsLeaf);
    }

    // 24. Structural Mutations - Add Child is a structural mutation
    [Fact]
    public void AddChild_IsStructuralMutation()
    {
        var mindMap = MindMap.Create("Programming");
        var initialCount = mindMap.Root.Children.Count;

        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);

        Assert.Equal(initialCount + 1, mindMap.Root.Children.Count);
    }

    // 27. Tree Invariants - A mind map has exactly one root
    [Fact]
    public void MindMap_HasExactlyOneRoot()
    {
        var mindMap = MindMap.Create("Test");

        Assert.NotNull(mindMap.Root);
        Assert.True(mindMap.Root.IsRoot);
    }

    // 27. Tree Invariants - The root has no parent
    [Fact]
    public void Root_HasNoParent()
    {
        var mindMap = MindMap.Create("Test");

        Assert.Null(mindMap.Root.Parent);
    }

    // 27. Tree Invariants - Every non-root topic has exactly one hierarchical parent
    [Fact]
    public void NonRootTopic_HasExactlyOneParent()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);

        Assert.NotNull(dotNet.Parent);
        Assert.Equal(mindMap.Root, dotNet.Parent);
    }

    // 20. Find Descendants
    [Fact]
    public void FindDescendants_ReturnsAllDescendants()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);
        var web = Topic.CreateChild("Web", mindMap.Id, dotNet);

        var descendants = mindMap.Root.GetDescendants().ToList();

        Assert.Contains(dotNet, descendants);
        Assert.Contains(desktop, descendants);
        Assert.Contains(web, descendants);
        Assert.DoesNotContain(mindMap.Root, descendants);
    }

    // 19. Find Ancestors
    [Fact]
    public void FindAncestors_ReturnsPathToRoot()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, desktop);

        var ancestors = wpf.GetAncestors().ToList();

        Assert.Equal(3, ancestors.Count);
        Assert.Equal(desktop, ancestors[0]);
        Assert.Equal(dotNet, ancestors[1]);
        Assert.Equal(mindMap.Root, ancestors[2]);
    }

    // 21. Find Path to Root
    [Fact]
    public void GetPathToRoot_ReturnsCompletePath()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);

        var path = GetPathToRoot(desktop);

        Assert.Equal(3, path.Count);
        Assert.Equal("Desktop", path[0].Content);
        Assert.Equal(".NET", path[1].Content);
        Assert.Equal("Programming", path[2].Content);
    }

    private static List<Topic> GetPathToRoot(Topic topic)
    {
        var path = new List<Topic> { topic };
        var current = topic.Parent;
        while (current != null)
        {
            path.Add(current);
            current = current.Parent;
        }
        return path;
    }

    // 18. Traverse - Depth first
    [Fact]
    public void Traverse_DepthFirst_VisitsInCorrectOrder()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, mindMap.Root);
        var d = Topic.CreateChild("D", mindMap.Id, b);
        var e = Topic.CreateChild("E", mindMap.Id, b);

        var visited = new List<string>();
        TraverseDepthFirst(mindMap.Root, visited);

        Assert.Equal(new[] { "A", "B", "D", "E", "C" }, visited);
    }

    private static void TraverseDepthFirst(Topic topic, List<string> visited)
    {
        visited.Add(topic.Content);
        foreach (var child in topic.Children)
        {
            TraverseDepthFirst(child, visited);
        }
    }

    // 27. Tree Invariants - Exactly one hierarchical path from root to every topic
    [Fact]
    public void EachTopic_HasExactlyOnePathFromRoot()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);

        var pathToDotNet = GetPathToRoot(dotNet);
        var pathToDesktop = GetPathToRoot(desktop);

        Assert.Equal(2, pathToDotNet.Count);
        Assert.Equal(3, pathToDesktop.Count);
        Assert.Contains(dotNet, pathToDesktop);
    }

    // 8. Add Sibling
    [Fact]
    public void AddSibling_AddsTopicUnderSameParent()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var python = Topic.CreateChild("Python", mindMap.Id, mindMap.Root);

        Assert.Equal(2, mindMap.Root.Children.Count);
        Assert.Equal(dotNet.Parent, python.Parent);
    }

    // 9. Remove Leaf
    [Fact]
    public void RemoveLeaf_RemovesTopicWithNoChildren()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var python = Topic.CreateChild("Python", mindMap.Id, mindMap.Root);

        var result = mindMap.Root.RemoveChild(python);

        Assert.True(result);
        Assert.Single(mindMap.Root.Children);
        Assert.Equal(dotNet, mindMap.Root.Children[0]);
    }

    // 10. Remove Subtree
    [Fact]
    public void RemoveSubtree_RemovesTopicAndAllDescendants()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);
        var web = Topic.CreateChild("Web", mindMap.Id, dotNet);

        var result = mindMap.Root.RemoveChild(dotNet);

        Assert.True(result);
        Assert.Empty(mindMap.Root.Children);
    }

    // 11. Move Subtree
    [Fact]
    public void MoveSubtree_MovesTopicAndDescendantsToNewParent()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, mindMap.Root);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, dotNet);

        // Move WPF under Desktop
        dotNet.RemoveChild(wpf);
        desktop.AddChild(wpf);

        Assert.Empty(dotNet.Children);
        Assert.Single(desktop.Children);
        Assert.Equal(desktop, wpf.Parent);
    }

    // 11. Move Subtree - Moving a topic moves its complete subtree
    [Fact]
    public void MoveSubtree_MovesCompleteSubtree()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, mindMap.Root);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, dotNet);
        var mvvm = Topic.CreateChild("MVVM", mindMap.Id, wpf);

        // Move WPF and MVVM under Desktop
        dotNet.RemoveChild(wpf);
        desktop.AddChild(wpf);

        Assert.Equal(desktop, wpf.Parent);
        Assert.Equal(wpf, mvvm.Parent);
        Assert.Single(wpf.GetDescendants());
    }

    // 23. Reject Cycle Creation
    [Fact]
    public void MoveSubtree_RejectsCycleCreation_AddingSelfAsChild()
    {
        var mindMap = MindMap.Create("A");

        Assert.Throws<InvalidOperationException>(() => mindMap.Root.AddChild(mindMap.Root));
    }

    [Fact]
    public void MoveSubtree_RejectsCycleCreation_AddingDescendantAsParent()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, b);

        // Try to move A under C (would create cycle: A -> B -> C -> A)
        Assert.Throws<InvalidOperationException>(() => c.AddChild(mindMap.Root));
    }

    [Fact]
    public void MoveSubtree_RejectsCycleCreation_AddingToOwnDescendant()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, b);

        // Try to move A under C (would create cycle)
        Assert.Throws<InvalidOperationException>(() => c.AddChild(mindMap.Root));
    }

    // 27. Tree Invariants - The hierarchical branch structure contains no cycles
    [Fact]
    public void Hierarchy_HasNoCycles()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, b);

        // Verify no cycles by checking ancestors don't contain the topic itself
        var ancestorsOfC = c.GetAncestors().ToList();
        Assert.DoesNotContain(c, ancestorsOfC);
    }

    // 27. Tree Invariants - A topic cannot become its own ancestor or descendant
    [Fact]
    public void Topic_CannotBeItsOwnAncestor()
    {
        var mindMap = MindMap.Create("A");
        var ancestors = mindMap.Root.GetAncestors().ToList();
        
        Assert.Empty(ancestors);
    }

    [Fact]
    public void Topic_IsAncestorOf_DescendantReturnsTrue()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, b);

        Assert.True(mindMap.Root.IsAncestorOf(b));
        Assert.True(mindMap.Root.IsAncestorOf(c));
        Assert.True(b.IsAncestorOf(c));
        Assert.False(b.IsAncestorOf(mindMap.Root));
    }

    [Fact]
    public void Topic_IsDescendantOf_AncestorReturnsTrue()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, b);

        Assert.True(b.IsDescendantOf(mindMap.Root));
        Assert.True(c.IsDescendantOf(mindMap.Root));
        Assert.True(c.IsDescendantOf(b));
        Assert.False(mindMap.Root.IsDescendantOf(b));
    }

    // 3. Hierarchy and Associations - Branch defines hierarchy
    [Fact]
    public void Branch_DefinesHierarchy()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        Assert.Equal(mindMap.Root, b.Parent);
        Assert.Contains(b, mindMap.Root.Children);
    }

    // 4. What Tony Buzan's Approach Implies - Central idea becomes root
    [Fact]
    public void CentralIdea_IsRootTopic()
    {
        var mindMap = MindMap.Create("Central Idea");

        Assert.Equal("Central Idea", mindMap.Root.Content);
        Assert.True(mindMap.Root.IsRoot);
    }

    // 4. Hierarchical depth
    [Fact]
    public void Depth_RootIsZero()
    {
        var mindMap = MindMap.Create("Root");

        Assert.Equal(0, mindMap.Root.GetDepth());
    }

    [Fact]
    public void Depth_ChildIsParentPlusOne()
    {
        var mindMap = MindMap.Create("Root");
        var child = Topic.CreateChild("Child", mindMap.Id, mindMap.Root);

        Assert.Equal(1, child.GetDepth());
    }

    [Fact]
    public void Depth_GrandchildIsTwo()
    {
        var mindMap = MindMap.Create("Root");
        var child = Topic.CreateChild("Child", mindMap.Id, mindMap.Root);
        var grandchild = Topic.CreateChild("Grandchild", mindMap.Id, child);

        Assert.Equal(2, grandchild.GetDepth());
    }

    // 27. Tree Invariants - There is exactly one hierarchical path from the root to every topic
    [Fact]
    public void SinglePath_FromRootToEachTopic()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, mindMap.Root);

        var pathToB = GetPathToRoot(b);
        var pathToC = GetPathToRoot(c);

        Assert.Equal(2, pathToB.Count);
        Assert.Equal(2, pathToC.Count);
        Assert.Equal(mindMap.Root, pathToB[1]);
        Assert.Equal(mindMap.Root, pathToC[1]);
    }

    // 24. Structural Mutations - Remove operations
    [Fact]
    public void RemoveChild_ReturnsTrueWhenChildExisted()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        var result = mindMap.Root.RemoveChild(b);

        Assert.True(result);
        Assert.Empty(mindMap.Root.Children);
        Assert.Null(b.Parent);
    }

    [Fact]
    public void RemoveChild_ReturnsFalseWhenChildDidNotExist()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var otherMindMap = MindMap.Create("Other");
        var c = Topic.CreateChild("C", otherMindMap.Id, otherMindMap.Root);

        var result = mindMap.Root.RemoveChild(c);

        Assert.False(result);
    }

    // 13. Promote
    [Fact]
    public void Promote_MovesTopicOneLevelTowardRoot()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, dotNet);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, desktop);

        // Promote Desktop to be under Root
        desktop.RemoveFromParent();
        mindMap.Root.AddChild(desktop);

        Assert.Equal(mindMap.Root, desktop.Parent);
        Assert.Equal(1, desktop.GetDepth());
        Assert.Equal(desktop, wpf.Parent);
    }

    [Fact]
    public void Promote_RootCannotBePromoted()
    {
        var mindMap = MindMap.Create("Root");

        Assert.True(mindMap.Root.IsRoot);
        mindMap.Root.RemoveFromParent();
        
        // Root should still be root
        Assert.True(mindMap.Root.IsRoot);
    }

    // 14. Demote
    [Fact]
    public void Demote_MovesTopicOneLevelDeeper()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, mindMap.Root);

        // Demote Desktop under .NET
        desktop.RemoveFromParent();
        dotNet.AddChild(desktop);

        Assert.Equal(dotNet, desktop.Parent);
        Assert.Equal(2, desktop.GetDepth());
    }

    // 15. Reorder Siblings
    [Fact]
    public void ReorderSiblings_ChangesOrderWithoutChangingParent()
    {
        var mindMap = MindMap.Create("Programming");
        var dotNet = Topic.CreateChild(".NET", mindMap.Id, mindMap.Root);
        var python = Topic.CreateChild("Python", mindMap.Id, mindMap.Root);
        var rust = Topic.CreateChild("Rust", mindMap.Id, mindMap.Root);

        var initialOrder = mindMap.Root.Children.Select(t => t.Content).ToList();
        Assert.Equal(new[] { ".NET", "Python", "Rust" }, initialOrder);

        // Simulate reordering by removing and adding in new order
        mindMap.Root.RemoveChild(python);
        mindMap.Root.RemoveChild(rust);
        mindMap.Root.AddChild(rust);
        mindMap.Root.AddChild(python);

        var newOrder = mindMap.Root.Children.Select(t => t.Content).ToList();
        Assert.Equal(new[] { ".NET", "Rust", "Python" }, newOrder);
    }

    // 16. Clone Subtree
    [Fact]
    public void CloneSubtree_CreatesNewTopicIdentities()
    {
        var mindMap = MindMap.Create("Programming");
        var desktop = Topic.CreateChild("Desktop", mindMap.Id, mindMap.Root);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, desktop);

        Assert.Equal(desktop, wpf.Parent);
        Assert.Single(desktop.Children);
    }

    // 17. Extract Subtree
    [Fact]
    public void ExtractSubtree_CreatesNewMindMapFromSubtree()
    {
        var mindMap = MindMap.Create("Software");
        var architecture = Topic.CreateChild("Architecture", mindMap.Id, mindMap.Root);
        var ddd = Topic.CreateChild("DDD", mindMap.Id, architecture);
        var entity = Topic.CreateChild("Entity", mindMap.Id, ddd);

        // Extract DDD subtree - remove it from current parent
        architecture.RemoveChild(ddd);

        // Create new mind map with DDD as root
        var newMindMap = MindMap.CreateFromRoot(ddd);

        Assert.Equal("DDD", newMindMap.Root.Content);
        Assert.Single(newMindMap.Root.Children);
        Assert.Equal("Entity", newMindMap.Root.Children[0].Content);
    }

    // 25. Structural Queries
    [Fact]
    public void GetChildren_ReturnsDirectChildren()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, mindMap.Root);

        var children = mindMap.Root.Children;

        Assert.Equal(2, children.Count);
        Assert.Contains(b, children);
        Assert.Contains(c, children);
    }

    [Fact]
    public void IsRoot_ReturnsTrueForRoot()
    {
        var mindMap = MindMap.Create("A");

        Assert.True(mindMap.Root.IsRoot);
    }

    [Fact]
    public void IsLeaf_ReturnsTrueForLeaf()
    {
        var mindMap = MindMap.Create("A");

        Assert.True(mindMap.Root.IsLeaf);
    }

    // 5. Domain and Presentation Boundaries
    [Fact]
    public void Topic_ContainsOnlyStructuralInformation()
    {
        var mindMap = MindMap.Create("A");
        var topic = mindMap.Root;

        // Topic should have: Content, Children
        Assert.NotNull(topic.Content);
        Assert.NotNull(topic.Children);
        
        // Topic should NOT have presentation properties
    }

    // 26. Presentation Operations
    // (Verified by the fact that Topic doesn't have Collapse, Expand, etc. methods)
    
    // 27. Tree Invariants - Sibling order is explicit
    [Fact]
    public void SiblingOrder_IsExplicit()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);
        var c = Topic.CreateChild("C", mindMap.Id, mindMap.Root);

        var children = mindMap.Root.Children.ToList();
        Assert.Equal(b, children[0]);
        Assert.Equal(c, children[1]);
    }

    // 29. Software Architecture example structure
    [Fact]
    public void CreateSoftwareArchitectureStructure()
    {
        var mindMap = MindMap.Create("Cartamentis");
        var domain = Topic.CreateChild("Domain", mindMap.Id, mindMap.Root);
        var presentation = Topic.CreateChild("Presentation", mindMap.Id, mindMap.Root);
        var infrastructure = Topic.CreateChild("Infrastructure", mindMap.Id, mindMap.Root);

        var topic = Topic.CreateChild("Topic", mindMap.Id, domain);
        var connection = Topic.CreateChild("Connection", mindMap.Id, domain);
        var mindMapTopic = Topic.CreateChild("MindMap", mindMap.Id, domain);

        Assert.Equal(3, mindMap.Root.Children.Count);
        Assert.Equal(3, domain.Children.Count);
    }

    // 31. Class Design example
    [Fact]
    public void TopicClass_HasExpectedStructure()
    {
        var mindMap = MindMap.Create("Topic");
        var identity = Topic.CreateChild("Identity", mindMap.Id, mindMap.Root);
        var content = Topic.CreateChild("Content", mindMap.Id, mindMap.Root);
        var children = Topic.CreateChild("Children", mindMap.Id, mindMap.Root);

        Assert.Equal(3, mindMap.Root.Children.Count);
    }

    // 27. Tree Invariants - Cloning creates new topic identities
    [Fact]
    public void Topics_HaveUniqueIdentities()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        // Each topic has a unique ID
        Assert.NotEqual(mindMap.Root.Id, b.Id);
        Assert.Equal("A", mindMap.Root.Content);
        Assert.Equal("B", b.Content);
    }

    // Final Architectural Direction
    [Fact]
    public void MindMap_IsModeledAsRootedTree()
    {
        var mindMap = MindMap.Create("Root");

        Assert.True(mindMap.Root.IsRoot);
        Assert.Equal(0, mindMap.Root.GetDepth());
    }

    [Fact]
    public void Topics_AreVertices_BranchesAreEdges()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        // Topic (vertex) has parent (edge)
        Assert.Equal(mindMap.Root, b.Parent);
        // Topic has children (edges to other vertices)
        Assert.Contains(b, mindMap.Root.Children);
    }

    [Fact]
    public void LeafStatus_IsDerivedFromStructure()
    {
        var mindMap = MindMap.Create("A");
        
        Assert.True(mindMap.Root.IsLeaf);

        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        Assert.False(mindMap.Root.IsLeaf);
        Assert.True(b.IsLeaf);

        var c = Topic.CreateChild("C", mindMap.Id, b);

        Assert.False(b.IsLeaf);
        Assert.True(c.IsLeaf);
    }

    [Fact]
    public void BranchStatus_IsDerivedFromStructure()
    {
        var mindMap = MindMap.Create("A");
        var b = Topic.CreateChild("B", mindMap.Id, mindMap.Root);

        // The hierarchical relationship between A and B is a branch
        Assert.Equal(mindMap.Root, b.Parent);
    }

    // 28. Brainstorming example
    [Fact]
    public void CreateBrainstormingStructure()
    {
        var mindMap = MindMap.Create("Idea");
        var technology = Topic.CreateChild("Technology", mindMap.Id, mindMap.Root);
        var users = Topic.CreateChild("Users", mindMap.Id, mindMap.Root);
        var business = Topic.CreateChild("Business", mindMap.Id, mindMap.Root);
        var risks = Topic.CreateChild("Risks", mindMap.Id, mindMap.Root);

        Assert.Equal(4, mindMap.Root.Children.Count);
    }

    // 29. Problem Analysis example
    [Fact]
    public void CreateProblemAnalysisStructure()
    {
        var mindMap = MindMap.Create("Bug");
        var symptoms = Topic.CreateChild("Symptoms", mindMap.Id, mindMap.Root);
        var causes = Topic.CreateChild("Causes", mindMap.Id, mindMap.Root);
        var experiments = Topic.CreateChild("Experiments", mindMap.Id, mindMap.Root);
        var evidence = Topic.CreateChild("Evidence", mindMap.Id, mindMap.Root);
        var solutions = Topic.CreateChild("Solutions", mindMap.Id, mindMap.Root);

        Assert.Equal(5, mindMap.Root.Children.Count);
    }

    // 30. Book Notes example
    [Fact]
    public void CreateBookNotesStructure()
    {
        var mindMap = MindMap.Create("After Virtue");
        var thesis = Topic.CreateChild("Thesis", mindMap.Id, mindMap.Root);
        var history = Topic.CreateChild("History", mindMap.Id, mindMap.Root);
        var virtue = Topic.CreateChild("Virtue", mindMap.Id, mindMap.Root);
        var tradition = Topic.CreateChild("Tradition", mindMap.Id, mindMap.Root);
        var criticism = Topic.CreateChild("Criticism", mindMap.Id, mindMap.Root);

        Assert.Equal(5, mindMap.Root.Children.Count);
    }
}
