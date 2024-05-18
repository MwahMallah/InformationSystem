using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Activity;

namespace InformationSystem.App.Views.Activity;

public partial class ActivityDetailView
{
    public ActivityDetailView(ActivityDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}