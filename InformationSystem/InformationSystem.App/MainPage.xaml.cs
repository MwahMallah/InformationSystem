namespace InformationSystem.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void RadioButton_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        await Shell.Current.GoToAsync($"students");
    }
}