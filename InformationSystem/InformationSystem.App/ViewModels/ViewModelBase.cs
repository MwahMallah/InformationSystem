﻿using CommunityToolkit.Mvvm.ComponentModel;
using InformationSystem.App.Services.Interfaces;

namespace InformationSystem.App.ViewModels;

public abstract class ViewModelBase: ObservableRecipient, IViewModel
{
    private bool _isRefreshRequired = true;
    protected readonly IMessengerService MessengerService;

    protected ViewModelBase(IMessengerService messengerService)
        : base(messengerService.Messenger)
    {
        MessengerService = messengerService;
        
        //Enables message receiving from messenger service 
        IsActive = true;
    }
    
    public async Task OnAppearingAsync()
    {
        if (_isRefreshRequired)
        {
            await LoadDataAsync();

            _isRefreshRequired = false;
        }
    }

    protected virtual Task LoadDataAsync()
    {
        return Task.CompletedTask;
    }
}