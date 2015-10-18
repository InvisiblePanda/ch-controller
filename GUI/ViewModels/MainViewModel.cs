using GUI.Utils;
using GUI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ClickerHeroesControl.CHController controller;

        private const string ClickerHeroesWindowTitle = "Clicker Heroes";
        private CancellationTokenSource cts;

        private CHMask chMask = null;

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

        public ICommand CheckForFishCmd
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    var p = controller.FindClickable();
                    if (chMask != null && p != null)
                    {
                        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                        rect.Stroke = new SolidColorBrush(Colors.Black);
                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 1;
                        rect.Height = 1;
                        Canvas.SetLeft(rect, p.Value.X);
                        Canvas.SetTop(rect, p.Value.Y);
                        chMask.canvas.Children.Add(rect);
                    }
                });
            }
        }

        public ICommand ShowMaskWindowCmd
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    if (chMask == null)
                    {
                        chMask = new CHMask();
                        var targetSize = controller.CHClientRectangle;
                        chMask.Width = targetSize.Width;
                        chMask.Height = targetSize.Height;
                        chMask.Left = targetSize.Left;
                        chMask.Top = targetSize.Top;
                        chMask.Show();
                    }
                    else
                    {
                        chMask.Close();
                        chMask = null;
                    }
                });
            }
        }
    }
}