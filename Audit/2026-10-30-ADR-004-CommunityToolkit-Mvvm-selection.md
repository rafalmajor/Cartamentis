# 2026-10-30 - ADR-004: Select CommunityToolkit.Mvvm as MVVM Framework

## Metadata
- **Agent/Prompt**: MVVM Framework Decision Documentation
- **Date**: 2026-10-30 22:30:00 UTC
- **Related Files**: 
  - `docs/decisions/ADR-004-MVVM-Framework-Selection.md`
  - `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`
- **Project Area**: Architecture / Decision
- **Milestone**: General (Applies to all milestones)

## Changes Made
- [x] Created: `docs/decisions/ADR-004-MVVM-Framework-Selection.md` - Architecture Decision Record formalizing the selection of CommunityToolkit.Mvvm as the MVVM framework for Cartamentis project

## Validation Results
- **Tests Passed**: N.A. (Documentation only, no code changes)
- **Build Status**: N.A. (Documentation only)
- **Test Coverage**: N.A.
- **New Tests Added**: None
- **Validation Notes**: ADR document follows the project's required template format from AGENTS.md and properly references the brainstorming analysis.

## Conclusions
- **Decision Made**: CommunityToolkit.Mvvm selected as the official MVVM framework for Cartamentis WPF project
- **Rationale**: Best fit based on corporate backing (Microsoft + .NET Foundation), MIT license, WPF support, .NET 10 compatibility, and modern features
- **Alternatives Evaluated**: 8 different MVVM frameworks were considered and documented in the brainstorming analysis
- **Confidence Level**: Very High - Meets all project criteria perfectly

## Key Decision Points
1. **Primary Criteria**: Corporate backing, open source license, WPF support, .NET 10 compatibility
2. **Top Candidates**: CommunityToolkit.Mvvm (Score: 45/45) vs ReactiveUI (Score: 44/45)
3. **Final Choice**: CommunityToolkit.Mvvm due to direct Microsoft backing and simpler API

## Related Documentation
- **Brainstorming**: `docs/experiments/2026-10-30-MVVM-WPF-Libraries-Brainstorming.md`
- **Previous ADRs**: ADR-001, ADR-002, ADR-003
- **References**: Official CommunityToolkit.Mvvm documentation and GitHub repository

## Implementation Next Steps
1. Add CommunityToolkit.Mvvm NuGet package to Cartamentis.Wpf project
2. Update ViewModels to use CommunityToolkit.Mvvm patterns
3. Team training on source generators and Messenger pattern
4. Create proof-of-concept to validate integration

## Recommendations
- Proceed with CommunityToolkit.Mvvm implementation as documented in the ADR
- Use the provided code examples as templates for ViewModel development
- Reference the official Microsoft documentation for advanced scenarios
- Consider creating a project-specific style guide for CommunityToolkit.Mvvm usage
