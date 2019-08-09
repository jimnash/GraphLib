using System;
using System.Windows.Forms;

namespace GraphLib
{
    public class Options : IDisposable
    {
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParameters;
        private readonly GrTabPanel _graphTabPanel;
        private AxisForm _axisConfiguration;

        public Options(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphTabPanel = _graphPanel.GraphParameters.GraphTabPanel;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _axisConfiguration?.Dispose();
            }
        }
               
        internal void UseFollowingCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.UseFollowingCursoToolStripMenuItem.Checked =
                !_graphPanel.UseFollowingCursoToolStripMenuItem.Checked;
            _graphParameters.DisplayGraphMover = _graphPanel.UseFollowingCursoToolStripMenuItem.Checked;
            ShowCursor(_graphPanel.UseFollowingCursoToolStripMenuItem.Checked);
            _graphParameters.DisplayActiveValues = _graphPanel.UseFollowingCursoToolStripMenuItem.Checked;
            _graphPanel.XValueLabel.Visible = _graphPanel.UseFollowingCursoToolStripMenuItem.Checked;
            _graphPanel.YValueLabel.Visible = _graphPanel.UseFollowingCursoToolStripMenuItem.Checked;
        }

        private void ShowCursor(bool seen)
        {
            _graphParameters.DisplayCursor = seen;
            _graphPanel.RedrawAll();
        }

        internal void TurnOffFollowingCursor()
        {
            _graphPanel.UseFollowingCursoToolStripMenuItem.Checked = false;
               
            _graphParameters.DisplayGraphMover = false;         
            ShowCursor(false);
            _graphParameters.DisplayActiveValues = false;
            _graphPanel.XValueLabel.Visible = false;
            _graphPanel.YValueLabel.Visible = false;
        }

        internal void ShowGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowGridToolStripMenuItem.Checked = !_graphPanel.ShowGridToolStripMenuItem.Checked;
            _graphParameters.DrawTheGrid = _graphPanel.ShowGridToolStripMenuItem.Checked;
        }

        internal void ShowXAxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowXAxisToolStripMenuItem.Checked = !_graphPanel.ShowXAxisToolStripMenuItem.Checked;
            _graphPanel.SetTheXAxis(_graphPanel.ShowXAxisToolStripMenuItem.Checked);
        }

        internal void ShowYAxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowYAxisToolStripMenuItem.Checked = !_graphPanel.ShowYAxisToolStripMenuItem.Checked;
            _graphPanel.SetTheYAxis(_graphPanel.ShowYAxisToolStripMenuItem.Checked);
        }

        internal void ShowYAxisLegendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowYAxisLegendToolStripMenuItem.Checked = !_graphPanel.ShowYAxisLegendToolStripMenuItem.Checked;
            _graphPanel.SetTheYAxisLegend(_graphPanel.ShowYAxisLegendToolStripMenuItem.Checked);
        }

        internal void ShowXAxisLegendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowXAxisLegendToolStripMenuItem.Checked = !_graphPanel.ShowXAxisLegendToolStripMenuItem.Checked;
            _graphPanel.SetTheXAxisLegend(_graphPanel.ShowXAxisLegendToolStripMenuItem.Checked);
        }

        internal void AxisConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_axisConfiguration == null)
            {
                _axisConfiguration = new AxisForm(_graphPanel);
                _axisConfiguration.Show();
                _axisConfiguration.Hide();
            }
            _axisConfiguration.Top = 100;
            _axisConfiguration.Left = 100;
            _axisConfiguration.Start();
        }
                 
        internal void AddPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.Gt.AddPanel(null);
        }

        internal void ForceDarkBackground()
        {
            _graphParameters.DarkBackground = true;
            _graphPanel.UseDarkBackgroundItem.Checked = true;
        }
        internal void UseDarkBackgroundItem_Click(object sender, EventArgs e)
        {
            _graphParameters.DarkBackground = _graphPanel.UseDarkBackgroundItem.Checked;
        }
        internal void RemovePanelItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowLegend(false);
            _graphParameters.PauseLiveData(true);
            _graphTabPanel.RemoveLastGraphicsPanel(_graphPanel.Width, _graphPanel.Height);
            _graphParameters.PauseLiveData(false);
        }

        internal void ShowDifferenceMarkersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowDifferenceMarkersToolStripMenuItem.Checked =
                !_graphPanel.ShowDifferenceMarkersToolStripMenuItem.Checked;
            _graphParameters.ShowDifferenceMarkers = _graphPanel.ShowDifferenceMarkersToolStripMenuItem.Checked;
            _graphPanel.RedrawAll();
        }

        internal void ShowGraphTooltipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphParameters.CheckGraphTooltip = _graphPanel.ShowGraphTooltipToolStripMenuItem.Checked;
        }

        internal void ShowBoundaryValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphPanel.ShowBoundaryValuesToolStripMenuItem.Checked =
                !_graphPanel.ShowBoundaryValuesToolStripMenuItem.Checked;
            _graphParameters.DrawBoundaryStrings = _graphPanel.ShowBoundaryValuesToolStripMenuItem.Checked;
            _graphPanel.RedrawAll();
        }

        internal void ForceBoundaryStrings(bool on)
        {
            _graphPanel.ShowBoundaryValuesToolStripMenuItem.Checked = on;              
            _graphParameters.DrawBoundaryStrings = on;
        }      

        internal void UseAlternateZeroYAxisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _graphParameters.OriginalZeroAxis = _graphPanel.UseAlternateZeroYAxisToolStripMenuItem.Checked;
            _graphPanel.UseAlternateZeroYAxisToolStripMenuItem.Checked =
                !_graphPanel.UseAlternateZeroYAxisToolStripMenuItem.Checked;
            _graphPanel.RedrawAll();
        }
     
        internal void SetHighlightWidth(object sender, EventArgs e)
        {
            var t = (ToolStripMenuItem) sender;
            var w = Convert.ToInt32((string) t.Tag);

            _graphParameters.HighlightWidth = w;
            _graphPanel.X2ToolStripMenuItem.Checked = false;
            _graphPanel.X3ToolStripMenuItem.Checked = false;
            _graphPanel.X4ToolStripMenuItem.Checked = false;
            _graphPanel.X5ToolStripMenuItem.Checked = false;
            _graphPanel.X6ToolStripMenuItem.Checked = false;
            _graphPanel.X7ToolStripMenuItem.Checked = false;
            _graphPanel.X8ToolStripMenuItem.Checked = false;
            _graphPanel.X9ToolStripMenuItem.Checked = false;

            t.Checked = true;

            _graphPanel.RedrawAll();
        }
    }
}