using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace NecDMS.Helper
{
	public class ConverterBase64ImageSource : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string base64Image = (string)value;

			if (base64Image == null)
				return null;

			// Convert base64Image from string to byte-array
			var imageBytes = System.Convert.FromBase64String(base64Image);

			// Return a new ImageSource
			return ImageSource.FromStream(() => { return new MemoryStream(imageBytes); });
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			// Not implemented as we do not convert back
			throw new NotSupportedException();
		}
	}

	public class SubstringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;

			var text = ((string)value);
			return text.Length <= 9 ? text : text.Substring(0, 9) + " ...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}
	}
}
