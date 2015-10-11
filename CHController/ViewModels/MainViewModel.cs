using CHController.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

		public MainViewModel()
		{
			clickerHeroesHandle = FindWindow(null, ClickerHeroesWindowTitle);
		}

		public ICommand AutoFireCmd
		{
			get { return new RelayCommand(_ => ClickOnWindow(100, 200)); }
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
	}
}