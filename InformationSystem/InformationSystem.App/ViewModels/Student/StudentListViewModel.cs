using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Student;

public partial class StudentListViewModel(
    IStudentFacade studentFacade,
    INavigationService navigationService,
    IMessengerService messengerService
    ) : ViewModelBase(messengerService), IRecipient<MessageEditStudent>, IRecipient<MessageDeleteStudent>
{
    private bool _isGroupAscending = true;
    private bool _isCurrentYearAscending = true;
    private bool _isNameAscending = true;
    private string? _filterText = null;
    
    [ObservableProperty]
    private IEnumerable<StudentListModel> students;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Students = await studentFacade.GetAsync();
    }

    [RelayCommand]
    private async Task SortAsync(string sortingQuery)
    {
        var isAscending = true;
        switch (sortingQuery)
        {
            case "group":
                _isGroupAscending = !_isGroupAscending;
                isAscending = _isGroupAscending;
                break;
            case "current_year":
                _isCurrentYearAscending = !_isCurrentYearAscending;
                isAscending = _isCurrentYearAscending;
                break;
            case "name":
                _isNameAscending = !_isNameAscending;
                isAscending = _isNameAscending;
                break;
        }
        
        Students = await studentFacade.GetAsync(searchQuery: _filterText, 
            orderQuery:sortingQuery, isAscending: isAscending);
    }

    [RelayCommand]
    private async Task FilterAsync(string filterText)
    {
        _filterText = filterText;
        Students = await studentFacade.GetAsync(searchQuery: filterText);
    }
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<StudentDetailViewModel>(
                new Dictionary<string, object?>() {[nameof(StudentDetailViewModel.Id)] = id}
            );
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    public async void Receive(MessageEditStudent message)
    {
        //Performs this code after receiving edit student message.
        await LoadDataAsync();
    }

    public async void Receive(MessageDeleteStudent message)
    {
        await LoadDataAsync();
    }
}