using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Evaluation;

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
    private ObservableCollection<ActivityListModel> activities = [];

    [ObservableProperty]
    private ActivityListModel? selectedActivity = null;
    
    [ObservableProperty] 
    private ObservableCollection<StudentListModel> students = [];
    
    [ObservableProperty]
    private StudentListModel? selectedStudent = null;
    
    public CourseListModel? CourseFromQuery {get; set; } = null;

    public bool ArePickersEnabled { get; set; }

    partial void OnSelectedCourseChanged(CourseListModel? value)
    {
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
    }

    private async void ChangeAvailableActivities()
    {
        Activities = new ObservableCollection<ActivityListModel>
            (await activityFacade.GetFromCourseAsync(selectedCourse.Id));
    }
    
    private async void ChangeAvailableStudents()
    {
        Students = new ObservableCollection<StudentListModel>
            (await studentFacade.GetCourseStudentsAsync(selectedCourse.Id));
    }
    
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        if (SelectedCourse != null && SelectedStudent != null && SelectedActivity != null)
        {
            Evaluation.StudentId  = SelectedStudent.Id;
            Evaluation.ActivityId = SelectedActivity.Id;
            
            await evaluationFacade.SaveAsync(Evaluation);
            
            MessengerService.Send(new MessageAddEvaluation() {EvaluationId = Evaluation.Id});
            navigationService.SendBackButtonPressed();
        }
    }
    
    private bool CanSave()
    {
        return SelectedCourse != null 
               && SelectedActivity != null 
               && SelectedStudent  != null;
    }
}