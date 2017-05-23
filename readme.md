## 一些有用的 MSDN posts, good reads

### On XAML and related topics
- [ControlTemplate class](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.controltemplate)
- [Concept of attached properties](https://docs.microsoft.com/en-us/windows/uwp/xaml-platform/attached-properties-overview)
    
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
    element.SetValue(ContainerControl.PositionAttachedProperty, new Vector2());

    // XAML's property management system will take care about all those
    // parent-child stuff for you.

    ```
    ```xml
    <Element x:Name="element" ContainerControl.Position="...">
    ```
- Layout:
    + [custom panel overview](https://docs.microsoft.com/en-us/windows/uwp/layout/custom-panels-overview)
    + [FrameworkElement.ArrangeOverride(Size)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.FrameworkElement#Windows_UI_Xaml_FrameworkElement_ArrangeOverride_Windows_Foundation_Size_)
    + [FrameworkElement.MeasureOverride(Size)](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.FrameworkElement#Windows_UI_Xaml_FrameworkElement_MeasureOverride_Windows_Foundation_Size_)
    + [Layout(Windows Presentation Foundation)](https://msdn.microsoft.com/en-us/library/ms745058(v=vs.110).aspx) I think up to this point WPF is still better documented than UWP.

### On data virtualization
- [ListView and GridView data virtualization](https://docs.microsoft.com/en-us/windows/uwp/debug-test-perf/listview-and-gridview-data-optimization)

### On data binding
- [Data binding in depth](https://docs.microsoft.com/en-us/windows/uwp/data-binding/data-binding-in-depth)
- [Incremental loading](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Data.ISupportIncrementalLoading)
- [DataContext of FrameworkElement](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.FrameworkElement#Windows_UI_Xaml_FrameworkElement_DataContext)

### On file access
- [KnownFolder class](https://docs.microsoft.com/en-us/uwp/api/Windows.Storage.KnownFolders)

### On C# language, .NET
- [Asynchronous programming](https://docs.microsoft.com/en-us/dotnet/articles/csharp/async) 