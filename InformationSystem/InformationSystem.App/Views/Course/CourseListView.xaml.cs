using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Course;

namespace InformationSystem.App.Views.Course;

public partial class CourseListView
{
    public CourseListView(CourseListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var courseViewModel = ViewModel as CourseListViewModel;
        courseViewModel?.FilterCommand.Execute(e.NewTextValue);
    }
}