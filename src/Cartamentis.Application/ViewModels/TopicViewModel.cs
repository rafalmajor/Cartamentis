using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cartamentis.Domain.Topics;

namespace Cartamentis.Application.ViewModels;

/// <summary>
/// ViewModel representing a single topic in a mind map.
/// </summary>
public class TopicViewModel : ObservableObject
{
    private readonly Topic _domainTopic;
    private double _x;
    private double _y;
    private bool _isSelected;
    private string _content;
    private TopicViewModel? _parentViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicViewModel"/> class.
    /// </summary>
    /// <param name="domainTopic">The domain topic to wrap.</param>
    public TopicViewModel(Topic domainTopic)
    {
        _domainTopic = domainTopic ?? throw new ArgumentNullException(nameof(domainTopic));
        _content = domainTopic.Content;
        Children = new ObservableCollection<TopicViewModel>();
    }

    /// <summary>
    /// Gets the unique identifier of this topic.
    /// </summary>
    public Guid Id => _domainTopic.Id.Value;

    /// <summary>
    /// Gets or sets the content (text) of this topic.
    /// </summary>
    public string Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged();
                _domainTopic.Rename(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the X coordinate for layout.
    /// </summary>
    public double X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    /// <summary>
    /// Gets or sets the Y coordinate for layout.
    /// </summary>
    public double Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this topic is selected.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set => SetField(ref _isSelected, value);
    }

    /// <summary>
    /// Gets or sets the parent ViewModel.
    /// </summary>
    public TopicViewModel? ParentViewModel
    {
        get => _parentViewModel;
        set => SetField(ref _parentViewModel, value);
    }

    /// <summary>
    /// Gets the domain Topic object.
    /// </summary>
    public Topic DomainTopic => _domainTopic;

    /// <summary>
    /// Gets the collection of child topics.
    /// </summary>
    public ObservableCollection<TopicViewModel> Children { get; }

    /// <summary>
    /// Gets a value indicating whether this topic is the root topic (has no parent).
    /// </summary>
    public bool IsRoot => _domainTopic.Parent == null;

    /// <summary>
    /// Gets a value indicating whether this topic is a leaf (has no children).
    /// </summary>
    public bool IsLeaf => Children.Count == 0;

    /// <summary>
    /// Gets the depth of this topic in the hierarchy (0 for root).
    /// </summary>
    public int Depth
    {
        get
        {
            var depth = 0;
            var parent = ParentViewModel;
            while (parent != null)
            {
                depth++;
                parent = parent.ParentViewModel;
            }
            return depth;
        }
    }

    /// <summary>
    /// Renames the topic.
    /// </summary>
    /// <param name="newContent">The new content for the topic.</param>
    public void Rename(string newContent)
    {
        if (newContent != Content)
        {
            Content = newContent;
        }
    }

    /// <summary>
    /// Adds a child topic to this topic.
    /// </summary>
    /// <param name="child">The child topic to add.</param>
    public void AddChild(TopicViewModel child)
    {
        child.ParentViewModel = this;
        Children.Add(child);
    }

    /// <summary>
    /// Removes a child topic from this topic.
    /// </summary>
    /// <param name="child">The child topic to remove.</param>
    public void RemoveChild(TopicViewModel child)
    {
        child.ParentViewModel = null;
        Children.Remove(child);
    }

    /// <summary>
    /// Inserts a child at a specific index.
    /// </summary>
    /// <param name="index">The index to insert at.</param>
    /// <param name="child">The child to insert.</param>
    public void InsertChild(int index, TopicViewModel child)
    {
        child.ParentViewModel = this;
        Children.Insert(index, child);
    }

    /// <summary>
    /// Gets the index of a child in the Children collection.
    /// </summary>
    /// <param name="child">The child to find.</param>
    /// <returns>The index, or -1 if not found.</returns>
    public int IndexOfChild(TopicViewModel child)
    {
        return Children.IndexOf(child);
    }

    /// <summary>
    /// Updates the parent reference of this topic.
    /// </summary>
    /// <param name="newParent">The new parent ViewModel.</param>
    public void UpdateParent(TopicViewModel? newParent)
    {
        ParentViewModel = newParent;
    }
}
