using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Films.GUIApp.Components;

public partial class InputComponent : UserControl
{
    public string LabelName { get; set; }
    public string? InputPlaceholder { get; set; }
    public string KeyName { get; set; }

    public InputComponent()
    {
        InitializeComponent();

        Loaded += (_, _) =>
        {
            Label.Content = LabelName;
            Input.Watermark = InputPlaceholder;
        };
    }

    private void Input_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        Application.Current!.Resources[KeyName] = Input.Text;
    }

    public void Clear()
    {
        Input.Clear();
    }

    public void SetText(string text)
    {
        Input.Text = text;
    }
}

