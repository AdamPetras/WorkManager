using System;

namespace WorkManager.DAL.Enums
{
	[Serializable]
	[Flags]
	public enum EWorkType
	{
		Time = 1,
		Piece = 2,
		Both = 3
	}
}