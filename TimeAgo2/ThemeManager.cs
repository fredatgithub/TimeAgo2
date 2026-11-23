namespace TimeAgo2;

public static class ThemeManager
{
  private const string ThemePreferenceKey = "app_theme";

  public static AppTheme CurrentTheme { get; private set; } = AppTheme.Light;

  public static void Initialize()
  {
    var savedTheme = Preferences.Default.Get(ThemePreferenceKey, "light");
    SetTheme(savedTheme);
  }

  public static void SetTheme(string themeName)
  {
    var theme = themeName == "dark" ? AppTheme.Dark : AppTheme.Light;
    CurrentTheme = theme;
    Preferences.Default.Set(ThemePreferenceKey, themeName == "dark" ? "dark" : "light");

    if (Application.Current is not null)
    {
      Application.Current.UserAppTheme = theme;
    }
  }
}
