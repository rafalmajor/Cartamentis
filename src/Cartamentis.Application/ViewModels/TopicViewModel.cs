using System.Collections.ObjectModel;
using Cartamentis.Domain.Topics;

namespace Cartamentis.Application.ViewModels;

/// <summary>
/// ViewModel representing a topic in the mind map visualization.
/// </summary>
public class TopicViewModel : ObservableObject
{
    private double _x;
    private double _y;

    /// <summary>
    /// Gets the unique identifier of the topic.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the content/text of the topic.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets or sets the X coordinate for positioning on the canvas.
    /// </summary>
    public double X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    /// <summary>
    /// Gets or sets the Y coordinate for positioning on the canvas.
    /// </summary>
    public double Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    /// <summary>
    /// Gets the collection of child topics.
    /// </summary>
    public ObservableCollection<TopicViewModel> Children { get; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicViewModel"/> class.
    /// </summary>
    /// <param name="domainTopic">The domain topic to wrap.</param>
    public TopicViewModel(Topic domainTopic)
    {
        // Parse the Guid from TopicId's ToString()
        if (Guid.TryParse(domainTopic.Id.ToString(), out var guid))
        {
            Id = guid;
        }
        else
        {
            // Fallback: generate new Guid if parsing fails
            Id = Guid.NewGuid();
        }
        Content = domainTopic.Content;
    }

    /// <summary>
    /// Adds a child topic to this topic.
    /// </summary>
    /// <param name="child">The child topic to add.</param>
    public void AddChild(TopicViewModel child)
    {
        Children.Add(child);
    }

    /// <summary>
    /// Removes a child topic from this topic.
    /// </summary>
    /// <param name="child">The child topic to remove.</param>
    /// <returns>True if the child was removed; otherwise, false.</returns>
    public bool RemoveChild(TopicViewModel child)
    {
        return Children.Remove(child);
    }

    /// <summary>
    /// Clears all child topics.
    /// </summary>
    public void ClearChildren()
    {
        Children.Clear();
    }
}
