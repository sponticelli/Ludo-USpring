# Enhanced Editors Guide

The USpring Enhanced Editors provide advanced tools for working with spring animations in Unity, offering improved usability, visual feedback, and debugging capabilities.

## Overview

The enhanced editor system includes:

- **Enhanced Property Drawers** - Visual feedback and real-time preview
- **Spring Analyzer** - Performance monitoring and debugging
- **Preset Manager** - Advanced preset management with categories
- **Curve Editor** - Visual spring curve editing with real-time analysis
- **Component Editors** - Enhanced inspector interfaces

## Enhanced Property Drawers

### Features

- **Visual Type Indicators**: Color-coded spring types with icons
- **Real-time Preview**: Live spring curve visualization
- **Quick Presets**: Dropdown preset selection with one-click apply
- **Copy/Paste**: Easy parameter sharing between springs
- **Interactive Controls**: Direct parameter manipulation

### Usage

Enhanced property drawers automatically replace the default drawers for all spring types:

```csharp
public class MyComponent : MonoBehaviour
{
    [SerializeField] private SpringFloat bounceSpring;
    [SerializeField] private SpringVector3 positionSpring;
    [SerializeField] private SpringColor colorSpring;
}
```

The enhanced drawers provide:
- Foldout sections with color-coded headers
- Preset dropdown with built-in and custom presets
- Real-time curve preview (toggle on/off)
- Quick action buttons (Reset, Copy, Paste)

## Spring Analyzer Window

Access via: `Tools > USpring > Spring Analyzer`

### Features

- **Component Overview**: List all spring components in the scene
- **Performance Analysis**: Monitor CPU impact and detect issues
- **Optimization Suggestions**: Automated recommendations
- **Debugging Tools**: Real-time spring value monitoring

### Tabs

#### Overview Tab
- Lists all spring components with status indicators
- Filter by active/inactive components
- Search functionality
- Quick selection and debugging access

#### Performance Tab
- Shows active spring count and estimated CPU impact
- Identifies performance issues
- Provides auto-fix suggestions for common problems

#### Optimization Tab
- Analyzes spring configurations
- Suggests improvements for better performance
- One-click optimization application

#### Debugging Tab
- Real-time spring value monitoring
- Global spring controls (pause/resume/reset)
- Individual spring debugging tools

## Preset Manager Window

Access via: `Tools > USpring > Preset Manager`

### Features

- **Categorized Presets**: Organized by use case (UI, Gameplay, etc.)
- **Custom Presets**: Create and save your own configurations
- **Import/Export**: Share presets between projects
- **Visual Preview**: See spring curves before applying

### Categories

#### Built-in Presets
- **Bouncy**: High force, low drag for bouncy effects
- **Smooth**: Balanced force and drag for smooth motion
- **Elastic**: Medium force, low drag for elastic motion
- **Tight**: High force, high drag for controlled motion
- **Critical**: Critically damped for no overshoot

#### UI Animation
- **Button Press**: Quick, snappy button feedback
- **Panel Slide**: Smooth panel transitions
- **Popup**: Bouncy popup appearance

#### Gameplay
- **Camera Follow**: Smooth camera following
- **Physics Object**: Realistic physics response
- **Character Movement**: Responsive character motion

### Creating Custom Presets

1. Click "New Preset" in the Preset Manager
2. Configure parameters with real-time preview
3. Add name and description
4. Save to Custom category

### Applying Presets

- **Individual**: Use dropdown in property drawers
- **Bulk**: Select multiple GameObjects and use "Apply to Selection"
- **Quick**: Use preset buttons in enhanced property drawers

## Curve Editor Window

Access via: `Tools > USpring > Curve Editor`

### Features

- **Visual Curve Editing**: Interactive parameter adjustment
- **Real-time Analysis**: Settling time, overshoot, frequency analysis
- **Animation Preview**: See spring behavior over time
- **Parameter Visualization**: Understand the relationship between force and drag

### Interface

#### Parameter Controls
- **Force Slider**: Adjust spring stiffness (0.1 - 100)
- **Drag Slider**: Adjust damping (0.1 - 50)
- **Analytical Toggle**: Switch between numerical and analytical solutions

#### Curve Display
- **Grid**: Optional grid overlay for reference
- **Target Line**: Shows the target value
- **Velocity Curve**: Optional velocity visualization
- **Animation Marker**: Real-time position indicator

