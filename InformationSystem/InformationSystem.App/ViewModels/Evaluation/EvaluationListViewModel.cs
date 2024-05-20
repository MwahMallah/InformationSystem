using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.Common.Enums;
using InformationSystem.DAL.Seeds;

namespace InformationSystem.App.ViewModels.Evaluation;

public partial class EvaluationListViewModel(
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService)
{
    private string? _filterText = null;

    [ObservableProperty] 
    private IEnumerable<EvaluationListModel> evaluations = [];

    private bool _isCourseAbbreviationAscending = false;
    private bool _isMaxPointsAscending;
    private bool _isStartTimeAscending;
    private bool _isFinishTimeAscending;
    private bool _isActivityTypeAscending;

    protected override async Task LoadDataAsync()
    {
        Evaluations = await evaluationFacade.GetAsync();
    }

    [RelayCommand]
    private async Task SortAsync(string sortingQuery)
    {
        // var isAscending = true;
        // switch (sortingQuery)
        // {
        //     case "course_abbreviation":
        //         _isCourseAbbreviationAscending = !_isCourseAbbreviationAscending;
        //         isAscending = _isCourseAbbreviationAscending;
        //         break;
        //     case "max_points":
        //         _isMaxPointsAscending = !_isMaxPointsAscending;
        //         isAscending = _isMaxPointsAscending;
        //         break;
        //     case "start_time":
        //         _isStartTimeAscending = !_isStartTimeAscending;
        //         isAscending = _isStartTimeAscending;
        //         break;
        //     case "finish_time":
        //         _isFinishTimeAscending = !_isFinishTimeAscending;
        //         isAscending = _isFinishTimeAscending;
        //         break;
        //     case "activity_type":
        //         _isActivityTypeAscending = !_isActivityTypeAscending;
        //         isAscending = _isActivityTypeAscending;
        //         break;
        // }
        //
        // Evaluations = await evaluationFacade.GetAsync(searchQuery: _filterText,
        //     orderQuery: sortingQuery, isAscending: isAscending);
    }

    [RelayCommand]
    private async Task FilterAsync(string filterText)
    {
        // _filterText = filterText;
        // Activities = await activityFacade.GetAsync(searchQuery: filterText);
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?>() { [nameof(ActivityDetailViewModel.Id)] = id }
        );
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }
}