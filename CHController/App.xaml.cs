using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CHController.Views;
using CHController.ViewModels;
using Ninject.Parameters;

namespace CHController
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private IKernel ninjectKernel;

		protected override void OnStartup(StartupEventArgs e)
		{
			ninjectKernel = new StandardKernel();

			//var mainVm = ninjectKernel.Get<MainViewModel>();
			var mainWindow = ninjectKernel.Get<MainWindow>();
			mainWindow.Show();

			base.OnStartup(e);
		}
	}
}