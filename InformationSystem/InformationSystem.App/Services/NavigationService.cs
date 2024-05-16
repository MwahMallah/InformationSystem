﻿using InformationSystem.App.Models;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.App.Views.Student;

namespace InformationSystem.App.Services;

public class NavigationService : INavigationService
{
    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//students", typeof(StudentListView), typeof(StudentListViewModel)),
        new("//students/edit", typeof(StudentEditView), typeof(StudentEditViewModel)),
        new("//students/detail", typeof(StudentDetailView), typeof(StudentDetailViewModel))
    };
    
    public async Task GoToAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
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