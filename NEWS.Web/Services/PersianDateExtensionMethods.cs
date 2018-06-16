using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace NEWS.Web.Services
{
	/// <summary>
	/// https://www.dotnettips.info/post/1244/%D8%AA%D8%A7%D8%B1%DB%8C%D8%AE-%D8%B4%D9%85%D8%B3%DB%8C-%D8%A8%D8%A7-extension-method-%D8%A8%D8%B1%D8%A7%DB%8C-datetime
	/// </summary>
	public static class PersianDateExtensionMethods
	{
		private static CultureInfo _Culture;
		public static CultureInfo GetPersianCulture()
		{
			if (_Culture == null)
			{
				_Culture = new CultureInfo("fa-IR");
				DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
				formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
				formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
				var monthNames = new[]
				{
					"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
					"اسفند",
					""
				};
				formatInfo.AbbreviatedMonthNames =
					formatInfo.MonthNames =
					formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
				formatInfo.AMDesignator = "ق.ظ";
				formatInfo.PMDesignator = "ب.ظ";
				formatInfo.ShortDatePattern = "yyyy/MM/dd";
				formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
				formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
				System.Globalization.Calendar cal = new PersianCalendar();

				FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
				if (fieldInfo != null)
					fieldInfo.SetValue(_Culture, cal);

				FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
				if (info != null)
					info.SetValue(formatInfo, cal);

				_Culture.NumberFormat.NumberDecimalSeparator = "/";
				_Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
				_Culture.NumberFormat.NumberNegativePattern = 0;
			}
			return _Culture;
		}

		public static string ToPeString(this DateTime date, string format = "yyyy/MM/dd")
		{
			return date.ToString(format, GetPersianCulture());
		}
	}
}