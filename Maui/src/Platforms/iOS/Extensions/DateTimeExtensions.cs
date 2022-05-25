
using System;
using Foundation;

namespace Global.InputForms.iOS.Extensions
{
	public static class DateTimeExtensions
	{
		internal static DateTime ReferenceDate = new DateTime(2001, 1, 1, 0, 0, 0);

		public static DateTime ToGlobalDateTime(this NSDate date)
		{
			return ReferenceDate.AddSeconds(date.SecondsSinceReferenceDate);
		}

		public static NSDate ToGlobalNSDate(this DateTime date)
		{
			return NSDate.FromTimeIntervalSinceReferenceDate((date - ReferenceDate).TotalSeconds);
		}
	}
}

