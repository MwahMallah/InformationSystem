using InformationSystem.App.Models;
using InformationSystem.App.ViewModels;

namespace InformationSystem.App.Services.Interfaces;

public interface INavigationService
{
    public IEnumerable<RouteModel> Routes { get; }

    public Task GoToAsync(string route);
    public Task GoToAsync(string route, IDictionary<string, object?> parameters);
    public Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
        where TViewModel: IViewModel;
    public bool SendBackButtonPressed();
}