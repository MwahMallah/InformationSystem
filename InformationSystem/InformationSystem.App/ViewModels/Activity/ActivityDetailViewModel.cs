using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Activity;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel(
    IActivityFacade activityFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private ActivityDetailModel? activity;
    
    protected override async Task LoadDataAsync()
    {
        Activity = await activityFacade.GetAsync(Id);
    }
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        
        await navigationService.GoToAsync("/edit", new Dictionary<string, object?>()
        {
            [nameof(ActivityEditViewModel.Activity)] = Activity
        });
    }
    
    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Activity != null)
        {
            var isConfirmed = await alertService
                .WaitConfirmationAsync("Delete Activity", "Are you sure you want to Delete Activity?");

            if (isConfirmed)
            {
                await activityFacade.DeleteAsync(Activity.Id);
                Messenger.Send(new MessageDeleteActivity() {ActivityId = Activity.Id});
                Activity = null;
                navigationService.SendBackButtonPressed();
            }
        }
    }
}