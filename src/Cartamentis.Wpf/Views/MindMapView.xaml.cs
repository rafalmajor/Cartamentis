using System.Windows;
using System.Windows.Controls;
using Cartamentis.Application.ViewModels;

namespace Cartamentis.Wpf.Views;

/// <summary>
/// Interaction logic for MindMapView.xaml
/// </summary>
public partial class MindMapView : UserControl
{
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
}
