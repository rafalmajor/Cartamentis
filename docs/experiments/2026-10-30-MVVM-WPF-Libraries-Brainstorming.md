# 2026-10-30 - MVVM & WPF Libraries Brainstorming

## Overview

This document provides a comprehensive brainstorming analysis of free, open-source-friendly MVVM and WPF libraries for .NET that have corporate backing and long-term support guarantees. The goal is to identify suitable frameworks for the Cartamentis project that meet the following criteria:

1. **Free for open source projects** - Can be used without cost in open source initiatives
2. **Corporate backing** - Supported by a large corporation (Microsoft, .NET Foundation, etc.)
3. **Long-term support** - Has a track record of maintenance and future commitment
4. **WPF compatibility** - Works with Windows Presentation Foundation
5. **Active maintenance** - Regular updates and bug fixes

---

## Selection Criteria

### Must Have (Non-Negotiable)
- [ ] **Open Source License** - Permissive license (MIT, Apache 2.0, MS-PL) that allows free use in open source projects
- [ ] **WPF Support** - Full or partial support for WPF applications
- [ ] **Active Development** - Not archived, recent commits (within last 6 months)
- [ ] **Good Documentation** - Available documentation, samples, or tutorials

### Highly Desirable
- [ ] **Corporate/Foundation Backing** - Backed by Microsoft, .NET Foundation, or major corporation
- [ ] **Long-Term Support Policy** - Clear LTS or support commitment
- [ ] **Large Community** - Active community, many stars on GitHub
- [ ] **.NET 10 Support** - Compatible with latest .NET versions
- [ ] **Comprehensive Features** - Messenger, DI, Commands, ObservableObject, etc.

### Nice to Have
- [ ] **Source Generators** - Code generation for reduced boilerplate
- [ ] **Modular Design** - Can use only needed components
- [ ] **Cross-Platform** - Also supports other XAML platforms (WinUI, MAUI, Avalonia)
- [ ] **Strong Typing** - Type-safe messaging and commands

---

## Candidate Libraries Evaluation

### 1. **CommunityToolkit.Mvvm** (formerly Microsoft.Toolkit.Mvvm)

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ Permissive, open source friendly |
| **Backing** | Microsoft + .NET Foundation | 5 | ✅ Strong corporate backing |
| **WPF Support** | Full | 5 | ✅ Native support for WPF |
| **Activity** | Very Active (2026-10-30) | 5 | ✅ Regular updates, part of CommunityToolkit |
| **Stars (GitHub)** | Part of CommunityToolkit/dotnet | 5 | ✅ High adoption |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Extensive | 5 | ✅ Official docs, samples, tutorials |
| **Features** | ObservableObject, RelayCommand, Messenger, WeakReference, Source Generators | 5 | ✅ Modern, comprehensive |
| **Long-Term** | Yes (Microsoft) | 5 | ✅ Microsoft commitment |

**Pros:**
- Official Microsoft recommendation for new projects
- Actively maintained and updated
- Source generators reduce boilerplate code
- Strong community and ecosystem
- Works with all .NET platforms (WPF, WinUI, MAUI, etc.)
- Excellent documentation and learning resources

**Cons:**
- Part of larger toolkit (may have more than needed)
- Learning curve for advanced features

**Verdict:** ⭐⭐⭐⭐⭐ **TOP RECOMMENDATION**

---

### 2. **ReactiveUI**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ .NET Foundation |
| **Backing** | .NET Foundation | 4 | ✅ Foundation-backed, not direct corporate |
| **WPF Support** | Full | 5 | ✅ Native WPF support |
| **Activity** | Very Active (2026-10-30) | 5 | ✅ 8,551 stars, active development |
| **Stars (GitHub)** | 8,551 | 5 | ✅ Very popular |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Good | 4 | ✅ Comprehensive but complex |
| **Features** | Reactive programming model, Observables, Commands, Routing, Dependency Injection | 5 | ✅ Powerful reactive features |
| **Long-Term** | Yes (.NET Foundation) | 4 | ✅ Foundation commitment |

