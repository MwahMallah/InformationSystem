using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels.Evaluation;

namespace InformationSystem.App.Views.Evaluation;

public partial class EvaluationListView
{
    public EvaluationListView(EvaluationListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var evaluationListViewModel = ViewModel as EvaluationListViewModel;
        evaluationListViewModel?.FilterCommand.Execute(e.NewTextValue);
    }
}