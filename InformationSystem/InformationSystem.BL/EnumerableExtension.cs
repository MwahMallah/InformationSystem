using System.Collections.ObjectModel;

namespace InformationSystem.BL;

public static class EnumerableExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new ObservableCollection<T>(values);
}