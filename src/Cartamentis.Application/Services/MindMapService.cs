using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;

namespace Cartamentis.Application.Services;

/// <summary>
/// Service for managing mind map operations at the application layer.
/// This service bridges the domain model with application-level concerns.
/// </summary>
public class MindMapService
{
    private readonly LayoutService _layoutService;

    /// <summary>
    /// Gets the current mind map being managed.
    /// </summary>
    public MindMap CurrentMindMap { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MindMapService"/> class.
    /// </summary>
    /// <param name="mindMap">The initial mind map.</param>
    /// <param name="layoutService">The layout service to use.</param>
    public MindMapService(MindMap mindMap, LayoutService layoutService)
    {
        CurrentMindMap = mindMap ?? throw new ArgumentNullException(nameof(mindMap));
        _layoutService = layoutService ?? throw new ArgumentNullException(nameof(layoutService));
    }

    /// <summary>
    /// Finds a topic by its ID in the current mind map.
    /// </summary>
    /// <param name="topicId">The topic ID to find.</param>
    /// <returns>The topic, or null if not found.</returns>
    public Topic? FindTopic(TopicId topicId)
    {
        return FindTopicRecursive(CurrentMindMap.Root, topicId);
    }

    /// <summary>
    /// Finds a topic by its Guid.
    /// </summary>
    public Topic? FindTopic(Guid topicId)
    {
        return FindTopic(new TopicId(topicId));
    }

    private Topic? FindTopicRecursive(Topic topic, TopicId topicId)
    {
        if (topic.Id == topicId)
            return topic;

        foreach (var child in topic.Children)
        {
            var found = FindTopicRecursive(child, topicId);
            if (found != null)
                return found;
        }

        return null;
    }

    /// <summary>
    /// Adds a child topic to the specified parent topic.
    /// </summary>
    /// <param name="parentId">The ID of the parent topic.</param>
    /// <param name="content">The content of the new child topic.</param>
    /// <returns>The newly created topic.</returns>
    public Topic AddChild(TopicId parentId, string content)
    {
        var parent = FindTopic(parentId);
        if (parent == null)
            throw new ArgumentException("Parent topic not found.", nameof(parentId));

        var newTopic = new Topic(content, CurrentMindMap);
        parent.AddChild(newTopic);
        return newTopic;
    }

    /// <summary>
    /// Adds a sibling topic next to the specified topic.
    /// </summary>
    /// <param name="siblingId">The ID of the topic to add a sibling next to.</param>
    /// <param name="content">The content of the new sibling topic.</param>
    /// <returns>The newly created topic.</returns>
    public Topic AddSibling(TopicId siblingId, string content)
    {
        var sibling = FindTopic(siblingId);
        if (sibling == null)
            throw new ArgumentException("Sibling topic not found.", nameof(siblingId));

        if (sibling.IsRoot)
            throw new InvalidOperationException("Cannot add sibling to root topic.");

        var parent = sibling.Parent;
        if (parent == null)
            throw new InvalidOperationException("Sibling has no parent.");

        var newTopic = new Topic(content, CurrentMindMap);
        parent.AddChild(newTopic);
        return newTopic;
    }

    /// <summary>
    /// Removes a leaf topic (a topic with no children).
    /// </summary>
    /// <param name="topicId">The ID of the topic to remove.</param>
    public void RemoveLeaf(TopicId topicId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicId));

        if (!topic.IsLeaf)
            throw new InvalidOperationException("Cannot remove non-leaf topic as leaf. Use RemoveSubtree instead.");

        if (topic.IsRoot)
            throw new InvalidOperationException("Cannot remove root topic.");

