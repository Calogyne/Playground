## 一些有用的 MSDN posts

### On XAML and related topics
* [ControlTemplate class](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.controltemplate)
* [Concept of attached properties](https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/attached-properties-overview)
    
    tl;dr:
    ```csharp
    // lets say, in container control definition:
    class ContainerControl : Control 
    {
        public static readonly DependencyProperty PositionAttachedProperty =
            DependencyProperty.RegisterAttached("Position", typeof(Vector2), typeof(ContainerControl), null);

        // convenient accessors:
        public static Vector2 GetPosition(DependencyObject target) =>
            (Vector2)target.GetValue(PositionAttachedProperty);
        
        public static void SetPosition(DependencyObject target, Vector2 value) =>
            target.SetValue(PositionAttachedProperty, value);
        ...
    }

    // now, for some UI element inside the container control, you can:
    element.SetValue(ContainerControl.PositionProperty, new Vector2());

    // XAML's property management system will take care about all those
    // parent-child stuff for you.

    ```
    ```xml
    <Element x:Name="element" ContainerControl.Position="...">
    ```


### On data virtualization
* [ListView and GridView data virtualization](https://docs.microsoft.com/en-us/windows/uwp/debug-test-perf/listview-and-gridview-data-optimization)

### On file access
* [KnownFolder class](https://docs.microsoft.com/en-us/uwp/api/Windows.Storage.KnownFolders)