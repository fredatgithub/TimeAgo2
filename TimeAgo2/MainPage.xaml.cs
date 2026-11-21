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

    private readonly ObservableCollection<ActivityEvent> _events = [];
    private readonly string _activitiesFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "activities.json");
    private ActivityEvent? _selectedEvent;
    private bool _hasLoaded;

    public MainPage()
    {
        InitializeComponent();

        EventsView.ItemsSource = _events;
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

    private async void OnMainActionClicked(object? sender, EventArgs e)
    {
        if (_selectedEvent is not null)
        {
            await AddInstanceAsync(_selectedEvent, DateTimeOffset.Now);
            return;
        }

        ActivityNameEntry.Text = string.Empty;
        ActivityDatePicker.Date = DateTime.Today;
        ActivityTimePicker.Time = DateTime.Now.TimeOfDay;
        AddEventForm.IsVisible = true;
        MainActionButton.IsVisible = false;
        ActivityNameEntry.Focus();
    }

    private async void OnCreateEventClicked(object? sender, EventArgs e)
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

        if (!await CreateEventOrAddInstanceAsync(name, new DateTimeOffset(localDateTime)))
        {
            return;
        }

        ActivityNameEntry.Text = string.Empty;
        AddEventForm.IsVisible = false;
        if (_selectedEvent is null)
        {
            MainActionButton.IsVisible = true;
        }
    }

    private void OnCancelAddEventClicked(object? sender, EventArgs e)
    {
        AddEventForm.IsVisible = false;
        MainActionButton.IsVisible = true;
    }

    private async Task<bool> CreateEventOrAddInstanceAsync(string name, DateTimeOffset createdAt)
    {
        var existingEvent = _events.FirstOrDefault(activity =>
            string.Equals(activity.Name.Trim(), name, StringComparison.OrdinalIgnoreCase));
        if (existingEvent is not null)
        {
            if (!await AddInstanceAsync(existingEvent, createdAt))
            {
                return false;
            }

            ShowEventDetails(existingEvent);
            return true;
        }

        var activityEvent = new ActivityEvent
        {
            Name = name,
            Instances =
            [
                new ActivityInstance { CreatedAt = createdAt }
            ]
        };
        activityEvent.UpdateDisplay(DateTimeOffset.Now);
        InsertEventSorted(activityEvent);

        if (await SaveActivitiesAsync())
        {
            return true;
        }

        _events.Remove(activityEvent);
        return false;
    }

    private async void OnAddInstanceClicked(object? sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: ActivityEvent activityEvent })
        {
            await AddInstanceAsync(activityEvent, DateTimeOffset.Now);
        }
    }

    private async Task<bool> AddInstanceAsync(ActivityEvent activityEvent, DateTimeOffset createdAt)
    {
        var instance = new ActivityInstance { CreatedAt = createdAt };
        instance.UpdateDisplay(DateTimeOffset.Now);
        activityEvent.Instances.Insert(0, instance);
        activityEvent.UpdateDisplay(DateTimeOffset.Now);

        if (await SaveActivitiesAsync())
        {
            RefreshInstancesView(activityEvent);
            return true;
        }

        activityEvent.Instances.Remove(instance);
        activityEvent.UpdateDisplay(DateTimeOffset.Now);
        RefreshInstancesView(activityEvent);
        return false;
    }

    private void OnEventTapped(object? sender, TappedEventArgs e)
    {
        if (sender is TapGestureRecognizer { CommandParameter: ActivityEvent activityEvent })
        {
            ShowEventDetails(activityEvent);
        }
    }

    private void OnBackClicked(object? sender, EventArgs e)
    {
        _selectedEvent = null;
        ShowEventList();
    }

    private async void OnDeleteEventClicked(object? sender, EventArgs e)
    {
        if (_selectedEvent is not { } activityEvent)
        {
            return;
        }

        var confirmed = await DisplayAlert(
            Localization.Get("delete_event_title"),
            Localization.Format("delete_event_message", activityEvent.Name),
            Localization.Get("confirm_delete"),
            Localization.Get("cancel"));
        if (!confirmed)
        {
            return;
        }

        var index = _events.IndexOf(activityEvent);
        if (index < 0)
        {
            return;
        }

        _events.RemoveAt(index);
        _selectedEvent = null;
        ShowEventList();
        if (!await SaveActivitiesAsync())
        {
            _events.Insert(index, activityEvent);
            _selectedEvent = activityEvent;
            ShowEventDetails(activityEvent);
        }
    }

    private async void OnDeleteInstanceClicked(object? sender, EventArgs e)
    {
        if (_selectedEvent is not { } activityEvent ||
            sender is not Button { CommandParameter: ActivityInstance instance })
        {
            return;
        }

        var isLastInstance = activityEvent.Instances.Count == 1;
        var messageKey = isLastInstance
            ? "delete_last_instance_message"
            : "delete_instance_message";
        var confirmed = await DisplayAlert(
            Localization.Get("delete_instance_title"),
            Localization.Format(messageKey, activityEvent.Name),
            Localization.Get("confirm_delete"),
            Localization.Get("cancel"));
        if (!confirmed)
        {
            return;
        }

        if (isLastInstance)
        {
            var eventIndex = _events.IndexOf(activityEvent);
            if (eventIndex < 0)
            {
                return;
            }

            _events.RemoveAt(eventIndex);
            _selectedEvent = null;
            ShowEventList();
            if (!await SaveActivitiesAsync())
            {
                _events.Insert(eventIndex, activityEvent);
                _selectedEvent = activityEvent;
                ShowEventDetails(activityEvent);
            }

            return;
        }

        var instanceIndex = activityEvent.Instances.IndexOf(instance);
        if (instanceIndex < 0)
        {
            return;
        }

        activityEvent.Instances.RemoveAt(instanceIndex);
        activityEvent.UpdateDisplay(DateTimeOffset.Now);
        RefreshInstancesView(activityEvent);
        if (!await SaveActivitiesAsync())
        {
            activityEvent.Instances.Insert(instanceIndex, instance);
            activityEvent.UpdateDisplay(DateTimeOffset.Now);
            RefreshInstancesView(activityEvent);
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
            using var document = JsonDocument.Parse(json);
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new JsonException("The activities file must contain a JSON array.");
            }

            var records = document.RootElement;
            var isCurrentFormat = records.GetArrayLength() > 0 &&
                records[0].ValueKind == JsonValueKind.Object &&
                records[0].TryGetProperty(nameof(ActivityEvent.Instances), out _);
            var wasMigrated = false;
            List<ActivityEvent> loadedEvents;

            if (isCurrentFormat)
            {
                var deserializedEvents =
                    JsonSerializer.Deserialize<List<ActivityEvent?>>(json, JsonOptions) ?? [];
                if (deserializedEvents.Any(activityEvent => activityEvent is null))
                {
                    throw new JsonException("The activities file contains an empty event.");
                }

                loadedEvents = deserializedEvents.Select(activityEvent => activityEvent!).ToList();
                ValidateEvents(loadedEvents);
            }
            else
            {
                loadedEvents = MigrateLegacyActivities(json);
                wasMigrated = true;
            }

            foreach (var activityEvent in loadedEvents
                         .OrderBy(item => item.Name, StringComparer.CurrentCultureIgnoreCase))
            {
                activityEvent.UpdateDisplay(DateTimeOffset.Now);
                _events.Add(activityEvent);
            }

            if (wasMigrated)
            {
                await SaveActivitiesAsync();
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

    private static List<ActivityEvent> MigrateLegacyActivities(string json)
    {
        var activities = JsonSerializer.Deserialize<List<LegacyActivityRecord?>>(json, JsonOptions) ?? [];
        if (activities.Any(activity =>
                activity is null ||
                string.IsNullOrWhiteSpace(activity.Name) ||
                activity.CreatedAt == default))
        {
            throw new JsonException("An activity is missing its name or date.");
        }

        return activities
            .Select(activity => activity!)
            .GroupBy(activity => activity.Name.Trim(), StringComparer.OrdinalIgnoreCase)
            .Select(group =>
            {
                var name = group
                    .OrderByDescending(activity => activity.CreatedAt)
                    .First()
                    .Name
                    .Trim();
                var instances = group
                    .OrderByDescending(activity => activity.CreatedAt)
                    .Select(activity => new ActivityInstance
                    {
                        Id = activity.Id == Guid.Empty ? Guid.NewGuid() : activity.Id,
                        CreatedAt = activity.CreatedAt
                    });
                var migratedEvent = new ActivityEvent
                {
                    Name = name,
                    Instances = new ObservableCollection<ActivityInstance>(instances)
                };
                migratedEvent.UpdateDisplay(DateTimeOffset.Now);
                return migratedEvent;
            })
            .ToList();
    }

    private static void ValidateEvents(IEnumerable<ActivityEvent> activityEvents)
    {
        foreach (var activityEvent in activityEvents)
        {
            if (string.IsNullOrWhiteSpace(activityEvent.Name) ||
                activityEvent.Instances is null ||
                activityEvent.Instances.Count == 0 ||
                activityEvent.Instances.Any(instance =>
                    instance is null || instance.CreatedAt == default))
            {
                throw new JsonException("An event is missing its name or occurrence data.");
            }

            if (activityEvent.Id == Guid.Empty)
            {
                activityEvent.Id = Guid.NewGuid();
            }

            foreach (var instance in activityEvent.Instances)
            {
                if (instance.Id == Guid.Empty)
                {
                    instance.Id = Guid.NewGuid();
                }
            }
        }
    }

    private async Task<bool> SaveActivitiesAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_events, JsonOptions);
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

    private void InsertEventSorted(ActivityEvent activityEvent)
    {
        for (var index = 0; index < _events.Count; index++)
        {
            if (string.Compare(
                    _events[index].Name,
                    activityEvent.Name,
                    ignoreCase: true,
                    culture: Localization.CurrentCulture) > 0)
            {
                _events.Insert(index, activityEvent);
                return;
            }
        }

        _events.Add(activityEvent);
    }

    private void ShowEventDetails(ActivityEvent activityEvent)
    {
        _selectedEvent = activityEvent;
        var now = DateTimeOffset.Now;
        activityEvent.UpdateDisplay(now);
        foreach (var instance in activityEvent.Instances)
        {
            instance.UpdateDisplay(now);
        }

        AddEventForm.IsVisible = false;
        MainActionButton.IsVisible = true;
        EventsView.IsVisible = false;
        InstancesView.IsVisible = true;
        InstancesView.ItemsSource = activityEvent.Instances
            .OrderByDescending(instance => instance.CreatedAt)
            .ToList();
        HeaderTaglineLabel.IsVisible = false;
        HeaderTitleLabel.Text = activityEvent.Name;
        HeaderSubtitleLabel.Text = Localization.Get("instance_history");
        BackButton.IsVisible = true;
        SettingsButton.IsVisible = false;
        DeleteEventButton.IsVisible = true;
        MainActionButton.Text = $"+ {Localization.Get("add_instance")}";
        SemanticProperties.SetDescription(BackButton, Localization.Get("back"));
        SemanticProperties.SetDescription(DeleteEventButton, Localization.Get("delete_event"));
    }

    private void ShowEventList()
    {
        _selectedEvent = null;
        AddEventForm.IsVisible = false;
        MainActionButton.IsVisible = true;
        EventsView.IsVisible = true;
        InstancesView.IsVisible = false;
        HeaderTaglineLabel.IsVisible = true;
        HeaderTitleLabel.Text = Localization.Get("main_title");
        HeaderSubtitleLabel.Text = Localization.Get("main_subtitle");
        BackButton.IsVisible = false;
        SettingsButton.IsVisible = true;
        DeleteEventButton.IsVisible = false;
        MainActionButton.Text = $"+ {Localization.Get("add_event")}";
    }

    private void RefreshInstancesView(ActivityEvent activityEvent)
    {
        if (_selectedEvent == activityEvent)
        {
            InstancesView.ItemsSource = activityEvent.Instances
                .OrderByDescending(instance => instance.CreatedAt)
                .ToList();
        }
    }

    private void UpdateElapsedTimes()
    {
        var now = DateTimeOffset.Now;
        foreach (var activityEvent in _events)
        {
            activityEvent.UpdateDisplay(now);
        }

        if (_selectedEvent is { } selectedEvent)
        {
            foreach (var instance in selectedEvent.Instances)
            {
                instance.UpdateDisplay(now);
            }
        }
    }

    private void ApplyTranslations()
    {
        Title = Localization.Get("main_title");
        HeaderTaglineLabel.Text = Localization.Get("tagline");
        DateLabel.Text = Localization.Get("date");
        TimeLabel.Text = Localization.Get("time");
        ActivityNameEntry.Placeholder = Localization.Get("activity_placeholder");
        SaveEventButton.Text = Localization.Get("create_event");
        CancelAddEventButton.Text = Localization.Get("cancel");
        EmptyTitleLabel.Text = Localization.Get("empty_title");
        EmptySubtitleLabel.Text = Localization.Get("empty_subtitle");
        EmptyInstancesLabel.Text = Localization.Get("empty_instances");
        SemanticProperties.SetDescription(SettingsButton, Localization.Get("settings_button"));
        UpdateElapsedTimes();

        if (_selectedEvent is null)
        {
            ShowEventList();
        }
        else
        {
            ShowEventDetails(_selectedEvent);
        }
    }
}

