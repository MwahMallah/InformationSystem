using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Messages;
using InformationSystem.App.Services.Interfaces;
using InformationSystem.App.ViewModels.Student;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Models;

namespace InformationSystem.App.ViewModels.Course;

public partial class CourseListViewModel(
    ICourseFacade courseFacade,
    INavigationService navigationService,
    IMessengerService messengerService
) : ViewModelBase(messengerService), IRecipient<MessageEditCourse>
{
    private bool _isAbbreviationAscending = true;
    private bool _isCreditsAscending = true;
    private bool _isNameAscending = true;
    private bool _isMaxStudentsAscending = true;
    private string? _filterText = null;
    
    [ObservableProperty]
    private IEnumerable<CourseListModel> courses;

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Courses = await courseFacade.GetAsync();
    }
    
    [RelayCommand]
    private async Task SortAsync(string sortingQuery)
    {
        var isAscending = true;
        switch (sortingQuery)
        {
            case "name":
                _isNameAscending = !_isNameAscending;
                isAscending = _isNameAscending;
                break;
            case "abbreviation":
                _isAbbreviationAscending = !_isAbbreviationAscending;
                isAscending = _isAbbreviationAscending;
                break;
            case "credits":
                _isCreditsAscending = !_isCreditsAscending;
                isAscending = _isCreditsAscending;
                break;
            case "max_students":
                _isMaxStudentsAscending = !_isMaxStudentsAscending;
                isAscending = _isMaxStudentsAscending;
                break;
        }
        
        Courses = await courseFacade.GetAsync(searchQuery: _filterText, 
            orderQuery:sortingQuery, isAscending: isAscending);
    }

    [RelayCommand]
    private async Task FilterAsync(string filterText)
    {
        _filterText = filterText;
        Courses = await courseFacade.GetAsync(searchQuery: filterText);
    }
    
    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await navigationService.GoToAsync<CourseDetailViewModel>(
            new Dictionary<string, object?>() {[nameof(CourseDetailViewModel.Id)] = id}
        );
    }

    public async void Receive(MessageEditCourse message)
    {
        await LoadDataAsync();
    }
}