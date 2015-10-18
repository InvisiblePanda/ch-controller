using System;
using System.Drawing;

namespace ClickerHeroesControl
{
	internal static class ClickerHeroesPositions
	{
		public static readonly IntPtr Fish1 = PositionPointer(740, 400);
		public static readonly IntPtr Fish2 = PositionPointer(750, 350);
		public static readonly IntPtr Fish3 = PositionPointer(865, 480);
		public static readonly IntPtr Fish4 = PositionPointer(1050, 410);
		public static readonly IntPtr Fish5 = PositionPointer(1005, 453);
		public static readonly IntPtr FishOutOfField = PositionPointer(515, 490);

		public static readonly IntPtr ScrollUp = PositionPointer(545, 190);
		public static readonly IntPtr ScrollDown = PositionPointer(545, 625);

		public static readonly IntPtr FightTab = PositionPointer(50, 100);
		public static readonly IntPtr AncientTab = PositionPointer(300, 100);
		public static readonly IntPtr RelicTab = PositionPointer(350, 100);
		public static readonly IntPtr ClanTab = PositionPointer(450, 100);

		public static readonly IntPtr TodaysRaid = PositionPointer(150, 400);
		public static readonly IntPtr FightAgain = PositionPointer(300, 580);

		private static IntPtr PositionPointer(uint x, uint y)
		{
			return (IntPtr)((y << 16) | x);
		}
	}
}