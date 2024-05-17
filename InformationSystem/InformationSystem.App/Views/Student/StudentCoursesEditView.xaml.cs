using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Student;

namespace InformationSystem.App.Views.Student;

public partial class StudentCoursesEditView
{
    public StudentCoursesEditView(StudentCoursesEditViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}