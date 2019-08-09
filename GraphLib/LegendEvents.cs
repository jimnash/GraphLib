using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphLib
{
    public class LegendEvents
    {
        private readonly GraphPanel _graphPanel;       
        private readonly Legend _legend;
        private bool _isChecking;

        public LegendEvents(GraphPanel gp, Legend legend)
        {
            _graphPanel = gp;
            _legend = legend;
        }

        public void HighlightChanged(object sender, EventArgs e)
        {
            if (_legend.DoNotUpdate)
                return;
            var cb = (CheckBox) sender;
            var tt = (Legend.HlTag) cb.Tag;

            _graphPanel.GraphParameters.SetGraphHighlightPan(tt.PanelControl, tt.GraphTagId, cb.Checked);
            _graphPanel.RedrawAll();
        }

        public void SolidChanged(object sender, EventArgs e)
        {
            if (_legend.DoNotUpdate)
                return;
            var cb = (CheckBox) sender;
            var tt = (Legend.HlTag) cb.Tag;

            _graphPanel.GraphParameters.SetGraphSolidPan(tt.PanelControl, tt.GraphTagId, !cb.Checked);
            _graphPanel.RedrawAll();
        }

        public void YControlChanged(object sender, EventArgs e)
        {
            if (_isChecking)
                return;
            if (_legend.DoNotUpdate)
                return;
            var cb = (CheckBox) sender;
            var tt = (Legend.HlTag) cb.Tag;

            _isChecking = true;
            foreach (var t in _legend.YControlBox)
                t.Checked = false;

            cb.Checked = true;

            Params.SetGraphMasterPan(tt.PanelControl, tt.GraphTagId, cb.Checked);

            _graphPanel.GraphParameters.MasterCallBack();
            _graphPanel.RedrawAll();
            _isChecking = false;
        }

        public void YScaleClick(object sender, EventArgs e)
        {
            if (_legend.DoNotUpdate)
                return;
            var cb = (ComboBox) sender;
            var tt = (Legend.HlTag) cb.Tag;

            if (cb.Text.Length <= 0)
                return;

            if (!Params.IsNumeric(cb.Text))
            {
                cb.Text = "";
                return;
            }
            var yScale = Convert.ToDouble(cb.Text);
            if (yScale < 1E-07)
                return;

            tt.PanelControl.TagList[tt.GraphTagId].DisplayYScale = yScale;
            _graphPanel.RedrawAll();
            _graphPanel.RedrawAll();
        }

        public void ColClick(object sender, EventArgs e)
        {
            if (_legend.DoNotUpdate)
                return;
            var cb = (PictureBox) sender;
            var tt = (Legend.HlTag) cb.Tag;

            _legend.ColorDialog1.Color = tt.PanelControl.TagList[tt.GraphTagId].Colour.Color;
            _legend.ColorDialog1.ShowDialog();
            cb.BackColor = _legend.ColorDialog1.Color;
            var p = new Pen(_legend.ColorDialog1.Color);
            tt.PanelControl.TagList[tt.GraphTagId].Colour = p;

            _graphPanel.GraphParameters.SetArgbGraphColour(tt.PanelControl.TagList[tt.GraphTagId].Tag,
                _legend.ColorDialog1.Color.ToArgb());
            _graphPanel.GraphParameters.LegendChangeCallback?.Invoke();
            _graphPanel.RedrawAll();
        }
    }
}