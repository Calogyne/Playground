## 一些有用的 MSDN posts

### On XAML and related topics
* [ControlTemplate class](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.controltemplate)
* [Concept of attached properties](https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/attached-properties-overview)
    
    tl;dr:
    ```csharp
    // lets say, in container control definition:
    class ContainerControl : Control 
    {
        public static DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Vector2), typeof(ContainerControl), null);
        // no accessor property definition!
        ...
    }

    // now, for some UI element inside the container control, you can:
    element.SetValue(ContainerControl.PositionProperty, new Vector2());

    // XAML's property management system will take care about all those
    // parent-child stuff for you, no need to explicity specify the parent 
    //while getting/setting the attached property.

    ```
    ```xml
    <Element x:Name="element" ContainerControl.Position="...">
    ```


### On data virtualization
* [ListView and GridView data virtualization](https://docs.microsoft.com/en-us/windows/uwp/debug-test-perf/listview-and-gridview-data-optimization)

### On file access
* [KnownFolder class](https://docs.microsoft.com/en-us/uwp/api/Windows.Storage.KnownFolders)