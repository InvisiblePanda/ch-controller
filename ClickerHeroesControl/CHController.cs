using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ClickerHeroesControl.WinApiConstants;

namespace ClickerHeroesControl
{
	public sealed class CHController
	{
		private const string ClickerHeroesWindowTitle = "Clicker Heroes";
		private readonly IntPtr targetHandle;

		private static readonly IntPtr fish1Position = GetPosition(865, 480);
		private static readonly IntPtr fish2Position = GetPosition(740, 400);
		private static readonly IntPtr fish3Position = GetPosition(1050, 410);
		private static readonly IntPtr fish4Position = GetPosition(750, 350);
		private static readonly IntPtr fish5Position = GetPosition(740, 400); // has to be found out still
		private static readonly IntPtr fishOutOfFieldPosition = GetPosition(515, 490);

		public CHController()
		{
			targetHandle = FindWindow(null, ClickerHeroesWindowTitle);
		}

		#region Methods

		#region DllImports

		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		#endregion

		public async Task AutoFire(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				TargetClick(fish1Position);
				TargetClick(fish2Position);
				TargetClick(fish3Position);
				TargetClick(fish4Position);
				TargetClick(fish5Position);

				try
				{
					await Task.Delay(110, ct);
				}
				catch (TaskCanceledException)
				{
				}
			}
		}

		public void TargetClick(IntPtr position)
		{
			PostMessage(targetHandle, WinApiConstants.WM_LBUTTONDOWN, (IntPtr)1, position);
			PostMessage(targetHandle, WinApiConstants.WM_LBUTTONUP, (IntPtr)0, position);
		}

		private void KeyPlusClick(uint keyCode, IntPtr position)
		{
			PostMessage(targetHandle, WM_KEYDOWN, (IntPtr)keyCode, IntPtr.Zero);
			TargetClick(position);
			PostMessage(targetHandle, WM_KEYUP, (IntPtr)keyCode, IntPtr.Zero);
		}

		private static IntPtr GetPosition(uint x, uint y)
		{
			return (IntPtr)((y << 16) | x);
		}

		#endregion
	}
}