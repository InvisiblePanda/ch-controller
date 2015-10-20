using ClickerHeroesControl;
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
        private CHController controller;

        private const string ClickerHeroesWindowTitle = "Clicker Heroes";
        private CancellationTokenSource cts;

        private CHMask chMask = null;

        private bool isAutoFiring = false;
        private bool isIdling = false;

        public MainViewModel()
        {
            controller = new CHController();
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
                        await controller.AutoFire(FireMode.DoNotCollectClickables, cts.Token);
                    }
                });
            }
        }

        public ICommand CheckForFishCmd
        {
            get
            {
                return new RelayCommand(async _ =>
                {
                    await controller.StartAfterAscension();
                    //var p = controller.FindClickable();
                    if (chMask != null)
                    {
                        System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
                        rect.Stroke = new SolidColorBrush(Colors.Black);
                        rect.Fill = new SolidColorBrush(Colors.Red);
                        rect.Width = 2;
                        rect.Height = 2;
                        Canvas.SetLeft(rect, 350);
                        Canvas.SetTop(rect, 575);
                        chMask.canvas.Children.Add(rect);
                    }
                });
            }
        }

        public ICommand IdleCmd
        {
            get
            {
                return new RelayCommand(async _ =>
                {
                    if (isIdling)
                    {
                        isIdling = false;
                        cts.Cancel();
                    }
                    else
                    {
                        isIdling = true;
                        cts = new CancellationTokenSource();
                        await controller.Idle(cts.Token);
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