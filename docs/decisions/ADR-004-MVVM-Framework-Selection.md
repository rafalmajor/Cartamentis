# ADR-004: Select CommunityToolkit.Mvvm as MVVM Framework

## Status
✅ Accepted

## Context

The Cartamentis project requires an MVVM (Model-View-ViewModel) framework for its WPF application layer. The framework must:

1. **Be free for open source use** - The project is open source and requires permissive licensing
2. **Have corporate backing** - Prefer frameworks backed by Microsoft or major corporations for long-term stability
3. **Support WPF** - Full compatibility with Windows Presentation Foundation
4. **Support .NET 10** - Compatibility with the project's target framework
5. **Be actively maintained** - Regular updates and bug fixes
6. **Provide modern features** - Source generators, strong typing, and comprehensive MVVM support

The decision was preceded by a comprehensive brainstorming analysis documented in `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`, which evaluated 8 different MVVM frameworks against these criteria.

## Decision

**We will use CommunityToolkit.Mvvm as the MVVM framework for the Cartamentis WPF project.**

### Decision Date
2026-10-30

### Decision Maker
Project Maintainer (based on brainstorming analysis)

## Alternatives Considered

### 1. **CommunityToolkit.Mvvm** (SELECTED)
- **License**: MIT
- **Backing**: Microsoft + .NET Foundation
- **WPF Support**: Full
- **Activity**: Very Active
- **.NET 10**: ✅ Supported
- **Pros**:
  - Official Microsoft recommendation for new projects
  - Strong corporate backing (Microsoft + .NET Foundation)
  - MIT license - completely free for open source and commercial use
  - Source generators reduce boilerplate code significantly
  - Strong typing prevents errors at compile time
  - Comprehensive features: ObservableObject, RelayCommand, Messenger, WeakReference
  - Excellent official documentation from Microsoft
  - Large and active community
  - Part of Microsoft's official toolkit ecosystem
  - Future-proof with regular updates aligned with .NET releases
- **Cons**:
  - Part of larger toolkit (may include more than needed)
  - Learning curve for advanced features (source generators, messenger pattern)

### 2. **ReactiveUI**
- **License**: MIT
- **Backing**: .NET Foundation
- **WPF Support**: Full
- **Activity**: Very Active
- **.NET 10**: ✅ Supported
- **Pros**:
  - Very popular and mature (established in 2011)
  - Functional reactive programming approach
  - Cross-platform support (WPF, WinForms, WinUI, MAUI, Avalonia, Uno, Blazor, Android)
  - Strong community support
  - Modern patterns and practices
  - Powerful features for complex reactive scenarios
- **Cons**:
  - Steeper learning curve (reactive programming paradigm)
  - More complex than traditional MVVM
  - Overkill for simple applications
  - Not directly corporate-backed (foundation vs direct corporation)
  - Different programming paradigm requires team training

### 3. **Prism**
- **License**: Custom (Prism Community/Commercial)
- **Backing**: Prism Library (Community organization)
- **WPF Support**: Full
- **Activity**: Active
- **.NET 10**: ✅ Supported
- **Pros**:
  - Very mature and enterprise-focused
  - Excellent for large, complex applications
  - Strong modularity support
  - Good documentation and samples
  - Large community
- **Cons**:
  - **LICENSE RESTRICTIONS**: Custom license with revenue-based limitations
    - Community license only for organizations with <$1M annual revenue
    - Commercial license required for organizations exceeding revenue threshold
    - **NOT suitable for unrestricted open source use**
  - Heavier framework with more overhead
  - Originally Microsoft Patterns & Practices, but now community-driven

### 4. **Caliburn.Micro**
- **License**: MIT
- **Backing**: Community (Blue Spire Consulting)
- **WPF Support**: Full
- **Activity**: Active
- **.NET 10**: ✅ Supported
- **Pros**:
  - Convention-over-configuration reduces boilerplate
  - Lightweight and flexible
  - Good for WPF and XAML applications
  - Mature (existed since 2010)
- **Cons**:
  - **NO corporate backing** - Community maintained only
  - Smaller community than CommunityToolkit.Mvvm or ReactiveUI
  - Learning curve for convention-based approach
  - Less active development than Microsoft-backed options

### 5. **Other Options Considered**
- **MvvmCross**: Cross-platform focus, WPF is secondary, no corporate backing
- **Catel**: Full application framework (overkill for MVVM needs), no corporate backing
- **Stylet**: Single maintainer, higher risk of abandonment
- **MVVM Light**: Discontinued/archived, no longer maintained

## Consequences

### Positive Consequences

