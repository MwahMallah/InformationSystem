﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Student;

namespace InformationSystem.App.Views.Student;

public partial class StudentEditView : BaseView
{
    public StudentEditView(StudentEditViewModel studentEditViewModel) : base(studentEditViewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var studentEditViewModel = ViewModel as StudentEditViewModel;
        studentEditViewModel?.UpdateCommand.Execute(e.NewTextValue);
    }
}