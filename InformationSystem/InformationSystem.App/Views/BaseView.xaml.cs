using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.App.ViewModels;

namespace InformationSystem.App.Views;

public partial class BaseView : ContentPage
{
    protected IViewModel ViewModel { get; }
    
    public BaseView(IViewModel viewModel)
    {
        BindingContext = ViewModel = viewModel;
        
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await ViewModel.OnAppearingAsync();
    }
}