using Cartamentis.Domain.MindMaps;

namespace Cartamentis.Domain.Topics;

public sealed class Topic
{
    public TopicId Id { get; }
    public string Content { get; }
    public MindMapId MindMapId { get; }
    public Topic? Parent { get; internal set; }
    public IReadOnlyList<Topic> Children => _children.AsReadOnly();

    private readonly List<Topic> _children = new();

    internal Topic(TopicId id, string content, MindMapId mindMapId, Topic? parent = null)
    {
        Id = id;
        Content = content;
        MindMapId = mindMapId;
        Parent = parent;
    }

    public static Topic CreateRoot(string content, MindMapId mindMapId)
    {
        return new Topic(TopicId.New(), content, mindMapId, null);
    }

    public static Topic CreateChild(string content, MindMapId mindMapId, Topic parent)
    {
        var topic = new Topic(TopicId.New(), content, mindMapId, parent);
        parent.AddChild(topic);
        return topic;
    }

    // For cloning purposes
    internal static Topic CreateOrphan(string content, MindMapId mindMapId)
    {
        return new Topic(TopicId.New(), content, mindMapId, null);
    }

    public void AddChild(Topic child)
    {
        if (child.Parent != null && child.Parent != this)
        {
            throw new InvalidOperationException("Topic already has a different parent");
        }
        
        if (WouldCreateCycle(child))
        {
            throw new InvalidOperationException("Cannot add child: would create a cycle");
        }
        
        _children.Add(child);
        child.Parent = this;
    }

    public bool RemoveChild(Topic child)
    {
        var removed = _children.Remove(child);
        if (removed)
        {
            child.Parent = null;
        }
        return removed;
    }

    public void RemoveFromParent()
    {
        if (Parent != null)
        {
            Parent._children.Remove(this);
            Parent = null;
        }
    }

    public bool IsRoot => Parent == null;
    public bool IsLeaf => _children.Count == 0;

    public IEnumerable<Topic> GetDescendants()
    {
        foreach (var child in _children)
        {
            yield return child;
            foreach (var descendant in child.GetDescendants())
            {
                yield return descendant;
            }
        }
    }

    public IEnumerable<Topic> GetAncestors()
    {
        var current = Parent;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    public int GetDepth()
    {
        if (IsRoot)
            return 0;
        
        var depth = 0;
        var current = Parent;
        while (current != null)
        {
            depth++;
            current = current.Parent;
        }
        return depth;
    }

    public bool IsAncestorOf(Topic other)
    {
        var current = other.Parent;
        while (current != null)
        {
            if (current == this)
                return true;
            current = current.Parent;
        }
        return false;
    }

    public bool IsDescendantOf(Topic other)
    {
        return other.IsAncestorOf(this);
    }

    private bool WouldCreateCycle(Topic potentialChild)
    {
        // Check if potentialChild is this or any of its descendants
        if (potentialChild == this)
            return true;
        
        return IsDescendantOf(potentialChild);
    }
}
