using System.Collections.ObjectModel;
using Cartamentis.Application.Services;
using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;

namespace Cartamentis.Application.ViewModels;

/// <summary>
/// ViewModel representing the entire mind map for visualization.
/// </summary>
public class MindMapViewModel : ObservableObject
{
    private readonly LayoutService _layoutService;
    private TopicViewModel _root = null!;

    /// <summary>
    /// Gets the root topic of the mind map.
    /// </summary>
    public TopicViewModel Root
    {
        get => _root;
        private set => SetField(ref _root, value);
    }

    /// <summary>
    /// Gets all topics in the mind map (flattened list).
    /// </summary>
    public IEnumerable<TopicViewModel> AllTopics => Flatten(Root);

    /// <summary>
    /// Gets all connections between topics as pairs of (parent, child).
    /// </summary>
    public IEnumerable<(TopicViewModel From, TopicViewModel To)> Connections => GetAllConnections(Root);

    /// <summary>
    /// Initializes a new instance of the <see cref="MindMapViewModel"/> class.
    /// </summary>
    /// <param name="mindMap">The domain mind map to visualize.</param>
    /// <param name="layoutService">The layout service to use for positioning topics.</param>
    public MindMapViewModel(MindMap mindMap, LayoutService layoutService)
    {
        _layoutService = layoutService ?? throw new ArgumentNullException(nameof(layoutService));
        
        if (mindMap == null)
        {
            throw new ArgumentNullException(nameof(mindMap));
        }

        // Map the domain model to view models
        Root = MapTopic(mindMap.Root);
        
        // Calculate initial layout with default canvas size
        CalculateLayout(800, 600);
    }

    /// <summary>
    /// Recalculates the layout with the specified canvas dimensions.
    /// </summary>
    /// <param name="width">The width of the canvas.</param>
    /// <param name="height">The height of the canvas.</param>
    public void CalculateLayout(double width, double height)
    {
        if (Root != null)
        {
            _layoutService.CalculateLayout(Root, width, height);
        }
    }

    /// <summary>
    /// Maps a domain topic to a TopicViewModel.
    /// </summary>
    /// <param name="domainTopic">The domain topic to map.</param>
    /// <returns>The corresponding TopicViewModel.</returns>
    private TopicViewModel MapTopic(Topic domainTopic)
    {
        var viewModel = new TopicViewModel(domainTopic);
        
        // Recursively map children
        foreach (var child in domainTopic.Children)
        {
            viewModel.AddChild(MapTopic(child));
        }
        
        return viewModel;
    }

    /// <summary>
    /// Flattens the topic hierarchy into a single list.
    /// </summary>
    /// <param name="topic">The root topic to start from.</param>
    /// <returns>A flattened list of all topics.</returns>
    private IEnumerable<TopicViewModel> Flatten(TopicViewModel topic)
    {
        if (topic == null)
        {
            yield break;
        }

        yield return topic;

        foreach (var child in topic.Children)
        {
            foreach (var descendant in Flatten(child))
            {
                yield return descendant;
            }
        }
    }

    /// <summary>
    /// Gets all connections (parent-child relationships) in the mind map.
    /// </summary>
    /// <param name="topic">The root topic to start from.</param>
    /// <returns>A list of connections as (parent, child) tuples.</returns>
    private IEnumerable<(TopicViewModel From, TopicViewModel To)> GetAllConnections(TopicViewModel topic)
    {
        if (topic == null)
        {
            yield break;
        }

        foreach (var child in topic.Children)
        {
            yield return (topic, child);

            foreach (var connection in GetAllConnections(child))
            {
                yield return connection;
            }
        }
    }
}
