using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.App.ViewModels.Student;

public class StudentListViewModel(
    IStudentFacade studentFacade
    ) : ViewModelBase
{
    public IEnumerable<StudentListModel> Students { get; set; } = null!;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Students = await studentFacade.GetAsync();
    }
}