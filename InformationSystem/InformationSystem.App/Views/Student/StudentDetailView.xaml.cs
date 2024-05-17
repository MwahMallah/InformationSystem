using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Student;

namespace InformationSystem.App.Views.Student;

public partial class StudentDetailView
{
    public StudentDetailView(StudentDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var studentViewModel = ViewModel as StudentDetailViewModel;
        studentViewModel?.FilterCoursesCommand.Execute(e.NewTextValue);
    }
}