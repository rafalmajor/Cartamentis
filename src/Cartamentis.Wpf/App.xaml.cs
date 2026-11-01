using System.Windows;
using Cartamentis.Application.Services;
using Cartamentis.Application.ViewModels;
using Cartamentis.Domain.MindMaps;
using Cartamentis.Domain.Topics;

namespace Cartamentis.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Create a sample mind map for demonstration
        var mindMap = CreateSampleMindMap();

        // Set up services
        var layoutService = new LayoutService();
        var viewModel = new MindMapViewModel(mindMap, layoutService);

        // Create and show the main window
        MainWindow = new MainWindow { DataContext = viewModel };
        MainWindow.Show();
    }

    /// <summary>
    /// Creates a sample mind map for testing and demonstration.
    /// </summary>
    /// <returns>A sample MindMap instance.</returns>
    private static MindMap CreateSampleMindMap()
    {
        // Create the mind map with a root topic
        var mindMap = MindMap.Create("Cartamentis");

        // Add main branches
        var domain = Topic.CreateChild("Domain", mindMap.Id, mindMap.Root);
        var application = Topic.CreateChild("Application", mindMap.Id, mindMap.Root);
        var wpf = Topic.CreateChild("WPF", mindMap.Id, mindMap.Root);
        var infrastructure = Topic.CreateChild("Infrastructure", mindMap.Id, mindMap.Root);

        // Add domain subtopics
        var mindMapClass = Topic.CreateChild("MindMap", mindMap.Id, domain);
        var topicClass = Topic.CreateChild("Topic", mindMap.Id, domain);
        var connectionClass = Topic.CreateChild("Connection", mindMap.Id, domain);

        // Add more to domain
        var mindMapId = Topic.CreateChild("MindMapId", mindMap.Id, domain);
        var topicId = Topic.CreateChild("TopicId", mindMap.Id, domain);

        // Add application subtopics
        var viewModels = Topic.CreateChild("ViewModels", mindMap.Id, application);
        var services = Topic.CreateChild("Services", mindMap.Id, application);

        // Add WPF subtopics
        var views = Topic.CreateChild("Views", mindMap.Id, wpf);
        var viewModelsWpf = Topic.CreateChild("ViewModels", mindMap.Id, wpf);

        // Add infrastructure subtopics
        var persistence = Topic.CreateChild("Persistence", mindMap.Id, infrastructure);

        return mindMap;
    }
}
