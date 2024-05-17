using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
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
    IMessengerService messengerService
) : ViewModelBase(messengerService), IRecipient<MessageEditStudent>
{
    public Guid Id { get; set; }
    
    [ObservableProperty]
    private StudentDetailModel? student;
    
    [ObservableProperty]
    private ObservableCollection<CourseListModel> courses = [];

    protected override async Task LoadDataAsync()
    {
        Student = await studentFacade.GetAsync(Id);
        Courses = Student.Courses;
    }
    
    [RelayCommand]
    private async Task FilterCoursesAsync(string filterText)
    {
        Courses = new ObservableCollection<CourseListModel>(await courseFacade
            .GetStudentsCoursesAsync(Student!.Id, filterText));
    }
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit", new Dictionary<string, object?>()
        {
            [nameof(StudentEditViewModel.Student)] = Student
        });
    }

    public async void Receive(MessageEditStudent message)
    {
        //Performs this code after receiving edit student message.
        await LoadDataAsync();
    }
}