using System;
using System.Windows.Forms;

namespace GraphLib
{
    public partial class YAxisForm : Form
    {
        private GraphPanel _graphPanel;
        private PanelControl _panelControl;
        private bool _yChanged;

        public YAxisForm(GraphPanel g)
        {
            InitializeComponent();
            _graphPanel = g;
        }

        private void YAxisForm_Load(object sender, EventArgs e)
        {
            TopMost = true;
        }

        internal void SetGraphPanel(GraphPanel g, PanelControl p)
        {
            _graphPanel = g;
            _panelControl = p;
            FillDataItems();
        }

        private void YAxisAutoFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null || _panelControl == null)
                return;

            if (YAxisAutoFixed.Checked)
            {
                YAxisMin.Enabled = false;
                YAxisMax.Enabled = false;
            }
            _yChanged = true;
        }

        private void YAxisAutoFree_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null || _panelControl == null)
                return;

            if (YAxisAutoFree.Checked)
            {
                YAxisMin.Enabled = false;
                YAxisMax.Enabled = false;
            }
            _yChanged = true;
        }

        private void YAXisFixedMinMax_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null || _panelControl == null)
                return;

            if (YAXisFixedMinMax.Checked)
            {
                YAxisMin.Enabled = true;
                YAxisMax.Enabled = true;
            }
            _yChanged = true;
        }

        private void YAxisMax_ValueChanged(object sender, EventArgs e)
        {
            _yChanged = true;
            if (YAxisMax.Value < YAxisMin.Value)
                YAxisMax.Value = (decimal) ((double) YAxisMin.Value*1.1);
        }

        private void YAxisMin_ValueChanged(object sender, EventArgs e)
        {
            _yChanged = true;
            if (YAxisMin.Value > YAxisMax.Value)
                YAxisMin.Value = (decimal) ((double) YAxisMax.Value*0.9);
        }

        private void SetFixed_Click(object sender, EventArgs e)
        {
            YAXisFixedMinMax.Checked = true;
            var dif = (double) YAxisMax.Value - (double) YAxisMin.Value;
            dif = dif*0.1;
            YAxisMin.Value -= (decimal) dif;
            YAxisMax.Value += (decimal) dif;
            _yChanged = true;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Hide();
            if (_graphPanel == null || _panelControl == null)
                return;

            if (_yChanged)
            {
                SetPanel(_panelControl);
                _graphPanel.RedrawAll();
                UpdateYCallers();
            }

            _yChanged = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Hide();
            _yChanged = false;
        }

        private void SetPanel(PanelControl pan)
        {
            pan.YAxisMin = (double) YAxisMin.Value;
            pan.YAxisMax = (double) YAxisMax.Value;

            if (YAxisAutoFixed.Checked)
                pan.YAxisType = YAxisType.Auto;
            else if (YAxisAutoFree.Checked)
                pan.YAxisType = YAxisType.Free;
            else if (YAXisFixedMinMax.Checked)
                pan.YAxisType = YAxisType.MinMax;
            _graphPanel.ResetY(pan);
        }

        private void FillDataItems()
        {
            YAxisMin.Value = (decimal) _panelControl.YAxisMin;
            YAxisMax.Value = (decimal) _panelControl.YAxisMax;

            if (_panelControl.YAxisMin < -90000)
                YAxisMin.Value = -90000;
            if (_panelControl.YAxisMax > 90000)
                YAxisMax.Value = 90000;

            var yType = _panelControl.YAxisType;
            YAxisAutoFree.Checked = false;
            YAxisAutoFixed.Checked = false;
            YAXisFixedMinMax.Checked = false;
            YAxisMin.Enabled = false;
            YAxisMax.Enabled = false;
            switch (yType)
            {
                case YAxisType.Auto:
                    YAxisAutoFixed.Checked = true;
                    break;
                case YAxisType.Free:
                    YAxisAutoFree.Checked = true;
                    break;
                case YAxisType.MinMax:
                    YAXisFixedMinMax.Checked = true;
                    YAxisMin.Enabled = true;
                    YAxisMax.Enabled = true;
                    break;
            }

            _yChanged = false;
        }

        private void UpdateYCallers()
        {
            _graphPanel.GraphParameters.YAxisCallback?.Invoke();
        }
    }
}