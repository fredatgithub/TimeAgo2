namespace TimeAgo2;

public partial class SettingsPage : ContentPage
{
  public SettingsPage()
  {
    InitializeComponent();
    LanguagePicker.ItemsSource = Localization.Languages.Select(language => language.Name).ToList();
    ThemePicker.ItemsSource = new[]
    {
            Localization.Get("theme_light"),
            Localization.Get("theme_dark")
        };
    ApplyTranslations();
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();
    LanguagePicker.SelectedIndex = Localization.Languages
        .ToList()
        .FindIndex(language => language.Code == Localization.CurrentLanguage);
    ThemePicker.SelectedIndex = ThemeManager.CurrentTheme == AppTheme.Dark ? 1 : 0;
    ApplyTranslations();
  }

  private async void OnSaveClicked(object? sender, EventArgs e)
  {
    var selectedIndex = LanguagePicker.SelectedIndex;
    if (selectedIndex < 0 || selectedIndex >= Localization.Languages.Count)
    {
      await DisplayAlert(
          Localization.Get("choose_language_title"),
          Localization.Get("choose_language_message"),
          Localization.Get("ok"));
      return;
    }

    Localization.SetLanguage(Localization.Languages[selectedIndex].Code);
    ThemeManager.SetTheme(ThemePicker.SelectedIndex == 1 ? "dark" : "light");
    ApplyTranslations();
    await Shell.Current.GoToAsync("..");
  }

  private async void OnAboutClicked(object? sender, EventArgs e)
  {
    await Shell.Current.GoToAsync(nameof(AboutPage));
  }

  private void ApplyTranslations()
  {
    Title = Localization.Get("settings_title");
    PageTitleLabel.Text = Localization.Get("settings_title");
    PageSubtitleLabel.Text = Localization.Get("settings_subtitle");
    LanguageLabel.Text = Localization.Get("language");
    LanguagePicker.Title = Localization.Get("language");
    ThemeLabel.Text = Localization.Get("theme");
    ThemePicker.Title = Localization.Get("theme");
    ThemePicker.ItemsSource = new[]
    {
            Localization.Get("theme_light"),
            Localization.Get("theme_dark")
        };
    SaveButton.Text = Localization.Get("save_settings");
    AboutButton.Text = Localization.Get("about");
  }
}
