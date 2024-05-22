using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.App.ViewModels.Course;
using InformationSystem.App.ViewModels.Evaluation;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Seeds;

namespace InformationSystem.App.ViewModels.Student;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class StudentDetailViewModel(
    IStudentFacade studentFacade,
    ICourseFacade courseFacade,
    IActivityFacade activityFacade,
    IEvaluationFacade evaluationFacade,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService), 
    IRecipient<MessageEditStudent>, 
    IRecipient<MessageEditCourse>,
    IRecipient<MessageAddActivity>, 
    IRecipient<MessageAddEvaluation>,
    IRecipient<MessageDeleteCourse>, 
    IRecipient<MessageDeleteActivity>,
    IRecipient<MessageDeleteEvaluation>
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private StudentDetailModel? student;

    [ObservableProperty] 
    private ObservableCollection<ActivityListModel> activities = [];

    [ObservableProperty] 
    private ObservableCollection<ActivityEvaluationListModel> activityEvaluationList = [];
    
    [ObservableProperty]
    private ObservableCollection<CourseListModel> courses = [];
    [ObservableProperty]
    private ObservableCollection<CourseListModel> coursesToPick = [];

    [ObservableProperty] 
    private CourseListModel? selectedCourse = null;
    
    [ObservableProperty]
    private DateTime? startDate = null;
    [ObservableProperty]
    private TimeSpan? startTime = null;
    [ObservableProperty]
    private DateTime? finishDate = null;
    [ObservableProperty]
    private TimeSpan? finishTime = null;

    partial void OnStartDateChanged(DateTime? value) => FilterActivitiesAsync();
    partial void OnStartTimeChanged(TimeSpan? value) => FilterActivitiesAsync();
    partial void OnFinishDateChanged(DateTime? value) => FilterActivitiesAsync();
    partial void OnFinishTimeChanged(TimeSpan? value) => FilterActivitiesAsync();
    partial void OnSelectedCourseChanged(CourseListModel? value) => FilterActivitiesAsync();

    protected override async Task LoadDataAsync()
    {
        Student = await studentFacade.GetAsync(Id);
        if (Student == null)
        {
            Messenger.Send(new MessageDeleteStudent() {StudentId = Guid.Empty});
            navigationService.SendBackButtonPressed();
        }
        else
        {
            Courses = Student.Courses;
            CoursesToPick = new ObservableCollection<CourseListModel>(Courses);
            CoursesToPick.Insert(0, CourseListModel.AllCourses);
            Activities = Student.Activities;
            LoadActivitiesWithEvaluationsAsync();
        }
    }
    
    private async void FilterActivitiesAsync()
    {
        var startDateTime = CombineDateAndTime(StartDate, StartTime);
        var finishDateTime = CombineDateAndTime(FinishDate, FinishTime);
        
        if (SelectedCourse != null && SelectedCourse.Id != Guid.Empty)
        {
            Activities = new ObservableCollection<ActivityListModel>(await activityFacade
                .GetFromCourseAsync(SelectedCourse.Id, startDateTime, finishDateTime));
        }
        else
        {
            Activities = new ObservableCollection<ActivityListModel>(await activityFacade
                .FilterByTime(Student.Activities, startDateTime, finishDateTime));
        }

        LoadActivitiesWithEvaluationsAsync();
    }

    private async void LoadActivitiesWithEvaluationsAsync()
    {
        ActivityEvaluationList = new();
        
        var evaluations 
            = await evaluationFacade.GetStudentEvaluationsAsync(Student.Id);
            
        foreach (var activity in Activities)
        {
            var evaluation = evaluations
                .FirstOrDefault(e => e.ActivityId == activity.Id);
            
            ActivityEvaluationList.Add(new ActivityEvaluationListModel()
            {
                Activity = activity,
                Evaluation = evaluation
            });
        }
    }

    private DateTime? CombineDateAndTime(DateTime? date, TimeSpan? time)
    {
        if (date == null)
        {
            return null;
        }

        return date + (time ?? TimeSpan.Zero);
    }

    [RelayCommand]
    private async Task FilterCoursesAsync(string filterText)
    {
        Courses = new ObservableCollection<CourseListModel>(await courseFacade
            .GetStudentsCoursesAsync(Student!.Id, filterText)) { CourseListModel.AllCourses };
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
                Student = null;
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
    private async Task GoToActivityDetailAsync(ActivityListModel activityListModel)
    {
        await navigationService.GoToAsync("//activities/detail", new Dictionary<string, object?>()
        {
            {nameof(ActivityDetailViewModel.Id), activityListModel.Id},
            {"ViewModel", typeof(ActivityDetailViewModel)}
        });
    }
    
    [RelayCommand]
    private async Task GoToEvaluationAsync(ActivityEvaluationListModel activityEvaluationListModel)
    {
        if (activityEvaluationListModel.Evaluation != null)
        {
            await navigationService.GoToAsync("//evaluations/detail", new Dictionary<string, object?>()
            {
                {nameof(EvaluationDetailViewModel.Id), activityEvaluationListModel.Evaluation.Id},
            });
        }
    }
    
    [RelayCommand]
    private async Task GoToCourseDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("//courses/detail", new Dictionary<string, object?>()
        {
            [nameof(CourseDetailViewModel.Id)] = id
        });
    }
    
    public async void Receive(MessageEditStudent message)
    {
        //Performs this code after receiving edit student message.
        if (Student?.Id == message.StudentId)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageAddActivity message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageDeleteCourse message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }
    
    public async void Receive(MessageEditCourse message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageDeleteActivity message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageAddEvaluation message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageDeleteEvaluation message)
    {
        if (Student != null)
        {
            await LoadDataAsync();
        }
    }
}