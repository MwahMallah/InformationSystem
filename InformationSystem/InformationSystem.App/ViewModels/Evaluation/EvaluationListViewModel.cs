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
) : ViewModelBase(messengerService), 
    IRecipient<MessageAddEvaluation>,
    IRecipient<MessageDeleteEvaluation>,
    IRecipient<MessageDeleteActivity>
{
    private string? _filterText = null;

    [ObservableProperty] 
    private IEnumerable<EvaluationListModel> evaluations = [];

    private bool _isCourseAbbreviationAscending;
    private bool _isPointsAscending;
    private bool _isFullNameAscending;
    private bool _isActivityTypeAscending;

    protected override async Task LoadDataAsync()
    {
        Evaluations = await evaluationFacade.GetAsync();
    }

    [RelayCommand]
    private async Task SortAsync(string sortingQuery)
    {
        var isAscending = true;
        switch (sortingQuery)
        {
            case "course_abbreviation":
                _isCourseAbbreviationAscending = !_isCourseAbbreviationAscending;
                isAscending = _isCourseAbbreviationAscending;
                break;
            case "activity_type":
                _isActivityTypeAscending = !_isActivityTypeAscending;
                isAscending = _isActivityTypeAscending;
                break;
            case "full_name":
                _isFullNameAscending = !_isFullNameAscending;
                isAscending = _isFullNameAscending;
                break;
            case "points":
                _isPointsAscending = !_isPointsAscending;
                isAscending = _isPointsAscending;
                break;
        }
        
        Evaluations = await evaluationFacade.GetAsync(searchQuery: _filterText,
            orderQuery: sortingQuery, isAscending: isAscending);
    }

    [RelayCommand]
    private async Task FilterAsync(string filterText)
    {
        _filterText = filterText;
        Evaluations = await evaluationFacade.GetAsync(searchQuery: filterText);
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<EvaluationDetailViewModel>(
            new Dictionary<string, object?>() { [nameof(EvaluationDetailViewModel.Id)] = id }
        );
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    public async void Receive(MessageAddEvaluation message)
    {
        await LoadDataAsync();
    }

    public async void Receive(MessageDeleteEvaluation message)
    {
        await LoadDataAsync();
    }

    public async void Receive(MessageDeleteActivity message)
    {
        await LoadDataAsync();
    }
}