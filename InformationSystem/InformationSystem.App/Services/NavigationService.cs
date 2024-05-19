using InformationSystem.App.Models;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.App.ViewModels.Course;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.App.Views.Activity;
using InformationSystem.App.Views.Course;
using InformationSystem.App.Views.Student;
using InformationSystem.BL.Models;

namespace InformationSystem.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//students/detail", typeof(StudentDetailView), typeof(StudentDetailViewModel)),
        
        new("//students/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//students/detail/courses", typeof(StudentCoursesEditView), typeof(StudentCoursesEditViewModel)),
        
        new("//activities", typeof(ActivityListView), typeof(ActivityListViewModel)),
        new("//activities/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),

        new("//activities/detail", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),
        
        new("//courses", typeof(CourseListView), typeof(CourseListViewModel)),
        new("//courses/edit", typeof(CourseEditView), typeof(CourseEditViewModel)),
        
    };
    
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
    {
        await Shell.Current.GoToAsync(route, parameters);
    }

    public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters) 
        where TViewModel : IViewModel
    {
        var route = GetRouteByViewModel<TViewModel>();
        await Shell.Current.GoToAsync(route, parameters);
    }

    private string GetRouteByViewModel<TViewModel>() 
        where TViewModel: IViewModel
    {
        return Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
    }

    public bool SendBackButtonPressed()
    {
        return Shell.Current.SendBackButtonPressed();
    }
}