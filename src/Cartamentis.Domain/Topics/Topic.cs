using System.Collections.Immutable;
using Cartamentis.Domain.MindMaps;

namespace Cartamentis.Domain.Topics;

/// <summary>
/// Represents a single topic in a mind map.
/// A topic is a vertex in the mind map graph.
/// </summary>
public class Topic
{
    private string _content;
    private Topic? _parent;
    private readonly List<Topic> _children = new();

    /// <summary>
    /// Gets the unique identifier of this topic.
    /// </summary>
    public TopicId Id { get; }

    /// <summary>
    /// Gets or sets the content (text) of this topic.
    /// </summary>
    public string Content
    {
        get => _content;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Topic content cannot be null or whitespace.", nameof(value));
            }

            _content = value;
        }
    }

    /// <summary>
    /// Gets the parent topic. Null for the root topic.
    /// </summary>
    public Topic? Parent
    {
        get => _parent;
        internal set
        {
            // Validate: prevent cycle creation
            if (value != null && (value == this || value.IsDescendantOf(this)))
            {
                throw new InvalidOperationException("Cannot set parent: would create a cycle.");
            }

            _parent = value;
        }
    }

    /// <summary>
    /// Gets the collection of child topics.
    /// </summary>
    public IEnumerable<Topic> Children => _children.AsReadOnly();

    /// <summary>
    /// Gets a value indicating whether this topic is the root (has no parent).
    /// </summary>
    public bool IsRoot => _parent == null;

    /// <summary>
    /// Gets a value indicating whether this topic is a leaf (has no children).
    /// </summary>
    public bool IsLeaf => _children.Count == 0;

    /// <summary>
    /// Gets the depth of this topic in the hierarchy. Root has depth 0.
    /// </summary>
    public int Depth
    {
        get
        {
            if (IsRoot)
                return 0;

            var depth = 0;
            var parent = Parent;
            while (parent != null)
            {
                depth++;
                parent = parent.Parent;
            }
            return depth;
        }
    }

    /// <summary>
    /// Gets the mind map that this topic belongs to.
    /// </summary>
    public MindMap MindMap { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Topic"/> class.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="content">The content of the topic.</param>
    /// <param name="mindMap">The mind map this topic belongs to.</param>
    /// <exception cref="ArgumentNullException">Thrown when content or mindMap is null.</exception>
    /// <exception cref="ArgumentException">Thrown when content is null or whitespace.</exception>
    public Topic(TopicId id, string content, MindMap mindMap)
    {
        Id = id;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        MindMap = mindMap ?? throw new ArgumentNullException(nameof(mindMap));
    }

    /// <summary>
    /// Creates a new topic with a randomly generated ID.
    /// </summary>
    /// <param name="content">The content of the topic.</param>
    /// <param name="mindMap">The mind map this topic belongs to.</param>
    public Topic(string content, MindMap mindMap)
        : this(TopicId.NewUnique(), content, mindMap)
    {
    }

    /// <summary>
    /// Renames this topic.
    /// </summary>
    /// <param name="newContent">The new content for the topic.</param>
    /// <exception cref="ArgumentException">Thrown when newContent is null or whitespace.</exception>
    public void Rename(string newContent)
    {
        Content = newContent ?? throw new ArgumentNullException(nameof(newContent));
    }

    /// <summary>
    /// Adds a child topic to this topic.
    /// </summary>
    /// <param name="child">The child topic to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
    /// <exception cref="ArgumentException">Thrown when child already has a parent.</exception>
    public void AddChild(Topic child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (child.Parent != null)
            throw new ArgumentException("Child already has a parent.", nameof(child));

        if (child == this)
            throw new ArgumentException("Cannot add self as child.", nameof(child));

        // Check for cycles
        if (this.IsDescendantOf(child))
            throw new InvalidOperationException("Cannot add child: would create a cycle.");

        child.Parent = this;
        _children.Add(child);
    }

    /// <summary>
    /// Removes a child topic from this topic.
    /// </summary>
    /// <param name="child">The child topic to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
    /// <exception cref="ArgumentException">Thrown when child is not a child of this topic.</exception>
    public void RemoveChild(Topic child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (!_children.Contains(child))
            throw new ArgumentException("Child is not a child of this topic.", nameof(child));

        child.Parent = null;
        _children.Remove(child);
    }

    /// <summary>
    /// Removes this topic from its parent's children list.
    /// </summary>
    internal void DetachFromParent()
    {
        if (_parent != null)
        {
            _parent._children.Remove(this);
            _parent = null;
        }
    }

    /// <summary>
    /// Inserts a child topic at a specific index.
    /// </summary>
    /// <param name="index">The zero-based index at which the child should be inserted.</param>
    /// <param name="child">The child topic to insert.</param>
    /// <exception cref="ArgumentNullException">Thrown when child is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
    public void InsertChild(int index, Topic child)
    {
        if (child == null)
            throw new ArgumentNullException(nameof(child));

        if (index < 0 || index > _children.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        if (child.Parent != null)
            throw new ArgumentException("Child already has a parent.", nameof(child));

        if (child == this)
            throw new ArgumentException("Cannot add self as child.", nameof(child));

        // Check for cycles
        if (this.IsDescendantOf(child))
            throw new InvalidOperationException("Cannot insert child: would create a cycle.");

        child.Parent = this;
        _children.Insert(index, child);
    }

    /// <summary>
    /// Removes all children from this topic.
    /// </summary>
    public void ClearChildren()
    {
        foreach (var child in _children)
        {
            child.Parent = null;
        }
        _children.Clear();
    }

    /// <summary>
    /// Gets the index of a child in the children list.
    /// </summary>
    /// <param name="child">The child to find.</param>
    /// <returns>The zero-based index of the child, or -1 if not found.</returns>
    public int IndexOfChild(Topic child)
    {
        return _children.IndexOf(child);
    }

    /// <summary>
    /// Gets all ancestors of this topic, starting from the parent up to the root.
    /// </summary>
    /// <returns>An enumerable of ancestor topics.</returns>
    public IEnumerable<Topic> GetAncestors()
    {
        var ancestor = Parent;
        while (ancestor != null)
        {
            yield return ancestor;
            ancestor = ancestor.Parent;
        }
    }

    /// <summary>
    /// Gets all descendants of this topic (children, grandchildren, etc.).
    /// </summary>
    /// <returns>An enumerable of descendant topics.</returns>
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

    /// <summary>
    /// Gets the path from this topic to the root.
    /// </summary>
    /// <returns>An enumerable of topics from this topic to the root.</returns>
    public IEnumerable<Topic> GetPathToRoot()
    {
        var current = this;
        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    /// <summary>
    /// Checks if this topic is an ancestor of the specified topic.
    /// </summary>
    /// <param name="topic">The topic to check.</param>
    /// <returns>True if this topic is an ancestor of the specified topic; otherwise, false.</returns>
    public bool IsAncestorOf(Topic topic)
    {
        if (topic == null)
            return false;

        var ancestor = topic.Parent;
        while (ancestor != null)
        {
            if (ancestor == this)
                return true;
            ancestor = ancestor.Parent;
        }
        return false;
    }

    /// <summary>
    /// Checks if this topic is a descendant of the specified topic.
    /// </summary>
    /// <param name="topic">The topic to check.</param>
    /// <returns>True if this topic is a descendant of the specified topic; otherwise, false.</returns>
    public bool IsDescendantOf(Topic topic)
    {
        if (topic == null)
            return false;

        return topic.IsAncestorOf(this);
    }

    /// <summary>
    /// Gets the sibling at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the sibling.</param>
    /// <returns>The sibling topic, or null if not found.</returns>
    public Topic? GetSiblingAt(int index)
    {
        if (Parent == null)
            return null;

        var siblings = Parent._children;
        var thisIndex = siblings.IndexOf(this);
        var targetIndex = thisIndex + index;

        if (targetIndex < 0 || targetIndex >= siblings.Count)
            return null;

        return siblings[targetIndex];
    }

    /// <summary>
    /// Gets the previous sibling (the sibling immediately before this topic in the parent's children list).
    /// </summary>
    public Topic? PreviousSibling => GetSiblingAt(-1);

    /// <summary>
    /// Gets the next sibling (the sibling immediately after this topic in the parent's children list).
    /// </summary>
    public Topic? NextSibling => GetSiblingAt(1);

    /// <summary>
    /// Gets the number of siblings this topic has.
    /// </summary>
    public int SiblingCount => Parent?._children.Count - 1 ?? 0;

    /// <summary>
    /// Creates a deep copy of this topic and all its descendants.
    /// </summary>
    /// <returns>A new Topic with the same content and structure.</returns>
    public Topic Clone()
    {
        var clone = new Topic(TopicId.NewUnique(), Content, MindMap);

        foreach (var child in _children)
        {
            var clonedChild = child.Clone();
            clone.AddChild(clonedChild);
        }

        return clone;
    }

    /// <summary>
    /// Returns a string representation of this topic.
    /// </summary>
    /// <returns>The content of the topic.</returns>
    public override string ToString() => Content;

    /// <summary>
    /// Determines whether the specified object is equal to this topic.
    /// </summary>
    /// <param name="obj">The object to compare.</param>
    /// <returns>True if the objects are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Topic other)
        {
            return Id == other.Id;
        }
        return false;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for this topic.</returns>
    public override int GetHashCode() => Id.GetHashCode();

    // Internal methods for MindMap operations

    /// <summary>
    /// Changes the parent of this topic.
    /// </summary>
    /// <param name="newParent">The new parent topic.</param>
    internal void ChangeParent(Topic newParent)
    {
        if (newParent == null)
            throw new ArgumentNullException(nameof(newParent));

        if (newParent == this)
            throw new ArgumentException("Cannot set self as parent.", nameof(newParent));

        // Check for cycles
        if (newParent.IsDescendantOf(this))
            throw new InvalidOperationException("Cannot set parent: would create a cycle.");

        // Remove from old parent
        var oldParent = Parent;
        if (oldParent != null)
        {
            oldParent._children.Remove(this);
        }

        // Add to new parent
        newParent._children.Add(this);
        Parent = newParent;
    }

    /// <summary>
    /// Reparents this topic to a new parent.
    /// </summary>
    /// <param name="newParent">The new parent topic.</param>
    internal void Reparent(Topic newParent)
    {
        if (newParent == null)
            throw new ArgumentNullException(nameof(newParent));

        if (newParent == this)
            throw new ArgumentException("Cannot set self as parent.", nameof(newParent));

        if (newParent.IsDescendantOf(this))
            throw new InvalidOperationException("Cannot reparent: would create a cycle.");

        var oldParent = Parent;

        // Detach from old parent
        if (oldParent != null)
        {
            oldParent._children.Remove(this);
        }

        // Attach to new parent
        Parent = newParent;
        newParent._children.Add(this);
    }

    /// <summary>
    /// Reorders the children of this topic by moving a child to a new index.
    /// </summary>
    /// <param name="childId">The ID of the child to move.</param>
    /// <param name="newIndex">The new index for the child.</param>
    internal void MoveChildTo(TopicId childId, int newIndex)
    {
        var child = _children.FirstOrDefault(c => c.Id == childId);
        if (child == null)
            throw new ArgumentException("Child not found.", nameof(childId));

        if (newIndex < 0 || newIndex >= _children.Count)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        _children.Remove(child);
        _children.Insert(newIndex, child);
    }

    /// <summary>
    /// Reorders siblings by moving this topic to a new position.
    /// </summary>
    /// <param name="newIndex">The new index among the parent's children.</param>
    internal void MoveToSiblingIndex(int newIndex)
    {
        if (Parent == null)
            throw new InvalidOperationException("Cannot move root topic.");

        if (newIndex < 0 || newIndex >= Parent._children.Count)
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        Parent._children.Remove(this);
        Parent._children.Insert(newIndex, this);
    }
}