**Pros:**
- Very popular and mature (established in 2011)
- Functional reactive programming approach
- Cross-platform (WPF, WinForms, WinUI, MAUI, Avalonia, Uno, Blazor, Android)
- Strong community support
- Modern patterns and practices

**Cons:**
- Steeper learning curve (reactive programming paradigm)
- More complex than traditional MVVM
- Overkill for simple applications
- Not directly corporate-backed (foundation vs corporation)

**Verdict:** ⭐⭐⭐⭐½ **EXCELLENT (if reactive programming is desired)**

---

### 3. **Prism**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | Custom (Prism Community/Commercial) | 2 | ⚠️ **RESTRICTIVE** - Not standard OSS |
| **Backing** | Prism Library (Community) | 3 | ⚠️ Originally Microsoft Patterns & Practices |
| **WPF Support** | Full | 5 | ✅ Primary target platform |
| **Activity** | Active (2026-10-30) | 4 | ✅ 6,866 stars, active development |
| **Stars (GitHub)** | 6,866 | 5 | ✅ Very popular |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Extensive | 5 | ✅ Excellent documentation |
| **Features** | Modularity, DI, Commands, EventAggregator, Navigation, Regions | 5 | ✅ Enterprise-grade features |
| **Long-Term** | Yes (Community) | 4 | ✅ Well-established |

**Pros:**
- Very mature and enterprise-focused
- Excellent for large, complex applications
- Strong modularity support
- Good documentation and samples
- Large community

**Cons:**
- **LICENSE ISSUE**: Uses custom license with restrictions based on revenue
  - Community license only for organizations with <$1M annual revenue or <$3M capital
  - Commercial license required otherwise
  - **NOT suitable for unrestricted open source use**
- Heavier framework with more overhead

**Verdict:** ⭐⭐½ **NOT RECOMMENDED (License restrictions)**

---

### 4. **Caliburn.Micro**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ Permissive |
| **Backing** | Community (Blue Spire Consulting) | 3 | ⚠️ No corporate backing |
| **WPF Support** | Full | 5 | ✅ Native support |
| **Activity** | Active (2026-10-30) | 4 | ✅ 2,865 stars |
| **Stars (GitHub)** | 2,865 | 4 | ✅ Good adoption |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Good | 4 | ✅ Available docs and samples |
| **Features** | Convention-over-configuration, Dependency Injection, Event Aggregation, Actions | 5 | ✅ Convention-based approach |
| **Long-Term** | Community | 3 | ⚠️ Depends on community support |

**Pros:**
- Convention-based reduces boilerplate
- Lightweight and flexible
- Good for WPF and XAML applications
- Mature (existed since 2010)

**Cons:**
- **NO corporate backing** - Community maintained only
- Smaller community than top options
- Learning curve for convention-based approach
- Less active development than CommunityToolkit.Mvvm

**Verdict:** ⭐⭐⭐ **GOOD (but lacks corporate backing)**

---

### 5. **Stylet**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ Permissive |
| **Backing** | Community (Antony Male) | 2 | ⚠️ Single maintainer |
| **WPF Support** | Full | 5 | ✅ Native support |
| **Activity** | Moderate (2026-10-30) | 3 | ⚠️ 1,060 stars |
| **Stars (GitHub)** | 1,060 | 3 | ⚠️ Smaller community |
| **.NET Support** | .NET Framework + .NET Core | 4 | ⚠️ May need verification for .NET 10 |
| **Documentation** | Good | 4 | ✅ Available docs |
| **Features** | ViewModel-First, Dependency Injection, Micro-kernel, Event Aggregation | 4 | ✅ ViewModel-first approach |
| **Long-Term** | Community | 2 | ⚠️ Single maintainer risk |

**Pros:**
- ViewModel-First approach
- Lightweight and simple
- Good for small to medium applications

**Cons:**
- **NO corporate backing** - Single maintainer
- Smaller community and ecosystem
- Less established than other options
- Risk of abandonment (single maintainer)

**Verdict:** ⭐⭐ **ACCEPTABLE (but high risk)**

---

