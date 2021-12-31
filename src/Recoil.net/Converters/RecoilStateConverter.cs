using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecoilNet.Converters
{
	/// <summary>
	/// Converter used to allow WPF binding to get the value from a <see cref="RecoilState"/> object
	/// without having to bind to it's property
	/// </summary>
	internal class RecoilStateConverter : TypeConverter
	{
		private readonly Type m_valueType;

		public RecoilStateConverter(Type stateType)
		{
			ArgumentNullException.ThrowIfNull(stateType);

			if(!typeof(RecoilState).IsAssignableFrom(stateType))
			{
				throw new ArgumentException($"The {nameof(RecoilStateConverter)} only supports converting recoil state types.");
			}

			m_valueType = stateType.GenericTypeArguments[0];
		}


		/// <inheritdoc cref="TypeConverter"/>
		public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
			=> m_valueType == destinationType;

		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
			=> m_valueType == sourceType;


		/// <inheritdoc cref="TypeConverter"/>
		public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
		{
			if (value is RecoilState state)
			{
				return state.GetValue();
			}
			return null;
		}

		public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
		{
			return base.ConvertFrom(context, culture, value);
		}
	}
}
