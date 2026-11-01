# ADR-003: WPF Visualization Architecture for Milestone 3

## Status
✅ Accepted

## Context
The project needs to implement **Milestone 3: First Usable Map** which requires displaying a mind map on a canvas with topics and connections. The domain model (MindMap, Topic) is already implemented and must remain framework-agnostic.

Key requirements for M3:
- Display mind map on a WPF Canvas
- Show topics as visual elements
- Show connections between topics
- Support hierarchical structure (parent-child relationships)
- Enable future interactivity (M5)

The challenge is to design a visualization approach that:
1. Maintains clean architecture (Domain → Application → WPF)
2. Is simple to implement for M3
3. Is extensible for future milestones
4. Provides a natural mind map layout

## Decision
We will implement a **layered WPF architecture** with:
1. **Application Layer**: ViewModels and Services
2. **WPF Layer**: Views and XAML
3. **Circular Layout Algorithm**: Automatic positioning of topics

### Architecture Structure
```
Cartamentis.Application
├── ViewModels/
│   ├── ObservableObject.cs    (INotifyPropertyChanged base)
│   ├── TopicViewModel.cs      (Topic with X,Y position)
│   └── MindMapViewModel.cs    (Root, AllTopics, Connections)
└── Services/
    └── LayoutService.cs       (Circular layout algorithm)

Cartamentis.Wpf
├── Views/
│   └── MindMapView.xaml       (Canvas with ItemsControl)
├── Converters/
│   └── AddOffsetConverter.cs  (For connection line offsets)
├── App.xaml, App.xaml.cs      (Startup and DI)
└── MainWindow.xaml           (Hosts MindMapView)
```

### Layout Algorithm: Circular Layout
- **Root topic**: Placed at the center of the canvas
- **Children**: Arranged in a circle around their parent
- **Radius**: Increases by 150 units per level (DefaultRadius = 150, LevelSpacing = 150)
- **Angular spacing**: Children evenly distributed around the circle
- **Recursive**: Algorithm applies to all descendants

**Visual representation:**
```
        Child 1
      /           \
     /             \
Root               Child 2
     \             /
      \           /
        Child 3
```

### Connection Drawing
- Connections drawn as `Line` controls
- Lines connect from **center of parent box** to **center of child box**
- Box size: 120×40 pixels
- Center offset: +60 (X), +20 (Y) from top-left corner
- Uses `AddOffsetConverter` for clean XAML binding

## Alternatives Considered

### 1. **Tree Layout (Vertical/Horizontal)**
**Pros:**
- Simpler algorithm (no trigonometry)
- Easier to implement
- More space-efficient for deep hierarchies

**Cons:**
- Doesn't look like a traditional mind map
- Less natural for radial thinking
- Harder to accommodate many children at one level

**Decision**: Rejected - doesn't match the "mind map" visual paradigm

### 2. **Force-Directed Layout**
**Pros:**
- Organic, natural-looking layouts
- Handles complex graphs well
- Can avoid overlapping nodes

**Cons:**
- Complex physics simulation
- Requires iterative calculation
- Overkill for M3 requirements
- Performance concerns

**Decision**: Rejected - too complex for initial implementation

### 3. **Manual Positioning (User Drag & Drop)**
**Pros:**
- Maximum user control
- No layout algorithm needed

**Cons:**
- Requires persistent storage of positions
- No automatic layout for new topics
- Doesn't satisfy M3 requirement ("display on canvas" implies automatic)

**Decision**: Rejected - doesn't meet M3 requirements

### 4. **Graphviz DOT Export + Rendering**
**Pros:**
- Professional-quality layouts
- Handles complex graphs
- Quick to implement

**Cons:**
- Requires external dependency (Graphviz)
- Static images (no interactivity)
- Doesn't use WPF Canvas

**Decision**: Rejected - violates requirement to display on WPF canvas

### 5. **Custom Drawing (DrawingVisual)**
**Pros:**
- Maximum performance
- Full control over rendering

**Cons:**
- Complex code
- Hard to maintain
- Doesn't integrate well with MVVM
- No built-in hit-testing

**Decision**: Rejected - too low-level, not MVVM-friendly

## Consequences

### Positive
✅ **Clean Architecture**: Clear separation between Domain, Application, and WPF layers
✅ **MVVM Pattern**: Proper implementation with ViewModels and data binding
✅ **Extensible**: Easy to add features (drag&drop, editing, persistence)
✅ **Maintainable**: Simple, well-structured code
✅ **Testable**: ViewModels can be unit tested without UI
✅ **No External Dependencies**: Uses only .NET 10 and WPF
✅ **Natural Layout**: Circular layout matches mind map paradigm
✅ **Meets M3 Requirements**: Fully satisfies "display on canvas" requirement

