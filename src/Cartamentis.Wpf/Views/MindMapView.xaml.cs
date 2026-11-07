using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cartamentis.Application.ViewModels;

namespace Cartamentis.Wpf.Views;

/// <summary>
/// Interaction logic for MindMapView.xaml
/// </summary>
public partial class MindMapView : UserControl
{
    private Point _dragStartPoint;
    private TopicViewModel? _draggedTopic;
    private bool _isDragging;

    public MindMapView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (DataContext is MindMapViewModel viewModel)
        {
            // Initial layout calculation
            CalculateAndApplyLayout(viewModel);

            // Recalculate layout when canvas size changes
            MindMapCanvas.SizeChanged += (s, args) =>
            {
                CalculateAndApplyLayout(viewModel);
            };
        }
    }

    private void CalculateAndApplyLayout(MindMapViewModel viewModel)
    {
        // Calculate layout with current canvas dimensions
        viewModel.CalculateLayout(MindMapCanvas.ActualWidth, MindMapCanvas.ActualHeight);
    }

    // ==================== Drag and Drop ====================

    private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(MindMapCanvas);
        _isDragging = false;
        
        // Get the topic under the mouse
        var hitTestResult = VisualTreeHelper.HitTest(MindMapCanvas, _dragStartPoint);
        if (hitTestResult?.VisualHit is Border border)
        {
            var topic = border.DataContext as TopicViewModel;
            if (topic != null)
            {
                _draggedTopic = topic;
                border.CaptureMouse();
            }
        }
        else
        {
            // Clicked on empty space - clear selection
            if (DataContext is MindMapViewModel viewModel)
            {
                viewModel.SelectedTopic = null;
            }
        }
    }

    private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_draggedTopic == null || Mouse.Captured == null) return;

        var currentPosition = e.GetPosition(MindMapCanvas);
        var distance = Point.Subtract(currentPosition, _dragStartPoint).Length;

        // Start drag operation if mouse has moved far enough
        if (distance > SystemParameters.MinimumHorizontalDragDistance &&
            distance > SystemParameters.MinimumVerticalDragDistance && !_isDragging)
        {
            _isDragging = true;
            
            if (DataContext is MindMapViewModel viewModel)
            {
                viewModel.DraggedTopic = _draggedTopic;
                viewModel.SelectedTopic = _draggedTopic;
            }

            // Show drag adorner
            DragAdorner.Visibility = Visibility.Visible;
            DragAdornerText.Text = _draggedTopic.Content;
            Canvas.SetLeft(DragAdorner, currentPosition.X - 60);
            Canvas.SetTop(DragAdorner, currentPosition.Y - 20);

            // Release mouse capture from the border
            if (Mouse.Captured is Border)
            {
                Mouse.Capture(null);
            }
        }

        if (_isDragging)
        {
            // Update adorner position
            Canvas.SetLeft(DragAdorner, currentPosition.X - 60);
            Canvas.SetTop(DragAdorner, currentPosition.Y - 20);
        }
    }

    private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isDragging && _draggedTopic != null)
        {
            // Single click - select topic
            if (DataContext is MindMapViewModel currentViewModel)
            {
                currentViewModel.SelectedTopic = _draggedTopic;
            }
        }
        else if (_isDragging)
        {
            // Drop operation
            var dropPosition = e.GetPosition(MindMapCanvas);
            var hitTestResult = VisualTreeHelper.HitTest(MindMapCanvas, dropPosition);
            
            TopicViewModel? dropTarget = null;
            
            // Find the topic under the drop position
            if (hitTestResult?.VisualHit is Border targetBorder)
            {
                dropTarget = targetBorder.DataContext as TopicViewModel;
            }
            
            if (DataContext is MindMapViewModel currentViewModel)
            {
                if (dropTarget != null && currentViewModel.CanDrop(dropTarget))
                {
                    currentViewModel.CompleteDrag(dropTarget);
                }
                else
                {
                    currentViewModel.CancelDrag();
                }
            }

            // Hide drag adorner
            DragAdorner.Visibility = Visibility.Collapsed;
        }

        // Clean up
        if (Mouse.Captured != null)
        {
            Mouse.Capture(null);
        }
        
        _draggedTopic = null;
        _isDragging = false;
        
        if (DataContext is MindMapViewModel viewModel)
        {
            viewModel.DraggedTopic = null;
        }
    }

    private void Canvas_Drop(object sender, DragEventArgs e)
    {
        // Handle external drag/drop if needed
        e.Handled = true;
    }

    private void Canvas_DragOver(object sender, DragEventArgs e)
    {
        if (DataContext is MindMapViewModel viewModel && viewModel.DraggedTopic != null)
        {
            var position = e.GetPosition(MindMapCanvas);
            var hitTestResult = VisualTreeHelper.HitTest(MindMapCanvas, position);
            
            if (hitTestResult?.VisualHit is Border border)
            {
                var target = border.DataContext as TopicViewModel;
                if (target != null && viewModel.CanDrop(target))
                {
                    e.Effects = DragDropEffects.Move;
                    e.Handled = true;
                    return;
                }
            }
        }

        e.Effects = DragDropEffects.None;
        e.Handled = true;
    }

    private void Canvas_DragLeave(object sender, DragEventArgs e)
    {
        // Clean up drag state if needed
    }

    // ==================== Mouse Events ====================

    private void Topic_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is Border border && border.DataContext is TopicViewModel topic)
        {
            // If we're starting a drag from the topic itself, capture it
            if (_draggedTopic == null)
            {
                _draggedTopic = topic;
                border.CaptureMouse();
                e.Handled = true;
            }
        }
    }

    private void Topic_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        // PreviewMouseMove handles tunneling - we use this to check if drag should start
        if (_draggedTopic != null && Mouse.Captured == sender)
        {
            var currentPosition = e.GetPosition(MindMapCanvas);
            var distance = Point.Subtract(currentPosition, _dragStartPoint).Length;

            if (distance > SystemParameters.MinimumHorizontalDragDistance &&
                distance > SystemParameters.MinimumVerticalDragDistance && !_isDragging)
            {
                _isDragging = true;
                
                if (DataContext is MindMapViewModel viewModel)
                {
                    viewModel.DraggedTopic = _draggedTopic;
                    viewModel.SelectedTopic = _draggedTopic;
                }

                // Show drag adorner
                DragAdorner.Visibility = Visibility.Visible;
                DragAdornerText.Text = _draggedTopic.Content;
                Canvas.SetLeft(DragAdorner, currentPosition.X - 60);
                Canvas.SetTop(DragAdorner, currentPosition.Y - 20);

                // Release mouse capture
                Mouse.Capture(null);
            }
        }
    }

    private void Topic_MouseMove(object sender, MouseEventArgs e)
    {
        // Handle mouse move for drag adorner positioning
        if (_isDragging)
        {
            var currentPosition = e.GetPosition(MindMapCanvas);
            Canvas.SetLeft(DragAdorner, currentPosition.X - 60);
            Canvas.SetTop(DragAdorner, currentPosition.Y - 20);
        }
    }

    private void Topic_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Select the topic and show context menu
        if (sender is Border border && border.DataContext is TopicViewModel topic)
        {
            if (DataContext is MindMapViewModel viewModel)
            {
                viewModel.SelectedTopic = topic;
            }
        }
        
        e.Handled = false; // Allow context menu to open
    }

    private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Right-click on empty canvas - clear selection
        var hitTestResult = VisualTreeHelper.HitTest(MindMapCanvas, e.GetPosition(MindMapCanvas));
        if (!(hitTestResult?.VisualHit is Border))
        {
            if (DataContext is MindMapViewModel viewModel)
            {
                viewModel.SelectedTopic = null;
            }
        }
    }
}
