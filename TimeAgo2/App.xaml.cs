namespace TimeAgo2
{
  public partial class App : Application
  {
    public App()
    {
      InitializeComponent();
      Localization.Initialize();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
      return new Window(new AppShell());
    }
  }
}