using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.Common.Enums;

namespace InformationSystem.App.ViewModels.Activity;

public partial class ActivityEditViewModel(
    IActivityFacade activityFacade,
    ICourseFacade courseFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    private ActivityDetailModel activity = ActivityDetailModel.Empty;
    
    [ObservableProperty] 
    private ObservableCollection<CourseListModel> courses = [];
    
    public CourseListModel? SelectedCourse { get; set; } = null;

    public IEnumerable<ActivityType> ActivityTypes { get; set; } = new[]
    {
        ActivityType.Exam,
        ActivityType.Exercise,
        ActivityType.ComputerExercise,
    };
    
    protected override async Task LoadDataAsync()
    {
        var allCourses = await courseFacade.GetAsync();
        foreach (var courseListModel in allCourses)
        {
            Courses.Add(courseListModel);
        }
    }
    
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedCourse != null)
        {
            Activity.CourseId = SelectedCourse.Id;
            await activityFacade.SaveAsync(Activity);
            
            MessengerService.Send(new MessageAddActivity() {ActivityId = Activity.Id});
            navigationService.SendBackButtonPressed();
        }
    }
}