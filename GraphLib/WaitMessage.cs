using System;
using System.Windows.Forms;
// ReSharper disable UnusedMember.Global

namespace GraphLib
{
    public partial class WaitMessage : Form
    {

        private readonly System.Drawing.Graphics _netGraph;
        private bool _isVisible;
        private bool _wasYes = true;
        private bool _isSet;
        private int _progressMaxCount = 1;
        private int _progressCount = 1;

        public WaitMessage()
        {
            InitializeComponent();

            _netGraph = CreateGraphics();
        }

        public void SetCentre(int x, int y)
        {
            Top = y - Height / 2;
            Left = x - Width / 2;
            _isSet = true;

        }
        public void ShowMessage(string message)
        {
            YesButton.Visible = false;
            NoButton.Visible = false;
            Width = GetTextWidth(message);
            Height = GetTextHeight(message) - 30;
            MessageText.Text = message;
            MessageText.Left = 20;
            if (!_isSet)
                CenterToScreen();
            Show();
            TopLevel = true;
            TopMost = true;
            Update();
            _isVisible = true;
        }



        public void SetProgressBar()
        {
            SetProgressBar(1);
        }
        public void SetProgressBar(int count)
        {
            _progressMaxCount = count;
            _progressCount = 1;
            Height += 45;
            Percent.Top = Height - 45;
            Percent.Value = 0;
            Percent.Left = 20;
            Percent.Width = Width - 50;
            Percent.Visible = true;
        }


        public void UpdatePercent(double percentValue)
        {
            var val = (int)percentValue;

            if (Percent.Value == val)
                return;
            if (_progressCount < _progressMaxCount)
            {
                ++_progressCount;
                return;
            }
            _progressCount = 1;

            try
            {
                if (val > Percent.Maximum)
                {
                    Percent.Maximum = val + 1;
                    Percent.Value = val + 1;
                    Percent.Maximum = val;
                }
                else
                    Percent.Value = val + 1;

                Percent.Value = val;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }

            Percent.Update();
            Percent.Refresh();
            Application.DoEvents();
        }

        private delegate void Hide2();
        public void HideMessage()
        {
            if (!_isVisible)
                return;
            if (InvokeRequired)
            {
                Hide2 d = HideIt;
                Invoke(d);
            }
            else
                HideIt();
            _isVisible = false;
            Percent.Visible = false;
        }

        private void HideIt()
        {
            TopLevel = false;
            TopMost = false;
            _isSet = false;
            Hide();
        }

        public bool YesNo(string message)
        {
            YesButton.Visible = true;
            NoButton.Visible = true;
            YesButton.Text = @"Yes";
            NoButton.Text = @"No";
            return Ask(20, GetTextHeight(message), message);
        }

        private void Ok(int x, int width, int height, string message)
        {
            var lastValue = YesButton.Left;
            YesButton.Visible = true;
            YesButton.Text = @"OK";
            YesButton.Left = (width / 2) - 45;
            NoButton.Visible = false;
            Ask(x, height, message);
            NoButton.Visible = true;
            YesButton.Left = lastValue;        
        }

        public void ShowMessageOk(string message)
        {
            Ok(20, GetTextWidth(message), GetTextHeight(message), message);
        }

        private int GetTextHeight(string message)
        {
            var sz = _netGraph.MeasureString(message, MessageText.Font);
            return (int)sz.Height + 70;
        }

        private int GetTextWidth(string message)
        {
            var sz = _netGraph.MeasureString(message, MessageText.Font);
            return (int)sz.Width + 40;
        }

        private bool Ask(int x, int height, string message)
        {
            YesButton.Top = height - 50;
            NoButton.Top = height - 50;
            HideMessage();
            Width = GetTextWidth(message);
            Height = GetTextHeight(message);
            MessageText.Text = message;
            MessageText.Left = x;
            TopLevel = true;
            TopMost = true;
            _isVisible = true;
            CenterToScreen();
            ShowDialog();
            _isVisible = false;


            return _wasYes;
        }


        private void YesButton_Click(object sender, EventArgs e)
        {
            _wasYes = true;
            Hide();
            _isVisible = false;
        }

        private void NoButton_Click(object sender, EventArgs e)
        {
            _wasYes = false;
            Hide();
            _isVisible = false;
        }
    }
}
