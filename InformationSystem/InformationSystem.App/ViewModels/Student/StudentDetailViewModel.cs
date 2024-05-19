using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Seeds;

namespace InformationSystem.App.ViewModels.Student;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class StudentDetailViewModel(
    IStudentFacade studentFacade,
    ICourseFacade courseFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService), IRecipient<MessageEditStudent>, IRecipient<MessageAddActivity>
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private StudentDetailModel? student;

    [ObservableProperty] 
    private ObservableCollection<ActivityListModel> activities = [];
    
    [ObservableProperty]
    private ObservableCollection<CourseListModel> courses = [];

    protected override async Task LoadDataAsync()
    {
        Student = await studentFacade.GetAsync(Id);
        Courses = Student.Courses;
        Activities = Student.Activities;
    }
    
    [RelayCommand]
    private async Task FilterCoursesAsync(string filterText)
    {
        Courses = new ObservableCollection<CourseListModel>(await courseFacade
            .GetStudentsCoursesAsync(Student!.Id, filterText));
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Student != null)
        {
            var isConfirmed = await alertService
                .WaitConfirmationAsync("Delete Student", "Are you sure you want to Delete Student?");

            if (isConfirmed)
            {
                await studentFacade.DeleteAsync(Student.Id);
                Messenger.Send(new MessageDeleteStudent() {StudentId = Student.Id});
                navigationService.SendBackButtonPressed();
            }
        }
    }
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await navigationService.GoToAsync("/edit", new Dictionary<string, object?>()
        {
            [nameof(StudentEditViewModel.Student)] = Student
        });
    }

    [RelayCommand]
    private async Task GoToActivityDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("//activities/detail", new Dictionary<string, object?>()
        {
            [nameof(ActivityDetailViewModel.Id)] = id
        });
    }
    
    public async void Receive(MessageEditStudent message)
    {
        //Performs this code after receiving edit student message.
        await LoadDataAsync();
    }

    public async void Receive(MessageAddActivity message)
    {
        await LoadDataAsync();
    }
}