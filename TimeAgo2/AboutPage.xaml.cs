namespace TimeAgo2;

public partial class AboutPage : ContentPage
{
  public AboutPage()
  {
    InitializeComponent();
    ApplyTranslations();
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();
    ApplyTranslations();
  }

  private static async void OnBackClicked(object? sender, EventArgs e)
  {
    await Shell.Current.GoToAsync("..");
  }

  private void ApplyTranslations()
  {
    Title = Localization.Get("about");
    TitleLabel.Text = Localization.Get("about_title");
    SubtitleLabel.Text = Localization.Get("about_subtitle");
    DescriptionLabel.Text = Localization.Get("about_description");
    VersionLabel.Text = $@"{Localization.Get("about_version")} :";
    VersionValueLabel.Text = AppInfo.Current.VersionString;
    AuthorLabel.Text = $@"{Localization.Get("about_author")} :";
    ContactLabel.Text = $@"{Localization.Get("about_contact")} :";
    WebsiteLabel.Text = $@"{Localization.Get("about_website")} :";
    BackButton.Text = Localization.Get("back_to_settings");
  }

  private async void OnWebsiteTapped(object? sender, TappedEventArgs e)
  {
    await Launcher.Default.OpenAsync("https://github.com/fredatgithub/TimeAgo2");
  }
}