### 6. **Catel**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ Permissive |
| **Backing** | Community (CatenaLogic) | 3 | ⚠️ No corporate backing |
| **WPF Support** | Full | 5 | ✅ Native support |
| **Activity** | Active (2026-10-30) | 4 | ✅ 907 stars |
| **Stars (GitHub)** | 907 | 3 | ⚠️ Moderate adoption |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Good | 4 | ✅ Available docs |
| **Features** | Application framework, MVVM, Dependency Injection, Logging, Argument validation, Type conversion | 5 | ✅ Full application framework |
| **Long-Term** | Community | 3 | ⚠️ Community maintained |

**Pros:**
- More than just MVVM - full application framework
- Strong validation and type conversion features
- Good for enterprise applications
- Actively maintained

**Cons:**
- **NO corporate backing**
- More complex (application framework vs just MVVM)
- Smaller community
- Overkill for simple MVVM needs

**Verdict:** ⭐⭐⭐ **GOOD (for full framework needs, but no corporate backing)**

---

### 7. **MvvmCross (MvvmCross)**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MS-PL (Microsoft Public License) | 5 | ✅ Permissive (OSI-approved) |
| **Backing** | Community | 3 | ⚠️ Originally created by Stuart Lodge, now community |
| **WPF Support** | Full | 5 | ✅ Native support |
| **Activity** | Active (2026-10-30) | 4 | ✅ 3,922 stars |
| **Stars (GitHub)** | 3,922 | 4 | ✅ Good adoption |
| **.NET Support** | .NET 6+ | 5 | ✅ Supports .NET 10 |
| **Documentation** | Good | 4 | ✅ Available docs |
| **Features** | Cross-platform, Dependency Injection, Messaging, Navigation, Plugins | 5 | ✅ Cross-platform focus |
| **Long-Term** | Community | 3 | ⚠️ Community maintained |

**Pros:**
- Cross-platform (WPF, Android, iOS, macOS, WinUI, etc.)
- Mature and established
- Good for mobile + desktop convergence
- Plugin architecture

**Cons:**
- **NO corporate backing** - Community maintained
- Primarily focused on mobile (WPF is secondary)
- More complex due to cross-platform nature
- Smaller WPF-specific community

**Verdict:** ⭐⭐⭐ **GOOD (if cross-platform is needed, but WPF is secondary focus)**

---

### 8. **MVVM Light Toolkit**

| Aspect | Details | Score (1-5) | Notes |
|--------|---------|-------------|-------|
| **License** | MIT | 5 | ✅ Permissive |
| **Backing** | Community (Laurent Bugnion) | 2 | ⚠️ No corporate backing |
| **WPF Support** | Full | 5 | ✅ Native support |
| **Activity** | **ARCHIVED** | 1 | ❌ Repository archived |
| **Stars (GitHub)** | 1,188 | 3 | ⚠️ Was popular |
| **.NET Support** | Legacy | 2 | ⚠️ May not support .NET 10 |
| **Documentation** | Legacy | 3 | ⚠️ Outdated |
| **Features** | Basic MVVM, Messaging, Dependency Injection | 4 | ✅ Simple and lightweight |
| **Long-Term** | **DISCONTINUED** | 1 | ❌ No longer maintained |

**Pros:**
- Very simple and lightweight
- Easy to learn
- Historically popular

**Cons:**
- **ARCHIVED** - No longer actively maintained
- **NO corporate backing**
- Outdated for modern .NET
- Not recommended for new projects

**Verdict:** ❌ **NOT RECOMMENDED (Discontinued)**

---

## Comparison Matrix

| Library | License | Corporate Backing | WPF Support | Activity | Stars | .NET 10 | LTS | Score |
|---------|---------|-------------------|-------------|----------|-------|---------|-----|-------|
| **CommunityToolkit.Mvvm** | MIT | ✅ Microsoft + .NET Foundation | ✅ Full | ✅ Very Active | High | ✅ | ✅ | **45/45** |
| **ReactiveUI** | MIT | ⚠️ .NET Foundation | ✅ Full | ✅ Very Active | Very High | ✅ | ✅ | **44/45** |
| **Prism** | ❌ Custom | ⚠️ Prism Library | ✅ Full | ✅ Active | Very High | ✅ | ✅ | **34/45** |
| **Caliburn.Micro** | MIT | ❌ Community | ✅ Full | ✅ Active | Medium | ✅ | ⚠️ | **38/45** |
| **Stylet** | MIT | ❌ Community | ✅ Full | ⚠️ Moderate | Medium | ⚠️ | ❌ | **30/45** |
| **Catel** | MIT | ❌ Community | ✅ Full | ✅ Active | Medium | ✅ | ⚠️ | **36/45** |
| **MvvmCross** | MS-PL | ❌ Community | ✅ Full | ✅ Active | High | ✅ | ⚠️ | **38/45** |
| **MVVM Light** | MIT | ❌ Community | ✅ Full | ❌ Archived | Medium | ❌ | ❌ | **18/45** |

