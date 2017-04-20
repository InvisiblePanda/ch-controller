using ClickerHeroesControl.GameControl;
using System;
using System.Windows.Forms;

namespace Angler
{
    public partial class Angler : Form
    {
        private System.Threading.Timer timer;
        private CHController controller;

        public Angler()
        {
            InitializeComponent();
            controller = new CHController();
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(_ => controller.CollectRubies()), null, 0, 10000);
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated)
                CreateHandle();

            base.SetVisibleCore(false);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}