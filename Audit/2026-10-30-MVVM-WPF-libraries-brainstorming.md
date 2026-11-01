# 2026-10-30 - MVVM & WPF Libraries Brainstorming

## Metadata
- **Agent/Prompt**: MVVM WPF Libraries Brainstorming
- **Date**: 2026-10-30 22:00:00 UTC
- **Related Files**: 
  - `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`
- **Project Area**: Architecture / Decision Analysis
- **Milestone**: General (Applies to all milestones)

## Changes Made
- [x] Created: `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md` - Comprehensive brainstorming analysis of MVVM and WPF libraries for .NET

## Validation Results
- **Tests Passed**: N.A. (Documentation only, no code changes)
- **Build Status**: N.A. (Documentation only)
- **Test Coverage**: N.A.
- **New Tests Added**: None
- **Validation Notes**: Document has been created and contains comprehensive analysis of 8 MVVM/WPF libraries evaluated against criteria of being free for open source, having corporate backing, and long-term support.

## Conclusions
- **Primary Recommendation**: CommunityToolkit.Mvvm (Microsoft-backed, MIT license, perfect fit for Cartamentis)
- **Alternative Recommendation**: ReactiveUI (.NET Foundation-backed, MIT license, good for reactive programming needs)
- **Avoid**: Prism (license restrictions), MVVM Light (discontinued/archived)
- **Other Options**: Caliburn.Micro, MvvmCross, Catel, Stylet (all lack corporate backing)

## Key Findings
1. **CommunityToolkit.Mvvm** is the only library that meets ALL criteria: Microsoft + .NET Foundation backing, MIT license, WPF support, active development, .NET 10 compatibility
2. **ReactiveUI** is a close second with .NET Foundation backing but has a steeper learning curve
3. **Prism** has license restrictions that make it unsuitable for unrestricted open source use
4. **MVVM Light** is discontinued and should not be used for new projects
5. Other libraries (Caliburn.Micro, MvvmCross, Catel, Stylet) are good but lack corporate backing

## Related Documentation
- Brainstorming document: `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`
- Future ADR should reference this analysis when making the final framework selection decision

## Recommendations for Next Steps
1. Create ADR documenting the decision to use CommunityToolkit.Mvvm for Cartamentis
2. Implement proof-of-concept using CommunityToolkit.Mvvm
3. Update project dependencies to include CommunityToolkit.Mvvm
4. Schedule team training on CommunityToolkit.Mvvm features and patterns
