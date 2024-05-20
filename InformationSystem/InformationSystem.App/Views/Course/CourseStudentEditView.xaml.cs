using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Course;
using InformationSystem.App.ViewModels.Student;

namespace InformationSystem.App.Views.Course;

public partial class CourseStudentEditView : BaseView
{
    public CourseStudentEditView(CourseStudentEditViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}