public sealed class ActivityEvent : INotifyPropertyChanged
{
    private string _elapsedText = string.Empty;
    private string _addInstanceText = string.Empty;

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public ObservableCollection<ActivityInstance> Instances { get; set; } = [];

    [JsonIgnore]
    public string AddInstanceText
    {
        get => _addInstanceText;
        private set
        {
            if (_addInstanceText == value)
            {
                return;
            }

            _addInstanceText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddInstanceText)));
        }
    }

    [JsonIgnore]
    public string ElapsedText
    {
        get => _elapsedText;
        private set
        {
            if (_elapsedText == value)
            {
                return;
            }

            _elapsedText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ElapsedText)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void UpdateDisplay(DateTimeOffset now)
    {
        var latestInstance = Instances
            .OrderByDescending(instance => instance.CreatedAt)
            .FirstOrDefault();
        latestInstance?.UpdateDisplay(now);
        ElapsedText = latestInstance is null
            ? string.Empty
            : Localization.FormatElapsed(latestInstance.CreatedAt, now);
        AddInstanceText = Localization.Get("add_instance");
    }
}

public sealed class ActivityInstance : INotifyPropertyChanged
{
    private string _elapsedText = string.Empty;
    private string _createdAtText = string.Empty;
    private string _deleteText = string.Empty;

    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; }

    [JsonIgnore]
    public string ElapsedText
    {
        get => _elapsedText;
        private set => SetProperty(ref _elapsedText, value);
    }

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

    public event PropertyChangedEventHandler? PropertyChanged;

    public void UpdateDisplay(DateTimeOffset now)
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

public sealed class LegacyActivityRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}