---

## Filtered by Criteria

### ✅ **Meets ALL Requirements** (Corporate-backed + OSS-friendly + Active + WPF)

1. **CommunityToolkit.Mvvm** (Score: 45/45)
   - Microsoft + .NET Foundation backing
   - MIT License
   - Very active development
   - Full WPF support
   - .NET 10 compatible

2. **ReactiveUI** (Score: 44/45)
   - .NET Foundation backing
   - MIT License
   - Very active development
   - Full WPF support
   - .NET 10 compatible

### ⚠️ **Meets Most Requirements (but lacks corporate backing)**

3. **Caliburn.Micro** (Score: 38/45)
4. **MvvmCross** (Score: 38/45)
5. **Catel** (Score: 36/45)
6. **Stylet** (Score: 30/45)

### ❌ **Does NOT Meet Requirements**

7. **Prism** (Score: 34/45) - License restrictions for commercial use
8. **MVVM Light** (Score: 18/45) - Archived/discontinued

---

## Deep Dive: Corporate Backing Analysis

### What constitutes "corporate backing"?

For this analysis, we consider the following as **corporate backing**:
- Directly maintained by a major corporation (Microsoft, Google, etc.)
- Backed by a major foundation with corporate members (.NET Foundation)
- Has a corporate sponsor providing resources and support

### Libraries with True Corporate/Foundation Backing:

1. **CommunityToolkit.Mvvm**
   - **Backer**: Microsoft + .NET Foundation
   - **Relationship**: Directly maintained and published by Microsoft
   - **Commitment**: Part of official .NET tooling ecosystem
   - **Support**: Long-term commitment as part of CommunityToolkit
   - **Confidence**: **VERY HIGH**

2. **ReactiveUI**
   - **Backer**: .NET Foundation
   - **Relationship**: .NET Foundation project
   - **Commitment**: Foundation-backed with corporate contributors
   - **Support**: Long-term commitment through foundation
   - **Confidence**: **HIGH**

3. **Prism** (with caveats)
   - **Backer**: Prism Library (Community organization)
   - **Relationship**: Originally Microsoft Patterns & Practices
   - **Commitment**: Community-driven but established
   - **Support**: Long-term but with license restrictions
   - **Confidence**: **MEDIUM** (License issues)

### Libraries WITHOUT Corporate Backing:

- Caliburn.Micro - Community (Blue Spire Consulting)
- Stylet - Single maintainer (Antony Male)
- Catel - Community (CatenaLogic)
- MvvmCross - Community
- MVVM Light - Community (Laurent Bugnion) - **DISCONTINUED**

---

## Recommendations

### For Cartamentis Project

Based on the project requirements:
- **WPF-focused** (not cross-platform)
- **Open source** (must be freely usable)
- **Long-term support** (corporate backing preferred)
- **Modern .NET** (.NET 10 support)
- **Minimal complexity** (MVVM without unnecessary features)

#### 🥇 **Primary Recommendation: CommunityToolkit.Mvvm**

**Why:**
1. ✅ **Microsoft-backed** - Official Microsoft recommendation
2. ✅ **MIT License** - Completely free for open source
3. ✅ **WPF Support** - Native and well-tested
4. ✅ **Active Development** - Regular updates, .NET 10 support
5. ✅ **Modern Features** - Source generators, type-safe messaging
6. ✅ **Good Documentation** - Extensive official docs and samples
7. ✅ **Minimal** - Can use only what you need (modular)
8. ✅ **Future-proof** - Part of Microsoft's official toolkit ecosystem

