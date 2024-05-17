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
    private ObservableCollection<CourseListModel> courses = [];

    public CourseListModel? SelectedCourse { get; set; } = null;
    
    protected override async Task LoadDataAsync()
    {
        var allCourses = await courseFacade.GetAsync();

        Courses.Clear();
        foreach (var course in allCourses)
        {
            Courses.Add(course);
        }
    }

    [RelayCommand]
    private async Task AddCourseToStudentAsync()
    {
        if (SelectedCourse != null)
        {
            var courseListModel = courseListModelMapper
                .MapToListModel(await courseFacade.GetAsync(SelectedCourse.Id));
            
            Student.Courses.Add(courseListModel);
            await studentFacade.SaveAsync(Student);
        }
    }
}