namespace LeaderAnalytics.LeaderPivot.XAML.WPF;

public class Selectable<T> : PropertyChangedBase
{
    private bool _IsSelected;
    public bool IsSelected
    { 
        get => _IsSelected;
        set => SetProp(ref _IsSelected, value);
    }

    public T Item { get; set; }

    public Selectable(T item, bool isSelected = false)
    {
        Item = item;
        IsSelected = isSelected;
    }
}
