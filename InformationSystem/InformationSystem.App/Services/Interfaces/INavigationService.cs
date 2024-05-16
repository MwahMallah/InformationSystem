﻿using InformationSystem.App.Models;

namespace InformationSystem.App.Services;

public interface INavigationService
{
    public IEnumerable<RouteModel> Routes { get; }

    public Task GoToAsync(string route);
    public bool SendBackButtonPressed();
}