1. **Corporate Backing**: Microsoft + .NET Foundation backing ensures long-term support and stability
2. **License Compliance**: MIT license allows unrestricted use in open source projects
3. **Modern Development**: Source generators reduce boilerplate code and improve developer productivity
4. **Type Safety**: Strong typing prevents runtime errors and improves code quality
5. **Documentation**: Excellent official Microsoft documentation and community resources available
6. **Community Support**: Large and active community for troubleshooting and best practices
7. **Future Compatibility**: Regular updates aligned with .NET releases ensure future compatibility
8. **Ecosystem Integration**: Part of Microsoft's official toolkit ecosystem, ensuring good integration with other Microsoft technologies
9. **Minimal Learning Curve**: Simple API and excellent documentation make it easy to adopt
10. **Flexibility**: Modular design allows using only the components needed

### Negative Consequences

1. **Dependency**: Introduces a dependency on Microsoft's CommunityToolkit ecosystem
2. **Learning Curve**: Team members may need to learn source generator syntax and messenger pattern
3. **Overhead**: Part of larger toolkit, may include features not needed for the project
4. **Breaking Changes**: Future updates may introduce breaking changes (though Microsoft maintains good backward compatibility)

## Implementation Plan

### Phase 1: Package Installation
- Add `CommunityToolkit.Mvvm` NuGet package to the Cartamentis.Wpf project
- Verify .NET 10 compatibility

### Phase 2: Basic Setup
- Create base ViewModel classes inheriting from `ObservableObject`
- Implement `RelayCommand` for command handling
- Set up `Messenger` for ViewModel-to-ViewModel communication

### Phase 3: Migration (if applicable)
- If existing ViewModels exist, migrate them to use CommunityToolkit.Mvvm
- Replace `INotifyPropertyChanged` with `[ObservableProperty]`
- Replace manual commands with `[RelayCommand]`
- Refactor messaging to use CommunityToolkit's Messenger

### Phase 4: Training
- Team training session on CommunityToolkit.Mvvm features
- Review source generator usage and best practices
- Understand Messenger pattern and its use cases

### Phase 5: Integration
- Integrate with existing application architecture
- Test with existing views and controls
- Verify all functionality works correctly

## Migration Strategy

### For New Code
- All new ViewModels should inherit from `CommunityToolkit.Mvvm.ObservableObject`
- Use `[ObservableProperty]` attribute for properties instead of manual `INotifyPropertyChanged`
- Use `[RelayCommand]` attribute for commands instead of manual `ICommand` implementations
- Use `WeakReferenceMessenger` for communication between ViewModels

### For Existing Code (if applicable)
1. **Gradual Migration**: Migrate ViewModels one at a time
2. **Testing**: Ensure each migrated ViewModel passes all tests
3. **Documentation**: Update documentation to reflect new patterns
4. **Cleanup**: Remove old INotifyPropertyChanged implementations

## Code Examples

### Basic ViewModel
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "Cartamentis";

    [ObservableProperty]
    private ObservableCollection<TopicViewModel> _topics = new();

    [RelayCommand]
    private void AddTopic()
    {
        Topics.Add(new TopicViewModel());
    }

    [RelayCommand]
    private void RemoveTopic(TopicViewModel topic)
    {
        Topics.Remove(topic);
    }
}
```

### Messaging Between ViewModels
```csharp
using CommunityToolkit.Mvvm.Messaging;

// Sender
public partial class TopicViewModel : ObservableObject
{
    [RelayCommand]
    private void RequestDeletion()
    {
        WeakReferenceMessenger.Default.Send(new TopicDeletedMessage(this));
    }
}

// Receiver
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        WeakReferenceMessenger.Default.Register<TopicDeletedMessage>(this, (sender, message) =>
        {
            Topics.Remove(message.Topic);
        });
    }
}

public class TopicDeletedMessage : ValueChangedMessage<TopicViewModel>
{
    public TopicDeletedMessage(TopicViewModel topic) : base(topic) { }
    public TopicViewModel Topic => Value;
}
```

## Validation

### Proof of Concept
- Create a simple WPF application using CommunityToolkit.Mvvm
- Verify basic functionality (property change notification, commands, messaging)
- Test with .NET 10
- Confirm integration with existing project structure

### Testing Strategy
- Unit tests for ViewModels using CommunityToolkit.Mvvm
- Integration tests for View-ViewModel binding
- Manual testing of UI functionality

## References

1. **Brainstorming Document**: `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`
2. **Official Documentation**: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/
3. **GitHub Repository**: https://github.com/CommunityToolkit/dotnet
4. **NuGet Package**: https://www.nuget.org/packages/CommunityToolkit.Mvvm/
5. **CommunityToolkit Overview**: https://github.com/CommunityToolkit/dotnet

## Related

- [ADR-001: Technology Stack Selection](ADR-001-Technology-Stack-Selection.md)
- [ADR-002: .NET 10 Migration](ADR-002-NET10-Migration.md)
- [ADR-003: WPF Visualization Architecture](ADR-003-WPF-Visualization-Architecture.md)

---

*ADR created: 2026-10-30*
*Last updated: 2026-10-30*
*Status: Accepted*
*Project: Cartamentis*