**Fit for Cartamentis:**
- Perfect for a clean, modern WPF MVVM implementation
- Aligns with Microsoft's recommended practices
- Will be supported long-term
- Easy to integrate and learn
- Good community support for troubleshooting

#### 🥈 **Alternative: ReactiveUI**

**Use if:** You want reactive programming capabilities or potential future cross-platform support.

**Pros:**
- More powerful for complex reactive scenarios
- Cross-platform if future needs change
- Very mature and battle-tested

**Cons:**
- Steeper learning curve
- More complex than needed for basic MVVM
- Not directly Microsoft-backed (though .NET Foundation is close)

#### 🥉 **Fallback Options**

If for some reason CommunityToolkit.Mvvm cannot be used:

1. **Caliburn.Micro** - Good convention-based approach, but no corporate backing
2. **MvvmCross** - Good cross-platform option, but WPF is secondary
3. **Catel** - Full framework, but overkill for simple MVVM needs

### What to AVOID

1. **❌ Prism** - License restrictions make it unsuitable for unrestricted open source use
2. **❌ MVVM Light** - Discontinued/archived, no longer maintained
3. **❌ Stylet** - Single maintainer, higher risk of abandonment

---

## Feature Comparison

| Feature | CommunityToolkit.Mvvm | ReactiveUI | Prism | Caliburn.Micro | Stylet |
|---------|------------------------|------------|-------|----------------|--------|
| **ObservableObject** | ✅ | ✅ (ReactiveObject) | ✅ | ✅ | ✅ |
| **RelayCommand** | ✅ | ✅ (ReactiveCommand) | ✅ | ✅ | ✅ |
| **Messenger/EventAggregator** | ✅ | ✅ (MessageBus) | ✅ | ✅ | ✅ |
| **Dependency Injection** | ⚠️ (External) | ✅ | ✅ | ✅ | ✅ |
| **Navigation** | ⚠️ (External) | ✅ (Routing) | ✅ | ✅ | ✅ |
| **Source Generators** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Strong Typing** | ✅ | ✅ | ⚠️ | ✅ | ✅ |
| **ViewModel-First** | ⚠️ | ⚠️ | ⚠️ | ⚠️ | ✅ |
| **Convention-based** | ❌ | ❌ | ⚠️ | ✅ | ✅ |
| **Reactive Programming** | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Modularity** | ✅ | ✅ | ✅ | ⚠️ | ✅ |

---

## Implementation Considerations

### CommunityToolkit.Mvvm Implementation

**Package:** `CommunityToolkit.Mvvm`

**Basic Setup:**
```csharp
// ViewModel
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = "Cartamentis";

    [RelayCommand]
    private void Save() { /* logic */ }
}
```

**Pros:**
- Source generators eliminate boilerplate
- Strong typing prevents errors
- Clean, modern API
- Well-documented

**Cons:**
- Requires .NET 6+ (fine for .NET 10)
- Some features require separate packages

### ReactiveUI Implementation

**Package:** `ReactiveUI.Wpf`

**Basic Setup:**
```csharp
public class MainViewModel : ReactiveObject
{
    private string _title;
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    public MainViewModel()
    {
        SaveCommand = ReactiveCommand.Create(() => { /* logic */ });
    }
}
```

**Pros:**
- Reactive programming enables powerful patterns
- Built-in support for async operations
- Cross-platform ready

**Cons:**
- More complex setup
- Steeper learning curve
- Different programming paradigm

---

## Risk Assessment

### CommunityToolkit.Mvvm

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| Microsoft abandons project | Very Low | High | Part of .NET Foundation, widely adopted |
| Breaking changes | Low | Medium | Microsoft maintains backward compatibility |
| Performance issues | Low | Low | Well-tested, optimized code |
| Learning curve | Medium | Low | Excellent documentation available |

**Overall Risk:** **VERY LOW**

### ReactiveUI

| Risk | Likelihood | Impact | Mitigation |
|------|------------|--------|------------|
| .NET Foundation abandons | Low | High | Widely adopted, many contributors |
| Breaking changes | Medium | Medium | Regular updates, good change management |
| Complexity | Medium | Medium | Team training, start with basics |
| Performance overhead | Low | Low | Optimized for performance |

