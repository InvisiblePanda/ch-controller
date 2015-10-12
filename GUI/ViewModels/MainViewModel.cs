using GUI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private ClickerHeroesControl.CHController controller;

		private const string ClickerHeroesWindowTitle = "Clicker Heroes";
		private CancellationTokenSource cts;

		private bool isAutoFiring = false;

		public MainViewModel()
		{
			controller = new ClickerHeroesControl.CHController();
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
						await controller.AutoFire(cts.Token);
					}
				});
			}
		}
	}
}