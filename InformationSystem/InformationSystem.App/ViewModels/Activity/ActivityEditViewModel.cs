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

[QueryProperty(nameof(Activity), nameof(Activity))]
[QueryProperty(nameof(CourseFromQuery), nameof(CourseFromQuery))]
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
    
    [ObservableProperty]
    private CourseListModel? selectedCourse = null;
    
    public CourseListModel? CourseFromQuery {get; set; } = null;

    public DateTime StartDate { get; set; } = DateTime.Today;
    public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;
    
    public DateTime FinishDate { get; set; } = DateTime.Today;
    public TimeSpan FinishTime { get; set; } = DateTime.Now.TimeOfDay;

    partial void OnSelectedCourseChanged(CourseListModel? value) => SaveCommand.NotifyCanExecuteChanged();
    
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

        if (CourseFromQuery != null)
        {
            SelectedCourse = CourseFromQuery;
        }
    }
    
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (SelectedCourse != null)
        {
            Activity.CourseId = SelectedCourse.Id;
            Activity.StartTime = StartDate + StartTime;
            Activity.FinishTime = FinishDate + FinishTime;
            
            await activityFacade.SaveAsync(Activity);
            
            MessengerService.Send(new MessageAddActivity() {ActivityId = Activity.Id});
            navigationService.SendBackButtonPressed();
        }
    }
    
    private bool CanSave()
    {
        return SelectedCourse != null;
    }
}