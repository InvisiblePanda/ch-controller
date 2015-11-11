using ClickerHeroesControl;
using ClickerHeroesControl.GameControl;
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
        private Task autoFire;
        private Task idleMode;
        private CancellationTokenSource autoFireCancelSource;
        private CancellationTokenSource idleCancellationSource;
        private bool autoAscend = false;
        private bool showMask = false;
        private int idleRunDuration = 30;
        private int noCollectingTime = 5;

        public MainViewModel()
        {
            controller = new CHController();
        }

        public string IdleCaption
        {
            get { return idleMode != null ? "Stop Idle Mode" : "Start Idle Mode"; }
        }

        public string AutoFireCaption
        {
            get { return autoFire != null ? "Stop AutoFire" : "AutoFire"; }
        }

        public bool AutoAscend
        {
            get { return autoAscend; }
            set { Set(ref autoAscend, value, nameof(AutoAscend)); }
        }

        public bool ShowMask
        {
            get { return showMask; }
            set { Set(ref showMask, value, nameof(ShowMask)); }
        }

        public int IdleRunDuration
        {
            get { return idleRunDuration; }
            set { Set(ref idleRunDuration, value, nameof(IdleRunDuration)); }
        }

        public int NoCollectingTime
        {
            get { return noCollectingTime; }
            set { Set(ref noCollectingTime, value, nameof(NoCollectingTime)); }
        }

        public ICommand AutoFireCmd
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    if (autoFire != null)
                    {
                        TurnOffAutoFire();
                    }
                    else
                    {
                        AutoFire();
                    }
                });
            }
        }

        public ICommand IdleCmd
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    if (idleMode != null)
                    {
                        TurnOffIdleMode();
                    }
                    else
                    {
                        StartIdleMode();
                    }
                });
            }
        }

        public ICommand AscendCmd
        {
            get
            {
                return new RelayCommand(_ =>
                {
                    Ascend();
                });
            }
        }

        private async void Ascend()
        {
            await controller.Ascend();
            await controller.StartAfterAscension();
            StartIdleMode();
            OnPropertyChanged(nameof(IdleCaption));
        }

        private void StartIdleMode()
        {
            idleCancellationSource = new CancellationTokenSource();
            idleMode = controller.Idle(idleCancellationSource.Token);
            OnPropertyChanged(nameof(IdleCaption));
        }

        private void TurnOffIdleMode()
        {
            idleCancellationSource.Cancel();
            idleMode = null;
            OnPropertyChanged(nameof(IdleCaption));
        }

        private void AutoFire()
        {
            autoFireCancelSource = new CancellationTokenSource();
            autoFire = controller.AutoFire(autoFireCancelSource.Token);
            OnPropertyChanged(nameof(AutoFireCaption));
        }

        private void TurnOffAutoFire()
        {
            autoFireCancelSource.Cancel();
            autoFire = null;
            OnPropertyChanged(nameof(AutoFireCaption));
        }
    }
}