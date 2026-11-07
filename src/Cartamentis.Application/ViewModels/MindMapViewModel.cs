using System.Collections.ObjectModel;
using System.Windows;
using Cartamentis.Application.Commands;
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
    private readonly MindMapService _mindMapService;
    private TopicViewModel _root = null!;
    private TopicViewModel? _selectedTopic;
    private TopicViewModel? _draggedTopic;
    private double _canvasWidth = 800;
    private double _canvasHeight = 600;

    /// <summary>
    /// Gets the root topic of the mind map.
    /// </summary>
    public TopicViewModel Root
    {
        get => _root;
        private set => SetField(ref _root, value);
    }

    /// <summary>
    /// Gets or sets the currently selected topic.
    /// </summary>
    public TopicViewModel? SelectedTopic
    {
        get => _selectedTopic;
        set
        {
            // Clear selection on previous topic
            if (_selectedTopic != null)
            {
                _selectedTopic.IsSelected = false;
            }

            // Set new selection
            if (value != null)
            {
                value.IsSelected = true;
            }

            SetField(ref _selectedTopic, value);

            // Refresh command states
            RefreshCommands();
        }
    }

    /// <summary>
    /// Gets or sets the currently dragged topic (for drag and drop).
    /// </summary>
    public TopicViewModel? DraggedTopic
    {
        get => _draggedTopic;
        set => SetField(ref _draggedTopic, value);
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
    /// Gets the canvas width.
    /// </summary>
    public double CanvasWidth
    {
        get => _canvasWidth;
        set => SetField(ref _canvasWidth, value);
    }

    /// <summary>
    /// Gets the canvas height.
    /// </summary>
    public double CanvasHeight
    {
        get => _canvasHeight;
        set => SetField(ref _canvasHeight, value);
    }

    /// <summary>
    /// Gets the underlying MindMapService for operations.
    /// </summary>
    public MindMapService MindMapService => _mindMapService;

    // ==================== Commands ====================

    /// <summary>
    /// Commands without parameters (use SelectedTopic)
    /// </summary>
    public RelayCommand AddChildCommand { get; private set; } = null!;
    public RelayCommand AddSiblingCommand { get; private set; } = null!;
    public RelayCommand RemoveLeafCommand { get; private set; } = null!;
    public RelayCommand RemoveSubtreeCommand { get; private set; } = null!;
    public RelayCommand PromoteCommand { get; private set; } = null!;
    public RelayCommand DemoteCommand { get; private set; } = null!;
    public RelayCommand CloneSubtreeCommand { get; private set; } = null!;
    public RelayCommand NewMindMapCommand { get; private set; } = null!;
    public RelayCommand SelectRootCommand { get; private set; } = null!;

    /// <summary>
    /// Commands with parameters
    /// </summary>
    public RelayCommand<TopicViewModel> SelectTopicCommand { get; private set; } = null!;
    public RelayCommand<TopicViewModel> StartDragCommand { get; private set; } = null!;
    public RelayCommand<TopicViewModel> EditTopicCommand { get; private set; } = null!;
    public RelayCommand<TopicViewModel> ExtractSubtreeCommand { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="MindMapViewModel"/> class.
    /// </summary>
    /// <param name="mindMap">The domain mind map to visualize.</param>
    /// <param name="layoutService">The layout service to use for positioning topics.</param>
    public MindMapViewModel(MindMap mindMap, LayoutService layoutService)
    {
        _layoutService = layoutService ?? throw new ArgumentNullException(nameof(layoutService));
        _mindMapService = new MindMapService(mindMap, layoutService);

        if (mindMap == null)
        {
            throw new ArgumentNullException(nameof(mindMap));
        }

        // Map the domain model to view models
        Root = MapTopic(mindMap.Root);

        // Select the root by default
        SelectedTopic = Root;

        // Calculate initial layout with default canvas size
        CalculateLayout(_canvasWidth, _canvasHeight);

        // Initialize commands
        InitializeCommands();
    }

    /// <summary>
    /// Initializes the commands for the view model.
    /// </summary>
    private void InitializeCommands()
    {
        // Commands without parameters
        AddChildCommand = new RelayCommand(
            execute: () => AddChild("New Topic"),
            canExecute: () => SelectedTopic != null
        );

        AddSiblingCommand = new RelayCommand(
            execute: () => AddSibling("New Topic"),
            canExecute: () => SelectedTopic != null && !SelectedTopic.IsRoot
        );

        RemoveLeafCommand = new RelayCommand(
            execute: RemoveLeaf,
            canExecute: CanRemoveLeaf
        );

        RemoveSubtreeCommand = new RelayCommand(
            execute: RemoveSubtree,
            canExecute: CanRemoveSubtree
        );

        PromoteCommand = new RelayCommand(
            execute: Promote,
            canExecute: CanPromote
        );

        DemoteCommand = new RelayCommand(
            execute: Demote,
            canExecute: CanDemote
        );

        CloneSubtreeCommand = new RelayCommand(
            execute: CloneSubtree,
            canExecute: () => SelectedTopic != null
        );

        NewMindMapCommand = new RelayCommand(
            execute: () => RebuildViewModel()
        );

        SelectRootCommand = new RelayCommand(
            execute: () => SelectedTopic = Root
        );

        // Commands with parameters
        SelectTopicCommand = new RelayCommand<TopicViewModel>(
            execute: SelectTopic,
            canExecute: _ => true
        );

        StartDragCommand = new RelayCommand<TopicViewModel>(
            execute: StartDrag,
            canExecute: _ => true
        );

        EditTopicCommand = new RelayCommand<TopicViewModel>(
            execute: EditTopicName,
            canExecute: _ => true
        );

        ExtractSubtreeCommand = new RelayCommand<TopicViewModel>(
            execute: ExtractSubtree,
            canExecute: _ => true
        );
    }

    /// <summary>
    /// Refreshes the CanExecute state for all commands.
    /// With CommunityToolkit.Mvvm, we need to recreate the commands to update CanExecute.
    /// </summary>
    private void RefreshCommands()
    {
        InitializeCommands();
    }

    // ==================== Selection & Drag/Drop ====================

    /// <summary>
    /// Selects a topic.
    /// </summary>
    /// <param name="topic">The topic to select.</param>
    private void SelectTopic(TopicViewModel? topic)
    {
        SelectedTopic = topic;
    }

    /// <summary>
    /// Starts dragging a topic.
    /// </summary>
    /// <param name="topic">The topic to drag.</param>
    private void StartDrag(TopicViewModel topic)
    {
        DraggedTopic = topic;
    }

    /// <summary>
    /// Completes the drag operation.
    /// </summary>
    /// <param name="target">The target topic for the drop.</param>
    public void CompleteDrag(TopicViewModel target)
    {
        if (DraggedTopic == null || target == null) return;
        if (DraggedTopic == target) return;

        var draggedDomain = FindDomainTopic(DraggedTopic.Id);
        var targetDomain = FindDomainTopic(target.Id);

        if (draggedDomain == null || targetDomain == null) return;

        // Check if this would create a cycle
        if (targetDomain.IsDescendantOf(draggedDomain))
        {
            // Cannot move parent under its own descendant
            DraggedTopic = null;
            return;
        }

        _mindMapService.MoveSubtree(draggedDomain.Id, targetDomain.Id);
        RebuildViewModel();
        SelectedTopic = FindViewModelByTopicId(draggedDomain.Id);
        DraggedTopic = null;
    }

    /// <summary>
    /// Cancels the drag operation.
    /// </summary>
    public void CancelDrag()
    {
        DraggedTopic = null;
    }

    /// <summary>
    /// Checks if a drop is allowed on the target topic.
    /// </summary>
    /// <param name="target">The target topic.</param>
    /// <returns>True if the drop is allowed; otherwise, false.</returns>
    public bool CanDrop(TopicViewModel target)
    {
        if (DraggedTopic == null || target == null) return false;
        if (DraggedTopic == target) return false;

        var draggedDomain = FindDomainTopic(DraggedTopic.Id);
        var targetDomain = FindDomainTopic(target.Id);

        if (draggedDomain == null || targetDomain == null) return false;

        // Check if this would create a cycle
        return !targetDomain.IsDescendantOf(draggedDomain);
    }

    // ==================== Operation Methods ====================

    /// <summary>
    /// Adds a child topic to the selected topic.
    /// </summary>
    /// <param name="content">The content of the new child topic.</param>
    public void AddChild(string content)
    {
        if (SelectedTopic == null) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        var newTopic = _mindMapService.AddChild(domainTopic.Id, content);
        var newViewModel = MapTopic(newTopic);

        SelectedTopic.AddChild(newViewModel);
        CalculateLayout(CanvasWidth, CanvasHeight);
        SelectedTopic = newViewModel;
    }

    /// <summary>
    /// Adds a sibling topic to the selected topic.
    /// </summary>
    /// <param name="content">The content of the new sibling topic.</param>
    public void AddSibling(string content)
    {
        if (SelectedTopic == null || SelectedTopic.IsRoot) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        var newTopic = _mindMapService.AddSibling(domainTopic.Id, content);
        var newViewModel = MapTopic(newTopic);

        var parent = FindViewModelByTopicId(domainTopic.Parent!.Id);
        if (parent != null)
        {
            parent.AddChild(newViewModel);
        }

        CalculateLayout(CanvasWidth, CanvasHeight);
        SelectedTopic = newViewModel;
    }

    /// <summary>
    /// Removes the selected topic if it is a leaf.
    /// </summary>
    public void RemoveLeaf()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null || !domainTopic.IsLeaf) return;

        var parent = FindParentViewModel(SelectedTopic);
        if (parent == null) return;

        _mindMapService.RemoveLeaf(domainTopic.Id);
        parent.RemoveChild(SelectedTopic);
        CalculateLayout(CanvasWidth, CanvasHeight);
        SelectedTopic = null;
    }

    /// <summary>
    /// Removes the selected topic and all its descendants.
    /// </summary>
    public void RemoveSubtree()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        var parent = FindParentViewModel(SelectedTopic);
        if (parent == null) return;

        _mindMapService.RemoveSubtree(domainTopic.Id);
        parent.RemoveChild(SelectedTopic);
        CalculateLayout(CanvasWidth, CanvasHeight);
        SelectedTopic = null;
    }

    /// <summary>
    /// Promotes the selected topic one level toward the root.
    /// </summary>
    public void Promote()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        _mindMapService.Promote(domainTopic.Id);
        RebuildViewModel();
        SelectedTopic = FindViewModelByTopicId(domainTopic.Id);
    }

    /// <summary>
    /// Demotes the selected topic one level deeper.
    /// </summary>
    public void Demote()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        _mindMapService.Demote(domainTopic.Id);
        RebuildViewModel();
        SelectedTopic = FindViewModelByTopicId(domainTopic.Id);
    }

    /// <summary>
    /// Clones the selected topic and all its descendants.
    /// </summary>
    public void CloneSubtree()
    {
        if (SelectedTopic == null) return;

        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null) return;

        var parent = domainTopic.Parent ?? domainTopic;
        var clonedTopic = _mindMapService.CloneSubtree(domainTopic.Id, parent.Id);

        RebuildViewModel();
        SelectedTopic = FindViewModelByTopicId(clonedTopic.Id);
    }

    /// <summary>
    /// Extracts the selected topic as a new mind map.
    /// </summary>
    /// <param name="topic">The topic to extract.</param>
    public void ExtractSubtree(TopicViewModel topic)
    {
        if (topic == null) return;

        var domainTopic = FindDomainTopic(topic.Id);
        if (domainTopic == null) return;

        var newMindMapService = _mindMapService.ExtractSubtree(domainTopic.Id);

        // TODO: Open new window with the extracted mind map
        // For now, just rebuild the current view
        RebuildViewModel();
    }

    /// <summary>
    /// Edits the name of a topic.
    /// </summary>
    /// <param name="topic">The topic to edit.</param>
    public void EditTopicName(TopicViewModel topic)
    {
        if (topic == null) return;

        var domainTopic = FindDomainTopic(topic.Id);
        if (domainTopic == null) return;

        // For now, just append " (Edited)" to the content
        // In a real implementation, you'd show a proper dialog
        var newContent = topic.Content + " (Edited)";
        domainTopic.Rename(newContent);
        topic.Rename(newContent);
    }

    // ==================== Layout ====================

    /// <summary>
    /// Recalculates the layout with the specified canvas dimensions.
    /// </summary>
    /// <param name="width">The width of the canvas.</param>
    /// <param name="height">The height of the canvas.</param>
    public void CalculateLayout(double width, double height)
    {
        CanvasWidth = width;
        CanvasHeight = height;

        if (Root != null)
        {
            _layoutService.CalculateLayout(Root, width, height);
        }
    }

    // ==================== Helper Methods ====================

    /// <summary>
    /// Rebuilds the entire view model from the domain model.
    /// </summary>
    private void RebuildViewModel()
    {
        var oldSelectedId = SelectedTopic?.Id;
        Root = MapTopic(_mindMapService.CurrentMindMap.Root);
        CalculateLayout(CanvasWidth, CanvasHeight);

        // Restore selection if possible
        if (oldSelectedId != null)
        {
            SelectedTopic = FindViewModel(oldSelectedId.Value);
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
            var childViewModel = MapTopic(child);
            viewModel.AddChild(childViewModel);
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

    // ==================== Validation Methods ====================

    /// <summary>
    /// Checks if the selected topic can be removed as a leaf.
    /// </summary>
    private bool CanRemoveLeaf()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return false;
        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        return domainTopic?.IsLeaf ?? false;
    }

    /// <summary>
    /// Checks if the selected topic can be removed with its subtree.
    /// </summary>
    private bool CanRemoveSubtree()
    {
        return SelectedTopic != null && SelectedTopic != Root;
    }

    /// <summary>
    /// Checks if the selected topic can be promoted.
    /// </summary>
    private bool CanPromote()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return false;
        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        return domainTopic != null && domainTopic.Parent != null && domainTopic.Parent.Parent != null;
    }

    /// <summary>
    /// Checks if the selected topic can be demoted.
    /// </summary>
    private bool CanDemote()
    {
        if (SelectedTopic == null || SelectedTopic == Root) return false;
        var domainTopic = FindDomainTopic(SelectedTopic.Id);
        if (domainTopic == null || domainTopic.Parent == null) return false;

        // Check if there is a previous sibling
        var siblings = domainTopic.Parent.Children.ToList();
        var index = siblings.IndexOf(domainTopic);
        return index > 0;
    }

    // ==================== Find Methods ====================

    /// <summary>
    /// Finds the domain topic corresponding to a view model.
    /// </summary>
    /// <param name="viewModelId">The view model identifier (Guid).</param>
    /// <returns>The domain topic, or null if not found.</returns>
    private Topic? FindDomainTopic(Guid viewModelId)
    {
        var topicId = new TopicId(viewModelId);
        return _mindMapService.FindTopic(topicId);
    }

    /// <summary>
    /// Finds the view model by Guid.
    /// </summary>
    private TopicViewModel? FindViewModel(Guid topicId)
    {
        return AllTopics.FirstOrDefault(t => t.Id == topicId);
    }

    /// <summary>
    /// Finds the view model by TopicId.
    /// </summary>
    private TopicViewModel? FindViewModelByTopicId(TopicId topicId)
    {
        if (Guid.TryParse(topicId.ToString(), out var guid))
        {
            return AllTopics.FirstOrDefault(t => t.Id == guid);
        }
        return null;
    }

    /// <summary>
    /// Finds the parent view model of the specified topic.
    /// </summary>
    private TopicViewModel? FindParentViewModel(TopicViewModel topic)
    {
        var domainTopic = FindDomainTopic(topic.Id);
        if (domainTopic == null || domainTopic.Parent == null) return null;
        return FindViewModelByTopicId(domainTopic.Parent.Id);
    }
}
