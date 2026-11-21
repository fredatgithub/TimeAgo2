using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TimeAgo2;

public partial class MainPage : ContentPage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly ObservableCollection<ActivityRecord> _activities = [];
    private readonly string _activitiesFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "activities.json");
    private bool _hasLoaded;

    public MainPage()
    {
        InitializeComponent();

        ActivitiesView.ItemsSource = _activities;
        ActivityDatePicker.MaximumDate = DateTime.Today;
        ActivityTimePicker.Time = DateTime.Now.TimeOfDay;

        var timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (_, _) => UpdateElapsedTimes();
        timer.Start();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ApplyTranslations();
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (_hasLoaded)
        {
            return;
        }

        _hasLoaded = true;
        await LoadActivitiesAsync();
    }

    private async void OnAddActivityClicked(object? sender, EventArgs e)
    {
        var name = ActivityNameEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert(
                Localization.Get("missing_name_title"),
                Localization.Get("missing_name_message"),
                Localization.Get("ok"));
            return;
        }

        var localDateTime = ActivityDatePicker.Date.Date + ActivityTimePicker.Time;
        if (localDateTime > DateTime.Now)
        {
            await DisplayAlert(
                Localization.Get("invalid_date_title"),
                Localization.Get("invalid_date_message"),
                Localization.Get("ok"));
            return;
        }

        var activity = new ActivityRecord
        {
            Name = name,
            CreatedAt = new DateTimeOffset(localDateTime)
        };
        activity.UpdateElapsed(DateTimeOffset.Now);
        _activities.Insert(0, activity);

        if (!await SaveActivitiesAsync())
        {
            _activities.Remove(activity);
            return;
        }

        ActivityNameEntry.Text = string.Empty;
        ActivityDatePicker.Date = DateTime.Today;
        ActivityTimePicker.Time = DateTime.Now.TimeOfDay;
    }

    private async void OnDeleteActivityClicked(object? sender, EventArgs e)
    {
        if (sender is not Button { CommandParameter: ActivityRecord activity })
        {
            return;
        }

        var confirmed = await DisplayAlert(
            Localization.Get("delete_title"),
            Localization.Format("delete_message", activity.Name),
            Localization.Get("confirm_delete"),
            Localization.Get("cancel"));
        if (!confirmed)
        {
            return;
        }

        var index = _activities.IndexOf(activity);
        if (index < 0)
        {
            return;
        }

        _activities.RemoveAt(index);
        if (!await SaveActivitiesAsync())
        {
            _activities.Insert(index, activity);
        }
    }

    private async void OnSettingsClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }

    private async Task LoadActivitiesAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(_activitiesFilePath);
            var savedActivities = JsonSerializer.Deserialize<List<ActivityRecord>>(json, JsonOptions);
            if (savedActivities is null)
            {
                return;
            }

            foreach (var activity in savedActivities.OrderByDescending(item => item.CreatedAt))
            {
                activity.UpdateElapsed(DateTimeOffset.Now);
                _activities.Add(activity);
            }
        }
        catch (FileNotFoundException)
        {
            return;
        }
        catch (JsonException exception)
        {
            await DisplayAlert(
                Localization.Get("invalid_json_title"),
                Localization.Format("invalid_json_message", exception.Message),
                Localization.Get("ok"));
        }
        catch (IOException exception)
        {
            await DisplayAlert(
                Localization.Get("read_error_title"),
                Localization.Format("read_error_message", exception.Message),
                Localization.Get("ok"));
        }
        catch (UnauthorizedAccessException exception)
        {
            await DisplayAlert(
                Localization.Get("access_denied_title"),
                Localization.Format("read_access_denied_message", exception.Message),
                Localization.Get("ok"));
        }
    }

    private async Task<bool> SaveActivitiesAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_activities, JsonOptions);
            await File.WriteAllTextAsync(_activitiesFilePath, json);
            return true;
        }
        catch (IOException exception)
        {
            await DisplayAlert(
                Localization.Get("save_error_title"),
                Localization.Format("save_access_denied_message", exception.Message),
                Localization.Get("ok"));
        }
        catch (UnauthorizedAccessException exception)
        {
            await DisplayAlert(
                Localization.Get("access_denied_title"),
                Localization.Format("save_access_denied_message", exception.Message),
                Localization.Get("ok"));
        }

        return false;
    }

    private void UpdateElapsedTimes()
    {
        var now = DateTimeOffset.Now;
        foreach (var activity in _activities)
        {
            activity.UpdateElapsed(now);
        }
    }

    private void ApplyTranslations()
    {
        Title = Localization.Get("main_title");
        HeaderTaglineLabel.Text = Localization.Get("tagline");
        HeaderTitleLabel.Text = Localization.Get("main_title");
        HeaderSubtitleLabel.Text = Localization.Get("main_subtitle");
        ActivityNameEntry.Placeholder = Localization.Get("activity_placeholder");
        DateLabel.Text = Localization.Get("date");
        TimeLabel.Text = Localization.Get("time");
        AddActivityButton.Text = Localization.Get("add_activity");
        EmptyTitleLabel.Text = Localization.Get("empty_title");
        EmptySubtitleLabel.Text = Localization.Get("empty_subtitle");
        SemanticProperties.SetDescription(SettingsButton, Localization.Get("settings_button"));
        UpdateElapsedTimes();
    }
}

public sealed class ActivityRecord : INotifyPropertyChanged
{
    private string _elapsedText = string.Empty;
    private string _createdAtText = string.Empty;
    private string _deleteText = string.Empty;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public string CreatedAtText
    {
        get => _createdAtText;
        private set => SetProperty(ref _createdAtText, value);
    }

    [JsonIgnore]
    public string DeleteText
    {
        get => _deleteText;
        private set => SetProperty(ref _deleteText, value);
    }

    [JsonIgnore]
    public string ElapsedText
    {
        get => _elapsedText;
        private set => SetProperty(ref _elapsedText, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void UpdateElapsed(DateTimeOffset now)
    {
        ElapsedText = Localization.FormatElapsed(CreatedAt, now);
        CreatedAtText = CreatedAt.ToLocalTime().ToString("g", Localization.CurrentCulture);
        DeleteText = Localization.Get("delete");
    }

    private void SetProperty(ref string property, string value, [System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
    {
        if (property == value)
        {
            return;
        }

        property = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
