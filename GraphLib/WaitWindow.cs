using System.Windows.Forms;
// ReSharper disable UnusedMember.Global

namespace GraphLib
{
    public partial class WaitWindow : Form
    {
        public WaitWindow()
        {
            InitializeComponent();
        }

        public void SetMaximum(int max)
        {
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = max;
        }

        public void SetProgress(int cnt)
        {
            ProgressBar1.Value = cnt;
        }
    }
}