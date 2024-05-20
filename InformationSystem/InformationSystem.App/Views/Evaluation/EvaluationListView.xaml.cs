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
}