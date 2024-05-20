using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Course;

[QueryProperty(nameof(Course), nameof(Course))]
public partial class CourseStudentEditViewModel(
    IMessengerService messengerService,
    ICourseModelMapper courseListModelMapper,
    IStudentFacade studentFacade,
    ICourseFacade courseFacade) 
    : ViewModelBase(messengerService)
{
    [ObservableProperty] 
    private CourseDetailModel course;

    [ObservableProperty] 
    private ObservableCollection<StudentListModel> studentsToShow = [];

    public StudentListModel? SelectedStudent { get; set; } = null;
    
    protected override async Task LoadDataAsync()
    {
        var allStudents = await studentFacade.GetAsync();

        StudentsToShow.Clear();
        foreach (var student in allStudents)
        {
            if (Course.Students.Any(s => s.Id == student.Id))
            {
                continue;
            }
            StudentsToShow.Add(student);
        }
    }

    [RelayCommand]
    private async Task AddStudentToCourseAsync()
    {
        if (SelectedStudent != null)
        {
            Course.Students.Add(SelectedStudent);
            await courseFacade.SaveAsync(Course);
            MessengerService.Send(new MessageEditCourse() {CourseId = Course.Id});
            StudentsToShow.Remove(SelectedStudent);
            SelectedStudent = null;
        }
    }

    [RelayCommand]
    private async Task RemoveStudentAsync(StudentListModel modelToRemove)
    {
        Course.Students.Remove(modelToRemove);
        await courseFacade.SaveAsync(Course);
        StudentsToShow.Add(modelToRemove);
        MessengerService.Send(new MessageEditCourse() {CourseId = Course.Id});
    }
}