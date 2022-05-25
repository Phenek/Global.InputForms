using System;
using System.IO;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Util;
using Android.Views.InputMethods;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;

namespace Global.InputForms.Droid.Extensions
{
	public static class ContextExtensions
	{
		static float s_displayDensity = float.MinValue;

		public static float ToGlobalPixels(this Context self, double dp)
		{
			EnsureMetrics(self);

			return (float)Math.Ceiling(dp * s_displayDensity);
		}

		static void EnsureMetrics(Context context)
		{
			if (s_displayDensity != float.MinValue)
				return;

			context ??= Android.App.Application.Context;

			using (DisplayMetrics metrics = context.Resources?.DisplayMetrics)
				s_displayDensity = metrics != null ? metrics.Density : 1;
		}
	}
}

