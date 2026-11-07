using CommunityToolkit.Mvvm.Input;

public class Test
{
    public RelayCommand MyCommand { get; } = new RelayCommand(() => { });
    public RelayCommand MyCommand2 { get; } = new RelayCommand(() => { }, () => true);
}
