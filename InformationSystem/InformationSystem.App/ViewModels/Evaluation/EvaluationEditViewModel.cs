using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Evaluation;

[QueryProperty(nameof(CourseFromQuery), nameof(CourseFromQuery))]
[QueryProperty(nameof(StudentFromQuery), nameof(StudentFromQuery))]
[QueryProperty(nameof(ActivityFromQuery), nameof(ActivityFromQuery))]
[QueryProperty(nameof(Evaluation), nameof(Evaluation))]
public partial class EvaluationEditViewModel(
    IActivityFacade activityFacade,
    IStudentFacade studentFacade,
    ICourseFacade courseFacade,
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService)
{
    [ObservableProperty]
    private EvaluationDetailModel evaluation = EvaluationDetailModel.Empty;
    
    [ObservableProperty] 
    private ObservableCollection<CourseListModel> courses = [];
    
    [ObservableProperty]
    private CourseListModel? selectedCourse = null;
    [ObservableProperty]
    private CourseListModel? courseFromQuery = null;
    
    [ObservableProperty] 
    private ObservableCollection<ActivityListModel> activities = [];

    [ObservableProperty]
    private ActivityListModel? selectedActivity = null;
    [ObservableProperty]
    private ActivityListModel? activityFromQuery = null;
    
    [ObservableProperty] 
    private ObservableCollection<StudentListModel> students = [];
    
    [ObservableProperty]
    private StudentListModel? selectedStudent = null;
    [ObservableProperty]
    private StudentListModel? studentFromQuery = null;
    
    public bool ArePickersEnabled { get; set; }

    partial void OnSelectedCourseChanged(CourseListModel? value)
    {
        if (CourseFromQuery != null) return;
        
        ChangeAvailableActivities();
        ChangeAvailableStudents();
        SaveCommand.NotifyCanExecuteChanged();
    }

    partial void OnSelectedActivityChanged(ActivityListModel? value) 
        => SaveCommand.NotifyCanExecuteChanged();

    partial void OnSelectedStudentChanged(StudentListModel? value) 
        => SaveCommand.NotifyCanExecuteChanged();

    protected override async Task LoadDataAsync()
    {
        var allCourses = await courseFacade.GetAsync();
        foreach (var courseListModel in allCourses)
        {
            Courses.Add(courseListModel);
        }

        ArePickersEnabled = Evaluation.Id == Guid.Empty;
        SelectedCourse = CourseFromQuery ?? SelectedCourse;
        SelectedActivity = ActivityFromQuery ?? SelectedActivity;
        SelectedStudent = StudentFromQuery ?? SelectedStudent;
    }

    private async void ChangeAvailableActivities()
    {
        if (SelectedCourse != null)
        {
            Activities = new ObservableCollection<ActivityListModel>
                (await activityFacade.GetFromCourseAsync(SelectedCourse.Id));
        }
    }
    
    private async void ChangeAvailableStudents()
    {
        if (SelectedCourse != null)
        {
            Students = new ObservableCollection<StudentListModel>
                (await studentFacade.GetCourseStudentsAsync(SelectedCourse.Id));
        }
    }
    
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        Evaluation.StudentId  = SelectedStudent?.Id ?? Evaluation.StudentId;
        Evaluation.ActivityId = SelectedActivity?.Id ?? Evaluation.ActivityId;
        
        await evaluationFacade.SaveAsync(Evaluation);
        
        MessengerService.Send(new MessageAddEvaluation() {EvaluationId = Evaluation.Id});
        navigationService.SendBackButtonPressed();
    }
    
    private bool CanSave()
    {
        return Evaluation.Id != Guid.Empty ||  
            (SelectedCourse != null 
               && SelectedActivity != null 
               && SelectedStudent  != null);
    }
}