
using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.Core;
using WorkManager.Xamarin.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkManager.Extensions
{
	[ContentProperty(nameof(Type))]
	public class EnumBindingSourceExtension : IMarkupExtension
	{
		public Type Type { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Type is null || !Type.IsEnum)
				throw new Exception("You must provide a valid enum type");

            List<LocalizedEnum> lst = new List<LocalizedEnum>();
            foreach (object value in Enum.GetValues(Type))
            {
                lst.Add(new LocalizedEnum(value));
            }
			return lst;
        }
	}
}