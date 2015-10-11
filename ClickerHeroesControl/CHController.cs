using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CHController
{
	public sealed class CHController
	{
		private const string ClickerHeroesWindowTitle = "Clicker Heroes";
		private readonly IntPtr targetHandle;

		public CHController()
		{
			targetHandle = FindWindow(null, ClickerHeroesWindowTitle);
		}

		#region Methods

		public void TargetClick(uint x, uint y)
		{
			IntPtr coords = (IntPtr)((y << 16) | x);
			PostMessage(targetHandle, WinApiConstants.WM_LBUTTONDOWN, (IntPtr)1, coords);
			PostMessage(targetHandle, WinApiConstants.WM_LBUTTONUP, (IntPtr)0, coords);
		}

		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		#endregion
	}
}