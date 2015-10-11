using CHController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CHController.ViewModels
{
	public sealed class MainViewModel : ViewModelBase
	{
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_LBUTTONUP = 0x202;

		private const string ClickerHeroesWindowTitle = "Clicker Heroes";
		private readonly IntPtr clickerHeroesHandle;
		private CancellationTokenSource cts;

		private bool isAutoFiring = false;

		public MainViewModel()
		{
			clickerHeroesHandle = FindWindow(null, ClickerHeroesWindowTitle);
		}

		public ICommand AutoFireCmd
		{
			get
			{
				return new RelayCommand(async _ =>
				{
					if (isAutoFiring)
					{
						isAutoFiring = false;
						cts.Cancel();
					}
					else
					{
						isAutoFiring = true;
						cts = new CancellationTokenSource();
						await AutoFire(cts.Token);
					}
				});
			}
		}

		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		private void ClickOnWindow(int x, int y)
		{
			IntPtr coordinates = (IntPtr)((y << 16) | x);
			PostMessage(clickerHeroesHandle, WM_LBUTTONDOWN, (IntPtr)1, coordinates);
			PostMessage(clickerHeroesHandle, WM_LBUTTONUP, (IntPtr)1, coordinates);
		}

		private async Task AutoFire(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				ClickOnWindow(700, 500);
				ClickOnWindow(700, 500);
				ClickOnWindow(700, 500);
				ClickOnWindow(700, 500);
				ClickOnWindow(700, 500);
				try
				{
					await Task.Delay(110, token);
				}
				catch (TaskCanceledException)
				{
				}
			}
		}
	}
}