using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CHController.Views;

namespace CHController
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		IKernel ninjectKernel;

		protected override void OnStartup(StartupEventArgs e)
		{
			ninjectKernel = new StandardKernel();

			var mainWindow = ninjectKernel.Get<MainWindow>();
			mainWindow.Show();

			base.OnStartup(e);
		}
	}
}