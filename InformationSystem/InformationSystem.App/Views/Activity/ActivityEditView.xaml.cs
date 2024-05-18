using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Activity;

namespace InformationSystem.App.Views.Activity;

public partial class ActivityEditView
{
    public ActivityEditView(ActivityEditViewModel editViewModel) : base(editViewModel)
    {
        InitializeComponent();
    }
}