using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Course;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CourseDetailViewModel(
    IStudentFacade studentFacade,
    ICourseFacade courseFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService)
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private CourseDetailModel course;

    [ObservableProperty] 
    private ObservableCollection<ActivityListModel> activities = [];
    
    [ObservableProperty]
    private ObservableCollection<StudentListModel> students = [];

    protected override async Task LoadDataAsync()
    {
        Course = await courseFacade.GetAsync(Id);
        Students = Course.Students;
        Activities = Course.Activities;
    }
}