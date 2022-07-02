using Global.InputForms;
//using Global.InputForms.Handlers;

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

		builder.ConfigureMauiHandlers(handlers =>
		{
			handlers.AddLibraryHandlers();
		});

		return builder.Build();
	}

	public static IMauiHandlersCollection AddLibraryHandlers(this IMauiHandlersCollection handlers)
	{
        //handlers.AddHandler(typeof(BlankButton), typeof(BlankButtonHandler));
        //handlers.AddHandler(typeof(BlankEntry), typeof(BlankEntryHandler));
        //handlers.AddHandler(typeof(BlankPicker), typeof(BlankPickerHandler));
        //handlers.AddHandler(typeof(BlankDatePicker), typeof(BlankDatePickerHandler));
        //handlers.AddHandler(typeof(BlankTimePicker), typeof(BlankTimePickerHandler));

        return handlers;
	}
}

