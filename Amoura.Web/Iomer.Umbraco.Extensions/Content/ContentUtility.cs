using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Iomer.Umbraco.Extensions.Content
{
	public static class ContentUtility
	{
		public static TValue GetValue<TValue>(this IContent content, string propertyAlias, TValue defaultValue)
		{
			TValue result = defaultValue;

			try
			{
				result = content.GetValue<TValue>(propertyAlias);
			}
			catch (InvalidCastException)
			{
				object rawValue = content.GetValue(propertyAlias);
				if (rawValue is IConvertible)
				{
					try
					{
						result = (TValue)Convert.ChangeType(rawValue, typeof(TValue));
					}
					catch
					{
						result = defaultValue;
					}
				}
				else
				{
					result = defaultValue;
				}
			}

			return result;
		}
	}
}
