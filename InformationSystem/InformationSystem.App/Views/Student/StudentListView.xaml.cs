using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels;

namespace InformationSystem.App.Views.Student;

public partial class StudentListView : BaseView
{
    public StudentListView(IViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}