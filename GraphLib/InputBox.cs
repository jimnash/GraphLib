using System;
using System.Windows.Forms;

namespace GraphLib
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public double TheStartAngle { get; private set; }

        public double TheStopAngle { get; private set; }

        private void Apply_Click(object sender, EventArgs e)
        {
            TheStartAngle = (double) StartAngle.Value;
            TheStopAngle = (double) StopAngle.Value;
            Hide();
        }

        public void Start(double ang1, double ang2)
        {
            if (ang1 < 0)
                ang1 = 0;
            if (ang1 > 360)
                ang1 = 360;
            if (ang2 < 0)
                ang2 = 0;
            if (ang2 > 360)
                ang2 = 360;

            TheStartAngle = ang1;
            StartAngle.Value = (decimal) TheStartAngle;
            TheStopAngle = ang2;
            StopAngle.Value = (decimal) TheStopAngle;
            ShowDialog();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}