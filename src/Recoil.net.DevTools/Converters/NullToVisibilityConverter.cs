using Recoil.net.DevTools.Converters;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Recoil.net.DevTools.Converters
{
	/// <summary>
	/// Convertert to convert from null to visibility
	/// </summary>
	public class NullToVisibilityConverter : IValueConverter
	{
		public static NullToVisibilityConverter CollapsedWhenNull { get; }
		public static NullToVisibilityConverter CollapsedWhenNotNull { get; }
		public static NullToVisibilityConverter HiddenWhenNull { get; }
		public static NullToVisibilityConverter HiddenWhenNotNull { get; }

		/// <summary>
		/// Gets if the converter is inverted 
		/// </summary>
		public bool Inverted { get; }

		/// <summary>
		/// Gets the state what happens when it's not visible 
		/// </summary>
		public Visibility NonVisibleState { get; }

		static NullToVisibilityConverter()
		{
			CollapsedWhenNull = new NullToVisibilityConverter(false, Visibility.Collapsed);
			CollapsedWhenNotNull = new NullToVisibilityConverter(true, Visibility.Collapsed);
			HiddenWhenNull = new NullToVisibilityConverter(false, Visibility.Hidden);
			HiddenWhenNotNull = new NullToVisibilityConverter(true, Visibility.Hidden);
		}

		public NullToVisibilityConverter(bool inverted, Visibility visibility)
		{
			Inverted = inverted;
			NonVisibleState = visibility;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isVisible = value != null;

			if (Inverted)
			{
				isVisible = !isVisible;
			}

			return isVisible ? Visibility.Visible : NonVisibleState;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
