# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2024-12-19

### Added
- **Enhanced Custom Editors System** with comprehensive visual feedback and usability improvements
- **Enhanced Property Drawers** with real-time preview, preset integration, and visual type indicators
- **Spring Analyzer Window** for performance monitoring, debugging, and optimization
- **Enhanced Preset Manager** with categorized presets, custom preset creation, and import/export
- **Visual Spring Curve Editor** with interactive parameter adjustment and real-time analysis
- **Enhanced Component Editor Base Class** for consistent advanced editor features
- **Comprehensive Enhanced Editors Documentation** with usage guides and best practices

### Enhanced Editor Features
- Real-time spring curve visualization in property drawers
- Interactive parameter adjustment with immediate visual feedback
- Performance monitoring and optimization suggestions
- Advanced debugging tools with runtime value monitoring
- Categorized preset system (Built-in, UI Animation, Gameplay, Custom)
- Visual spring curve analysis (settling time, overshoot, frequency)
- Copy/paste functionality for spring parameters
- Quick action buttons for common operations
- Status indicators and color-coded spring types
- Animation preview controls with time scrubbing

### Improved
- Editor usability with organized, collapsible sections
- Visual feedback with color-coded headers and status indicators
- Workflow efficiency with quick presets and bulk operations
- Debugging capabilities with real-time monitoring tools
- Documentation with comprehensive editor usage guides

## [1.1.0] - 2024-12-19

### Added
- Complete interface implementations for all spring classes
- Fluent API support with method chaining for all spring types
- Missing interface methods for clamping queries:
  - `IsClampTargetEnabled()` for all spring types
  - `IsClampCurrentValueEnabled()` for all spring types
- Enhanced clamping configuration methods:
  - `SetClampRange()` overloads for all spring types
  - `ConfigureClamping()` for comprehensive clamping setup
- `Configure()` methods for quick spring setup
- `Clone()` methods for all spring types with full state preservation

### Changed
- All spring setter methods now return interface types for fluent API
- SpringVector4 now properly implements ISpringVector4 interface
- SpringVector2 now properly implements ISpringVector2 interface
- Improved API consistency across all spring types

### Fixed
- Missing interface implementations in SpringVector4
- Missing interface implementations in SpringVector2
- Compilation errors in component classes
- Backward compatibility for existing method names

## [1.0.0] - 2024-12-18

### Added
- Initial release of USpring physics-based spring animation system
- Support for float, Vector2, Vector3, Vector4, Color, and Quaternion springs
- Physically accurate damped harmonic oscillator implementation
- Semi-implicit Euler and analytical physics models
- Comprehensive clamping system with target and current value constraints
- Unity component integration for easy scene setup
- Extensive documentation and examples
