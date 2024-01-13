using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Films.GUIApp.Models;
using MsBox.Avalonia;

namespace Films.GUIApp.Windows;

public partial class MainWindow : Window
{
    private readonly string _appTitle;
    private readonly string _filmNameKey;
    private readonly string _filmPathKey;

    private readonly List<Film> _films;
    private List<string> _filmNames;

    const string VlcWin = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
    const string VlcLinux = "vlc";

    public MainWindow()
    {
        _appTitle = Application.Current!.Resources["AppTitle"]!.ToString()!;
        _filmNameKey = Application.Current!.Resources["FilmNameKey"]!.ToString()!;
        _filmPathKey = Application.Current!.Resources["FilmPathKey"]!.ToString()!;

        _films = new List<Film>();
        Mapper(_films, out _filmNames);

        InitializeComponent();

        Films.ItemsSource = _filmNames;
    }

    private async void ButtonSave_OnClick(object? sender, RoutedEventArgs e)
    {
        var name = (string)Application.Current!.Resources[_filmNameKey]!;
        var path = (string)Application.Current!.Resources[_filmPathKey]!;

        await MessageBoxManager.GetMessageBoxStandard(
            title:$"{_appTitle}. Сообщение",
            text:$"""
                  {name}
                  {path}
                  """)
            .ShowAsync();

        _films.Add(new Film()
        {
            Name = name,
            Path = path
        });
        Mapper(_films, out _filmNames);
        Films.ItemsSource = null;
        Films.ItemsSource = _filmNames;
    }

    private void ButtonClear_OnClick(object? sender, RoutedEventArgs e)
    {
        InputFilmName.Clear();
        InputFilmPath.Clear();
    }

    private async void ButtonOpenFile_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel!.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
        {
            Title = $"{_appTitle}. Выбрать фильм",
            AllowMultiple = false,
            FileTypeFilter = new []
            {
                new FilePickerFileType("фильмы")
                {
                    Patterns = new [] { "*.mov", "*.avi" }
                }
            }
        });
        InputFilmPath.SetText(files[0].Path.ToString());
    }

    private void Films_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selected = ((sender as ListBox)?.SelectedItem as string);
        var selectedFilm = _films.First(f => f.Name == selected);
        InputFilmName.SetText(selectedFilm.Name);
        InputFilmPath.SetText(selectedFilm.Path);
    }

    private void ButtonPlay_OnClick(object? sender, RoutedEventArgs e)
    {
        var os = new Os();
        var vlc = os.Type switch
        {
            OsType.Windows => VlcWin,
            OsType.Linux => VlcLinux,
            OsType.Unknown => null,
            _ => null
        };
        var path = (string)Application.Current!.Resources[_filmPathKey]!;
        if (vlc is not null)
        {
            Process.Start(vlc, $"{path} -f");
        }
    }

    private static void Mapper(in List<Film> films, out List<string> names)
    {
        /*
            names = new List<string>();
            foreach (var film in films)
            {
                names.Add(film.Name);
            }
         */
        names = films.Select(film => film.Name).ToList();
    }
}
