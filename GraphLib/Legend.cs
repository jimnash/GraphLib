using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GraphLib
{
    public delegate void SetUpdateCallBack();

    public partial class Legend : Form
    {
        public class HlTag
        {
            public int GraphTagId { get; }
            public PanelControl PanelControl { get; }
            public HlTag(int id, PanelControl pc)
            {
                GraphTagId = id;
                PanelControl = pc;
            }         
        }

        public string FormatPoint { get; set; } = "0.00";
        public bool DoNotUpdate;

        private readonly Graphics _netGraph;
        private readonly GraphPanel _graphPanel;             
        private readonly LegendEvents _events;
            
        private readonly List<Label> _lastValueLabel = new List<Label>(32);
        private readonly List<Label> _maxLabel = new List<Label>(32);
        private readonly List<Label> _minLabel = new List<Label>(32);
        private readonly List<Label> _nameLabel = new List<Label>(32);       
        private readonly List<PictureBox> _colorBox = new List<PictureBox>(32);
        private readonly List<CheckBox> _highlightBox = new List<CheckBox>(32);
        private readonly List<CheckBox> _solidBox = new List<CheckBox>(32);
        public readonly List<CheckBox> YControlBox = new List<CheckBox>(32);       
        private readonly List<ComboBox> _yScaleBox = new List<ComboBox>(32);

        private PanelControl _panelControl;
        private bool _wasSeen;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

        private const int WmSetRedraw = 11;

        private static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WmSetRedraw, false, 0);
        }

        private static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WmSetRedraw, true, 0);
            parent.Invalidate(true);
            parent.Update();          
        }
        
        public Legend(GraphPanel gp)
        {
            InitializeComponent();
            _graphPanel = gp;
            _netGraph = CreateGraphics();
            _events = new LegendEvents(_graphPanel,this);
        }

        private int GetTextWidth(string message)
        {
            var sz = _netGraph.MeasureString(message, Lnamelab.Font);
            return (int)sz.Width + 40;
        }

        public void SetTitle(string title)
        {          
            Text = title;
        }
           
        private void ResetData()
        {
           
            _minLabel.Clear();
            _maxLabel.Clear();
            _lastValueLabel.Clear();
            _nameLabel.Clear();
            _colorBox.Clear();
            _highlightBox.Clear();
            YControlBox.Clear();
            _solidBox.Clear();
            _yScaleBox.Clear();
        }

        private ComboBox SetYScaleBox(int y,int i,PanelControl panelControl)
        {
            var ys = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Text = panelControl.TagList[i].DisplayYScale.ToString(CultureInfo.InvariantCulture),
                Left = Lscalelab.Left + 4,
                Top = y - 3,
                Width = 62,
                Height = 21,
                ForeColor = Color.Black,
                Tag = new HlTag(i, panelControl)
            };
            ys.SelectedIndexChanged += _events.YScaleClick;
            if (_graphPanel.GraphParameters.LiveGraphs != null)
            {
                ys.Items.Add(1);
                ys.SelectedIndex = 0;
            }
            else
            {
                ys.Items.Add(0.0001);
                ys.Items.Add(0.001);
                ys.Items.Add(0.01);
                ys.Items.Add(0.1);
                ys.Items.Add(1);
                ys.Items.Add(10);
                ys.Items.Add(100);
                ys.Items.Add(1000);
                ys.Items.Add(10000);
                ys.SelectedIndex = 4;
            }

            return ys;
        }

        internal void FillLegend(PanelControl panelControl)
        {
            try
            {                              
                FillLegend2(panelControl);
            }
            catch
            {
                // ignored
            }                 
         
        }

        private static Label CreateLabel(int left,int top,string txt)
        {           
            var lab = new Label
            {
                Text = txt,
                Left = left,
                Top = top,
                Width = 60,
                Height = 18
            };

            return lab;
        }

        private string GetLegendNameLabel(GraphSurface gs)
        {
            string txt;
            var source = @" [" + gs.Source + @"]";
            if (gs.Source.Length < 1)
                source = " ";
         
            if (_graphPanel.ShowLegendGraphId)
                txt = @"[" + gs.TagId + @"]" + gs.Name + source;
            else
                txt = gs.Name + source;
            return txt;
        }

        private void FillLegend2(PanelControl panelControl)
        {         
            if (panelControl.TagList == null)
                return;         
            if (panelControl.TagList.Length <= 0)
                return;
            const int lineStep = 25;
            var tWidth = 0;
            var j = -1;
            var y = 0;
            var foundMaster = false;

            DoNotUpdate = true;                    
            ResetData();
                    
            for (var i = 0; i < panelControl.TagList.Length; i++)
            {
                if (!panelControl.TagList[i].Visible)
                    continue;

                var gs = _graphPanel.GetGraphSurfaceFromTag(panelControl.TagList[i].Tag);
                if (gs == null)
                    continue;
                if (gs.GType != GraphType.Graph && gs.GType != GraphType.Live)
                    continue;

                j += 1;
                y += lineStep;

                var hb = new CheckBox
                {
                    Checked = false,
                    Left = LHLLab.Left + 4,
                    Top = y - 4,
                    Text = "",
                    Width = 25,
                    Tag = new HlTag(gs.TagId, panelControl)
                };

                if (panelControl.TagList[i].Highlight)
                    hb.Checked = true;
                hb.CheckedChanged += _events.HighlightChanged;
                _highlightBox.Add(hb);

                var sb = new CheckBox
                {
                    Checked = true,
                    Left = LabSolid.Left + 4,
                    Top = y - 4,
                    Text = "",
                    Width = 25,
                    Tag = new HlTag(gs.TagId, panelControl)
                };

                if (panelControl.TagList[i].AsPoint)
                    sb.Checked = false;
                sb.CheckedChanged += _events.SolidChanged;
                _solidBox.Add(sb);

                var yc = new CheckBox
                {
                    Checked = false,
                    Left = YMaster.Left + 20,
                    Top = y - 4,
                    Text = "",
                    Width = 25,
                    Tag = new HlTag(gs.TagId, panelControl)
                };

                if (panelControl.TagList[i].Master)
                {
                    yc.Checked = true;
                    foundMaster = true;
                }
                    
                yc.CheckedChanged += _events.YControlChanged;
                YControlBox.Add(yc);

                var colourBox = new PictureBox
                {
                    BackColor = panelControl.TagList[i].Colour.Color,
                    Left = Lcollab.Left + 4,
                    Top = y,
                    Width = 28,
                    Height = 15,
                    BorderStyle = BorderStyle.Fixed3D
                };

                var toolTip = new ToolTip { InitialDelay = 200 };
                toolTip.SetToolTip(colourBox, "Single Click Here To Make A Temporary Change To The Colour");
                colourBox.Tag = new HlTag(i, panelControl);
                colourBox.Click += _events.ColClick;
                _colorBox.Add(colourBox);

                var ys = SetYScaleBox(y, i, panelControl);
                _yScaleBox.Add(ys);

                var minL = CreateLabel(Lminlab.Left, y, gs.MinD.ToString(CultureInfo.InvariantCulture));
                _minLabel.Add(minL);

                var maxL = CreateLabel(Lmaxlab.Left, y, gs.MaxD.ToString(CultureInfo.InvariantCulture));               
                _maxLabel.Add(maxL);

                var lastL = CreateLabel(Llastslab.Left, y, " ");              
                _lastValueLabel.Add(lastL);             

                var txt = GetLegendNameLabel(gs);
                var namL = CreateLabel(Lnamelab.Left, y, txt);
                namL.Width = GetTextWidth(txt);
                _nameLabel.Add(namL);

                var w = namL.Width + Lnamelab.Left;               
                if (w > tWidth)
                    tWidth = w;
             
            }

            if (j < 0)
            {
                DoNotUpdate = false;
                return;
            }
          

            for (j = 0; j < _solidBox.Count;++j)
            {
                    Controls.Add(_minLabel[j]);
                    Controls.Add(_maxLabel[j]);
                    Controls.Add(_lastValueLabel[j]);
                    Controls.Add(_colorBox[j]);
                    Controls.Add(_nameLabel[j]);
                    Controls.Add(_highlightBox[j]);
                    Controls.Add(YControlBox[j]);
                    Controls.Add(_yScaleBox[j]);
                    Controls.Add(_solidBox[j]);
            }
           
            Height = y + 75;
            if (Height > 500)
            {
                AutoScroll = true;
                Height = 500;
                Width = tWidth + 100;
            }
            else
            {
                AutoScroll = false;
                Width = tWidth;
            }
            
            //Check we have a master
            if (foundMaster)
            {
                DoNotUpdate = false;
                return;
            }

            if (YControlBox == null)
                return;
            if (YControlBox.Count <= 0)
                return;
            YControlBox[0].Checked = true;

            var tt = (HlTag)YControlBox[0].Tag;
            Params.SetGraphMasterPan(tt.PanelControl, tt.GraphTagId, true);
            DoNotUpdate = false;

        }

        internal void UpdateLegend(PanelControl pan)
        {
          
            try
            {
                SuspendDrawing(this);               
                EmptyLegend();
                FillLegend(pan);
                UpdateMinMax(pan);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {              
            }
            ResumeDrawing(this);
        }

        internal void EmptyLegend()
        {
            if (_solidBox.Count <= 0)
                return;

            for (var j = 0; j < _solidBox.Count; j++)
            {
                Controls.Remove(_minLabel[j]);
                Controls.Remove(_maxLabel[j]);
                Controls.Remove(_lastValueLabel[j]);
                Controls.Remove(_colorBox[j]);
                Controls.Remove(_nameLabel[j]);
                Controls.Remove(_highlightBox[j]);
                Controls.Remove(YControlBox[j]);
                Controls.Remove(_yScaleBox[j]);
                Controls.Remove(_solidBox[j]);               
            }

            _minLabel.Clear();
            _maxLabel.Clear();
            _lastValueLabel.Clear();
            _nameLabel.Clear();
            _highlightBox.Clear();
            _solidBox.Clear();
            _yScaleBox.Clear();
            YControlBox.Clear();
            _colorBox.Clear();
        }

        internal void UpdateMinMax(PanelControl pan)
        {
            _panelControl = pan;
            if (!Visible)
                return;

            SetUpdateCallBack d = DoTheUpdate;
            try
            {
                Invoke(d);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void DoTheUpdate()
        {
            var j = -1;
            if (_lastValueLabel.Count <= 0)
                return;
            try
            {
                foreach (var value in _panelControl.TagList)
                {
                    if (!value.Visible)
                        continue;

                    var gs = _graphPanel.GetGraphSurfaceFromTag(value.Tag);
                    if (gs == null)
                        continue;

                    if (gs.GType != GraphType.Graph && gs.GType != GraphType.Live)
                        continue;
                    j += 1;

                    try
                    {
                        double minimumPoint = value.DisplayPoints[0].Y;
                        double maximumPoint = value.DisplayPoints[0].Y;
                        var cnt = value.ScrPCount;
                        for (var k = 0; k < cnt; k++)
                        {
                            if (value.DisplayPoints[k].Y > maximumPoint)
                                maximumPoint = value.DisplayPoints[k].Y;
                            if (gs.DPts[k].Y < minimumPoint)
                                minimumPoint = value.DisplayPoints[k].Y;
                        }
                        minimumPoint = minimumPoint/value.DisplayYScale;
                        maximumPoint = maximumPoint/value.DisplayYScale;
                        _minLabel[j].Text = minimumPoint.ToString(FormatPoint);
                        _maxLabel[j].Text = maximumPoint.ToString(FormatPoint);
                        if (cnt <= 1)
                        {
                            _minLabel[j].Text = @"Waiting...";
                            _maxLabel[j].Text = @"Waiting...";
                            _lastValueLabel[j].Text = @"Waiting...";
                        }                         
                        else
                        {
                            var lastPoint = value.DisplayPoints[cnt - 1].Y/value.DisplayYScale;
                            _lastValueLabel[j].Text = lastPoint.ToString(FormatPoint);
                        }
                    }
                    catch
                    {
                        _lastValueLabel[j].Text = @"ERROR";
                    }
                }
            }
            catch
            {
                _lastValueLabel[0].Text = @"ERROR";
            }
        }

        internal bool MaybeHide()
        {
            if (!Visible)
                return false;

            _wasSeen = true;
            Hide();
            return true;
        }

        internal bool MaybeShow()
        {
            if (!_wasSeen)
                return false;

            Show();
            _wasSeen = false;
            return true;
        }    
       
        private void Legend_Load(object sender, EventArgs e)
        {           
        }      
    }
}