**Overall Risk:** **LOW**

---

## Decision Framework

### Choose CommunityToolkit.Mvvm if:
- [x] You want the officially recommended Microsoft solution
- [x] You prefer simplicity and minimal boilerplate
- [x] You need source generators
- [x] You want strong typing
- [x] WPF is your primary target
- [x] You value long-term Microsoft support

### Choose ReactiveUI if:
- [ ] You need reactive programming capabilities
- [ ] You might need cross-platform support in the future
- [ ] You're comfortable with functional reactive concepts
- [ ] You need advanced async operation handling

### Avoid Prism if:
- [x] Your project is open source and might exceed revenue limits
- [x] You want unrestricted usage rights

---

## Migration Paths

### From No Framework to CommunityToolkit.Mvvm
**Effort:** Low
- Add NuGet package
- Refactor ViewModels to inherit from ObservableObject
- Replace INotifyPropertyChanged with [ObservableProperty]
- Replace commands with [RelayCommand]
- Add Messenger for communication

**Timeline:** 1-2 weeks for small project

### From Other Framework to CommunityToolkit.Mvvm
**Effort:** Medium
- Similar to above but with existing framework removal
- May need adapter layer for gradual migration

**Timeline:** 2-4 weeks depending on project size

---

## Community and Ecosystem

### CommunityToolkit.Mvvm
- **GitHub Stars:** Part of CommunityToolkit/dotnet (thousands)
- **NuGet Downloads:** Very high (millions)
- **Community:** Active Discord, GitHub discussions
- **Documentation:** Official Microsoft docs + community tutorials
- **Third-party:** Many integrations and extensions

### ReactiveUI
- **GitHub Stars:** 8,551
- **NuGet Downloads:** High
- **Community:** Active Slack, GitHub discussions
- **Documentation:** Official docs + many community resources
- **Third-party:** Many plugins and extensions

---

## Long-Term Outlook

### CommunityToolkit.Mvvm
- **Future:** Will continue to be Microsoft's recommended solution
- **Roadmap:** Regular updates, new features aligned with .NET releases
- **Ecosystem:** Growing, becoming standard for new .NET projects
- **Support:** Microsoft support + community support

### ReactiveUI
- **Future:** Will continue under .NET Foundation
- **Roadmap:** Focus on cross-platform and reactive features
- **Ecosystem:** Stable, mature, but growing slower
- **Support:** Community-driven with foundation backing

---

## Cost Analysis

### CommunityToolkit.Mvvm
- **License Cost:** FREE (MIT)
- **Learning Cost:** Low-Medium
- **Maintenance Cost:** Low
- **Migration Cost:** Low
- **Total Cost:** **LOW**

### ReactiveUI
- **License Cost:** FREE (MIT)
- **Learning Cost:** Medium-High
- **Maintenance Cost:** Medium
- **Migration Cost:** Medium
- **Total Cost:** **MEDIUM**

---

## Final Verdict

| Criteria | CommunityToolkit.Mvvm | ReactiveUI | Winner |
|----------|------------------------|------------|--------|
| **License** | MIT | MIT | Tie |
| **Corporate Backing** | ✅ Microsoft + .NET Foundation | ✅ .NET Foundation | **CommunityToolkit.Mvvm** |
| **WPF Support** | ✅ Full | ✅ Full | Tie |
| **Activity** | ✅ Very Active | ✅ Very Active | Tie |
| **.NET 10 Support** | ✅ | ✅ | Tie |
| **Simplicity** | ✅ Very Simple | ⚠️ Complex | **CommunityToolkit.Mvvm** |
| **Learning Curve** | ✅ Low | ⚠️ Steep | **CommunityToolkit.Mvvm** |
| **Documentation** | ✅ Excellent | ✅ Good | **CommunityToolkit.Mvvm** |
| **Features** | ✅ Comprehensive | ✅ Powerful | Tie |
| **Community** | ✅ Large | ✅ Large | Tie |
| **Long-Term** | ✅ Very High | ✅ High | **CommunityToolkit.Mvvm** |
| **Cartamentis Fit** | ✅ Perfect | ✅ Good | **CommunityToolkit.Mvvm** |

