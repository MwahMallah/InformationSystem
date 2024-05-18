using InformationSystem.App.Services.Interfaces;

namespace InformationSystem.App.Services;

public class AlertService : IAlertService
{
    public async Task<bool> WaitConfirmationAsync(string title, string message)
    {
        bool isConfirmed = await Application
            .Current?.MainPage?.DisplayAlert(title, message, "OK", "Cancel")!;

        return isConfirmed;
    }
}