using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static ClickerHeroesControl.WinApiConstants;

namespace ClickerHeroesControl
{
	public sealed class CHController
	{
		private const string ClickerHeroesWindowTitle = "Clicker Heroes";

		private static readonly IntPtr fishOutOfFieldPosition = GetPosition(515, 490);
		private static readonly IntPtr fish1Position = GetPosition(740, 400);
		private static readonly IntPtr fish2Position = GetPosition(750, 350);
		private static readonly IntPtr fish3Position = GetPosition(865, 480);
		private static readonly IntPtr fish4Position = GetPosition(1050, 410);
		private static readonly IntPtr fish5Position = GetPosition(1005, 453);

		private readonly IntPtr targetHandle = IntPtr.Zero;

		public CHController()
		{
			targetHandle = FindWindow(null, ClickerHeroesWindowTitle);

			if (targetHandle == IntPtr.Zero)
			{
				throw new TargetWindowNotFoundException("Could not locate the Clicker Heroes (Steam) window.");
			}
		}

		#region Methods

		#region DllImports

		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("gdi32.dll")]
		private static extern int BitBlt(
			IntPtr srchDc,
			int srcX,
			int srcY,
			int srcW,
			int srcH,
			IntPtr desthDc,
			int destX,
			int destY,
			int op);

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

		private void TargetClick(IntPtr position)
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

		private Color GetPixelColor(int x, int y)
		{
			using (Bitmap pixel = new Bitmap(1, 1))
			using (Graphics destinationGraphics = Graphics.FromImage(pixel))
			using (Graphics sourceGraphics = Graphics.FromHwnd(targetHandle))
			{
				IntPtr hSrcDc = sourceGraphics.GetHdc();
				IntPtr hDc = destinationGraphics.GetHdc();
				BitBlt(hDc, 0, 0, 1, 1, hSrcDc, x, y, (int)CopyPixelOperation.SourceCopy);

				destinationGraphics.ReleaseHdc();
				sourceGraphics.ReleaseHdc();

				return pixel.GetPixel(0, 0);
			}
		}

		private static IntPtr GetPosition(uint x, uint y)
		{
			return (IntPtr)((y << 16) | x);
		}

		#endregion
	}
}