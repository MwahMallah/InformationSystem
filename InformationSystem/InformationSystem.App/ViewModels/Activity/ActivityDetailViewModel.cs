using CommunityToolkit.Mvvm.ComponentModel;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private ActivityDetailModel? activity;
    
    protected override async Task LoadDataAsync()
    {
        Activity = await activityFacade.GetAsync(Id);
    }
}