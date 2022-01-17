using System.ComponentModel;
using WorkManager.Core;

namespace WorkManager.DAL.Enums
{
	//[TypeConverter(typeof(LocalizedEnum))] nefunguje v xamarin :(
	public enum EPriority
	{
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(None))]
		None,
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(VeryLow))]
		VeryLow,
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(Low))]
		Low,
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(Medium))]
        Medium,
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(High))]
        High,
		[Localize(typeof(EPriority),typeof(EnumsSR), nameof(VeryHigh))]
		VeryHigh
    }
}