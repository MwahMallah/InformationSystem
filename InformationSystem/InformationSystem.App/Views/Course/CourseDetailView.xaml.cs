using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Course;

namespace InformationSystem.App.Views.Course;

public partial class CourseDetailView
{
    public CourseDetailView(CourseDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var courseDetailViewModel = (CourseDetailViewModel) ViewModel;
        courseDetailViewModel?.FilterStudentsCommand.Execute(e.NewTextValue);
    }
}