### Negative
❌ **Limited Layout Options**: Circular layout may not be optimal for all use cases
❌ **Fixed Spacing**: Level spacing is constant (150 units), may cause overlaps with many children
❌ **No Collision Detection**: Topics may overlap if too many at one level
❌ **Simple Styling**: Basic visual appearance (can be improved in M5)

## Implementation Details

### ViewModel Mapping
```csharp
// Domain -> ViewModel mapping in MindMapViewModel
private TopicViewModel MapTopic(Topic domainTopic)
{
    var viewModel = new TopicViewModel(domainTopic);
    foreach (var child in domainTopic.Children)
    {
        viewModel.AddChild(MapTopic(child));
    }
    return viewModel;
}
```

### Layout Calculation
```csharp
public void CalculateCircularLayout(
    TopicViewModel topic,
    double centerX, double centerY,
    double radius, double startAngle, double endAngle)
{
    topic.X = centerX + radius * Math.Cos(startAngle);
    topic.Y = centerY + radius * Math.Sin(startAngle);
    
    if (topic.Children.Count == 0) return;
    
    double angleStep = (endAngle - startAngle) / topic.Children.Count;
    double currentAngle = startAngle;
    
    foreach (var child in topic.Children)
    {
        CalculateCircularLayout(
            child, centerX, centerY,
            radius + LevelSpacing,
            currentAngle,
            currentAngle + angleStep);
        currentAngle += angleStep;
    }
}
```

### XAML Structure
```xml
<Canvas>
    <!-- Topics -->
    <ItemsControl ItemsSource="{Binding AllTopics}">
        <ItemsControl.ItemsPanel>
            <ItemsPanelTemplate>
                <Canvas />
            </ItemsPanelTemplate>
        </ItemsControl.ItemsPanel>
        <ItemsControl.ItemContainerStyle>
            <Style TargetType="ContentPresenter">
                <Setter Property="Canvas.Left" Value="{Binding X}" />
                <Setter Property="Canvas.Top" Value="{Binding Y}" />
            </Style>
        </ItemsControl.ItemContainerStyle>
        <ItemsControl.ItemTemplate>
            <DataTemplate>
                <Border Width="120" Height="40" Background="White" ...>
                    <TextBlock Text="{Binding Content}" ... />
                </Border>
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
    
    <!-- Connections -->
    <ItemsControl ItemsSource="{Binding Connections}">
        <ItemsControl.ItemTemplate>
            <DataTemplate>
                <Line X1="{Binding From.X, Converter={StaticResource AddOffsetConverter}, ConverterParameter=60}"
                      Y1="{Binding From.Y, Converter={StaticResource AddOffsetConverter}, ConverterParameter=20}"
                      X2="{Binding To.X, Converter={StaticResource AddOffsetConverter}, ConverterParameter=60}"
                      Y2="{Binding To.Y, Converter={StaticResource AddOffsetConverter}, ConverterParameter=20}"
                      Stroke="Gray" StrokeThickness="1" />
            </DataTemplate>
        </ItemsControl.ItemTemplate>
    </ItemsControl>
</Canvas>
```

## Future Considerations

### M4: Persistence
- Serialize ViewModel state (positions, etc.) or domain model
- JSON serialization using System.Text.Json
- Save/load to local files

### M5: Editing Experience
- **Drag & Drop**: Allow users to move topics
- **Undo/Redo**: Track changes and allow reversal
- **Selection**: Highlight selected topics
- **Keyboard Shortcuts**: Quick actions
- **Custom Styling**: Colors, fonts, shapes

### Layout Improvements
- **Collision Avoidance**: Adjust positions to prevent overlapping
- **Different Layouts**: Tree, organic, radial options
- **Auto-Zoom**: Fit all topics in view
- **Dynamic Resizing**: Recalculate layout when window resizes

### Performance
- For >100 topics, consider:
  - Virtualization
  - Incremental layout updates
  - Background calculation

## Related
- [ADR-001: Technology Stack Selection](ADR-001-Technology-Stack-Selection.md) - Original WPF decision
- [ADR-002: Migrate Runtime from .NET 8 to .NET 10](ADR-002-NET10-Migration.md) - Runtime update
- [README.md](../../README.md) - Project overview and milestones
- [AGENTS.md](../../AGENTS.md) - Project constraints and rules
