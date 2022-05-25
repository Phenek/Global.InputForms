using Sample.Styles;

namespace Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        Resources.MergedDictionaries.Add(new ButtonStyles());
        Resources.MergedDictionaries.Add(new CheckStyles());
        Resources.MergedDictionaries.Add(new EntryStyles());

        MainPage = new SplashScreen();
    }
}

