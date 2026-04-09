using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ZdaszToApp.Services;

namespace ZdaszToApp.ViewModels;

public class RankingEntry : INotifyPropertyChanged
{
    private int _rank;
    private string _nickname = string.Empty;
    private int _points;
    private string _avatarUrl = string.Empty;

    public int Rank
    {
        get => _rank;
        set { _rank = value; OnPropertyChanged(); }
    }

    public string Nickname
    {
        get => _nickname;
        set { _nickname = value; OnPropertyChanged(); }
    }

    public int Points
    {
        get => _points;
        set { _points = value; OnPropertyChanged(); }
    }

    public string AvatarUrl
    {
        get => _avatarUrl;
        set { _avatarUrl = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RankingViewModel : INotifyPropertyChanged
{
    private ObservableCollection<RankingEntry> _entries = new();

    public ObservableCollection<RankingEntry> Entries
    {
        get => _entries;
        set { _entries = value; OnPropertyChanged(); }
    }

    public RankingViewModel()
    {
    }

    public async Task LoadRankingAsync()
    {
        var items = await ApiService.Instance.GetTop100Async();
        
        Entries.Clear();
        
        for (int i = 0; i < Math.Min(items.Count, 30); i++)
        {
            var item = items[i];
            Entries.Add(new RankingEntry
            {
                Rank = item.Position,
                Nickname = item.Username,
                Points = (int)(item.Score ?? 0),
                AvatarUrl = ""
            });
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}