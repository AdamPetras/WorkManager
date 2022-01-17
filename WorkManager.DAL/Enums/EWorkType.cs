using System;
using WorkManager.Core;

namespace WorkManager.DAL.Enums
{
	[Serializable]
	[Flags]
	public enum EWorkType
	{
        [Localize(typeof(EWorkType), typeof(EnumsSR), nameof(Time))]
		Time = 1,
        [Localize(typeof(EWorkType), typeof(EnumsSR), nameof(Piece))]
		Piece = 2,
        [Localize(typeof(EWorkType), typeof(EnumsSR), nameof(Both))]
        Both = 3
    }
}