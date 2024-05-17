using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Messages;
using InformationSystem.App.Services;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Student;

[QueryProperty(nameof(Student), nameof(Student))]
public partial class StudentEditViewModel(
        IStudentFacade studentFacade,
        INavigationService navigationService,
        IMessengerService messengerService
    ) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    private StudentDetailModel student = StudentDetailModel.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        await studentFacade.SaveAsync(Student);
        
        MessengerService.Send(new MessageEditStudent() {StudentId = Student.Id});
        navigationService.SendBackButtonPressed();
    }

    [RelayCommand]
    private async Task GoToStudentCourseEditAsync()
    {
        await navigationService.GoToAsync("/courses", 
            new Dictionary<string, object?>()
            {
                [nameof(StudentCoursesEditViewModel.Student)] = Student
            });
    }
}