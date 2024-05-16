using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.BL.Models;

namespace InformationSystem.App.Views.Student;

public partial class StudentListView
{
    public StudentListView(StudentListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var studentViewModel = ViewModel as StudentListViewModel;
        studentViewModel?.FilterCommand.Execute(e.NewTextValue);
    }
}