### **🏆 FINAL RECOMMENDATION: CommunityToolkit.Mvvm**

**Score:** 10/10 for Cartamentis project needs

**Justification:**
1. **Meets all criteria perfectly** - Corporate-backed (Microsoft), free (MIT), active, WPF support
2. **Best fit for WPF** - Designed specifically for XAML-based applications
3. **Minimal complexity** - Simple to adopt and use
4. **Future-proof** - Microsoft's official recommendation, will be supported long-term
5. **Modern features** - Source generators reduce boilerplate, strong typing
6. **Excellent documentation** - Easy to learn and implement
7. **Active community** - Good support and resources available

### Backup Recommendation: ReactiveUI

**Use if:** You specifically need reactive programming capabilities or anticipate future cross-platform requirements.

**Score:** 9/10 for Cartamentis project needs

---

## Next Steps for Cartamentis

1. **Evaluate CommunityToolkit.Mvvm**
   - Create a proof-of-concept with basic ViewModel
   - Test integration with existing WPF code
   - Verify .NET 10 compatibility

2. **Review Documentation**
   - Official docs: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/
   - Samples: CommunityToolkit samples repository

3. **Team Training**
   - Organize training session on CommunityToolkit.Mvvm
   - Review source generator usage
   - Understand Messenger pattern

4. **Implementation Plan**
   - Phase 1: Add CommunityToolkit.Mvvm to project
   - Phase 2: Migrate existing ViewModels
   - Phase 3: Add new features using the toolkit

5. **Create ADR**
   - Document the decision to use CommunityToolkit.Mvvm
   - Include rationale and alternatives considered
   - Reference this brainstorming document

---

## References

1. CommunityToolkit.Mvvm GitHub: https://github.com/CommunityToolkit/dotnet
2. ReactiveUI GitHub: https://github.com/reactiveui/ReactiveUI
3. Prism GitHub: https://github.com/PrismLibrary/Prism
4. Caliburn.Micro GitHub: https://github.com/Caliburn-Micro/Caliburn.Micro
5. Stylet GitHub: https://github.com/canton7/Stylet
6. Catel GitHub: https://github.com/Catel/Catel
7. MvvmCross GitHub: https://github.com/MvvmCross/MvvmCross
8. MVVM Light GitHub: https://github.com/lbugnion/mvvmlight
9. .NET Foundation: https://dotnetfoundation.org/
10. CommunityToolkit Documentation: https://learn.microsoft.com/en-us/dotnet/communitytoolkit/

---

## Appendix: Quick Reference

### License Summary

| Library | License | OSS-Friendly | Corporate Use |
|---------|---------|--------------|---------------|
| CommunityToolkit.Mvvm | MIT | ✅ Yes | ✅ Yes |
| ReactiveUI | MIT | ✅ Yes | ✅ Yes |
| Prism | Custom | ⚠️ Conditional | ⚠️ Conditional |
| Caliburn.Micro | MIT | ✅ Yes | ✅ Yes |
| Stylet | MIT | ✅ Yes | ✅ Yes |
| Catel | MIT | ✅ Yes | ✅ Yes |
| MvvmCross | MS-PL | ✅ Yes | ✅ Yes |
| MVVM Light | MIT | ✅ Yes | ✅ Yes |

### Corporate Backing Summary

| Library | Backer | Type | Confidence |
|---------|--------|------|------------|
| CommunityToolkit.Mvvm | Microsoft + .NET Foundation | Corporate + Foundation | Very High |
| ReactiveUI | .NET Foundation | Foundation | High |
| Prism | Prism Library | Community Org | Medium |
| Caliburn.Micro | Blue Spire Consulting | Company | Low |
| Stylet | Antony Male | Individual | Low |
| Catel | CatenaLogic | Company | Low |
| MvvmCross | Community | Community | Low |
| MVVM Light | Laurent Bugnion | Individual | Very Low |

---

*Document created: 2026-10-30*
*Author: AI Brainstorming Session*
*Project: Cartamentis*
