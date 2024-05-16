using CommunityToolkit.Mvvm.ComponentModel;

namespace InformationSystem.App.ViewModels;

public class ViewModelBase: ObservableRecipient, IViewModel
{
    private bool _isRefreshRequired = true;
    
    public async Task OnAppearingAsync()
    {
        if (_isRefreshRequired)
        {
            await LoadDataAsync();

            _isRefreshRequired = false;
        }
    }

    protected virtual Task LoadDataAsync()
    {
        return Task.CompletedTask;
    }
}