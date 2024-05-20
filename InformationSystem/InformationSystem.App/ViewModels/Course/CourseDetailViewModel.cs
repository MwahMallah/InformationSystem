using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Activity;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Course;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CourseDetailViewModel(
    IStudentFacade studentFacade,
    ICourseFacade courseFacade,
    ICourseModelMapper modelMapper,
    INavigationService navigationService,
    IMessengerService messengerService,
    IAlertService alertService
) : ViewModelBase(messengerService), IRecipient<MessageDeleteStudent>, 
    IRecipient<MessageEditStudent>, IRecipient<MessageAddActivity>, IRecipient<MessageDeleteActivity>
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private CourseDetailModel? course;

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
    
    [RelayCommand]
    private async Task FilterStudentsAsync(string filterText)
    {
        Students = new ObservableCollection<StudentListModel>(await studentFacade
            .GetCourseStudentsAsync(Course!.Id, filterText));
    }
    
    [RelayCommand]
    private async Task GoToEditAsync()
    {
        
        await navigationService.GoToAsync("/edit", new Dictionary<string, object?>()
        {
            [nameof(ActivityEditViewModel.SelectedCourse)] = Course
        });
    }
    
    [RelayCommand]
    private async Task GoToStudentDetailAsync(Guid id)
    {
        await navigationService.GoToAsync("//students/detail", new Dictionary<string, object?>()
        {
            [nameof(StudentDetailViewModel.Id)] = id
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

    [RelayCommand]
    private async Task AddActivityAsync()
    {
        var listCourseModel = modelMapper.MapToListModel(Course);   
        await navigationService.GoToAsync("//activities/edit", new Dictionary<string, object?>()
        {
            [nameof(ActivityEditViewModel.CourseFromQuery)] = listCourseModel
        });
    }
    
    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Course != null)
        {
            var isConfirmed = await alertService
                .WaitConfirmationAsync("Delete Course", "Are you sure you want to Delete Course?");

            if (isConfirmed)
            {
                await courseFacade.DeleteAsync(Course.Id);
                Messenger.Send(new MessageDeleteCourse() {CourseId = Course.Id});
                Course = null;
                navigationService.SendBackButtonPressed();
            }
        }
    }

    public async void Receive(MessageDeleteStudent message)
    {
        if (Course != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageEditStudent message)
    {
        if (Course != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageAddActivity message)
    {
        if (Course != null)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(MessageDeleteActivity message)
    {
        if (Course != null)
        {
            await LoadDataAsync();
        }
    }
}