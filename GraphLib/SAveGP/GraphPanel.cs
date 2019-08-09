using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GraphLib
{
    public delegate void SetUpdateLastXy(string s1, string s2);
    public delegate void SetFCursor(bool canuse);

    /// <summary>
    ///     Main Panel Access to graphics
    /// </summary>
    public partial class GraphPanel : UserControl
    {
        private readonly DPoint[] _displayPoints = new DPoint[5000];
        private readonly Ep[] _endPointData = new Ep[256];    
        private readonly Point[] _screenPoints = new Point[5000];
        private readonly ArrayList _temporaryArray = new ArrayList();
       
        private Color _backTabColour;
        private Grid _grid;
        
        private bool _endUpThread;      
        private bool _highlightTab = true;
        private bool _inLiveUpdate;
        private bool _inRedraw;
        private bool _isUpdating;
        private bool _shIpgName;
        private bool _showAllPanBoundaries;
        private bool _showBoundaries = true;
        private bool _gridOveride;
        private bool _bigPoints;

        private int _endPointCount;
        private int _lastCrossX;
        private int _lastCrossY;
        private int _lastCursorX;
        private int _lastCursorY;
        private double _lastMaxd = 1.0;
        private double _lastMind = 1.0;
        private double _lastX;

        private Thread _liveUpdate;
        private DPoint[] _liveUpdatePoints;
        private int _lucnt;
        private Cursor _rememberCursor = Cursors.Cross;
       
        private YAxisForm _yAxisConfiguration;

        internal int LastXyVx = 0;
        internal bool ShowLegendGraphId = true;
        internal PanelControl ThelastPanel;
       
        public int LiveUpdateTime { get; set; }
        public bool SecondRedrawRequired { get; set; }
        public int SingleWidth { get; set; }
        public GrTabPanel Gt { get; private set; }
        public MouseOps Gmo { get; private set; }
        public GraphData Gpg { get; private set; }
        public Options Gopt { get; private set; }
        public Params GraphParameters { get; }

        public Graphics NetGraph { get; set; }
       
        public bool ZoomPanAll { get; private set; }
     
        /// <summary>
        /// 
        /// </summary>
        public GraphPanel()
        {
            SingleWidth = 1;
            LiveUpdateTime = 1000;            
            ZoomPanAll = true;
            InitializeComponent();
            GraphParameters = new Params(this);
            InitialiseGraphPanel(true);
            NetGraph = CreateGraphics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTextMarker"></param>
        public GraphPanel(bool allowTextMarker)
        {
            SingleWidth = 1;
            LiveUpdateTime = 1000;         
            InitializeComponent();
            GraphParameters = new Params(this);
            InitialiseGraphPanel(allowTextMarker);
            NetGraph = CreateGraphics();
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTextMarker"></param>
        private void InitialiseGraphPanel(bool allowTextMarker)
        {
            Gmo = new MouseOps();

            GraphParameters.GraphTabPanel = new GrTabPanel();
            GraphParameters.GraphBoundary = new GrBoundary();
            GraphParameters.ContextMenu = new GpContextMenu(this);
            GraphParameters.GraphTabPanel.Setup(this);
            GraphParameters.GraphBoundary.Setup(this);
            GraphParameters.AllowTextMarker = allowTextMarker;

            Gopt = new Options(this);
            _grid = new Grid(this);
            Gmo.Setup(this);
            Gpg = new GraphData(this);

            Gt = GraphParameters.GraphTabPanel;

            _liveUpdate = new Thread(LiveUpdateThread);

            _liveUpdatePoints = new DPoint[1024];

            Gt.Initialise(TabPage1);

            GraphParameters.AllReadyToGo = true;
            SetSize(Width, Height);

            XValueLabel.Text = "";
            YValueLabel.Text = "";

            SetTriggerPanel(false);
        }

        public void InitialiseDefaultParameters(string firstTabName)
        {
            GraphParameters.HideNewTab();

            DontHlTab();
            InitPanel();
            GraphParameters.AllReadyToGo = true;
            GraphParameters.GraphicsSet = true;
            GraphParameters.DisplayGraphMover = false;
            Gt.RenameCst(firstTabName);
            GraphParameters.OriginalZeroAxis = false;
            SingleWidth = 1;
            GraphParameters.HighlightWidth = 3;
            GraphParameters.IgnoreEndValues = true;
            SecondRedrawRequired = true;

            InitEvents();
        }

        public void InitialiseDefaultParameters2(string firstTabName)
        {
            InitialiseDefaultParameters(firstTabName);
            SecondRedrawRequired = false;
            SetLegendGraphId(false);
            SetReducedMenuItems();
            HlTab(true);
            SetTriggerPanel(false);
        }


        public void SetLegendGraphId(bool onoff)
        {
            ShowLegendGraphId = onoff;
        }

        public void SetTriggerPanel(bool onoff)
        {
            TriggerPanel.Visible = onoff;
            RPMLabel.Visible = onoff;
        }

        public void SetTriggerPanelActive(bool onoff)
        {
            if (onoff)
            {
                TriggerPanel.BackColor = Color.DarkGray;
                TriggerPanel.Update();
                Update();
                Thread.Sleep(200);
                TriggerPanel.BackColor = Color.DarkGreen;
            }
            else
                TriggerPanel.BackColor = Color.Red;

            TriggerPanel.Update();
        }

        public void SetTriggerPanelActive(bool onoff, double rpm)
        {
            SetTriggerPanelActive(onoff);
            RPMLabel.Text = @"Current RPM: " + rpm.ToString("0.00");
            TriggerPanel.Update();
        }

        public void OffsetXyLabels(int dx)
        {
            XValueLabel.Left -= dx;
            YValueLabel.Left -= dx;
        }

        private void GraphPanel_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            _backTabColour = TabPage1.BackColor;
            TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        public void InitEvents()
        {
            allowImprtedGraphManipulationToolStripMenuItem.Click += Gopt.AllowGraphManip_Click;

            ToolNewTabButton.Click += Gopt.ToolNewTabButton_Click;
            UseFollowingCursoToolStripMenuItem.Click += Gopt.UseFollowingCursoToolStripMenuItem_Click;
            ShowGridToolStripMenuItem.Click += Gopt.ShowGridToolStripMenuItem_Click;
            ShowXAxisToolStripMenuItem.Click += Gopt.ShowXAxisToolStripMenuItem_Click;
            ShowYAxisToolStripMenuItem.Click += Gopt.ShowYAxisToolStripMenuItem_Click;
            ShowYAxisLegendToolStripMenuItem.Click += Gopt.ShowYAxisLegendToolStripMenuItem_Click;
            ShowXAxisLegendToolStripMenuItem.Click += Gopt.ShowXAxisLegendToolStripMenuItem_Click;
            AxisConfigurationToolStripMenuItem.Click += Gopt.AxisConfigurationToolStripMenuItem_Click;
            MoveImportedGraphToolStripMenuItem.Click += Gopt.MoveImportedGraphToolStripMenuItem_Click;
            SetPassCountTo1ToolStripMenuItem.Click += Gopt.SetPassCountTo1ToolStripMenuItem_Click;
            SetPassCountTo3ToolStripMenuItem.Click += Gopt.SetPassCountTo3ToolStripMenuItem_Click;
            SetPassCountTo5ToolStripMenuItem.Click += Gopt.SetPassCountTo5ToolStripMenuItem_Click;
            ClearGraphToolStripMenuItem.Click += Gopt.ClearGraphToolStripMenuItem_Click;
            AddPanelToolStripMenuItem.Click += Gopt.AddPanelToolStripMenuItem_Click;
            RemovePanelItem.Click += Gopt.RemovePanelItem_Click;
            ShowDifferenceMarkersToolStripMenuItem.Click += Gopt.ShowDifferenceMarkersToolStripMenuItem_Click;
            ShowGraphTooltipToolStripMenuItem.Click += Gopt.ShowGraphTooltipToolStripMenuItem_Click;
            ShowBoundaryValuesToolStripMenuItem.Click += Gopt.ShowBoundaryValuesToolStripMenuItem_Click;

            X2ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X3ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X4ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X5ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X6ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X7ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X8ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            X9ToolStripMenuItem.Click += Gopt.SetHighlightWidth;
            RemoveAllImportedFileGraphsToolStripMenuItem.Click +=
                Gopt.RemoveAllImportedFileGraphsToolStripMenuItem_Click;
            UseAlternateZeroYAxisToolStripMenuItem.Click += Gopt.UseAlternateZeroYAxisToolStripMenuItem_Click;
            NormalToolStripMenuItem.Click += Gopt.EndValueSize;
            X2ToolStripMenuItem1.Click += Gopt.EndValueSize;
            X3ToolStripMenuItem1.Click += Gopt.EndValueSize;
            X5ToolStripMenuItem1.Click += Gopt.EndValueSize;
            X10ToolStripMenuItem.Click += Gopt.EndValueSize;

            TabControl1.DrawItem += TabControl1_DrawItem;
            TabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        public void ForceBoundarStrings(bool on)
        {
            Gopt.ForceBoundaryStrings(on);
        }

        private void TabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            IndexUpdate();
        }

        public void SetWaitCursor()
        {
            _rememberCursor = Gt.CST().Page.Cursor;
            Gt.CST().Page.Cursor = Cursors.WaitCursor;
        }

        public void ResetTheCursor()
        {
            Gt.CST().Page.Cursor = _rememberCursor;
        }

        public void EndUpdateThread()
        {
            _endUpThread = true;
            _liveUpdate.Abort();
        }

        private void LiveUpdateThread()
        {
            var k = 0;
            var counter = 0;

            Thread.Sleep(5000);
            while (true)
            {
                if (_endUpThread)
                    break;

                if (counter > 10)
                {
                    counter = 0;
                    _isUpdating = false;
                    GraphParameters.HaltLiveUpdate = false;
                    GraphParameters.HaltUpdatefromSplit = false;
                    _inRedraw = false;
                }

                Thread.Sleep(LiveUpdateTime);

              
                if (_endUpThread)
                    break;
              
                if (_isUpdating || GraphParameters.HaltLiveUpdate || GraphParameters.HaltUpdatefromSplit || _inRedraw)
                {
                    counter += 1;
                    continue;
                }
                if (GraphParameters.LiveGraphs == null)
                    continue;

                counter = 0;

                for (var i = 0; i < GraphParameters.LiveGraphs.Length; i++)
                {
                    var cnt = 0;
                    if (GraphParameters.LiveGraphs[i].Lpts.Count > 1)
                    {
                        cnt = GraphParameters.LiveGraphs[i].Lpts.Count - 1;
                        if (GraphParameters.LiveGraphs[i].CurrentPt == 0)
                        {
                            var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].Graphid);
                            if (gs != null)
                                gs.PtCount = 0;
                        }
                        k = 0;
                        try
                        {                          
                            for (var j = GraphParameters.LiveGraphs[i].CurrentPt; j <= cnt; j++)
                            {
                                if (j >= GraphParameters.LiveGraphs[i].Lpts.Count)
                                {
                                    cnt = GraphParameters.LiveGraphs[i].Lpts.Count - 1;
                                    break;
                                }
                                if (k >= _liveUpdatePoints.Length)
                                {
                                    var maxL = _liveUpdatePoints.Length;
                                    maxL = maxL*2;
                                    Array.Resize(ref _liveUpdatePoints, maxL);
                                }

                                _liveUpdatePoints[k] = (DPoint) GraphParameters.LiveGraphs[i].Lpts[j];
                                k += 1;
                            }
                        }
// ReSharper disable once EmptyGeneralCatchClause
                        catch
                        {
                        }

                        GraphParameters.LiveGraphs[i].CurrentPt = cnt + 1;
                        if (k > 0)
                            UpdateGraph(GraphParameters.LiveGraphs[i].Graphid, _liveUpdatePoints, k);
                    }
               

                    if (cnt > 10000 && k > 0)
                    {
                        GraphParameters.LiveGraphs[i].Lpts.Clear();
                        GraphParameters.LiveGraphs[i].Lpts.Add(_liveUpdatePoints[0]);
                        GraphParameters.LiveGraphs[i].Lpts.Add(_liveUpdatePoints[0]);
                        GraphParameters.LiveGraphs[i].CurrentPt = 2;
                    }
                }
                if (!_isUpdating)
                {
                    GraphParameters.GraphicsSet = true;
                    RedrawAll();
                }
            }
        }

        public GraphSurface GetGraphSurfaceFromTag(int tagid)
        {

            for (var i = 0; i < Gt.TabCount; i++)
            {
                var tt = Gt.GetTab(i);
                if (tt?.Graphs == null)
                    continue;

                foreach (var t in tt.Graphs.Where(t => t.Tagid == tagid))
                {
                    return t;
                }
            }

            if (Gt.CST() == null)
                return null;

            if (Gt.CST().Graphs == null)
                return null;

            foreach (var t in Gt.CST().Graphs)
            {
                if (t.Tagid == tagid)
                    return t;
            }

            return null;
        }


        public void RedrawAll()
        {
            RedrawAll_now();
            if (SecondRedrawRequired)
                RedrawAll_now();
        }

        private void RedrawAll_now()
        {
            if (_inRedraw)
                return;

            if (!GraphParameters.GraphicsSet)
                return;

            _inRedraw = true;
            GraphParameters.XGridDrawn = false;
            if (Gt.CST().MainPan.NeedToSetScale)
                SetXscaleFromPoints();

            try
            {
                Gt.CST().GrSurface.Clear(Gt.CST().Page.BackColor);
                DrawTheGraph(Gt.CST().MainPan, true);
                Gt.CST().MainPan.Legend.UpdateMinMax(Gt.CST().MainPan);

                if (Gt.CST().SubPan != null)
                {           
                    foreach (var t in Gt.CST().SubPan)
                    {
                        DrawTheGraph(t, false);
                        t.Legend.UpdateMinMax(t);
                    }
                }
                Gt.CST().TabSurface.DrawImage(Gt.CST().DrawingImage, 0, 0);
            }
            catch
            {
                _inRedraw = false;
                GraphParameters.XGridDrawn = false;
            }

            _inRedraw = false;
            GraphParameters.XGridDrawn = false;
        }

        public void UpdateGraph(int tid, double[] bpts)
        {          
            var pts = new DPoint[bpts.Length];
            for (var i = 0; i < bpts.Length; i++)
            {
                pts[i].X = (float) bpts[i];
                pts[i].Y = 0;
                pts[i].StartPt = i == 0;
            }
            UpdateGraph(tid, pts);
        }

        public void UpdateGraph(int tid, DPoint[] pts, int ppcnt)
        {
            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;
            gs.DYscale = 1.0;
            try
            {
                if (gs.PtCount + ppcnt >= gs.MaxptCount)
                {
                   
                    gs.MaxptCount *= 2;
                    Array.Resize(ref gs.Dpts, gs.MaxptCount + 1);
                    
                }
                var cnt = 0;
                for (var i = 0; i < ppcnt; i++)
                {
                    gs.Dpts[gs.PtCount + cnt] = pts[i];
                    gs.Dpts[gs.PtCount + cnt].Y *= (float) gs.DYscale; // JIM LEGEND SCALE

                    cnt += 1;
                }
                gs.PtCount += cnt;
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        public void LimitGraph(int tid, double span)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);

            if (gs?.Dpts == null)
                return;

            int len = gs.PtCount;
            if (len < 100)
                return;

            double current = gs.Dpts[len - 1].X - gs.Dpts[0].X;
            if (current < span)
                return;

            var count = 0;
            while (current >= span)
            {
                ++count;
                if (count >= len - 1)
                    return;
               current = gs.Dpts[len - 1].X - gs.Dpts[count].X;
            }
            len = len - count;
            DPoint[] pts = new DPoint[len];
            for (var i = count; i < gs.PtCount; ++i)
                pts[i-count] = gs.Dpts[i];

           
            UpdateGraph(tid, pts);

        }
        public void UpdateGraph(int tid, DPoint[] pts)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;

            var ptcnt = pts.Length;
            gs.DYscale = 1.0;
            try
            {
                if (ptcnt >= gs.MaxptCount)
                {
                    if (gs.MaxptCount > 100000)
                        gs.MaxptCount += 100000;
                    else
                        gs.MaxptCount *= 2;

                    gs.Dpts = new DPoint[gs.MaxptCount];
                }

                gs.PtCount = ptcnt;

               
                for (var i = 0; i < ptcnt; i++)
                    gs.Dpts[i] = pts[i];
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        public void DrawTheGraph()
        {
            if (!GraphParameters.GraphicsSet)
                return;

            if (Gt.CST().MainPan.Taglist == null)
                return;
            DrawTheGraph(Gt.CST().MainPan, true);
        }

        public void DrawTheGraph(PanelControl pan, bool isItMain)
        {
            if (Math.Abs(pan.Xscale) < 0.00000001 || Math.Abs(pan.Yscale) < 0.00000001)
                return;
            GraphParameters.GraphicsSet = true;
            UpdatePoints(pan, isItMain);
        }

        private void UpdatePoints(PanelControl pan, bool isItMain)
        {         
            if (_isUpdating)
                return;

            double min;
            double max;

            _endPointCount = 0;

            var pp = new Pen(Color.Black);

            _isUpdating = true;

            if (pan.Taglist == null)
            {
                _isUpdating = false;
                return;
            }

            pan.GrSurface.Clear(pan.GPan.BackColor);

            if (GraphParameters.Panning)
                pan.PanSurface.Clear(pan.GPan.BackColor);

            SetGetMinMax(pan, out min, out max);

            _grid.DrawGrid(pan);
            if (_showBoundaries)
            {
                if (isItMain || _showAllPanBoundaries)
                {
                    GraphParameters.GraphBoundary.FillBoundaries(pan);
                    GraphParameters.GraphBoundary.DrawBoundaries(pan);
                }
            }
            GraphParameters.GraphBoundary.Markers.DrawMarkers(pan);


            for (var i = 0; i < pan.Taglist.Length; i++)
            {
                if (!pan.Taglist[i].Visible)
                    continue;

                var gs = GetGraphSurfaceFromTag(pan.Taglist[i].Tag);

                if (gs == null)
                    continue;

                if (gs.Gtype != Graphtype.Graph && gs.Gtype != Graphtype.Live)
                    continue;

                pp.Color = pan.Taglist[i].Colour.Color;
                pp.Width = pan.Taglist[i].Highlight ? GraphParameters.HighlightWidth : SingleWidth;

// ReSharper disable once CompareOfFloatsByEqualityOperator
                if (pan.Taglist[i].DispYscale != gs.DYscale)
                {
                    var yscl = pan.Taglist[i].DispYscale/gs.DYscale;                    
                    for (var k = 0; k < gs.PtCount; k++)
                        gs.Dpts[k].Y *= (float) yscl;
                    gs.DYscale = pan.Taglist[i].DispYscale;
                }

                try
                {
                    DrawData(pan, gs, pp, i);
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }


            for (var i = 0; i < _endPointCount; i++)
            {
                pp.Color = _endPointData[i].Col;
                DrawEndValue(pan, _endPointData[i].Gs, _endPointData[i].X, _endPointData[i].Y, pp);
            }

            ShowLastXyValue();
            if (_showBoundaries)
            {
                if (isItMain || _showAllPanBoundaries)
                    GraphParameters.GraphBoundary.Drawtooltips(pan);
            }

            pan.PanSurface.DrawImage(pan.DrawingImage, pan.Xpan, pan.Ypan);
            _isUpdating = false;
            RefreshDrawing(GraphParameters.CursorPan);
        }

        private void SetGetMinMax(PanelControl pan, out double min, out double max)
        {
            var gs = GetMasterGraph(pan);

            if (pan.YAxisType == YaxisType.Free && !pan.Zooming)
            {
                var tid = GetMasterTag(pan);
                if (tid < 0)
                {
                    min = pan.YAxisMin;
                    max = pan.YAxisMax;
                }
                else
                {
                    max = pan.Taglist[tid].DispPoints[0].Y;
                    min = pan.Taglist[tid].DispPoints[0].Y;
                 
                    for (var j = 1; j < pan.Taglist[tid].ScrPCount; j++)
                    {
                        if (pan.Taglist[tid].DispPoints[j].Y > max)
                            max = pan.Taglist[tid].DispPoints[j].Y;
                        if (pan.Taglist[tid].DispPoints[j].Y < min)
                            min = pan.Taglist[tid].DispPoints[j].Y;
                    }
                    pan.YAxisMin = min;
                    pan.YAxisMax = max;
                }
            }
            else if (pan.YAxisType == YaxisType.MinMax || pan.Zooming)
            {
                min = pan.YAxisMin;
                max = pan.YAxisMax;
            }
            else
            {
                max = GetMaxOnTab(1, pan);
                min = GetMinOnTab(1, pan);
                pan.YAxisMin = min;
                pan.YAxisMax = max;
            }

            if ((max - min) < 1E-08)
            {
                max *= 1.05;
                min *= 0.95;
            }


            gs.Maxd = max;
            gs.Mind = min;
            _lastMaxd = max;
            _lastMind = min;
        }


        private void DrawData(PanelControl pan, GraphSurface gs, Pen pp, int gid)
        {                     
            pan.Taglist[gid].ScrPCount = 0;            
            var scnt = ScaleToScreen(pan, gs, gs.Dpts, gs.PtCount, 2000);

            if (scnt > 2)
            {
                if (pan.Taglist[gid].ScrPCount + scnt > pan.Taglist[gid].MaxscrPCount)
                {
                    Array.Resize(ref pan.Taglist[gid].ScrPoints, pan.Taglist[gid].ScrPCount + scnt + 1);
                    Array.Resize(ref pan.Taglist[gid].DispPoints, pan.Taglist[gid].ScrPCount + scnt + 1);
                }

                for (var i = 0; i < scnt; i++)
                {
                    pan.Taglist[gid].ScrPoints[pan.Taglist[gid].ScrPCount + i].X = _screenPoints[i].X;
                    pan.Taglist[gid].ScrPoints[pan.Taglist[gid].ScrPCount + i].Y = _screenPoints[i].Y;
                    pan.Taglist[gid].DispPoints[pan.Taglist[gid].ScrPCount + i].X = _displayPoints[i].X;
                    pan.Taglist[gid].DispPoints[pan.Taglist[gid].ScrPCount + i].Y = _displayPoints[i].Y;
                }
                pan.Taglist[gid].ScrPCount += scnt;
            }



            if (pan.Taglist[gid].AsPoint || _gridOveride)
                DrawAsPoints(pan, pp, gid);
            else
                DrawAsLines(pan, pp, gid);


            if (!pan.Taglist[gid].Highlight)
                return;

            if (GraphParameters.HighlightPoint)
            {               
                for (var j = 0; j < scnt; j++)
                {
                    if (_screenPoints[j].X < GraphParameters.HighlightX) 
                        continue;
                    scnt = j;
                    break;
                }
            }

            if (_endPointCount >= 250) 
                return;
            --scnt;
            if (scnt < 0)
                return;
            _endPointData[_endPointCount].X = _screenPoints[scnt].X;
            _endPointData[_endPointCount].Y = _screenPoints[scnt].Y;
            _endPointData[_endPointCount].Col = pp.Color;
            _endPointData[_endPointCount].Gs = gs;
            _endPointCount += 1;
        }

        private void DrawAsPoints(PanelControl pan, Pen pp, int gid)
        {
            int size = 1;
            if (_gridOveride || _bigPoints)
            {
                size = 3;
            }
            for (var i = 0; i < pan.Taglist[gid].ScrPCount; i++)
            {
                pan.GrSurface.DrawLine(pp, pan.Taglist[gid].ScrPoints[i].X - size, pan.Taglist[gid].ScrPoints[i].Y - size,
                    pan.Taglist[gid].ScrPoints[i].X + size, pan.Taglist[gid].ScrPoints[i].Y + size);
                pan.GrSurface.DrawLine(pp, pan.Taglist[gid].ScrPoints[i].X - size, pan.Taglist[gid].ScrPoints[i].Y + size,
                    pan.Taglist[gid].ScrPoints[i].X + size, pan.Taglist[gid].ScrPoints[i].Y - size);
            }
        }

        private static void DrawAsLines(PanelControl pan, Pen pp, int gid)
        {
            var hlcnt = 0;
            var hlcnt2 = 0;
            var hw = (int) pp.Width;
            for (var i = 3; i < pan.Taglist[gid].ScrPCount - 1; i++)
            {
                if (pan.Taglist[gid].ScrPoints[i + 1].X < pan.Taglist[gid].ScrPoints[i].X)
                    hlcnt += 1;
            }

            if (hlcnt != 0)
            {
                pp.Width = 1;
                pp.DashStyle = DashStyle.Dot;
            }


            for (var i = 0; i < pan.Taglist[gid].ScrPCount - 1; i++)
            {
                if (pan.Taglist[gid].ScrPoints[i + 1].X < pan.Taglist[gid].ScrPoints[i].X)
                {
                    hlcnt2 += 1;
                    if (hlcnt2 == hlcnt)
                    {
                        pp.Width = hw;
                        pp.DashStyle = DashStyle.Solid;
                    }

                    continue;
                }               
                
                pan.GrSurface.DrawLine(pp, pan.Taglist[gid].ScrPoints[i].X, pan.Taglist[gid].ScrPoints[i].Y,
                    pan.Taglist[gid].ScrPoints[i + 1].X, pan.Taglist[gid].ScrPoints[i + 1].Y);
            }
        }


        private void DrawEndValue(PanelControl pan, GraphSurface gs, int x, int y, Pen pp)
        {
            var str = "";
            var siz = 8;

            if (GraphParameters.IgnoreEndValues)
                return;

            siz = (int) (siz*GraphParameters.EndValueScale);
            var f = new Font("timesnewroman", siz);

            var pos = gs.PtCount - 1;
            if (pos >= 0)
            {
                double valu = gs.Dpts[pos].Y;
                valu = valu/gs.DYscale;
                str = valu.ToString("0.00");
            }

            var dx = (int) (str.Length*siz*0.9);
            x = x - dx;
            y = y - 10;
            var dy = (int) (15*GraphParameters.EndValueScale);
            dy += 5;

            if (y < 0)
            {
                if (y + dy > 0)
                    y = 0;
            }
            if (y < pan.GPan.Height)
            {
                if (y + dy > pan.GPan.Height)
                    y = pan.GPan.Height - dy;
            }

            pan.GrSurface.FillRectangle(Brushes.LightGray, x, y, dx, dy);
            pp.Width = 2;
            pan.GrSurface.DrawRectangle(pp, x, y, dx, dy);
            pan.GrSurface.DrawString(str, f, Brushes.Black, x + 2, y + 5);
        }

        private void ShowLastXyValue()
        {         
            var dumI = 0;
            double dumD = 0;
            if (!GraphParameters.DisplayActiveValues)
                return;

            GraphParameters.GetDataValueFromPosition(ThelastPanel, LastXyVx, ref dumI, ref dumD);

            var str1 = Params.SetPlaces(GraphParameters.LastMarkerTime);
            var str = GraphParameters.LastMarkerTime.ToString(str1);
            var str12 = Params.SetPlaces(GraphParameters.LastMarkerValue);
            var str2 = GraphParameters.LastMarkerTime.ToString(str12);
            DoupdXy("Time: " + str, "Value: " + str2);
        }

        public void RefreshDrawing(PanelControl pan)
        {
            if (pan == null)
                return;

            if (_isUpdating)
                return;

            if (!GraphParameters.GraphicsSet)
                return;

            if (!GraphParameters.CursorActive)
                return;

            if (pan.Taglist == null)
                return;

            var wid = pan.GPan.Width;
            var hei = pan.GPan.Height;

            try
            {
                _isUpdating = true;
                var found = pan.Taglist.Any(t => t.Visible);

                if (!found)
                {
                    _isUpdating = false;
                    return;
                }
              
                if (GraphParameters.BoxCursor)
                {
                    pan.PanSurface.DrawImage(pan.DrawingImage, 0, 0);
                    var dx = GraphParameters.BoxBotX - GraphParameters.BoxTopX;
                    var dy = GraphParameters.BoxBotY - GraphParameters.BoxTopY;
                    pan.RectVSurface.DrawLine(Pens.SlateGray, 0, 0, 0, Math.Abs(dy));
                    pan.RectHSurface.DrawLine(Pens.SlateGray, 0, 0, Math.Abs(dx), 0);
                    int x1;
                    int x2;
                    if (dx > 0)
                    {
                        x1 = GraphParameters.BoxTopX;
                        x2 = GraphParameters.BoxBotX;
                    }
                    else
                    {
                        x2 = GraphParameters.BoxTopX;
                        x1 = GraphParameters.BoxBotX;
                    }
                    int y1;
                    int y2;
                    if (dy > 0)
                    {
                        y1 = GraphParameters.BoxTopY;
                        y2 = GraphParameters.BoxBotY;
                    }
                    else
                    {
                        y2 = GraphParameters.BoxTopY;
                        y1 = GraphParameters.BoxBotY;
                    }
                    Rectangle r = default(Rectangle);
                    r.X = 0;
                    r.Y = 0;
                    r.Width = Math.Abs(dx);
                    r.Height = 1;

                    pan.PanSurface.DrawImage(pan.RectHImage, x1, y1, r, GraphicsUnit.Pixel);
                    pan.PanSurface.DrawImage(pan.RectHImage, x1, y2, r, GraphicsUnit.Pixel);
                    r.Width = 1;
                    r.Height = Math.Abs(dy);
                    pan.PanSurface.DrawImage(pan.RectVImage, x1, y1, r, GraphicsUnit.Pixel);
                    pan.PanSurface.DrawImage(pan.RectVImage, x2, y1, r, GraphicsUnit.Pixel);
                }
                else
                {
                  
                    ClearCursors(pan);
                    if (GraphParameters.DisplayCursor)
                    {
                        pan.PanSurface.DrawLine(Pens.SlateGray, GraphParameters.CursorX - 1, 0,
                            GraphParameters.CursorX - 1, hei);
                        pan.PanSurface.DrawLine(Pens.SlateGray, 0, GraphParameters.CursorY - 1, wid,
                            GraphParameters.CursorY - 1);
                    }
                    if (GraphParameters.DisplayGraphMover)
                        pan.PanSurface.FillRectangle(Brushes.Blue, GraphParameters.CrossX, GraphParameters.CrossY, 6, 6);

                    _lastCrossX = GraphParameters.CrossX;
                    _lastCrossY = GraphParameters.CrossY;
                    _lastCursorX = GraphParameters.CursorX;
                    _lastCursorY = GraphParameters.CursorY;                  
                }
                
            }
            catch
            {
                _isUpdating = false;
            }
            _isUpdating = false;
        }

        public GraphSurface GetMasterGraph(PanelControl pan)
        {  
            if (pan.Taglist == null)
                return null;

            var len = pan.Taglist.Length;
            if (len <= 0)
                return null;

            for (var i = 0; i < len; i++)
            {
                if (pan.Taglist[i].Master)
                    return GetGraphSurfaceFromTag(pan.Taglist[i].Tag);
            }

            pan.Taglist[0].Master = true;

            return GetGraphSurfaceFromTag(pan.Taglist[0].Tag);
        }

        public int GetMasterGraphId()
        {
            return GetMasterGraphId(Gt.CST().MainPan);
        }

        public static int GetMasterGraphId(PanelControl pan)
        {
            if (pan.Taglist == null)
                return -1;
            var len = pan.Taglist.Length;
            if (len <= 0)
                return -1;

            for (var i = 0; i < len; i++)
            {
                if (pan.Taglist[i].Master)
                    return pan.Taglist[i].Tag;
            }
            return 0;
        }

        public static int GetMasterTag(PanelControl pan)
        {
            if (pan.Taglist == null)
                return -1;
            var len = pan.Taglist.Length;
            if (len <= 0)
                return -1;

            for (var i = 0; i < len; i++)
            {
                if (pan.Taglist[i].Master)
                    return i;
            }
            return 0;
        }

        public double GetMaxOnTab(int stepcnt, PanelControl pan)
        {
            var max = 0.0;
            var firstTime = true;

            if (pan.Taglist == null)
                return 1.0;

            foreach (var t in pan.Taglist.Where(t => t.Visible))
            {
                if (!t.UseForYscale)
                    continue;
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;
                if (GrBoundary.IsAnyMarkerorBoundary(gs.Gtype))
                    continue;
               
                for (var j = 0; j < t.ScrPCount; j += stepcnt)
                {
                    if (firstTime)
                    {
                        max = t.DispPoints[j].Y;
                        firstTime = false;
                    }
                    if (max < t.DispPoints[j].Y)
                        max = t.DispPoints[j].Y;
                }
            }


            if (max < 1E-05)
                max = 1.0;
            return max;
        }

        public double GetMinOnTab(int stepcnt, PanelControl pan)
        {
            var min = 0.0;
            var firstTime = true;

            if (pan.Taglist == null)
                return 0.0;


            foreach (var t in pan.Taglist.Where(t => t.Visible))
            {
                if (!t.UseForYscale)
                    continue;
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;
                if (GrBoundary.IsAnyMarkerorBoundary(gs.Gtype))
                    continue;


                for (var j = 0; j < t.ScrPCount; j += stepcnt)
                {
                    if (firstTime)
                    {
                        min = t.DispPoints[j].Y;
                        firstTime = false;
                    }
                    if (min > t.DispPoints[j].Y)
                        min = t.DispPoints[j].Y;
                }
            }

            return min;
        }

        private int ScaleToScreen(PanelControl pan, GraphSurface ggs, DPoint[] pts, int pCount, int screenDx)
        {
            var cnt = 0;
            var ymin = 0;
            var ymax = 0;

            if (ggs == null)
                return 0;

            if (pCount <= 2)
                return 0;

            var xscl = pan.NeedToSetScale ? ggs.Xscale : pan.Xscale;


            //find start
            var startpt = GetDataStart(pan, pts, pCount, xscl);

            //find end
            var endpt = GetDataEnd(startpt, pan, pts, pCount, xscl);
            if (screenDx < 100)
                screenDx = 100;

            var stepcnt = (endpt - startpt)/screenDx;
            if (stepcnt <= 0)
                stepcnt = 1;

            var max = _lastMaxd;
            var min = _lastMind;
            var jhei = pan.GPan.Height - 5;

            try
            {
                cnt = 0;

                GraphParameters.Yscale = ((max - min)/(jhei));
                var setminmax = true;

               
                for (var j = startpt; j <= endpt; j++)
                {
                    var x = (int) ((pts[j].X*xscl) - pan.Xoffset);
                    double yy = pts[j].Y;

                    var tempP = ((yy - min)/GraphParameters.Yscale)*pan.Yscale;
                    var y = (int) (jhei - tempP - pan.Yoffset);

                    if (cnt == 0)
                    {
                        _screenPoints[cnt].X = x;
                        _screenPoints[cnt].Y = y;
                        _displayPoints[cnt].X = pts[j].X;
                        _displayPoints[cnt].Y = (float) yy;
                        ++cnt;
                        continue;
                    }

                    if (setminmax)
                    {
                        ymin = y;
                        ymax = y;
                        setminmax = false;
                    }
                    else
                    {
                        if (y > ymax)
                            ymax = y;
                        if (y < ymin)
                            ymin = y;
                    }


                    if ((j%stepcnt) != 0) 
                        continue;

                    _screenPoints[cnt].X = x;
                    _screenPoints[cnt].Y = ymin;
                    _displayPoints[cnt].X = pts[j].X;
                    _displayPoints[cnt].Y = (float) yy;
                    ++cnt;
                    if (ymin != ymax)
                    {
                        _screenPoints[cnt].X = x;
                        _screenPoints[cnt].Y = ymax;
                        _displayPoints[cnt].X = pts[j].X;
                        _displayPoints[cnt].Y = (float) yy;
                        cnt += 1;
                    }

                    setminmax = true;
                }
            }
            catch
            {               
                return cnt - 1;
            }

            return cnt;
           // return cnt - 1;
        }

        public void DrawCross(PanelControl pan, int x, double value)
        {
            var tid = GetMasterTag(pan);
            if (tid < 0)
                return;

            try
            {
                double hei = pan.GPan.Height - 5;


                var min = pan.YAxisMin;
                var max = pan.YAxisMax;
// ReSharper disable once CompareOfFloatsByEqualityOperator
                if (max == min)
                {
                    max *= 1.05;
                    min *= 0.95;
                }
                var tempP = value - min;
                tempP = tempP/((max - min)/(hei));
                tempP *= pan.Yscale;
                value = hei - tempP - pan.Yoffset;

                GraphParameters.CrossX = x - 3;
                GraphParameters.CrossY = (int) (value - 3);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }


        private void DoupdXy(string s1, string s2)
        {
            SetUpdateLastXy d = DoTheXy;
            try
            {
                Invoke(d, s1, s2);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void DoTheXy(string s1, string s2)
        {
            XValueLabel.Text = s1;
            YValueLabel.Text = s2;
        }

        public void ClearCursors(PanelControl pan)
        {
         
            try
            {
                ClearCursorsCatch(pan);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {    
                
            }
           
        }
      
        private void ClearCursorsCatch(PanelControl pan)
        {
            var rect = default(Rectangle);
            var wid = pan.GPan.Width;
            var hei = pan.GPan.Height;

            if (GraphParameters.DisplayCursor)
            {
                rect.X = _lastCursorX - 1;
                rect.Y = 0;
                rect.Width = 2;
                rect.Height = hei;
                pan.PanSurface.DrawImage(pan.DrawingImage, _lastCursorX - 1, 0, rect, GraphicsUnit.Pixel);

                rect.X = 0;
                rect.Y = _lastCursorY - 1;
                rect.Width = wid;
                rect.Height = 2;
                pan.PanSurface.DrawImage(pan.DrawingImage, 0, _lastCursorY - 1, rect, GraphicsUnit.Pixel);
            }

            if (!GraphParameters.DisplayGraphMover) 
                return;

            rect.X = _lastCrossX - 2;
            rect.Y = _lastCrossY - 2;
            rect.Width = 10;
            rect.Height = 10;
            pan.PanSurface.DrawImage(pan.DrawingImage, _lastCrossX - 2, _lastCrossY - 2, rect, GraphicsUnit.Pixel);
        }

        private static int GetDataStart(PanelControl pan, DPoint[] dpts, int ptCount, double xscl)
        {
            var x = (int) ((dpts[0].X*xscl) - pan.Xoffset);

            if (x >= 0)
                return 0;

            var endpt = ptCount - 1;
            var laststartpt = 0;
            var startpt = endpt/2;

            while (true)
            {
                x = (int) ((dpts[startpt].X*xscl) - pan.Xoffset);

                if (x == 0)
                    break;

                if (x > 0)
                {
                    if (Math.Abs(endpt - laststartpt) <= 2)
                        break;

                    endpt = startpt;
                    startpt = startpt - ((endpt - laststartpt)/2);
                }
                else if (x < 0)
                {
                    if ((endpt - startpt) < 20)
                        break;

                    laststartpt = startpt;
                    if (Math.Abs(endpt - startpt) <= 2)
                        break;
                    startpt = startpt + ((endpt - startpt)/2);
                }
                if (startpt == 0)
                    break;
            }

            return startpt;
        }

        private static int GetDataEnd(int startpt, PanelControl pan, DPoint[] dpts, int ptCount, double xscl)
        {
            var endpt = ptCount - 1;
            var x = (int) ((dpts[endpt].X*xscl) - pan.Xoffset);

            if (x <= 2000)
                return endpt;

            var lastendpt = endpt;
            endpt = endpt - ((endpt - startpt)/2);
            while (true)
            {
                x = (int) ((dpts[endpt].X*xscl) - pan.Xoffset);

                if (Math.Abs(x - 2000) <= 2)
                    break;
                if (x > 2000)
                {
                    if ((endpt - startpt) < 20)
                        break;
                    lastendpt = endpt;
                    if (Math.Abs(endpt - startpt) <= 2)
                        break;
                    endpt = endpt - ((endpt - startpt)/2);
                }
                else if (x < 2000)
                {
                    if (ptCount - endpt < 20)
                    {
                        endpt = ptCount - 1;
                        break;
                    }
                    if (Math.Abs(lastendpt - endpt) <= 2)
                        break;

                    endpt = endpt + ((lastendpt - endpt)/2);
                }
            }

            return endpt;
        }


        private void SetXscaleFromPoints()
        {
            double minvalu = 0;
            GraphSurface gs;
            var maxvalu = 0.0;
            var pan = Gt.CST().MainPan;

            if (pan.Taglist == null)
                return;

           // foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Visible))
            foreach (var t in Gt.CST().MainPan.Taglist)
            {
                gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs?.Dpts == null)
                    continue;
                var lastX = (float) 0.0;
                for (var j = 0; j < gs.PtCount; j++)
                {
                    if (float.IsInfinity(gs.Dpts[j].X))
                        gs.Dpts[j].X = lastX;
                    if (float.IsNaN(gs.Dpts[j].X))
                        gs.Dpts[j].X = lastX;
                    if (gs.Dpts[j].X > maxvalu)
                        maxvalu = gs.Dpts[j].X;
                    lastX = gs.Dpts[j].X;
                }
            }

            switch (pan.XAxisType)
            {
                case XaxisType.MinMax:
                    maxvalu = pan.XAxisMax - pan.XAxisMin;
                    break;
                case XaxisType.SetSpan:
                    minvalu = maxvalu - pan.XAxisSpan;
                    if (minvalu < 0)
                        minvalu = 0.0;
                    maxvalu = pan.XAxisSpan;
                    break;
            }

            maxvalu *= 1.01;
            // extra 1% so that right edge of graph is not hard against panel edge
            if (maxvalu < 1E-06)
                maxvalu = 1.0;

            Gt.CST().MainPan.Xscale = Gt.CST().MainPan.GPan.Width/maxvalu;
            switch (Gt.CST().MainPan.XAxisType)
            {
                case XaxisType.MinMax:
                    Gt.CST().MainPan.Xoffset = (Gt.CST().MainPan.XAxisMin*Gt.CST().MainPan.Xscale);
                    break;
                case XaxisType.SetSpan:
                    Gt.CST().MainPan.Xoffset = (minvalu*Gt.CST().MainPan.Xscale);
                    break;
            }

            if (Gt.CST().SubPan != null)
            {
                foreach (var t in Gt.CST().SubPan)
                {
                    t.Xscale = Gt.CST().MainPan.Xscale;
                    t.Xoffset = Gt.CST().MainPan.Xoffset;
                }
            }

            foreach (var t in Gt.CST().MainPan.Taglist)
            {
                gs = GetGraphSurfaceFromTag(t.Tag);
                if (gs == null)
                    continue;
                gs.Xscale = Gt.CST().MainPan.Xscale;
            }
        }

        private static void SetPointsScale(ref PanelControl pan, bool onoff)
        {
            if (pan == null)
                return;
            pan.NeedToSetScale = onoff;
        }

        public void SetTheGrid(bool onoff)
        {
            GraphParameters.DrawTheGrid = onoff;
        }
        public void SetGridStepOveride(bool onoff)
        {
            _grid.SetStepOveride(onoff);
            _gridOveride = onoff;
        }

        public void SetXGridStepOveride(bool onoff)
        {
            _grid.SetXStepOveride(onoff);          
        }

        public void SetBigPoints(bool onoff)
        {
            
            _bigPoints = onoff;
        }

        public void SetTheXaxis(bool onoff)
        {
            GraphParameters.DrawTheXaxis = onoff;
            ShowXAxisToolStripMenuItem.Checked = onoff;
        }

        public void SetTheYaxis(bool onoff)
        {
            GraphParameters.DrawTheYaxis = onoff;
            ShowYAxisToolStripMenuItem.Checked = onoff;
        }

        public void SetTheYaxisLegend(bool onoff)
        {
            GraphParameters.DrawTheYaxisLegend = onoff;
        }

        public void SetTheXaxisLegend(bool onoff)
        {
            GraphParameters.DrawTheXaxisLegend = onoff;
        }


        public void ResetX()
        {

            if (Gt.CST().SubPan != null)
            {
                for (var i = 0; i < Gt.CST().SubPan.Length; i++)
                {
                    SetPointsScale(ref Gt.CST().SubPan[i], true);
                    ResetSingleX(ref Gt.CST().SubPan[i]);
                }
            }
            SetPointsScale(ref Gt.CST().MainPan, true);
            ResetSingleX(ref Gt.CST().MainPan);
        }


        public void ResetY(PanelControl pan)
        {
            SetPointsScale(ref pan, true);
            ResetSingleY(ref pan);
        }


        public void Reset()
        {

            if (Gt.CST().SubPan != null)
            {
                for (var i = 0; i < Gt.CST().SubPan.Length; i++)
                {
                    SetPointsScale(ref Gt.CST().SubPan[i], true);
                    ResetSingleX(ref Gt.CST().SubPan[i]);
                    ResetSingleY(ref Gt.CST().SubPan[i]);
                }
            }
            SetPointsScale(ref Gt.CST().MainPan, true);
            ResetSingleX(ref Gt.CST().MainPan);
            ResetSingleY(ref Gt.CST().MainPan);
            RedrawAll();
            // Again to force a real reset
            RedrawAll();
        }

        private void ResetSingleX(ref PanelControl pan)
        {
            pan.Xoffset = 0;
            pan.Xpan = 0;
            pan.Xscale = 1.0;
            pan.Zooming = false;
            GraphParameters.LastXscale = 1.0;
        }

        private void ResetSingleY(ref PanelControl pan)
        {
            pan.Yoffset = 0;
            pan.Ypan = 0;
            pan.Yscale = 1.0;
            pan.Zooming = false;
            GraphParameters.LastYscale = 1.0;
        }

        public void UpdateLegend()
        {
            if (Gt.CST().MainPan == null) 
                return;
            Gt.CST().MainPan.Legend.UpdateLegend(Gt.CST().MainPan);
            if (Gt.CST().SubPan == null) 
                return;
            foreach (var t in Gt.CST().SubPan)
                t.Legend.UpdateLegend(t);
        }

        public void HideLegend()
        {
            if (Gt.CST().MainPan == null) 
                return;
            if (Gt.CST().MainPan.Legend.MaybeHide())
                SetlegendTick(ref Gt.CST().MainPan, false);

            if (Gt.CST().SubPan == null) 
                return;
            for (var i = 0; i < Gt.CST().SubPan.Length; i++)
            {
                if (Gt.CST().SubPan[i].Legend.MaybeHide())
                    SetlegendTick(ref Gt.CST().SubPan[i], false);
            }
        }

        public void ShowLegend(bool showIt)
        {
            try
            {
                if (showIt)
                    ShowLegend();
                else
                    HideLegend();
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        public void ShowLegend()
        {
            if (Gt.CST().MainPan == null) 
                return;
            if (Gt.CST().MainPan.Legend.MaybeShow())
                SetlegendTick(ref Gt.CST().MainPan, true);

            if (Gt.CST().SubPan == null) 
                return;
            for (var i = 0; i < Gt.CST().SubPan.Length; i++)
            {
                if (Gt.CST().SubPan[i].Legend.MaybeShow())
                    SetlegendTick(ref Gt.CST().SubPan[i], true);
            }
        }

        public void SetSize(int width, int height)
        {
            GraphParameters.HaltLiveUpdate = true;
            Gt.SetSizes(width, height);
            GraphParameters.HaltLiveUpdate = false;
        }

        public void SetSelectedTab(int id)
        {
            HideLegend();
            TabControl1.SelectedIndex = id;
            IndexUpdate();
        }

        public void RemoveGraph(int tagid)
        {
            var id = -1;
            var cnt = Gt.CST().Graphs.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (Gt.CST().Graphs[i].Tagid != tagid) 
                    continue;
                id = i;
                break;
            }
            if (id == -1)
                return;

            cnt -= 1;

            for (var i = id; i < cnt; i++)
                Gt.CST().Graphs[i] = Gt.CST().Graphs[i + 1];

            Array.Resize(ref Gt.CST().Graphs, cnt);
            id = -1;
            cnt = Gt.CST().MainPan.Taglist.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (Gt.CST().MainPan.Taglist[i].Tag != tagid) 
                    continue;
                id = i;
                break;
            }
            if (id == -1)
                return;

            for (var i = 0; i < cnt; i++)
                Gt.CST().MainPan.Taglist[i].Master = false;

            Gt.CST().MainPan.Taglist[0].Master = true;

            cnt -= 1;
            for (var i = id; i < cnt; i++)
                Gt.CST().MainPan.Taglist[i] = Gt.CST().MainPan.Taglist[i + 1];


            Array.Resize(ref Gt.CST().MainPan.Taglist, cnt);
            id = -1;
            for (var i = 0; i < Gt.CST().MainPan.ConMenu.MenuItems.Count; i++)
            {
                var mt = (MenuTag) Gt.CST().MainPan.ConMenu.MenuItems[i].Tag;
                if (mt.GraphTag != tagid) 
                    continue;
                id = i;
                break;
            }
            if (id != -1)
                Gt.CST().MainPan.ConMenu.MenuItems.RemoveAt(id);
        }

        public void DoYaxisMenu(int paneltag)
        {
            int pl;
            int pw;
            if (_yAxisConfiguration == null)
            {
                _yAxisConfiguration = new YAxisForm(this) {Top = 1000, Left = 500};
                _yAxisConfiguration.Show();
                _yAxisConfiguration.Hide();
            }

            if (paneltag == 0)
            {
                pl = Gt.CST().MainPan.GPan.Left;
                pw = Gt.CST().MainPan.GPan.Width;
                _yAxisConfiguration.SetGraphPanel(this, Gt.CST().MainPan);
                _yAxisConfiguration.Top = Gt.CST().MainPan.GPan.Top + GraphParameters.MouseY;
                _yAxisConfiguration.Left = Gt.CST().MainPan.GPan.Left + GraphParameters.MouseX;
                if (_yAxisConfiguration.Left + _yAxisConfiguration.Width > pl + pw)
                    _yAxisConfiguration.Left = pl + pw - _yAxisConfiguration.Width - 50;
                _yAxisConfiguration.Show();
            }
            else
            {
                if (Gt.CST().SubPan == null) 
                    return;
                paneltag = paneltag - 1;
                if (paneltag < 0 || paneltag >= Gt.CST().SubPan.Length) 
                    return;
                _yAxisConfiguration.SetGraphPanel(this, Gt.CST().SubPan[paneltag]);
                pl = Gt.CST().SubPan[paneltag].GPan.Left;
                pw = Gt.CST().SubPan[paneltag].GPan.Width;
                _yAxisConfiguration.Top = Gt.CST().SubPan[paneltag].GPan.Top + GraphParameters.MouseY;
                _yAxisConfiguration.Left = Gt.CST().SubPan[paneltag].GPan.Left + GraphParameters.MouseX;
                if (_yAxisConfiguration.Left + _yAxisConfiguration.Width > pl + pw)
                    _yAxisConfiguration.Left = pl + pw - _yAxisConfiguration.Width - 50;
                _yAxisConfiguration.Show();
            }
        }

        public void SetGraphColourPan(PanelControl pan, int tag, GraphColour col)
        {
            if (pan.Taglist == null)
                return;

            foreach (var t in pan.Taglist.Where(t => t.Tag == tag))
            {
                t.Colour = Params.GetPenColour(col);
                GraphParameters.ContextMenu.SetGraphMenuColourTick(t.Tag, t.Colour);
            }
        }

        public void SetGraphColour(int tag, GraphColour col)
        {
            SetGraphColourPan(Gt.CST().MainPan, tag, col);
        }

       


        public Pen GetGraphColour(int tag)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return Pens.Black;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
                return t.Colour;

            return Pens.Black;
        }

        public void SetGraphHl(int tag,bool highlight)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
                t.Highlight = highlight;

            if (Gt.CST().SubPan == null)
                return;

            foreach (var t in Gt.CST().SubPan)
            {
                foreach (var t1 in t.Taglist)
                {
                    if (t1.Tag == tag)
                        t1.Highlight = highlight;
                }
            }

            
        }

        public void SetGraphVisible(int tag, bool visible)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
            {
                t.Visible = visible;
                t.CanBeVisible = visible;
            }

            if (Gt.CST().SubPan == null)
                return;

            foreach (var t in Gt.CST().SubPan)
            {
                foreach (var t1 in t.Taglist)
                {
                    if (t1.Tag == tag)
                    {
                        t1.Visible = visible;
                        t1.CanBeVisible = visible;
                    }
                }
            }


        }

        public void SetGraphAsPoints(int tag, bool asPoints)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
                t.AsPoint = asPoints;

            if (Gt.CST().SubPan == null)
                return;

            foreach (var t in Gt.CST().SubPan)
            {
                foreach (var t1 in t.Taglist)
                {
                    if (t1.Tag == tag)
                        t1.AsPoint = asPoints;
                }
            }

        }
        public void SetUseForYscale(int tag, bool use)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
                t.UseForYscale = use;

            if (Gt.CST().SubPan == null)
                return;

            foreach (var t in Gt.CST().SubPan)
            {
                foreach (var t1 in t.Taglist)
                {
                    if (t1.Tag == tag)
                        t1.UseForYscale = use;
                }
            }
        }

        //public Color GetGraphRgbColour(int tag)
        //{

        //    if (Gt.CST().MainPan.Taglist == null)
        //        return Color.Transparent;

        //    foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
        //        return t.Colour.Color;
            
        //    return Color.Transparent;
        //}


        public void RedrawGraphics()
        {
            if (!GraphParameters.GraphicsSet)
                return;
            RedrawAll();
        }

        public void SetXScaleByXPos(PanelControl pan, int x1, int x2)
        {
            var wid = pan.GPan.Width;
            var dx = x2 - x1;
            var scl = (double)wid/Math.Abs(dx);

            x1 = (int) (x1/GraphParameters.LastXscale + (pan.Xoffset/GraphParameters.LastXscale));
            pan.Xscale = pan.Xscale*scl;
            pan.Xoffset = (x1*scl*GraphParameters.LastXscale);
            GraphParameters.LastXscale = scl*GraphParameters.LastXscale;

            Gt.CST().MainPan.NeedToSetScale = false;

            if (Gt.CST().SubPan == null) 
                return;

            var xoff = pan.Xoffset;
            var xscale = pan.Xscale;
            Gt.CST().MainPan.Xscale = xscale;
            Gt.CST().MainPan.Xoffset = xoff;

            foreach (var t in Gt.CST().SubPan)
            {
                t.NeedToSetScale = false;
                t.Xscale = xscale;
                t.Xoffset = xoff;
            }
        }


        private void IndexUpdate()
        {
            var id = TabControl1.SelectedIndex;
            if (id < 0)
                return;

            HideLegend();
            if (!Gt.SetSelectedTab(id))
                return;
            RedrawAll();
            ShowLegend();
            GraphParameters.TabChangedCallback?.Invoke();
        }

        public void SetGraphYAxisTitle(int tag, string ytit)
        {
            if (Gt.CST().Graphs == null)
                return;

            foreach (var t in Gt.CST().Graphs.Where(t => t.Tagid == tag))
                t.GYaxisTitle = ytit;
        }

        public void SetGraphAxisTitles(int tag, string xtit, string ytit)
        {
            if (Gt.CST().Graphs == null)
                return;

            foreach (var t in Gt.CST().Graphs.Where(t => t.Tagid == tag))
            {
                t.GXaxisTitle = xtit;
                t.GYaxisTitle = ytit;
            }
        }

        //public void SetGraphSource(int tag, string title)
        //{
        //    if (Gt.CST().Graphs == null)
        //        return;

        //    foreach (var t in Gt.CST().Graphs.Where(t => t.Tagid == tag))
        //        t.Source = title;
        //}

        public void ClearLiveGraph(int tagid)
        {
            if (GraphParameters.LiveGraphs == null)
                return;

            var cnt = GraphParameters.LiveGraphs.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (GraphParameters.LiveGraphs[i].Graphid != tagid) 
                    continue;
                GraphParameters.LiveGraphs[i].Lpts.Clear();
                GraphParameters.LiveGraphs[i].Started = false;
                GraphParameters.LiveGraphs[i].CurrentPt = 0;

                var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].Graphid);
                if (gs != null)
                    gs.PtCount = 0;
            }
        }

        public void RemoveAllFileGraphs()
        {
            var cnt = Gt.CST().Graphs.Length;

            for (var i = 0; i < cnt; i++)
            {
                if (!Gt.CST().Graphs[i].Source.Contains("File")) 
                    continue;
                RemoveGraph(Gt.CST().Graphs[i].Tagid);
                RemoveAllFileGraphs();
                return;
            }
        }

        public void ClearAllBoundaryTypeData(Graphtype gtyp)
        {
            if (Gt.CST().Graphs == null)
                return;

            var cnt = Gt.CST().Graphs.Length;

            for (var i = 0; i < cnt; i++)
            {
                if (Gt.CST().Graphs[i].Gtype != gtyp) 
                    continue;
                RemoveGraph(Gt.CST().Graphs[i].Tagid);
                ClearAllBoundaryTypeData(gtyp);
                return;
            }
        }

        public void ClearAllLiveGraphs()
        {
            if (GraphParameters.LiveGraphs == null)
                return;

            var cnt = GraphParameters.LiveGraphs.Length;
            for (var i = 0; i < cnt; i++)
            {
                GraphParameters.LiveGraphs[i].Lpts.Clear();
                GraphParameters.LiveGraphs[i].Started = false;
                GraphParameters.LiveGraphs[i].CurrentPt = 0;

                var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].Graphid);
                if (gs != null)
                    gs.PtCount = 0;
            }
        }


        public void DontHlTab()
        {
            _highlightTab = false;
        }

        public void HlTab(bool hl)
        {
            _highlightTab = hl;
        }

        private Color _tabText = Color.Black;
        private bool _tabBold;

        public void SetTabColour(Color col)
        {
            _tabText = col;
        }

        public void SetTabBold(bool bold)
        {
            _tabBold = bold;
        }
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tcol = Color.Black;
            int ypos = e.Bounds.Y + 7;
            var f = _tabBold ? new Font(e.Font.FontFamily,e.Font.Size,FontStyle.Bold) : e.Font;
            
            var g = e.Graphics;
            var b = new SolidBrush(_backTabColour);           
            if (e.Index == TabControl1.SelectedIndex && _highlightTab)
                b = new SolidBrush(Color.DarkKhaki);
            else
            {
                tcol = _tabText;
                ypos -= 2;
            }

            g.FillRectangle(b, e.Bounds);
            b.Color = tcol;           
            g.DrawString(TabControl1.TabPages[e.Index].Text, f, b, e.Bounds.X + 2,ypos);
            b.Dispose();
        }

        public void ClearGraphData()
        {
            ClearAllLiveGraphs();

          
            if (Gt.GraphTabs == null)
                return;

            var cnt = Gt.GraphTabs.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (Gt.GraphTabs[i].Graphs == null)
                    continue;
                var cnt2 = Gt.GraphTabs[i].Graphs.Length;
              
                for (var j = 0; j < cnt2; j++)
                    Gt.GraphTabs[i].Graphs[j].Dpts = null;
             
                foreach (var t in Gt.GraphTabs[i].MainPan.Taglist)
                {
                    t.ScrPoints = null;
                    t.DispPoints = null;
                }
                Gt.GraphTabs[i].MainPan.Taglist = null;
                if (Gt.GraphTabs[i].SubPan != null)
                {                  
                    foreach (var t in Gt.GraphTabs[i].SubPan)
                    {
                        if (t.Taglist != null)
                        {
                            foreach (var t1 in t.Taglist)
                            {
                                t1.ScrPoints = null;
                                t1.DispPoints = null;
                            }
                        }
                        t.Taglist = null;
                    }
                }
                Gt.GraphTabs[i].Graphs = null;
            }
        //    Gt.GraphTabs = null;
            _liveUpdatePoints = null;
        }

        public void InitPanel()
        {
            Gt.InitialisePanel();
        }

        public void ResetGraphData(int tid, DPoint[] pts)
        {
            if (Gt.CST().MainPan.Taglist == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;

            gs.PtCount = 0;
            UpdateGraph(tid, pts, pts.Length);
        }

        public int CreateLiveConnection(string title, string source)
        {
            int cnt;
            var pts = new DPoint[1];

            if (GraphParameters.LiveGraphs == null)
                _liveUpdate.Start();

            pts[0].X = 0;
            pts[0].Y = 0;

            if (GraphParameters.LiveGraphs == null)
            {
                cnt = 0;
                GraphParameters.LiveGraphs = new LiveData[1];
            }
            else
            {
                cnt = GraphParameters.LiveGraphs.Length;
                Array.Resize(ref GraphParameters.LiveGraphs, cnt + 1);
            }

            GraphParameters.LiveGraphs[cnt].Lpts = new ArrayList();
            GraphParameters.ResetPointCount = true;
            GraphParameters.LiveGraphs[cnt].Graphid = Gpg.AddNewGraph(pts, title, source, Graphtype.Live, null);
            GraphParameters.ResetPointCount = false;
            GraphParameters.LiveGraphs[cnt].Tabid = Gt.GraphTabSelected;
            GraphParameters.LiveGraphs[cnt].Liveid = cnt;
            GraphParameters.LiveGraphs[cnt].Started = false;
            GraphParameters.LiveGraphs[cnt].CurrentPt = 0;

            return cnt;
        }

        public int GetLiveGraphId(int liveId)
        {
            return GraphParameters.LiveGraphs[liveId].Graphid;
        }

        public void LiveFullClear(int id)
        {
            if (GraphParameters.LiveGraphs == null)
                return;
            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;
            GraphParameters.LiveGraphs[id].Lpts.Clear();
            GraphParameters.LiveGraphs[id].Started = false;
            GraphParameters.LiveGraphs[id].CurrentPt = 0;

        }

        public void ResetGraphCounters(int id)
        {
            var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[id].Graphid);
            if (gs != null)
                gs.PtCount = 0;
        }

        

        public void AddLivedata(int id, double y)
        {
            AddLivedata(id, _lastX, y, false, false);
        }
        public void AddLivedata(int id, double x, double y)
        {
            AddLivedata(id, x, y, false, false);
            _lastX = x;
        }
        public void AddLivedata(int id, double x, double y, bool startPt, bool clearit)
        {
            if (GraphParameters.LiveGraphs == null)
                return;

            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;

            if (_inLiveUpdate)
            {
                if (_lucnt > 15)
                    _lucnt = 0;
                else
                {
                    ++_lucnt;
                    return;
                }
            }

            _inLiveUpdate = true;
            try
            {
                DPoint dp;
                if (GraphParameters.LiveGraphs[id].Started)
                {
                    var cnt = GraphParameters.LiveGraphs[id].Lpts.Count;
                    dp = (DPoint) GraphParameters.LiveGraphs[id].Lpts[cnt - 1];

                    if (x <= dp.X && clearit)
                    {
                        if (GraphParameters.ClearListCount <= 0)
                        {
                            if (startPt)
                            {
                                GraphParameters.LiveGraphs[id].Lpts.Clear();
                                GraphParameters.LiveGraphs[id].CurrentPt = 0;
                            }
                            dp.X = (float) x;
                            dp.Y = (float) y;
                            dp.StartPt = startPt;

                            GraphParameters.LiveGraphs[id].Lpts.Add(dp);
                            _inLiveUpdate = false;
                            return;
                        }
                        var counter = 0;
                        var starti = 0;
                        for (var i = cnt - 1; i >= 0; --i)
                        {
                            dp = (DPoint) GraphParameters.LiveGraphs[id].Lpts[i];
                            if (!dp.StartPt) 
                                continue;
                            counter += 1;
                            if (counter < GraphParameters.ClearListCount) 
                                continue;
                            starti = i;
                            break;
                        }
                        if (starti != 0)
                        {
                            _temporaryArray.Clear();
                            for (var i = starti; i <= cnt; i++)
                            {
                                dp = (DPoint) GraphParameters.LiveGraphs[id].Lpts[i];
                                _temporaryArray.Add(dp);
                            }
                            GraphParameters.LiveGraphs[id].Lpts.Clear();
                            foreach (var t in _temporaryArray)
                            {
                                dp = (DPoint) t;
                                GraphParameters.LiveGraphs[id].Lpts.Add(dp);
                            }
                            GraphParameters.LiveGraphs[id].CurrentPt = 0;
                        }
                    }
                }
                else
                    GraphParameters.LiveGraphs[id].Started = true;

                dp.X = (float) x;
                dp.Y = (float) y;
                dp.StartPt = startPt;
                GraphParameters.LiveGraphs[id].Lpts.Add(dp);
            }
            catch
            {
                _inLiveUpdate = false;
            }
            _inLiveUpdate = false;
        }


        public void AddCyclicLivedata(int id, double[] x, double[] y)
        {
   
            if (GraphParameters.LiveGraphs == null)
                return;
            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;

            if (_inLiveUpdate)
            {
                if (_lucnt > 15)
                    _lucnt = 0;
                else
                {
                    ++_lucnt;
                    return;
                }
            }

            _inLiveUpdate = true;
            if (!GraphParameters.LiveGraphs[id].Started)
            {
                AddLiveArray(id, x, y);
                GraphParameters.LiveGraphs[id].Started = true;
                _inLiveUpdate = false;
                return;
            }

            if (GraphParameters.ClearListCount <= 0)
            {
                GraphParameters.LiveGraphs[id].Lpts.Clear();
                GraphParameters.LiveGraphs[id].CurrentPt = 0;
                AddLiveArray(id, x, y);
                _inLiveUpdate = false;
                return;
            }

            var cnt = GraphParameters.LiveGraphs[id].Lpts.Count - 1;
            DPoint dp;

            var counter = 0;
            var starti = 0;
            for (var i = cnt - 1; i >= 0; --i)
            {
                dp = (DPoint) GraphParameters.LiveGraphs[id].Lpts[i];
                if (!dp.StartPt) 
                    continue;
                counter += 1;
                if (counter < GraphParameters.ClearListCount) 
                    continue;
                starti = i;
                break;
            }
            if (starti != 0)
            {
                _temporaryArray.Clear();
                for (var i = starti; i < cnt; i++)
                {
                    dp = (DPoint) GraphParameters.LiveGraphs[id].Lpts[i];
                    _temporaryArray.Add(dp);
                }
                GraphParameters.LiveGraphs[id].Lpts.Clear();
                foreach (var t in _temporaryArray)
                {
                    dp = (DPoint) t;
                    GraphParameters.LiveGraphs[id].Lpts.Add(dp);
                }
                GraphParameters.LiveGraphs[id].CurrentPt = 0;
            }

            AddLiveArray(id, x, y);
            _inLiveUpdate = false;
        }

        private void AddLiveArray(int id, double[] x, double[] y)
        {
           
            var dp = default(DPoint);
            var startPt = true;
            for (var i = 0; i < x.Length; i++)
            {
                dp.X = (float) x[i];
                dp.Y = (float) y[i];
                dp.StartPt = startPt;
                GraphParameters.LiveGraphs[id].Lpts.Add(dp);
                startPt = false;
            }
        }

        public void SetRealGraphColour(int tag, Color col)
        {
          
            var p = new Pen(col);
          
            if (Gt.CST().MainPan.Taglist == null)
                return;

            foreach (var t in Gt.CST().MainPan.Taglist.Where(t => t.Tag == tag))
                t.Colour = p;
        }

        public void SetInitVisible(bool setit)
        {
            if (GraphParameters.CanSetVisible)
                GraphParameters.InitialyVisible = setit;
        }

        public void SetInitVisible2(bool setit, bool can)
        {
            GraphParameters.CanSetVisible = can;
            GraphParameters.InitialyVisible = setit;
        }

        public int GetTagPosFromGraphId(int tagid)
        {          
            if (Gt.CST().MainPan.Taglist == null)
                return -1;

            for (var i = 0; i < Gt.CST().MainPan.Taglist.Length; i++)
            {
                if (Gt.CST().MainPan.Taglist[i].Tag == tagid)
                    return i;
            }
            return 0;
        }

        public static int GetTagPosFromGraphId(GraphControl[] taglist, int tagid)
        {
          
            if (taglist == null)
                return -1;

            for (var i = 0; i < taglist.Length; i++)
            {
                if (taglist[i].Tag == tagid)
                    return i;
            }
            return 0;
        }

        public int GetTagFromGraphPos(int id)
        {
            if (id < 0)
                return -1;

            if (id >= Gt.CST().Graphs.Length)
                return -1;

            return Gt.CST().Graphs[id].Tagid;
        }

        public void ClearPanel(PanelControl pan)
        {
            pan.GrSurface.Clear(pan.GPan.BackColor);
            pan.PanSurface.Clear(pan.GPan.BackColor);
            _grid.DrawGrid(pan);
            GraphParameters.GraphBoundary.FillBoundaries(pan);
            pan.PanSurface.DrawImage(pan.DrawingImage, pan.Xpan, pan.Ypan);
        }

        public int GetHeight()
        {
            return Gt.CST().MainPan.GPan.Height;
        }

        public int GetWidth()
        {
            return Gt.CST().MainPan.GPan.Width;
        }

        public int ForceSelectedTab(int id)
        {
            var cid = Gt.GraphTabSelected;
            Gt.SetSelectedTab(GraphParameters.LiveGraphs[id].Tabid);
            return cid;
        }

        public int GetTabId(int id)
        {
            return GraphParameters.LiveGraphs[id].Tabid;
        }

        public void ResetSelectedTab(int id)
        {
            Gt.SetSelectedTab(id);
        }

        public void SetTabName(int id, string title)
        {
            if (id < 0 || id >= Gt.GraphTabs.Length)
                return;

            Gt.GraphTabs[id].Page.Text = title;
        }

        public void SetAllBoundariesVisible()
        {
            foreach (var t in Gt.CST().MainPan.Taglist)
            {
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs?.Gtype != Graphtype.Boundary)
                    continue;

                if (Gt.CST().SubPan == null) 
                    continue;
                foreach (var t1 in Gt.CST().SubPan)
                {
                    var tag = t.Tag;
                    GraphParameters.ContextMenu.SetAsubtag(t1,t);
                    GraphParameters.ContextMenu.ResetAsubtag(t1, tag, true);
                }
            }
        }
    
        public void ForceAlternateYaxis()
        {
            UseAlternateZeroYAxisToolStripMenuItem.Checked = true;
            GraphParameters.OriginalZeroAxis = false;
        }

        public bool ShowIpGraphName()
        {
            return _shIpgName;
        }

        public void ShowIpGraphName(bool sh)
        {
            _shIpgName = sh;
        }

        public DPoint[] GetGraphPointsFromTag(int tid, ref int cnt)
        {
            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return null;

            cnt = gs.PtCount;
            return gs.Dpts;
        }

        public string[] GetGraphNames(out int[] tags)
        {
            var pan = Gt.CST().MainPan;
            var gtag = new int[256];
            var gnames = new string[256];
            tags = null;

            if (pan.Taglist == null)
                return null;
            var cnt = 0;
            foreach (var t in pan.Taglist)
            {
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;

                if (GrBoundary.IsAnyMarkerorBoundary(gs.Gtype))
                    continue;

                gnames[cnt] = gs.Name.Replace(",", "");
                gtag[cnt] = t.Tag;
                if (++cnt >= 256)
                    break;
            }

            if (cnt == 0)
                return null;
            var names = new string[cnt];
            tags = new int[cnt];
            for (var i = 0; i < cnt; ++i)
            {
                names[i] = gnames[i];
                tags[i] = gtag[i];
            }

            return names;
        }


        public void SetReducedMenuItems()
        {
            RemovePanelItem.Visible = false;
            allowImprtedGraphManipulationToolStripMenuItem.Visible = false;
            MoveImportedGraphToolStripMenuItem.Visible = false;
            Set360DegreePassCountToolStripMenuItem.Visible = false;
            ResetOthersToolStripMenuItem.Visible = false;
            RemoveAllImportedFileGraphsToolStripMenuItem.Visible = false;
            UseAlternateZeroYAxisToolStripMenuItem.Visible = false;
            EndValueScaleToolStripMenuItem.Visible = false;
        }

        private void showWaferBoundariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showBoundaries = !_showBoundaries;
        }

        private void boundariesOnAllPanelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showAllPanBoundaries = !_showAllPanBoundaries;
        }

        public void SetBoundaryVisible()
        {
            boundariesOnAllPanelsToolStripMenuItem.Visible = true;
            showWaferBoundariesToolStripMenuItem.Visible = true;
        }

        private static void SetlegendTick(ref PanelControl pan, bool ticked)
        {
            if (pan?.ConMenu == null)
                return;
            for (var j = 0; j < pan.ConMenu.MenuItems.Count; j++)
            {
                var mtag = (MenuTag) pan.ConMenu.MenuItems[j].Tag;
                if (mtag.Itemtag == GraphOption.Legend)
                    pan.ConMenu.MenuItems[j].Checked = ticked;
            }
        }

       
        private void zoomAndPanAllTimePanelsTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomPanAll = !ZoomPanAll;
            zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Checked = ZoomPanAll;
        }

        public void SetFollowingCursor(bool canuse)
        {
            SetFCursor d = SetFollowingCursor2;
            try
            {
                Invoke(d,canuse);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void SetFollowingCursor2(bool canuse)
        {
            UseFollowingCursoToolStripMenuItem.Enabled = canuse;
            if(!canuse)
                Gopt.TurnOffFollowingCursor();
        }

        
    }
}