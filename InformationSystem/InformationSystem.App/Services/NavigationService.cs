using InformationSystem.App.Models;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.App.Views.Student;

namespace InformationSystem.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//students/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
    };
    
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public bool SendBackButtonPressed()
    {
        return Shell.Current.SendBackButtonPressed();
    }
}