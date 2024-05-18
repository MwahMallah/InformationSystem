namespace InformationSystem.App.Services.Interfaces;

public interface IAlertService
{
    Task<bool> WaitConfirmationAsync(string title, string message);

}