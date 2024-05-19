using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Course;

[QueryProperty(nameof(Course), nameof(Course))]
public partial class CourseEditViewModel(
    ICourseFacade courseFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    private CourseDetailModel course = CourseDetailModel.Empty;
    
    [ObservableProperty] 
    private string? abbreviation = null;
    [ObservableProperty] 
    private string? name = null;
    
    partial void OnAbbreviationChanged(string? value) => SaveCommand.NotifyCanExecuteChanged();
    partial void OnNameChanged(string? value) => SaveCommand.NotifyCanExecuteChanged();
    
    protected override Task LoadDataAsync()
    {
        Abbreviation = Course.Abbreviation;
        Name = Course.Name;
        
        return base.LoadDataAsync();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        Course.Abbreviation = Abbreviation!;
        Course.Name = Name!;
        
        await courseFacade.SaveAsync(Course);
        
        MessengerService.Send(new MessageEditCourse() {CourseId = Course.Id});
        navigationService.SendBackButtonPressed();
    }

    private bool CanSave()
    {
        return !string.IsNullOrEmpty(Abbreviation) 
               && !string.IsNullOrEmpty(Name);
    }
}