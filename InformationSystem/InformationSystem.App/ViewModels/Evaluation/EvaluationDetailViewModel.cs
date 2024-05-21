using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Evaluation;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class EvaluationDetailViewModel(
    IEvaluationFacade evaluationFacade,
    IActivityFacade activityFacade,
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private EvaluationDetailModel? evaluation;
    
    protected override async Task LoadDataAsync()
    {
        Evaluation = await evaluationFacade.GetAsync(Id);
        
        if (Evaluation == null)
        {
            Messenger.Send(new MessageDeleteEvaluation() {EvaluationId = Guid.Empty});
            navigationService.SendBackButtonPressed();
        }
    }
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/edit", new Dictionary<string, object?>()
        {
            [nameof(EvaluationEditViewModel.Evaluation)] = Evaluation,
        });
    }
    
    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Evaluation != null)
        {
            var isConfirmed = await alertService
                .WaitConfirmationAsync("Delete Evaluation", 
                    "Are you sure you want to Delete Evaluation?");

            if (isConfirmed)
            {
                await evaluationFacade.DeleteAsync(Evaluation.Id);
                Messenger.Send(new MessageDeleteEvaluation() { EvaluationId= Evaluation.Id});
                Evaluation = null;
                navigationService.SendBackButtonPressed();
            }
        }
    }
}