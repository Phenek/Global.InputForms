using Global.InputForms;
using Global.InputForms.Handlers;

namespace Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.ConfigureMauiHandlers(collection =>
		{
			collection.AddHandler(typeof(BlankButton), typeof(BlankButtonHandler));
			collection.AddHandler(typeof(BlankEntry), typeof(BlankEntryHandler));
			collection.AddHandler(typeof(BlankPicker), typeof(BlankPickerHandler));
			collection.AddHandler(typeof(BlankDatePicker), typeof(BlankDatePickerHandler));
			collection.AddHandler(typeof(BlankTimePicker), typeof(BlankTimePickerHandler));
		});

		return builder.Build();
	}
}