#### Analysis Panel
- **Settling Time**: Time to reach 98% of target
- **Overshoot**: Maximum overshoot percentage
- **Natural Frequency**: Oscillation frequency in Hz
- **Damping Type**: Under/Over/Critically damped classification
- **Quality Factor**: Measure of oscillation quality

### Interactive Editing

- **Mouse Drag**: Click and drag in the curve area to adjust parameters
- **Handle Manipulation**: Use visual handles for precise control
- **Real-time Feedback**: See changes immediately in the curve

## Enhanced Component Editors

### Base Features

All enhanced component editors include:

- **Organized Sections**: Collapsible sections for different property groups
- **Status Indicators**: Visual feedback on component state
- **Quick Actions**: Reset, copy, paste, and curve editor access
- **Performance Monitoring**: Real-time update time tracking
- **Debug Information**: Runtime value monitoring (play mode only)

### Sections

#### Header
- Component name with icon
- Status indicator (active/inactive/issues)
- Component description

#### Quick Actions
- Reset All: Reset all springs to default values
- Copy/Paste Settings: Share configurations between components
- Open Curve Editor: Launch curve editor with current parameters

#### Presets
- Dropdown preset selection
- Quick preset buttons
- One-click application

#### Spring Properties
- Main spring parameters (force, drag, target, current value)
- Color-coded section headers by spring type
- Tooltips with parameter explanations

#### Advanced Settings
- Clamping configuration
- Event settings
- Physics model selection

#### Preview
- Real-time spring curve visualization
- Animation controls (play/pause/stop)
- Time scrubbing

#### Debug Info (Play Mode Only)
- Current spring values
- Performance metrics
- Update timing information

## Best Practices

### Performance Optimization

1. **Monitor Active Springs**: Use the Spring Analyzer to track active spring count
2. **Disable When Not Needed**: Turn off springs when objects are not visible
3. **Use Appropriate Parameters**: Avoid extremely high force/drag values
4. **Batch Operations**: Use preset manager for bulk configuration changes

### Workflow Tips

1. **Start with Presets**: Use built-in presets as starting points
2. **Use Curve Editor**: Visualize spring behavior before implementation
3. **Test in Play Mode**: Use debug tools to monitor runtime behavior
4. **Save Custom Presets**: Create reusable configurations for your project

### Debugging

1. **Check Spring Analyzer**: Look for performance warnings
2. **Monitor Debug Values**: Watch real-time spring values in play mode
3. **Use Animation Preview**: Test spring behavior without entering play mode
4. **Profile Performance**: Monitor update times for optimization

## Troubleshooting

### Common Issues

#### Springs Not Responding
- Check if spring is enabled
- Verify force and drag values are reasonable
- Ensure target and current values are different

#### Performance Issues
- Use Spring Analyzer to identify problematic components
- Reduce number of active springs
- Optimize spring parameters

#### Unexpected Behavior
- Check clamping settings
- Verify physics model selection
- Use Curve Editor to analyze spring characteristics

### Getting Help

- Use the "?" button in component editors for quick help
- Check the Spring Analyzer for optimization suggestions
- Refer to the main USpring documentation for detailed API reference

## Advanced Features

### Custom Property Drawers

Extend the enhanced property drawer system:

```csharp
[CustomPropertyDrawer(typeof(MyCustomSpring))]
public class MyCustomSpringDrawer : EnhancedSpringPropertyDrawer
{
    // Custom implementation
}
```

### Custom Component Editors

Create enhanced editors for your components:

```csharp
[CustomEditor(typeof(MySpringComponent))]
public class MySpringComponentEditor : EnhancedSpringComponentEditor
{
    // Implement abstract methods
    protected override void DrawSpringProperties() { /* ... */ }
    protected override string GetComponentDisplayName() => "My Spring Component";
    // ... other implementations
}
```

### Extending the Analyzer

Add custom analysis rules to the Spring Analyzer by extending the analysis methods in `SpringAnalyzerWindow.cs`.

## Conclusion

The Enhanced Editors provide a comprehensive toolkit for working with USpring animations, offering improved productivity, better debugging capabilities, and enhanced visual feedback. Use these tools to create more polished spring animations with less effort and better performance.