        var parent = topic.Parent;
        if (parent != null)
        {
            parent.RemoveChild(topic);
        }
    }

    /// <summary>
    /// Removes a topic and all its descendants.
    /// </summary>
    /// <param name="topicId">The ID of the topic to remove.</param>
    public void RemoveSubtree(TopicId topicId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicId));

        if (topic.IsRoot)
            throw new InvalidOperationException("Cannot remove root topic.");

        var parent = topic.Parent;
        if (parent != null)
        {
            parent.RemoveChild(topic);
        }
    }

    /// <summary>
    /// Moves a subtree to a new parent.
    /// </summary>
    /// <param name="topicId">The ID of the topic to move.</param>
    /// <param name="newParentId">The ID of the new parent.</param>
    public void MoveSubtree(TopicId topicId, TopicId newParentId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic to move not found.", nameof(topicId));

        var newParent = FindTopic(newParentId);
        if (newParent == null)
            throw new ArgumentException("New parent not found.", nameof(newParentId));

        if (topic == newParent)
            throw new InvalidOperationException("Cannot move topic to itself.");

        // Check for cycles
        if (newParent.IsDescendantOf(topic))
            throw new InvalidOperationException("Cannot move topic: would create a cycle.");

        // Detach from old parent
        var oldParent = topic.Parent;
        if (oldParent != null)
        {
            oldParent.RemoveChild(topic);
        }

        // Attach to new parent
        newParent.AddChild(topic);
    }

    /// <summary>
    /// Promotes a topic one level toward the root.
    /// </summary>
    /// <param name="topicId">The ID of the topic to promote.</param>
    public void Promote(TopicId topicId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicId));

        if (topic.IsRoot)
            throw new InvalidOperationException("Cannot promote root topic.");

        var parent = topic.Parent;
        if (parent == null || parent.IsRoot)
            throw new InvalidOperationException("Cannot promote: already at root level.");

        var grandparent = parent.Parent;
        if (grandparent == null)
            throw new InvalidOperationException("Cannot promote: parent has no parent.");

        // Detach from parent
        parent.RemoveChild(topic);

        // Attach to grandparent
        grandparent.AddChild(topic);
    }

    /// <summary>
    /// Demotes a topic one level deeper (makes it a child of its previous sibling).
    /// </summary>
    /// <param name="topicId">The ID of the topic to demote.</param>
    public void Demote(TopicId topicId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicId));

        if (topic.IsRoot)
            throw new InvalidOperationException("Cannot demote root topic.");

        var parent = topic.Parent;
        if (parent == null)
            throw new InvalidOperationException("Cannot demote: topic has no parent.");

        var previousSibling = topic.PreviousSibling;
        if (previousSibling == null)
            throw new InvalidOperationException("Cannot demote: no previous sibling to demote under.");

        // Detach from parent
        parent.RemoveChild(topic);

        // Attach to previous sibling
        previousSibling.AddChild(topic);
    }

    /// <summary>
    /// Clones a subtree under a new parent.
    /// </summary>
    /// <param name="topicId">The ID of the topic to clone.</param>
    /// <param name="newParentId">The ID of the new parent for the cloned subtree.</param>
    /// <returns>The cloned topic (new instance).</returns>
    public Topic CloneSubtree(TopicId topicId, TopicId newParentId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic to clone not found.", nameof(topicId));

        var newParent = FindTopic(newParentId);
        if (newParent == null)
            throw new ArgumentException("New parent not found.", nameof(newParentId));

        // Deep clone the topic and its subtree
        var clonedTopic = CloneTopic(topic, CurrentMindMap);

        // Attach to new parent
        newParent.AddChild(clonedTopic);

        return clonedTopic;
    }

    /// <summary>
    /// Deep clones a topic and all its descendants.
    /// </summary>
    private Topic CloneTopic(Topic topic, MindMap mindMap)
    {
        var clonedTopic = new Topic(topic.Content, mindMap);

        foreach (var child in topic.Children)
        {
            var clonedChild = CloneTopic(child, mindMap);
            clonedTopic.AddChild(clonedChild);
        }

        return clonedTopic;
    }

    /// <summary>
    /// Extracts a subtree as a new mind map.
    /// </summary>
    /// <param name="topicId">The ID of the topic to extract.</param>
    /// <returns>A new MindMapService containing the extracted subtree.</returns>
    public MindMapService ExtractSubtree(TopicId topicId)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic to extract not found.", nameof(topicId));

        // Clone the topic and its subtree
        var clonedTopic = CloneTopic(topic, null!);

        // Create a new mind map with the cloned topic as root
        // Use CreateFromRoot which handles the construction properly
        var newMindMap = MindMap.CreateFromRoot(clonedTopic);

        return new MindMapService(newMindMap, _layoutService);
    }

    /// <summary>
    /// Reorders a topic among its siblings.
    /// </summary>
    /// <param name="topicId">The ID of the topic to move.</param>
    /// <param name="newIndex">The new index among the parent's children.</param>
    public void ReorderSibling(TopicId topicId, int newIndex)
    {
        var topic = FindTopic(topicId);
        if (topic == null)
            throw new ArgumentException("Topic not found.", nameof(topicId));

        if (topic.IsRoot)
            throw new InvalidOperationException("Cannot reorder root topic.");

        var parent = topic.Parent;
        if (parent == null)
            throw new InvalidOperationException("Topic has no parent.");

        var currentIndex = parent.IndexOfChild(topic);
        if (currentIndex < 0)
            throw new InvalidOperationException("Topic is not a child of its parent.");

        if (newIndex < 0 || newIndex >= parent.Children.Count())
            throw new ArgumentOutOfRangeException(nameof(newIndex));

        parent.MoveChildTo(topic.Id, newIndex);
    }
}
