using Windows.Media.Devices.Core;
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

    [ObservableProperty] 
    private string? firstName = null;
    [ObservableProperty] 
    private string? lastName = null;
    [ObservableProperty] 
    private string? group = null;
    [ObservableProperty] 
    private int currentYear;
    [ObservableProperty] 
    private string? imageText;
    [ObservableProperty] 
    private Uri image;

    partial void OnFirstNameChanged(string? value) => SaveCommand.NotifyCanExecuteChanged();
    partial void OnLastNameChanged(string? value) => SaveCommand.NotifyCanExecuteChanged();
    partial void OnGroupChanged(string? value) => SaveCommand.NotifyCanExecuteChanged();
    partial void OnCurrentYearChanged(int value) => SaveCommand.NotifyCanExecuteChanged();

    partial void OnImageTextChanged(string? value)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            Image = uri;
        }
        SaveCommand.NotifyCanExecuteChanged();
    }

    protected override Task LoadDataAsync()
    {
        FirstName = Student.FirstName;
        LastName = Student.LastName;
        Group = Student.Group;
        CurrentYear = Student.CurrentYear;
        ImageText = Student?.ImageUrl != null ? Student.ImageUrl.ToString() 
            : "https://upload.wikimedia.org/wikipedia/commons/e/ec/Raphael-bostic-300x400.jpg";
        
        return base.LoadDataAsync();
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        Student.FirstName = FirstName!;
        Student.LastName = LastName!;
        Student.Group = Group!;
        Student.CurrentYear = CurrentYear;
        Student.ImageUrl = Image;
        
        await studentFacade.SaveAsync(Student);
        
        MessengerService.Send(new MessageEditStudent() {StudentId = Student.Id});
        navigationService.SendBackButtonPressed();
    }

    private bool CanSave()
    {
        return !string.IsNullOrEmpty(FirstName) 
               && !string.IsNullOrEmpty(LastName)
               && !string.IsNullOrEmpty(Group)
               && CurrentYear > 0;
    }

    [RelayCommand]
    private async Task UpdateAsync()
    {
        await LoadDataAsync();
    }

    [RelayCommand(CanExecute = nameof(CanAddCourses))]
    private async Task GoToStudentCourseEditAsync()
    {
        await navigationService.GoToAsync("/courses", 
            new Dictionary<string, object?>()
            {
                [nameof(StudentCoursesEditViewModel.Student)] = Student
            });
    }

    private bool CanAddCourses()
    {
        return Student.Id != Guid.Empty;
    }
}