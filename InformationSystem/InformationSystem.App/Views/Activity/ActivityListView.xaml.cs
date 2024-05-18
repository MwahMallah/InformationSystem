using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Activity;

namespace InformationSystem.App.Views.Activity;

public partial class ActivityListView
{
    public ActivityListView(ActivityListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var activityListViewModel = ViewModel as ActivityListViewModel;
        activityListViewModel?.FilterCommand.Execute(e.NewTextValue);
    }
}