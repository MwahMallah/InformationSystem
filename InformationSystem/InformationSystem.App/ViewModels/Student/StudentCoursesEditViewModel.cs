using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Student;

[QueryProperty(nameof(Student), nameof(Student))]
public partial class StudentCoursesEditViewModel
    (IMessengerService messengerService,
        ICourseModelMapper courseListModelMapper,
        IStudentFacade studentFacade,
        ICourseFacade courseFacade) 
    : ViewModelBase(messengerService)
{
    [ObservableProperty] 
    private StudentDetailModel student;

    [ObservableProperty] 
    private ObservableCollection<CourseListModel> coursesToShow = [];

    public CourseListModel? SelectedCourse { get; set; } = null;
    
    protected override async Task LoadDataAsync()
    {
        var allCourses = await courseFacade.GetAsync();

        CoursesToShow.Clear();
        foreach (var course in allCourses)
        {
            if (Student.Courses.Any(c => c.Id == course.Id))
            {
                continue;
            }
            CoursesToShow.Add(course);
        }
    }

    [RelayCommand]
    private async Task AddCourseToStudentAsync()
    {
        if (SelectedCourse != null)
        {
            Student.Courses.Add(SelectedCourse);
            await studentFacade.SaveAsync(Student);
            CoursesToShow.Remove(SelectedCourse);
            SelectedCourse = null;
        }
    }

    [RelayCommand]
    private async Task RemoveCourseAsync(CourseListModel modelToRemove)
    {
        Student.Courses.Remove(modelToRemove);
        await studentFacade.SaveAsync(Student);
        CoursesToShow.Add(modelToRemove);
    }
}