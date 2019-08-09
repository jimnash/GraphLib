using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
// ReSharper disable UnusedMember.Global
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GraphLib
{
    public delegate void SetUpdateLastXy(string s1, string s2);
    public delegate void SetFCursor(bool canUse);

    /// <inheritdoc />
    /// <summary>
    ///     Main Panel Access to graphics
    ///     Also provides wrappers for access to lower level stuff
    /// </summary>
    public partial class GraphPanel : UserControl
    {
        
        private  NumberFormatInfo _numberFormat;
        public class LoadFileEventArgs : EventArgs
        {
            public string FileName { get; set; }
        }
        public event EventHandler<LoadFileEventArgs> LoadDataCallback;

        private readonly DPoint[] _displayPoints = new DPoint[5000];
         
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
        private bool _showAllPanMarkers = false;
        internal bool ShowBoundaries = true;
        private bool _gridOverride;
        private bool _bigPoints;
        internal bool AllowFileDrop;
        
        private int _lastCrossX;
        private int _lastCrossY;
        private int _lastCursorX;
        private int _lastCursorY;
        private double _lastMaxD = 1.0;
        private double _lastMind = 1.0;
        private double _lastX;
        private Thread _liveUpdate;
        private DPoint[] _liveUpdatePoints;
        private int _luCnt;
        private Cursor _rememberCursor = Cursors.Cross;     
        private YAxisForm _yAxisConfiguration;
        

        internal int LastXyVx = 0;
        internal bool ShowLegendGraphId = true;
        internal PanelControl TheLastPanel;
        private bool SecondRedrawRequired { get; set; }
        private int SingleWidth { get; set; }
        internal GrTabPanel Gt { get; private set; }
        internal MouseOps Gmo { get; private set; }
        internal GraphData Gdata { get; private set; }
        private Options GraphOptions { get; set; }
        internal Params GraphParameters { get; }

        internal Graphics NetGraph { get; set; }
        public bool ForceXReset = true;
        public bool NoRedraw;
        public int LiveUpdateTime { private get; set; }
        public bool ZoomPanAll { get; private set; }
        public bool DoNotAddToContextMenu
        {
            set => GraphParameters.DontAddToContextMenu(value);
        }
        public int HighlightWidth
        {
            set => GraphParameters.HighlightWidth = value;
        }
        public bool HideMarkers
        {
            set => GraphParameters.GraphBoundary.Markers.Hide = value;
        }

        public bool CanMoveMarker
        {
            set => GraphParameters.GraphBoundary.CanMoveMarker = value;
        }
        public int InitialAllocation
        {
            set => Gdata.InitialAllocation = value;
        }

        public bool IsDarkBackground()
        {
            return GraphParameters.DarkBackground;
        }
       
        public void SetPanelPercent(double p1, double p2)
        {
            try
            {
                var mainPan = GraphParameters.GraphTabPanel.Cst.MainPan;
                var subPan = GraphParameters.GraphTabPanel.Cst.SubPan;
                mainPan.PercentHeight = p1;
                subPan[0].PercentHeight = p2;
            }
            catch 
            {
                var mainPan = GraphParameters.GraphTabPanel.Cst.MainPan;                
                mainPan.PercentHeight =100;              
            }
           
        }

      
        public void AddZoomPan(int id)
        {
            Gmo.AddZoomPan(id);
        }

        public void ResetPanelZoomPan()
        {
            Gt.ResetPanelZoomPan();
        }

        public void SetMainPanelLegendName(string name)
        {
            Gt.Cst.MainPan.LegendTitle = name;
        }
        public void FormatLegendPoint(int id, string format)
        {
            if (id == -1)
                Gt.Cst.MainPan.Legend.FormatPoint = format;
            else
                Gt.Cst.SubPan[id].Legend.FormatPoint = format;
        }

        /// <summary>
        /// 
        /// </summary>
        public GraphPanel()
        {
            ZoomPanAll = true;
            InitializeComponent();
            GraphParameters = new Params(this);
            InitialiseGraphPanel(true);
            SetEnvironment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTextMarker"></param>
        public GraphPanel(bool allowTextMarker)
        {
              
            InitializeComponent();
            GraphParameters = new Params(this);
            InitialiseGraphPanel(allowTextMarker);
            SetEnvironment();
        }

        private void SetEnvironment()
        {

            NetGraph = CreateGraphics();

            var currentCulture = CultureInfo.InvariantCulture;
            _numberFormat = (NumberFormatInfo)currentCulture.NumberFormat.Clone();
            _numberFormat.NumberDecimalSeparator = ".";
        }

        public void RemoveBoundaryCheck()
        {
            GraphParameters.GraphBoundary.Check360 = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allowTextMarker"></param>
        private void InitialiseGraphPanel(bool allowTextMarker)
        {
            SingleWidth = 1;
            LiveUpdateTime = 1000;
            Gmo = new MouseOps();
            ForceXReset = true;

            GraphParameters.GraphTabPanel = new GrTabPanel();
            GraphParameters.GraphBoundary = new GrBoundary();
            GraphParameters.ContextMenu = new GpContextMenu(this);
            GraphParameters.GraphTabPanel.Setup(this);
            GraphParameters.GraphBoundary.Setup(this);
            GraphParameters.AllowTextMarker = allowTextMarker;

            GraphOptions = new Options(this);
            _grid = new Grid(this);
            Gmo.Setup(this);
            Gdata = new GraphData(this);

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

        public void SetSmallAxisFont()
        {
            _grid.SetSmallAxisFont();
        }

        public void SetLargeAxisFont()
        {
            _grid.SetLargeAxisFont();
        }

        public MethodInvoker StateButtonCallback;
        public void SetStateButton(MethodInvoker cbk)
        {
            StateButtonCallback = cbk;
            StateButton.Visible = true;
        }

        public void SetStatePanel(Color redCol, string redState, string redTime,Color blueCol, string blueState,string blueTime)
        {
            RedStateName.Visible = true;
            RedStatePanel.Visible = true;
            RedExtraLabel.Visible = true;
            RedStatePanel.BackColor = redCol;
            RedStateName.Text = redState;
            RedExtraLabel.Text = redTime;
            if (blueState == null)
                return;
            BlueStateName.Visible = true;
            BlueStatePanel.Visible = true;
            BlueExtraLabel.Visible = true;
            BlueStatePanel.BackColor = blueCol;
            BlueStateName.Text = blueState;
            BlueExtraLabel.Text = blueTime;

        }
        public void SetupGraphicsPanel(string tabName)
        {
            InitialiseDefaultParameters(tabName);
            DontHlTab();
            SetTriggerPanel(false);
            SetTabColour(Color.DarkBlue);

            SetLegendGraphId(false);
            SetReducedMenuItems();

        }

        public void InitialiseDefaultParameters(string firstTabName)
        {
           
            DontHlTab();
            InitPanel();
            GraphParameters.AllReadyToGo = true;
            GraphParameters.GraphicsSet = true;
            GraphParameters.DisplayGraphMover = false;
            Gt.RenameCst(firstTabName);
            GraphParameters.OriginalZeroAxis = false;
            SingleWidth = 1;
            GraphParameters.HighlightWidth = 3;           
            SecondRedrawRequired = true;

            InitEvents();
        }

        public void ReInit()
        {
            InitPanel();
            GraphParameters.AllReadyToGo = true;
            GraphParameters.GraphicsSet = true;
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

        public void InitialiseDefaultParameters3(string firstTabName)
        {
            InitialiseDefaultParameters(firstTabName);
            SecondRedrawRequired = true;
            GraphParameters.AllowRemoveMarker = false;
            GraphParameters.AllowTextMarker = false;
            SetLegendGraphId(false);
            SetReducedMenuItems();
            HlTab(true);
            SetTriggerPanel(false);
        }

        public GraphManipulationTag[] GetBoundaryData()
        {
            return GraphParameters.GraphBoundary.GetBoundaryData();
        }

        public int GetLastMarkerMoveId()
        {
            return GraphParameters.LastMarkerMoveId;
        }
        
        public void SortAngleNames(GraphManipulationType typ, string tit)
        {
            GraphParameters.GraphBoundary.SortAngleNames(typ, tit);
        }
        public void ForceGraphColour(GraphColour col, int tag, bool darker)
        {
            GraphParameters.ForceGraphColour(col, tag, darker);
        }
        public void AddDoubleClickFunction(MethodInvoker cbk)
        {
            GraphParameters.AddDoubleClickFunction(cbk ?? Gp_DoubleClick);
        }
        private static void Gp_DoubleClick()
        {
        }

        public void AddBoundaryCallback(EventHandler<BoundaryEventArgs> cbk)
        {
            GraphParameters.GraphBoundary.BoundaryCallback += cbk;
        }

        private readonly List<string> _fileExtensions = new List<string>(8);

        public void AddFileDataCallback(EventHandler<LoadFileEventArgs> cbk,string extension)
        {
            AllowFileDrop = true;
            LoadDataCallback += cbk;
            _fileExtensions.Clear();
            _fileExtensions.Add(extension);
        }

       

       
        public void MarkerChangeCallback(MethodInvoker cbk)
        {
            GraphParameters.MarkerChangeCallback += cbk;
        }

        public double LastDoubleClickTime => GraphParameters.LastDoubleClickTime;


        public void ShowHide(bool show)
        {
            Gt.ShowHide(show);
        }
        public void UseOffsetTagValue(bool use, int id)
        {
            Gdata.UseOffsetTagValue(use,id);
        }

        public void SetLegendOffset(int left, int top)
        {
            Gt.SetLegendOffset(left,top);
        }

        public int AddGeneralMarker(double time, string val)
        {
            return GraphParameters.GraphBoundary.AddGeneralMarker(time, val);
        }

        public int AddFixedMarker(double time, string val)
        {
            return GraphParameters.GraphBoundary.AddFixedMarker(time, val);
        }
        public int AddFixedMarker(double time, string val,GraphColour col)
        {
            var id = GraphParameters.GraphBoundary.AddFixedMarker(time, val);
            SetGraphColour(id, col);
            return id;
        }

        public int AddMoveableMarker(double pt,string name,string xTitle,string yTitle,GraphColour col,GraphManipulationTag tag,bool visible)
        {
            var id = AddNewGraph(pt, name, "BOU", GraphType.MoveableMarker, tag);
            SetMarkerStyle(id, xTitle, yTitle, col, visible);
            return id;
        }

        public int AddHorizontalMarker(double pt, string name, string xTitle, string yTitle, GraphColour col, GraphManipulationTag tag, GraphType typ)
        {
            var dp = new DPoint[1];
            dp[0].X = 0;
            dp[0].Y = (float)pt;
            var id = AddNewGraph(dp, name, "BOU", typ, tag);
            SetMarkerStyle(id, xTitle, yTitle, col, false);
            return id;
        }
        public int AddFixedMarker(double pt, string name, string xTitle, string yTitle, GraphColour col, GraphManipulationTag tag, bool visible)
        {
            var id = AddNewGraph(pt, name, "BOU", GraphType.FixedMarker, tag);
            SetMarkerStyle(id, xTitle, yTitle, col, visible);
            return id;
        }

        private void SetMarkerStyle(int id, string xTitle, string yTitle, GraphColour col, bool visible)
        {
            SetGraphColour(id, col);
            SetGraphHl(id, true);
            SetGraphAxisTitles(id, xTitle, yTitle);
            SetGraphVisible(id, visible);
        }

        public int AddNewGraph(double pt,string tit1,string tit2,GraphType gType, GraphManipulationTag tag)
        {
            var pts = new double[1];
            pts[0] = pt;
            return Gdata.AddNewGraph(pts, tit1, tit2, gType, tag);
        }
        public int AddNewGraph(double[] pts, string tit1, string tit2, GraphType gType, GraphManipulationTag tag)
        {
            return Gdata.AddNewGraph(pts, tit1, tit2, gType, tag);
        }

        public int AddNewGraph(DPoint[] pts, string tit1, string tit2, GraphType gType, GraphManipulationTag tag)
        {
            return Gdata.AddNewGraph(pts, tit1, tit2, gType, tag);
        }

        public int AddNewGraph(DPoint[] pts, string tit1, string tit2)
        {
            return Gdata.AddNewGraph(pts, tit1, tit2);
        }

        public int AddNewGraph(DPoint[] pts, string tit1, string tit2,
                              string xAxisTitle,string yAxisTitle,GraphColour col,bool visible)
        {
            SetInitVisible(visible);
            var tag = Gdata.AddNewGraph(pts, tit1, tit2);
            SetAxisAndColour(tag, xAxisTitle, yAxisTitle, col);
            SetInitVisible(true);
            return tag;
        }

        public void SetLegendGraphId(bool onOff)
        {
            ShowLegendGraphId = onOff;
        }

        public void SetYAuto(int id)
        {
            try
            {          
                if (id == -1)
                    Gt.Cst.MainPan.YAxisType = YAxisType.Auto;
                else
                    Gt.Cst.SubPan[id].YAxisType = YAxisType.Auto;
            }            
            catch 
            {
            }

        }

        public void SetYMinMax(double min, double max)
        {
            Gt.Cst.MainPan.YAxisType = YAxisType.MinMax;
            Gt.Cst.MainPan.YAxisMin = min;
            Gt.Cst.MainPan.YAxisMax = max;
        }

        public void SetYMinMax(int id, double min, double max)
        {
            try
            {                        
                Gt.Cst.SubPan[id].YAxisType = YAxisType.MinMax;
                Gt.Cst.SubPan[id].YAxisMin = min;
                Gt.Cst.SubPan[id].YAxisMax = max;               
            }          
            catch 
            {
                
            }
        }

        public void SetXMinMax(int tab,double min, double max)
        {
            SetSelectedTab(tab);
            SetXMinMax(min, max);
            SetSelectedTab(0);
        }

        public void SetXMinMax(double min, double max)
        {
            try
            {               
                Gt.Cst.MainPan.XAxisType = XAxisType.MinMax;
                Gt.Cst.MainPan.XAxisMin = min;
                Gt.Cst.MainPan.XAxisMax = max;               
            }           
            catch
            {

            }
        }      

        public void SetTabText(int id, string title)
        {
            Gt.GraphTabs[id].Page.Text = title;
        }
        public int AddNewTab(string title)
        {
            return Gt.AddNewTab(title);
        }
      

        public string PanelName = "";
        public void AddPanel(int[] list, int cnt,string name)
        {
            PanelName = name;
            Gt.AddPanel(list, cnt);
            PanelName = "";
        }
        public void AddPanel(int[] list, int cnt)
        {
            Gt.AddPanel(list, cnt);
        }
        public void SetSelectedTabOffset(int id)
        {
            Gt.SetSelectedTab(Gt.GraphTabSelected + id);
        }
        public void SetXSpan(double span)
        {
            Gt.Cst.MainPan.XAxisType = XAxisType.SetSpan;
            Gt.Cst.MainPan.XAxisSpan = span;
        }   

        public void SetXSpan(double span,string title)
        {
            SetXSpan(span);
            Gt.Cst.MainPan.XAxisSpanLabel = title;          
        }

        public void SetTriggerPanel(bool onOff)
        {
            TriggerPanel.Visible = onOff;
            RPMLabel.Visible = onOff;
        }

        public void SetTriggerPanelActive(bool onOff)
        {
            if (onOff)
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

        public void SetTriggerPanelActive(bool onOff, double rpm)
        {
            SetTriggerPanelActive(onOff);
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
                   
            UseFollowingCursoToolStripMenuItem.Click += GraphOptions.UseFollowingCursorToolStripMenuItem_Click;
            ShowGridToolStripMenuItem.Click += GraphOptions.ShowGridToolStripMenuItem_Click;
            ShowXAxisToolStripMenuItem.Click += GraphOptions.ShowXAxisToolStripMenuItem_Click;
            ShowYAxisToolStripMenuItem.Click += GraphOptions.ShowYAxisToolStripMenuItem_Click;
            ShowYAxisLegendToolStripMenuItem.Click += GraphOptions.ShowYAxisLegendToolStripMenuItem_Click;
            ShowXAxisLegendToolStripMenuItem.Click += GraphOptions.ShowXAxisLegendToolStripMenuItem_Click;
            AxisConfigurationToolStripMenuItem.Click += GraphOptions.AxisConfigurationToolStripMenuItem_Click;
          
          
            AddPanelToolStripMenuItem.Click += GraphOptions.AddPanelToolStripMenuItem_Click;
            RemovePanelItem.Click += GraphOptions.RemovePanelItem_Click;
            ShowDifferenceMarkersToolStripMenuItem.Click += GraphOptions.ShowDifferenceMarkersToolStripMenuItem_Click;
            ShowGraphTooltipToolStripMenuItem.Click += GraphOptions.ShowGraphTooltipToolStripMenuItem_Click;
            ShowBoundaryValuesToolStripMenuItem.Click += GraphOptions.ShowBoundaryValuesToolStripMenuItem_Click;
            UseDarkBackgroundItem.Click += GraphOptions.UseDarkBackgroundItem_Click;

            X2ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X3ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X4ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X5ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X6ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X7ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X8ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
            X9ToolStripMenuItem.Click += GraphOptions.SetHighlightWidth;
           
            UseAlternateZeroYAxisToolStripMenuItem.Click += GraphOptions.UseAlternateZeroYAxisToolStripMenuItem_Click;
           
            TabControl1.DrawItem += TabControl1_DrawItem;
            TabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            //TabControl1.DragEnter += TabControl1_DragEnter;
            //TabControl1.DragDrop += TabControl1_DragDrop;          
        }

       

        

        public void ForceBoundaryStrings(bool on)
        {
            GraphOptions.ForceBoundaryStrings(on);
        }

        private void TabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            IndexUpdate();
        }

        public void SetWaitCursor()
        {
            _rememberCursor = Gt.Cst.Page.Cursor;
            Gt.Cst.Page.Cursor = Cursors.WaitCursor;
        }

        public void ResetTheCursor()
        {
            Gt.Cst.Page.Cursor = _rememberCursor;
        }

        public void CloseGraphics(bool quit)
        {
            try
            {
                EndUpdateThread();
            }
            catch
            {
            }

            if (!quit)
                return;
            try
            {
                Environment.Exit(1);
            }
            catch
            {
            }
        }
       
        public void EndUpdateThread()
        {
            _endUpThread = true;
            Thread.Sleep(500);
            if (_liveUpdate.ThreadState != ThreadState.AbortRequested && _liveUpdate.ThreadState != ThreadState.Aborted)
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
                    GraphParameters.HaltUpdateFromSplit = false;
                    _inRedraw = false;
                }


                if (!_endUpThread)
                    Thread.Sleep(LiveUpdateTime);
                else
                    break;

                if (_isUpdating || GraphParameters.HaltLiveUpdate || GraphParameters.HaltUpdateFromSplit || _inRedraw)
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
                    if (GraphParameters.LiveGraphs[i].LivePoints.Count > 1)
                    {
                        cnt = GraphParameters.LiveGraphs[i].LivePoints.Count - 1;
                        if (GraphParameters.LiveGraphs[i].CurrentPt == 0)
                        {
                            var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].GraphId);
                            if (gs != null)
                                gs.PtCount = 0;
                        }
                        k = 0;
                        try
                        {                          
                            for (var j = GraphParameters.LiveGraphs[i].CurrentPt; j <= cnt; j++)
                            {
                                if (j >= GraphParameters.LiveGraphs[i].LivePoints.Count)
                                {
                                    cnt = GraphParameters.LiveGraphs[i].LivePoints.Count - 1;
                                    break;
                                }
                                if (k >= _liveUpdatePoints.Length)
                                {
                                    var maxL = _liveUpdatePoints.Length;
                                    maxL = maxL*2;
                                    Array.Resize(ref _liveUpdatePoints, maxL);
                                }

                                _liveUpdatePoints[k] = (DPoint) GraphParameters.LiveGraphs[i].LivePoints[j];
                                k += 1;
                            }
                        }
                        catch
                        {
                        }

                        GraphParameters.LiveGraphs[i].CurrentPt = cnt + 1;
                        if (k > 0)
                            UpdateGraph(GraphParameters.LiveGraphs[i].GraphId, _liveUpdatePoints, k);
                    }
               

                    if (cnt > 10000 && k > 0)
                    {
                        GraphParameters.LiveGraphs[i].LivePoints.Clear();
                        GraphParameters.LiveGraphs[i].LivePoints.Add(_liveUpdatePoints[0]);
                        GraphParameters.LiveGraphs[i].LivePoints.Add(_liveUpdatePoints[0]);
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

        public int GetNumberOfTabs()
        {
            return Gt.TabCount;
        }
        public GraphSurface GetGraphSurfaceFromTag(int tagId)
        {

            for (var i = 0; i < Gt.TabCount; i++)
            {
                var tt = Gt.GetTab(i);
                if (tt?.Graphs == null)
                    continue;

                foreach (var t in tt.Graphs.Where(t => t.TagId == tagId))
                {
                    return t;
                }
            }

            if (Gt.Cst?.Graphs == null)
                return null;

            foreach (var t in Gt.Cst.Graphs)
            {
                if (t.TagId == tagId)
                    return t;
            }

            return null;
        }

       
        public void RedrawAll()
        {
            if (NoRedraw)
                return;
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
            if (Gt.Cst.MainPan.NeedToSetScale)
                SetXScaleFromPoints();

            try
            {
                Gt.Cst.GrSurface.Clear(Gt.Cst.Page.BackColor);
                
                DrawTheGraph(Gt.Cst.MainPan, true);
                Gt.Cst.MainPan.Legend.UpdateMinMax(Gt.Cst.MainPan);

                if (Gt.Cst.SubPan != null)
                {           
                    foreach (var t in Gt.Cst.SubPan)
                    {
                        DrawTheGraph(t, false);
                        t.Legend.UpdateMinMax(t);
                    }
                }
                Gt.Cst.TabSurface.DrawImage(Gt.Cst.DrawingImage, 0, 0);
            }
            catch
            {
                _inRedraw = false;
                GraphParameters.XGridDrawn = false;
            }

            _inRedraw = false;
            GraphParameters.XGridDrawn = false;
        }

        public void UpdateGraph(int tid, double[] bPoints)
        {          
            var pts = new DPoint[bPoints.Length];
            for (var i = 0; i < bPoints.Length; i++)
            {
                pts[i].X = (float) bPoints[i];
                pts[i].Y = 0;
                pts[i].StartPt = i == 0;
            }
            UpdateGraph(tid, pts);
        }

        public void UpdateGraph(int tid, double pt)
        {
            var pts = new DPoint[1];           
            pts[0].X = (float)pt;
            pts[0].Y = 0;
            pts[0].StartPt = true;            
            UpdateGraph(tid, pts);
        }
        public void UpdateGraphY(int tid, double pt)
        {
            var pts = new DPoint[1];
            pts[0].X = 0;
            pts[0].Y = (float)pt;
            pts[0].StartPt = true;
            UpdateGraph(tid, pts);
        }

        public void UpdateGraph(int tid, DPoint[] pts, int pCount)
        {
            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;
            gs.DyScale = 1.0;
            try
            {
                if (gs.PtCount + pCount >= gs.MaxPtCount)
                {
                   
                    gs.MaxPtCount *= 2;
                    Array.Resize(ref gs.DPts, gs.MaxPtCount + 1);
                    
                }
                var cnt = 0;
                for (var i = 0; i < pCount; i++)
                {
                    gs.DPts[gs.PtCount + cnt] = pts[i];
                    gs.DPts[gs.PtCount + cnt].Y *= (float) gs.DyScale; // JIM LEGEND SCALE

                    cnt += 1;
                }
                gs.PtCount += cnt;
            }
            catch
            {
            }
        }

        public void LimitGraph(int tid, double span)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);

            if (gs?.DPts == null)
                return;

            int len = gs.PtCount;
            if (len < 100)
                return;

            double current = gs.DPts[len - 1].X - gs.DPts[0].X;
            if (current < span)
                return;

            var count = 0;
            while (current >= span)
            {
                ++count;
                if (count >= len - 1)
                    return;
                current = gs.DPts[len - 1].X - gs.DPts[count].X;
            }
            len = len - count;
            DPoint[] pts = new DPoint[len];
            for (var i = count; i < gs.PtCount; ++i)
                pts[i-count] = gs.DPts[i];

           
            UpdateGraph(tid, pts);

        }
        public void UpdateGraph(int tid, DPoint[] pts)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;

            var pCount = pts.Length;
            gs.DyScale = 1.0;
            try
            {
                if (pCount >= gs.MaxPtCount)
                {
                    if (gs.MaxPtCount > 100000)
                        gs.MaxPtCount += 100000;
                    else
                        gs.MaxPtCount *= 2;

                    gs.DPts = new DPoint[gs.MaxPtCount];
                }

                gs.PtCount = pCount;

               
                for (var i = 0; i < pCount; i++)
                    gs.DPts[i] = pts[i];
            }
            catch
            {
            }
        }

        public void DrawTheGraph()
        {
            if (!GraphParameters.GraphicsSet)
                return;

            if (Gt.Cst.MainPan.TagList == null)
                return;
            DrawTheGraph(Gt.Cst.MainPan, true);
        }

        public void DrawTheGraph(PanelControl pan, bool isItMain)
        {
            if (Math.Abs(pan.XScale) < 0.00000001 || Math.Abs(pan.YScale) < 0.00000001)
                return;
            GraphParameters.GraphicsSet = true;
            UpdatePoints(pan, isItMain);
        }

        public int AddNewPanelGraph(DPoint[] data,string t1,string t2,string t3,string t4,GraphColour col,bool visible)
        {
            var tag = AddNewGraph(data, t1,t2,t3,t4,col,visible);
            var subPanList = new int[1];
            subPanList[0] = GetTagPosFromGraphId(tag);
            AddPanel(subPanList, 1);
            SetGraphColour(0, tag, GraphColour.Red);
            return tag;
        }

        public void SetPanelColour(bool set)
        {
            Color col = Color.WhiteSmoke;
            if (set)
                col = Color.Black;
                   
            _grid.UseGreyGrid = set;
                           
            Gt.Cst.MainPan.GPan.BackColor = col;
            if (Gt.Cst.SubPan != null)
            {
                foreach (var t in Gt.Cst.SubPan)
                    t.GPan.BackColor = col;
            }
            RedrawAll_now();



        }
        private void UpdatePoints(PanelControl pan, bool isItMain)
        {         
            if (_isUpdating)
                return;

            var pp = new Pen(Color.Black);

            _isUpdating = true;

            if (pan.TagList == null)
            {
                _isUpdating = false;
                return;
            }

            pan.GrSurface.Clear(pan.GPan.BackColor);

            if (GraphParameters.Panning)
                pan.PanSurface.Clear(pan.GPan.BackColor);

            SetGetMinMax(pan);

            _grid.DrawGrid(pan);
            if (ShowBoundaries)
            {
                if (isItMain || _showAllPanBoundaries)
                {
                    GraphParameters.GraphBoundary.FillBoundaries(pan);
                    GraphParameters.GraphBoundary.DrawBoundaries(pan);
                }
            }
            if (isItMain || _showAllPanMarkers)
                GraphParameters.GraphBoundary.Markers.DrawMarkers(pan);


            for (var i = 0; i < pan.TagList.Length; i++)
            {
                if (!pan.TagList[i].Visible)
                    continue;

                var gs = GetGraphSurfaceFromTag(pan.TagList[i].Tag);

                if (gs == null)
                    continue;

                if (gs.GType != GraphType.Graph && gs.GType != GraphType.Live)
                    continue;

                pp.Color = pan.TagList[i].Colour.Color;
                pp.Width = pan.TagList[i].Highlight ? GraphParameters.HighlightWidth : SingleWidth;


                if (pan.TagList[i].DisplayYScale != gs.DyScale)
                {
                    var yScale = pan.TagList[i].DisplayYScale/gs.DyScale;                    
                    for (var k = 0; k < gs.PtCount; k++)
                        gs.DPts[k].Y *= (float) yScale;
                    gs.DyScale = pan.TagList[i].DisplayYScale;
                }

                try
                {
                    DrawData(pan, gs, pp, i);
                }
                catch
                {
                }
            }

            ShowLastXyValue();
            if (ShowBoundaries)
            {
                if (isItMain || _showAllPanBoundaries)
                    GraphParameters.GraphBoundary.DrawToolTips(pan);
            }

            pan.PanSurface.DrawImage(pan.DrawingImage, pan.XPan, pan.YPan);
            _isUpdating = false;
            RefreshDrawing(GraphParameters.CursorPan);
        }

        private void SetGetMinMax(PanelControl pan)
        {
            double min, max;
            var gs = GetMasterGraph(pan);

            if (pan.YAxisType == YAxisType.Free && !pan.Zooming)
            {
                var tid = GetMasterTag(pan);
                if (tid < 0)
                {
                    min = pan.YAxisMin;
                    max = pan.YAxisMax;
                }
                else
                {
                    max = pan.TagList[tid].DisplayPoints[0].Y;
                    min = pan.TagList[tid].DisplayPoints[0].Y;
                 
                    for (var j = 1; j < pan.TagList[tid].ScrPCount; j++)
                    {
                        if (pan.TagList[tid].DisplayPoints[j].Y > max)
                            max = pan.TagList[tid].DisplayPoints[j].Y;
                        if (pan.TagList[tid].DisplayPoints[j].Y < min)
                            min = pan.TagList[tid].DisplayPoints[j].Y;
                    }
                    pan.YAxisMin = min;
                    pan.YAxisMax = max;
                }
            }
            else if (pan.YAxisType == YAxisType.MinMax || pan.Zooming)
            {
                min = pan.YAxisMin;
                max = pan.YAxisMax;
            }
            else
            {
                max = GetMaxOnTab(1, pan);
                min = GetMinOnTab(1, pan);
                var dx = max - min;
                dx *= 0.01;
                max += dx;
                min -= dx;

                pan.YAxisMin = min;
                pan.YAxisMax = max;
              
            }

            if ((max - min) < 1E-08)
            {
                max *= 1.05;
                min *= 0.95;
            }


            gs.MaxD = max;
            gs.MinD = min;
            _lastMaxD = max;
            _lastMind = min;
        }


        private void DrawData(PanelControl pan, GraphSurface gs, Pen pp, int gid)
        {
            
            pan.TagList[gid].ScrPCount = 0;            
            var sCount = ScaleToScreen(pan, gs, gs.DPts, gs.PtCount, 2000);
            
            if (sCount > 2)
            {
                if (pan.TagList[gid].ScrPCount + sCount > pan.TagList[gid].MaxScreenPCount)
                {
                    Array.Resize(ref pan.TagList[gid].ScrPoints, pan.TagList[gid].ScrPCount + sCount + 1);
                    Array.Resize(ref pan.TagList[gid].DisplayPoints, pan.TagList[gid].ScrPCount + sCount + 1);
                }

                for (var i = 0; i < sCount; i++)
                {
                    pan.TagList[gid].ScrPoints[pan.TagList[gid].ScrPCount + i].X = _screenPoints[i].X;
                    pan.TagList[gid].ScrPoints[pan.TagList[gid].ScrPCount + i].Y = _screenPoints[i].Y;
                    pan.TagList[gid].DisplayPoints[pan.TagList[gid].ScrPCount + i].X = _displayPoints[i].X;
                    pan.TagList[gid].DisplayPoints[pan.TagList[gid].ScrPCount + i].Y = _displayPoints[i].Y;
                }
                pan.TagList[gid].ScrPCount += sCount;
            }


            if (pan.TagList[gid].AsBar)
                DrawAsBar(pan, pp, gid);
            else if (pan.TagList[gid].AsPoint || _gridOverride)
                DrawAsPoints(pan, pp, gid);
            else
                DrawAsLines(pan, pp, gid);

             
        }

        private static void DrawAsBar(PanelControl pan, Pen pp, int gid)
        {

            int red = pp.Color.R;
            int green = pp.Color.G;
            int blue = pp.Color.B;
           
            var opCol = new SolidBrush(Color.FromArgb(128, red, green, blue));
            var pts = pan.TagList[gid].ScrPoints;
            var offset = pan.TagList[gid].BarOffset;
            for (var i = 0; i < pan.TagList[gid].ScrPCount; i++)
            {
                if (i == 0)
                    continue;
                if (pts[i].X != pts[i - 1].X)
                    continue;
                var height = Math.Abs(pts[i].Y - pts[i - 1].Y);
                var x = pts[i].X - 7 + offset;
                pan.GrSurface.FillRectangle(opCol, x, pts[i].Y, 14, height);
                pan.GrSurface.DrawRectangle(pp, x, pts[i].Y, 14, height);
            }                   
        }

        private void DrawAsPoints(PanelControl pan, Pen pp, int gid)
        {
            var size = 1;
            if (_gridOverride || _bigPoints)
                size = 3;
           
            var pts = pan.TagList[gid].ScrPoints;
            for (var i = 0; i < pan.TagList[gid].ScrPCount; i++)
            {
                pan.GrSurface.DrawLine(pp, pts[i].X - size, pts[i].Y - size, pts[i].X + size, pts[i].Y + size);
                pan.GrSurface.DrawLine(pp, pts[i].X - size, pts[i].Y + size, pts[i].X + size, pts[i].Y - size);
               
            }
        }

        private static void DrawAsLines(PanelControl pan, Pen pp, int gid)
        {
            var lCount = 0;
            var lCount2 = 0;
            var pts = pan.TagList[gid].ScrPoints;
            var hw = (int) pp.Width;
            for (var i = 3; i < pan.TagList[gid].ScrPCount - 1; i++)
            {
                if (pts[i + 1].X < pts[i].X)                 
                    lCount += 1;
            }

            if (lCount != 0)
            {
                pp.Width = 1;
                pp.DashStyle = DashStyle.Dot;
            }

            for (var i = 0; i < pan.TagList[gid].ScrPCount - 1; i++)
            {            
                if (pts[i + 1].X < pts[i].X)
                {
                    lCount2 += 1;
                    if (lCount2 == lCount)
                    {
                        pp.Width = hw;
                        pp.DashStyle = DashStyle.Solid;
                    }

                    continue;
                }
                pan.GrSurface.DrawLine(pp, pts[i].X, pts[i].Y, pts[i + 1].X,pts[i + 1].Y);              
            }
        }


       

        private void ShowLastXyValue()
        {         
            var dumI = 0;
            double dumD = 0;
            if (!GraphParameters.DisplayActiveValues)
                return;

            GraphParameters.GetDataValueFromPosition(TheLastPanel, LastXyVx, ref dumI, ref dumD);

            var str1 = Params.SetPlaces(GraphParameters.LastMarkerTime);
            var str = GraphParameters.LastMarkerTime.ToString(str1);
            var str12 = Params.SetPlaces(GraphParameters.LastMarkerValue);
            var str2 = GraphParameters.LastMarkerTime.ToString(str12);
            DoUpDownXy("Time: " + str, "Value: " + str2);
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

            if (pan.TagList == null)
                return;

            var wid = pan.GPan.Width;
            var hei = pan.GPan.Height;

            try
            {
                _isUpdating = true;
                var found = pan.TagList.Any(t => t.Visible);

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
            if (pan.TagList == null)
                return null;

            var len = pan.TagList.Length;
            if (len <= 0)
                return null;

            for (var i = 0; i < len; i++)
            {
                if (pan.TagList[i].Master)
                    return GetGraphSurfaceFromTag(pan.TagList[i].Tag);
            }

            pan.TagList[0].Master = true;

            return GetGraphSurfaceFromTag(pan.TagList[0].Tag);
        }

        public int GetMasterGraphId()
        {
            return GetMasterGraphId(Gt.Cst.MainPan);
        }

        public static int GetMasterGraphId(PanelControl pan)
        {
            if (pan.TagList == null)
                return -1;
            var len = pan.TagList.Length;
            if (len <= 0)
                return -1;

            for (var i = 0; i < len; i++)
            {
                if (pan.TagList[i].Master)
                    return pan.TagList[i].Tag;
            }
            return 0;
        }

        public static int GetMasterTag(PanelControl pan)
        {
            if (pan.TagList == null)
                return -1;
            var len = pan.TagList.Length;
            if (len <= 0)
                return -1;

            for (var i = 0; i < len; i++)
            {
                if (pan.TagList[i].Master)
                    return i;
            }
            return 0;
        }

        public double GetMaxOnTab(int stepCount, PanelControl pan)
        {
            var max = 0.0;
            var firstTime = true;

            if (pan.TagList == null)
                return 1.0;

            foreach (var t in pan.TagList.Where(t => t.Visible))
            {
                if (!t.UseForYScale)
                    continue;
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;
                if (GrBoundary.IsAnyMarkerOrBoundary(gs.GType))
                    continue;
               
                for (var j = 0; j < t.ScrPCount; j += stepCount)
                {
                    if (firstTime)
                    {
                        max = t.DisplayPoints[j].Y;
                        firstTime = false;
                    }
                    if (max < t.DisplayPoints[j].Y)
                        max = t.DisplayPoints[j].Y;
                }
            }


            if (max < 1E-05)
                max = 1.0;
            return max;
        }

        public double GetMinOnTab(int stepCount, PanelControl pan)
        {
            var min = 0.0;
            var firstTime = true;

            if (pan.TagList == null)
                return 0.0;


            foreach (var t in pan.TagList.Where(t => t.Visible))
            {
                if (!t.UseForYScale)
                    continue;
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;
                if (GrBoundary.IsAnyMarkerOrBoundary(gs.GType))
                    continue;


                for (var j = 0; j < t.ScrPCount; j += stepCount)
                {
                    if (firstTime)
                    {
                        min = t.DisplayPoints[j].Y;
                        firstTime = false;
                    }
                    if (min > t.DisplayPoints[j].Y)
                        min = t.DisplayPoints[j].Y;
                }
            }

            return min;
        }

        private int ScaleToScreen(PanelControl pan, GraphSurface ggs, DPoint[] pts, int pCount, int screenDx)
        {
            var cnt = 0;
            var yMin = 0;
            var yMax = 0;

            if (ggs == null)
                return 0;

            if (pCount <= 2)
                return 0;

            var xScale = pan.NeedToSetScale ? ggs.XScale : pan.XScale;


            //find start
            var startPoint = GetDataStart(pan, pts, pCount, xScale);

            //find end
            var endPoint = GetDataEnd(startPoint, pan, pts, pCount, xScale);
            if (screenDx < 100)
                screenDx = 100;

            var stepCount = (endPoint - startPoint)/screenDx;
            if (stepCount <= 0)
                stepCount = 1;

            var max = _lastMaxD;
            var min = _lastMind;
            var height = pan.GPan.Height - 5;

            try
            {
                cnt = 0;

                GraphParameters.YScale = ((max - min)/(height));
                var setMinMax = true;

               
                for (var j = startPoint; j <= endPoint; j++)
                {
                    var x = (int) ((pts[j].X*xScale) - pan.XOffset);
                    double yy = pts[j].Y;

                    var tempP = ((yy - min)/GraphParameters.YScale)*pan.YScale;
                    var y = (int) (height - tempP - pan.YOffset);

                    if (cnt == 0)
                    {
                        _screenPoints[cnt].X = x;
                        _screenPoints[cnt].Y = y;
                        _displayPoints[cnt].X = pts[j].X;
                        _displayPoints[cnt].Y = (float) yy;
                        ++cnt;
                        continue;
                    }

                    if (setMinMax)
                    {
                        yMin = y;
                        yMax = y;
                        setMinMax = false;
                    }
                    else
                    {
                        if (y > yMax)
                            yMax = y;
                        if (y < yMin)
                            yMin = y;
                    }


                    if ((j%stepCount) != 0) 
                        continue;

                    _screenPoints[cnt].X = x;
                    _screenPoints[cnt].Y = yMin;
                    _displayPoints[cnt].X = pts[j].X;
                    _displayPoints[cnt].Y = (float) yy;
                    ++cnt;
                    if (yMin != yMax)
                    {
                        _screenPoints[cnt].X = x;
                        _screenPoints[cnt].Y = yMax;
                        _displayPoints[cnt].X = pts[j].X;
                        _displayPoints[cnt].Y = (float) yy;
                        cnt += 1;
                    }

                    setMinMax = true;
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
                tempP *= pan.YScale;
                value = hei - tempP - pan.YOffset;

                GraphParameters.CrossX = x - 3;
                GraphParameters.CrossY = (int) (value - 3);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void DoUpDownXy(string s1, string s2)
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

        private static int GetDataStart(PanelControl pan, DPoint[] dPoints, int ptCount, double xScale)
        {
            var x = (int) ((dPoints[0].X*xScale) - pan.XOffset);

            if (x >= 0)
                return 0;

            var endPoint = ptCount - 1;
            var lastStartPoint = 0;
            var startPoint = endPoint/2;

            while (true)
            {
                x = (int) ((dPoints[startPoint].X*xScale) - pan.XOffset);

                if (x == 0)
                    break;

                if (x > 0)
                {
                    if (Math.Abs(endPoint - lastStartPoint) <= 2)
                        break;

                    endPoint = startPoint;
                    startPoint = startPoint - ((endPoint - lastStartPoint)/2);
                }
                else if (x < 0)
                {
                    if ((endPoint - startPoint) < 20)
                        break;

                    lastStartPoint = startPoint;
                    if (Math.Abs(endPoint - startPoint) <= 2)
                        break;
                    startPoint = startPoint + ((endPoint - startPoint)/2);
                }
                if (startPoint == 0)
                    break;
            }

            return startPoint;
        }

        private static int GetDataEnd(int startPoint, PanelControl pan, DPoint[] dPoints, int ptCount, double xScale)
        {
            var endPoint = ptCount - 1;
            var x = (int) ((dPoints[endPoint].X*xScale) - pan.XOffset);

            if (x <= 2000)
                return endPoint;

            var lastEndPoint = endPoint;
            endPoint = endPoint - ((endPoint - startPoint)/2);
            while (true)
            {
                x = (int) ((dPoints[endPoint].X*xScale) - pan.XOffset);

                if (Math.Abs(x - 2000) <= 2)
                    break;
                if (x > 2000)
                {
                    if ((endPoint - startPoint) < 20)
                        break;
                    lastEndPoint = endPoint;
                    if (Math.Abs(endPoint - startPoint) <= 2)
                        break;
                    endPoint = endPoint - ((endPoint - startPoint)/2);
                }
                else if (x < 2000)
                {
                    if (ptCount - endPoint < 20)
                    {
                        endPoint = ptCount - 1;
                        break;
                    }
                    if (Math.Abs(lastEndPoint - endPoint) <= 2)
                        break;

                    endPoint = endPoint + ((lastEndPoint - endPoint)/2);
                }
            }

            return endPoint;
        }


        private void SetXScaleFromPoints()
        {
            double minValue = 0;
            GraphSurface gs;
            var maxValue = 0.0;
            var pan = Gt.Cst.MainPan;

            if (pan.TagList == null)
                return;
         
            foreach (var t in Gt.Cst.MainPan.TagList)
            {
                gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs?.DPts == null)
                    continue;
                var lastX = (float) 0.0;
                for (var j = 0; j < gs.PtCount; j++)
                {
                    if (float.IsInfinity(gs.DPts[j].X))
                        gs.DPts[j].X = lastX;
                    if (float.IsNaN(gs.DPts[j].X))
                        gs.DPts[j].X = lastX;
                    if (gs.DPts[j].X > maxValue)
                        maxValue = gs.DPts[j].X;
                    lastX = gs.DPts[j].X;
                }
            }

            switch (pan.XAxisType)
            {
                case XAxisType.MinMax:
                    maxValue = pan.XAxisMax - pan.XAxisMin;
                    break;
                case XAxisType.SetSpan:
                    minValue = maxValue - pan.XAxisSpan;
                    if (minValue < 0)
                        minValue = 0.0;
                    maxValue = pan.XAxisSpan;
                    break;
            }

            maxValue *= 1.01;
            // extra 1% so that right edge of graph is not hard against panel edge
            if (maxValue < 1E-06)
                maxValue = 1.0;

            Gt.Cst.MainPan.XScale = Gt.Cst.MainPan.GPan.Width/maxValue;
            switch (Gt.Cst.MainPan.XAxisType)
            {
                case XAxisType.MinMax:
                    if(ForceXReset)
                        Gt.Cst.MainPan.XOffset = (Gt.Cst.MainPan.XAxisMin*Gt.Cst.MainPan.XScale);
                    break;
                case XAxisType.SetSpan:
                    Gt.Cst.MainPan.XOffset = (minValue*Gt.Cst.MainPan.XScale);
                    break;
            }

            if (Gt.Cst.SubPan != null)
            {
                foreach (var t in Gt.Cst.SubPan)
                {
                    t.XScale = Gt.Cst.MainPan.XScale;
                    t.XOffset = Gt.Cst.MainPan.XOffset;
                }
            }

            foreach (var t in Gt.Cst.MainPan.TagList)
            {
                gs = GetGraphSurfaceFromTag(t.Tag);
                if (gs == null)
                    continue;
                gs.XScale = Gt.Cst.MainPan.XScale;
            }
        }

        private static void SetPointsScale(ref PanelControl pan, bool onOff)
        {
            if (pan == null)
                return;
            pan.NeedToSetScale = onOff;
        }

        public void SetTheGrid(bool onOff)
        {
            GraphParameters.DrawTheGrid = onOff;
        }

        public void SetGridStepOverride(bool onOff)
        {
            _grid.SetStepOverride(onOff);
            _gridOverride = onOff;
        }

        public void SetXGridStepOverride(bool onOff)
        {
            _grid.SetXStepOverride(onOff);          
        }

        public void SetBigPoints(bool onOff)
        {
            
            _bigPoints = onOff;
        }

        public void SetTheXAxis(bool onOff)
        {
            GraphParameters.DrawTheXAxis = onOff;
            ShowXAxisToolStripMenuItem.Checked = onOff;
        }

        public void SetTheYAxis(bool onOff)
        {
            GraphParameters.DrawTheYAxis = onOff;
            ShowYAxisToolStripMenuItem.Checked = onOff;
        }

        public void SetTheYAxisLegend(bool onOff)
        {
            GraphParameters.DrawTheYAxisLegend = onOff;
        }

        public void SetTheXAxisLegend(bool onOff)
        {
            GraphParameters.DrawTheXAxisLegend = onOff;
        }


        public void ResetX()
        {

            if (Gt.Cst.SubPan != null)
            {
                for (var i = 0; i < Gt.Cst.SubPan.Length; i++)
                {
                    SetPointsScale(ref Gt.Cst.SubPan[i], true);
                    ResetSingleX(ref Gt.Cst.SubPan[i]);
                }
            }
            SetPointsScale(ref Gt.Cst.MainPan, true);
            ResetSingleX(ref Gt.Cst.MainPan);
        }


        public void ResetY(PanelControl pan)
        {
            SetPointsScale(ref pan, true);
            ResetSingleY(ref pan);
        }


        public void Reset()
        {

            if (Gt.Cst.SubPan != null)
            {
                for (var i = 0; i < Gt.Cst.SubPan.Length; i++)
                {
                    SetPointsScale(ref Gt.Cst.SubPan[i], true);
                    ResetSingleX(ref Gt.Cst.SubPan[i]);
                    ResetSingleY(ref Gt.Cst.SubPan[i]);
                }
            }
            SetPointsScale(ref Gt.Cst.MainPan, true);
            ResetSingleX(ref Gt.Cst.MainPan);
            ResetSingleY(ref Gt.Cst.MainPan);
            RedrawAll();
            // Again to force a real reset
            RedrawAll();
        }

        private void ResetSingleX(ref PanelControl pan)
        {
            pan.XOffset = 0;
            pan.XPan = 0;
            pan.XScale = 1.0;
            pan.Zooming = false;
            GraphParameters.LastXScale = 1.0;
        }

        private void ResetSingleY(ref PanelControl pan)
        {
            pan.YOffset = 0;
            pan.YPan = 0;
            pan.YScale = 1.0;
            pan.Zooming = false;
            GraphParameters.LastYScale = 1.0;
        }

        public void UpdateLegend(PanelControl pan)
        {
            pan?.Legend.UpdateLegend(pan);
        }

        public void HideLegend()
        {
            if (Gt.Cst.MainPan == null) 
                return;
            if (Gt.Cst.MainPan.Legend.MaybeHide())
                SetLegendTick(ref Gt.Cst.MainPan, false);

            if (Gt.Cst.SubPan == null) 
                return;
            for (var i = 0; i < Gt.Cst.SubPan.Length; i++)
            {
                if (Gt.Cst.SubPan[i].Legend.MaybeHide())
                    SetLegendTick(ref Gt.Cst.SubPan[i], false);
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
            if (Gt.Cst.MainPan == null) 
                return;
            if (Gt.Cst.MainPan.Legend.MaybeShow())
                SetLegendTick(ref Gt.Cst.MainPan, true);

            if (Gt.Cst.SubPan == null) 
                return;
            for (var i = 0; i < Gt.Cst.SubPan.Length; i++)
            {
                if (Gt.Cst.SubPan[i].Legend.MaybeShow())
                    SetLegendTick(ref Gt.Cst.SubPan[i], true);
            }
        }

        public void SetSize(int width, int height)
        {
            GraphParameters.HaltLiveUpdate = true;
            Gt.SetSizes(width, height);
            GraphParameters.HaltLiveUpdate = false;
        }

        public int GraphTabSelected => Gt.GraphTabSelected;

        public void SetSelectedTab(int id)
        {
            HideLegend();
            TabControl1.SelectedIndex = id;
            IndexUpdate();
        }

        public void RenameCst(string name)
        {
            Gt.RenameCst(name);
        }

       
        public void SetActiveTab(int id,string name)
        {
            Gt.SetSelectedTab(id);
            SetMainPanelLegendName(name);
        }

        public void SetActiveTab(int id)
        {
            Gt.SetSelectedTab(id);
        }

        public int GetActiveTab()
        {
            return Gt.GraphTabSelected;
        }

        public void RemoveGraph(int tagId)
        {
            var id = -1;
            var cnt = Gt.Cst.Graphs.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (Gt.Cst.Graphs[i].TagId != tagId) 
                    continue;
                id = i;
                break;
            }
            if (id == -1)
                return;

            cnt -= 1;

            for (var i = id; i < cnt; i++)
                Gt.Cst.Graphs[i] = Gt.Cst.Graphs[i + 1];

            Array.Resize(ref Gt.Cst.Graphs, cnt);
            id = -1;
            cnt = Gt.Cst.MainPan.TagList.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (Gt.Cst.MainPan.TagList[i].Tag != tagId) 
                    continue;
                id = i;
                break;
            }
            if (id == -1)
                return;
            
            for (var i = 0; i < cnt; i++)
                Gt.Cst.MainPan.TagList[i].Master = false;

            Gt.Cst.MainPan.TagList[0].Master = true;

            cnt -= 1;
            for (var i = id; i < cnt; i++)
                Gt.Cst.MainPan.TagList[i] = Gt.Cst.MainPan.TagList[i + 1];


            Array.Resize(ref Gt.Cst.MainPan.TagList, cnt);
            id = -1;
            for (var i = 0; i < Gt.Cst.MainPan.ConMenu.MenuItems.Count; i++)
            {
                var mt = (MenuTag) Gt.Cst.MainPan.ConMenu.MenuItems[i].Tag;
                if (mt.GraphTag != tagId) 
                    continue;
                id = i;
                break;
            }
            if (id != -1)
                Gt.Cst.MainPan.ConMenu.MenuItems.RemoveAt(id);
        }

        public void DoYAxisMenu(int panelTag)
        {
            int pl;
            int pw;
            if (_yAxisConfiguration == null)
            {
                _yAxisConfiguration = new YAxisForm(this);
                _yAxisConfiguration.Show();
                _yAxisConfiguration.Hide();
            }

            if (panelTag == 0)
            {
                pl = Gt.Cst.MainPan.GPan.Left;
                pw = Gt.Cst.MainPan.GPan.Width;
                _yAxisConfiguration.SetGraphPanel(this, Gt.Cst.MainPan);
                _yAxisConfiguration.Top = Gt.Cst.MainPan.GPan.Top + GraphParameters.MouseY;
                _yAxisConfiguration.Left = Gt.Cst.MainPan.GPan.Left + GraphParameters.MouseX;
                if (_yAxisConfiguration.Left + _yAxisConfiguration.Width > pl + pw)
                    _yAxisConfiguration.Left = pl + pw - _yAxisConfiguration.Width - 50;
                _yAxisConfiguration.Show();
            }
            else
            {
                if (Gt.Cst.SubPan == null) 
                    return;
                panelTag = panelTag - 1;
                if (panelTag < 0 || panelTag >= Gt.Cst.SubPan.Length) 
                    return;
                _yAxisConfiguration.SetGraphPanel(this, Gt.Cst.SubPan[panelTag]);
                pl = Gt.Cst.SubPan[panelTag].GPan.Left;
                pw = Gt.Cst.SubPan[panelTag].GPan.Width;
                _yAxisConfiguration.Top = Gt.Cst.SubPan[panelTag].GPan.Top + GraphParameters.MouseY;
                _yAxisConfiguration.Left = Gt.Cst.SubPan[panelTag].GPan.Left + GraphParameters.MouseX;
                if (_yAxisConfiguration.Left + _yAxisConfiguration.Width > pl + pw)
                    _yAxisConfiguration.Left = pl + pw - _yAxisConfiguration.Width - 50;
                _yAxisConfiguration.Show();
            }
        }

        public void SetGraphColourPan(PanelControl pan, int tag, GraphColour col)
        {
            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList.Where(t => t.Tag == tag))
            {
                t.Colour = Params.GetPenColour(col);
                GraphParameters.ContextMenu.SetGraphMenuColourTick(t.Tag, t.Colour);
            }
        }

        public void SetGraphColour(int tag, GraphColour col)
        {
            SetGraphColourPan(Gt.Cst.MainPan, tag, col);
        }

        public void SetGraphColour(int panId,int tag, GraphColour col)
        {
            try
            {
                SetGraphColourPan(Gt.Cst.SubPan[panId], tag, col);
            }
            catch 
            {
               
            }
           
        }



        public Pen GetGraphColour(int tag)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return Pens.Black;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                return t.Colour;

            return Pens.Black;
        }

        public void SetGraphHl(int tag,bool highlight)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                t.Highlight = highlight;

            if (Gt.Cst.SubPan == null)
                return;

            foreach (var t in Gt.Cst.SubPan)
            {
                foreach (var t1 in t.TagList)
                {
                    if (t1.Tag == tag)
                        t1.Highlight = highlight;
                }
            }

            
        }

        public void SetGraphVisible(int tag, bool visible)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
            {
                t.Visible = visible;
                t.CanBeVisible = visible;
            }

            if (Gt.Cst.SubPan == null)
                return;

            foreach (var t in Gt.Cst.SubPan)
            {
                foreach (var t1 in t.TagList)
                {
                    if (t1.Tag == tag)
                    {
                        t1.Visible = visible;
                        t1.CanBeVisible = visible;
                    }
                }
            }


        }

        public void SetGraphAsBars(int tag, bool asBars,int offset)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
            {
                t.AsBar = asBars;
                t.BarOffset = offset;
            }

            if (Gt.Cst.SubPan == null)
                return;

            foreach (var t in Gt.Cst.SubPan)
            {
                
                foreach (var t1 in t.TagList)
                {
                    if (t1.Tag == tag)
                    {
                        t1.AsBar = asBars;
                        t1.BarOffset = offset;
                    }
                }
            }

        }

        public void SetGraphAsPoints(int tag, bool asPoints)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                t.AsPoint = asPoints;

            if (Gt.Cst.SubPan == null)
                return;

            foreach (var t in Gt.Cst.SubPan)
            {
                foreach (var t1 in t.TagList)
                {
                    if (t1.Tag == tag)
                        t1.AsPoint = asPoints;
                }
            }

        }

        public void SetUseForYScale(int tag, bool use)
        {
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                t.UseForYScale = use;

            if (Gt.Cst.SubPan == null)
                return;

            foreach (var t in Gt.Cst.SubPan)
            {
                foreach (var t1 in t.TagList)
                {
                    if (t1.Tag == tag)
                        t1.UseForYScale = use;
                }
            }
        }

        public Color GetGraphRgbColour(int tag)
        {

            if (Gt.Cst.MainPan.TagList == null)
                return Color.Transparent;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                return t.Colour.Color;
            
            return Color.Transparent;
        }


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

            x1 = (int) (x1/GraphParameters.LastXScale + (pan.XOffset/GraphParameters.LastXScale));
            pan.XScale = pan.XScale*scl;
            pan.XOffset = (x1*scl*GraphParameters.LastXScale);
            GraphParameters.LastXScale = scl*GraphParameters.LastXScale;

            Gt.Cst.MainPan.NeedToSetScale = false;

            if (Gt.Cst.SubPan == null) 
                return;

            var xOffset = pan.XOffset;
            var xScale = pan.XScale;
            Gt.Cst.MainPan.XScale = xScale;
            Gt.Cst.MainPan.XOffset = xOffset;

            foreach (var t in Gt.Cst.SubPan)
            {
                t.NeedToSetScale = false;
                t.XScale = xScale;
                t.XOffset = xOffset;
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

        public void AddTabChangedFunction(MethodInvoker cbk)
        {
            GraphParameters.AddTabChangedFunction(cbk);
        }

        public void SetGraphYAxisTitle(int tag, string yTitle)
        {
            if (Gt.Cst.Graphs == null)
                return;

            foreach (var t in Gt.Cst.Graphs.Where(t => t.TagId == tag))
                t.GyAxisTitle = yTitle;
        }

        public void SetAxisAndColour(int tag, string xTitle, string yTitle,GraphColour col)
        {
            SetGraphAxisTitles(tag,xTitle, yTitle);
            SetGraphColour(tag, col);
        }
        public void SetGraphAxisTitles(int tag, string xTitle, string yTitle)
        {
            if (Gt.Cst.Graphs == null)
                return;

            foreach (var t in Gt.Cst.Graphs.Where(t => t.TagId == tag))
            {
                t.GxAxisTitle = xTitle;
                t.GyAxisTitle = yTitle;
            }
        }

        public void SetGraphSource(int tag, string title)
        {
            if (Gt.Cst.Graphs == null)
                return;

            foreach (var t in Gt.Cst.Graphs.Where(t => t.TagId == tag))
                t.Source = title;
        }

        public void ClearLiveGraph(int tagId)
        {
            if (GraphParameters.LiveGraphs == null)
                return;

            var cnt = GraphParameters.LiveGraphs.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (GraphParameters.LiveGraphs[i].GraphId != tagId) 
                    continue;
                GraphParameters.LiveGraphs[i].LivePoints.Clear();
                GraphParameters.LiveGraphs[i].Started = false;
                GraphParameters.LiveGraphs[i].CurrentPt = 0;

                var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].GraphId);
                if (gs != null)
                    gs.PtCount = 0;
            }
        }

        public void RemoveAllFileGraphs()
        {
            var cnt = Gt.Cst.Graphs.Length;

            for (var i = 0; i < cnt; i++)
            {
                if (!Gt.Cst.Graphs[i].Source.Contains("File")) 
                    continue;
                RemoveGraph(Gt.Cst.Graphs[i].TagId);
                RemoveAllFileGraphs();
                return;
            }
        }

        public void RemoveAllGraphs()
        {

            var cnt = Gt.Cst.Graphs.Length;
        
            for (var i = cnt-1; i >= 0; i--)
            {               
                RemoveGraph(Gt.Cst.Graphs[i].TagId);                
            }
        }

        public void ClearAllBoundaryTypeData(GraphType gType)
        {
            if (Gt.Cst.Graphs == null)
                return;

            var cnt = Gt.Cst.Graphs.Length;

            for (var i = 0; i < cnt; i++)
            {
                if (Gt.Cst.Graphs[i].GType != gType) 
                    continue;
                RemoveGraph(Gt.Cst.Graphs[i].TagId);
                ClearAllBoundaryTypeData(gType);
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
                GraphParameters.LiveGraphs[i].LivePoints.Clear();
                GraphParameters.LiveGraphs[i].Started = false;
                GraphParameters.LiveGraphs[i].CurrentPt = 0;

                var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[i].GraphId);
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
        public void SetTabColour()
        {
            _tabText = Color.DarkBlue;
        }

        public void SetTabBold(bool bold)
        {
            _tabBold = bold;
        }
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabColour = Color.Black;
            var yPosition = e.Bounds.Y + 7;
            var f = _tabBold ? new Font(e.Font.FontFamily,e.Font.Size,FontStyle.Bold) : e.Font;
            
            var g = e.Graphics;
            var b = new SolidBrush(_backTabColour);           
            if (e.Index == TabControl1.SelectedIndex && _highlightTab)
                b = new SolidBrush(Color.DarkKhaki);
            else
            {
                tabColour = _tabText;
                yPosition -= 2;
            }

            g.FillRectangle(b, e.Bounds);
            b.Color = tabColour;           
            g.DrawString(TabControl1.TabPages[e.Index].Text, f, b, e.Bounds.X + 2,yPosition);
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
                    Gt.GraphTabs[i].Graphs[j].DPts = null;
             
                foreach (var t in Gt.GraphTabs[i].MainPan.TagList)
                {
                    t.ScrPoints = null;
                    t.DisplayPoints = null;
                }
                Gt.GraphTabs[i].MainPan.TagList = null;
                if (Gt.GraphTabs[i].SubPan != null)
                {                  
                    foreach (var t in Gt.GraphTabs[i].SubPan)
                    {
                        if (t.TagList != null)
                        {
                            foreach (var t1 in t.TagList)
                            {
                                t1.ScrPoints = null;
                                t1.DisplayPoints = null;
                            }
                        }
                        t.TagList = null;
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
            if (Gt.Cst.MainPan.TagList == null)
                return;

            var gs = GetGraphSurfaceFromTag(tid);
            if (gs == null)
                return;

            gs.PtCount = 0;
            UpdateGraph(tid, pts, pts.Length);
        }

        public int CreateLiveConnection(string title,string xAxis,string yAxis,GraphColour col)
        {
            var id = CreateLiveConnection(title, "");           
            SetGraphAxisTitles(id, xAxis, yAxis);
            ForceGraphColour(col, id, false);

            return id;
        }

        public int CreateLiveConnection(string title, string headTitle,string xAxis, string yAxis, GraphColour col)
        {
            var id = CreateLiveConnection(title, headTitle);
            SetGraphAxisTitles(id, xAxis, yAxis);
            ForceGraphColour(col, id, false);

            return id;
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

            GraphParameters.LiveGraphs[cnt].LivePoints = new ArrayList();
            GraphParameters.ResetPointCount = true;
            GraphParameters.LiveGraphs[cnt].GraphId = Gdata.AddNewGraph(pts, title, source, GraphType.Live, null);
            GraphParameters.ResetPointCount = false;
            GraphParameters.LiveGraphs[cnt].TabId = Gt.GraphTabSelected;   
            GraphParameters.LiveGraphs[cnt].Started = false;
            GraphParameters.LiveGraphs[cnt].CurrentPt = 0;

            return cnt;
        }

        public int GetLiveGraphId(int liveId)
        {
            return GraphParameters.LiveGraphs[liveId].GraphId;
        }

        public void LiveFullClear(int id)
        {
            if (GraphParameters.LiveGraphs == null)
                return;
            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;
            GraphParameters.LiveGraphs[id].LivePoints.Clear();
            GraphParameters.LiveGraphs[id].Started = false;
            GraphParameters.LiveGraphs[id].CurrentPt = 0;

        }

        public void ResetGraphCounters(int id)
        {
            var gs = GetGraphSurfaceFromTag(GraphParameters.LiveGraphs[id].GraphId);
            if (gs != null)
                gs.PtCount = 0;
        }
        
        public void AddLiveData(int id, double y)
        {
            AddLiveData(id, _lastX, y, false, false);
        }
        public void AddLiveData(int id, double x, double y)
        {
            AddLiveData(id, x, y, false, false);
            _lastX = x;
        }
        public void AddLiveData(int id, double x, double y, bool startPt, bool clearIt)
        {
            if (GraphParameters.LiveGraphs == null)
                return;

            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;

            if (_inLiveUpdate)
            {
                if (_luCnt > 15)
                    _luCnt = 0;
                else
                {
                    ++_luCnt;
                    return;
                }
            }

            _inLiveUpdate = true;
            try
            {
                DPoint dp = new DPoint();
                if (GraphParameters.LiveGraphs[id].Started)
                {
                    var cnt = GraphParameters.LiveGraphs[id].LivePoints.Count;
                    if(cnt > 0)
                        dp = (DPoint) GraphParameters.LiveGraphs[id].LivePoints[cnt - 1];

                    if (x <= dp.X && clearIt)
                    {
                        if (GraphParameters.ClearListCount <= 0)
                        {
                            if (startPt)
                            {
                                GraphParameters.LiveGraphs[id].LivePoints.Clear();
                                GraphParameters.LiveGraphs[id].CurrentPt = 0;
                            }
                            dp.X = (float) x;
                            dp.Y = (float) y;
                            dp.StartPt = startPt;

                            GraphParameters.LiveGraphs[id].LivePoints.Add(dp);
                            _inLiveUpdate = false;
                            return;
                        }
                        var counter = 0;
                        var startPosition = 0;
                        for (var i = cnt - 1; i >= 0; --i)
                        {
                            dp = (DPoint) GraphParameters.LiveGraphs[id].LivePoints[i];
                            if (!dp.StartPt) 
                                continue;
                            counter += 1;
                            if (counter < GraphParameters.ClearListCount) 
                                continue;
                            startPosition = i;
                            break;
                        }
                        if (startPosition != 0)
                        {
                            _temporaryArray.Clear();
                            for (var i = startPosition; i <= cnt; i++)
                            {
                                dp = (DPoint) GraphParameters.LiveGraphs[id].LivePoints[i];
                                _temporaryArray.Add(dp);
                            }
                            GraphParameters.LiveGraphs[id].LivePoints.Clear();
                            foreach (var t in _temporaryArray)
                            {
                                dp = (DPoint) t;
                                GraphParameters.LiveGraphs[id].LivePoints.Add(dp);
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
                GraphParameters.LiveGraphs[id].LivePoints.Add(dp);
            }
            catch
            {
                _inLiveUpdate = false;
            }
            _inLiveUpdate = false;
        }


        public void AddCyclicLiveData(int id, double[] x, double[] y)
        {
   
            if (GraphParameters.LiveGraphs == null)
                return;
            if (id < 0 || id >= GraphParameters.LiveGraphs.Length)
                return;

            if (_inLiveUpdate)
            {
                if (_luCnt > 15)
                    _luCnt = 0;
                else
                {
                    ++_luCnt;
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
                GraphParameters.LiveGraphs[id].LivePoints.Clear();
                GraphParameters.LiveGraphs[id].CurrentPt = 0;
                AddLiveArray(id, x, y);
                _inLiveUpdate = false;
                return;
            }

            var cnt = GraphParameters.LiveGraphs[id].LivePoints.Count - 1;
            DPoint dp;

            var counter = 0;
            var startPosition = 0;
            for (var i = cnt - 1; i >= 0; --i)
            {
                dp = (DPoint) GraphParameters.LiveGraphs[id].LivePoints[i];
                if (!dp.StartPt) 
                    continue;
                counter += 1;
                if (counter < GraphParameters.ClearListCount) 
                    continue;
                startPosition = i;
                break;
            }
            if (startPosition != 0)
            {
                _temporaryArray.Clear();
                for (var i = startPosition; i < cnt; i++)
                {
                    dp = (DPoint) GraphParameters.LiveGraphs[id].LivePoints[i];
                    _temporaryArray.Add(dp);
                }
                GraphParameters.LiveGraphs[id].LivePoints.Clear();
                foreach (var t in _temporaryArray)
                {
                    dp = (DPoint) t;
                    GraphParameters.LiveGraphs[id].LivePoints.Add(dp);
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
                GraphParameters.LiveGraphs[id].LivePoints.Add(dp);
                startPt = false;
            }
        }

        public void SetRealGraphColour(int tag, Color col)
        {
          
            var p = new Pen(col);
          
            if (Gt.Cst.MainPan.TagList == null)
                return;

            foreach (var t in Gt.Cst.MainPan.TagList.Where(t => t.Tag == tag))
                t.Colour = p;
        }

        public void SetInitVisible(bool setIt)
        {
            if (GraphParameters.CanSetVisible)
                GraphParameters.InitiallyVisible = setIt;
        }

        public void SetInitVisible2(bool setIt, bool can)
        {
            GraphParameters.CanSetVisible = can;
            GraphParameters.InitiallyVisible = setIt;
        }

        public int GetSubPanPosition(int id)
        {
            return GetTagPosFromGraphId(GetLiveGraphId(id));
        }
        public int GetTagPosFromGraphId(int tagId)
        {          
            if (Gt.Cst.MainPan.TagList == null)
                return -1;

            for (var i = 0; i < Gt.Cst.MainPan.TagList.Length; i++)
            {
                if (Gt.Cst.MainPan.TagList[i].Tag == tagId)
                    return i;
            }
            return 0;
        }

        public int GetTagPosFromLiveId(int id)
        {
            return GetTagPosFromGraphId(GetLiveGraphId(id));
        }

        //public static int GetTagPosFromGraphId(GraphControl[] tagList, int tagId)
        //{
          
        //    if (tagList == null)
        //        return -1;

        //    for (var i = 0; i < tagList.Length; i++)
        //    {
        //        if (tagList[i].Tag == tagId)
        //            return i;
        //    }
        //    return 0;
        //}

        public int GetTagFromGraphPos(int id)
        {
            if (id < 0)
                return -1;

            if (id >= Gt.Cst.Graphs.Length)
                return -1;

            return Gt.Cst.Graphs[id].TagId;
        }

        public void ClearPanel(PanelControl pan)
        {
            pan.GrSurface.Clear(pan.GPan.BackColor);
            pan.PanSurface.Clear(pan.GPan.BackColor);
            _grid.DrawGrid(pan);
            GraphParameters.GraphBoundary.FillBoundaries(pan);
            pan.PanSurface.DrawImage(pan.DrawingImage, pan.XPan, pan.YPan);
        }

        public int GetHeight()
        {
            return Gt.Cst.MainPan.GPan.Height;
        }

        public int GetWidth()
        {
            return Gt.Cst.MainPan.GPan.Width;
        }

        public int ForceSelectedTab(int id)
        {
            var cid = Gt.GraphTabSelected;
            Gt.SetSelectedTab(GraphParameters.LiveGraphs[id].TabId);
            return cid;
        }

        public int GetTabId(int id)
        {
            return GraphParameters.LiveGraphs[id].TabId;
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
            foreach (var t in Gt.Cst.MainPan.TagList)
            {
                var gs = GetGraphSurfaceFromTag(t.Tag);

                if (gs?.GType != GraphType.Boundary)
                    continue;

                if (Gt.Cst.SubPan == null) 
                    continue;
                foreach (var t1 in Gt.Cst.SubPan)
                {
                    var tag = t.Tag;
                    GraphParameters.ContextMenu.SetAsubtag(t1,t);
                    GraphParameters.ContextMenu.ResetAsubtag(t1, tag, true);
                }
            }
        }

        
        public void ForceAlternateYAxis()
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
            return gs.DPts;
        }

        public void ForceDarkBackground()
        {
            GraphOptions.ForceDarkBackground();
        }


        public void SetReducedMenuItems()
        {
            RemovePanelItem.Visible = false;          
            UseAlternateZeroYAxisToolStripMenuItem.Visible = false;          
        }

        private void ShowWaferBoundariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowBoundaries = !ShowBoundaries;
            HideMarkers = !ShowBoundaries;
        }

        private void BoundariesOnAllPanelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showAllPanBoundaries = !_showAllPanBoundaries;
        }

        public void ShowMarkersOnAllPanels(bool show)
        {
            _showAllPanBoundaries = show;
        }
        public void SetBoundaryVisible()
        {
            boundariesOnAllPanelsToolStripMenuItem.Visible = true;
            showWaferBoundariesToolStripMenuItem.Visible = true;
        }

        private static void SetLegendTick(ref PanelControl pan, bool ticked)
        {
            if (pan?.ConMenu == null)
                return;
            for (var j = 0; j < pan.ConMenu.MenuItems.Count; j++)
            {
                var menuTag = (MenuTag) pan.ConMenu.MenuItems[j].Tag;
                if (menuTag.ItemTag == GraphOption.Legend)
                    pan.ConMenu.MenuItems[j].Checked = ticked;
            }
        }

       
        private void ZoomAndPanAllTimePanelsTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomPanAll = !ZoomPanAll;
            zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Checked = ZoomPanAll;
        }

        public void SetFollowingCursor(bool canUse)
        {
            SetFCursor d = SetFollowingCursor2;
            try
            {
                Invoke(d, canUse);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private void SetFollowingCursor2(bool canUse)
        {
            UseFollowingCursoToolStripMenuItem.Enabled = canUse;
            if(!canUse)
                GraphOptions.TurnOffFollowingCursor();
        }

        private int _nextSubPanId;

        public void CreateSubPan(int min, int max, IList<int> gid)
        {
            CreateSubPan(_nextSubPanId, min, max, gid);
        }
        public void CreateSubPan(int pid, int min, int max, IList<int> gid)
        {
            var subPanList = new int[gid.Count];
            for (var i = 0; i < gid.Count; ++i)
                subPanList[i] = GetTagPosFromLiveId(gid[i]);

            Gt.AddPanel(subPanList, gid.Count);
            SetSubMinMaxType(pid, min, max);          
            _nextSubPanId = pid +1;
        }

        public void CreateSubPanCopy(IList<int> gid)
        {
            CreateSubPanCopy(_nextSubPanId,gid);
        }
        public void CreateSubPanCopy(int pid, IList<int> gid)
        {
            var subPanList = new int[gid.Count];
            for (var i = 0; i < gid.Count; ++i)
                subPanList[i] = GetTagPosFromLiveId(gid[i]);

            Gt.AddPanel(subPanList, gid.Count);
            SetSubMinMaxType(pid, Gt.Cst.MainPan.YAxisMin, Gt.Cst.MainPan.YAxisMax);
            
            _nextSubPanId = pid + 1;
        }

        public void SetMainMinMaxType(double min, double max)
        {
            Gt.Cst.MainPan.YAxisType = YAxisType.MinMax;
            Gt.Cst.MainPan.YAxisMin = min;
            Gt.Cst.MainPan.YAxisMax = max;
        }

        public void SetSubMinMaxType(int id,double min, double max)
        {
            Gt.Cst.SubPan[id].YAxisType = YAxisType.MinMax;
            Gt.Cst.SubPan[id].YAxisMin = min;
            Gt.Cst.SubPan[id].YAxisMax = max;
        }

        public void OverrideXAxisStep(double step, string format)
        {
            GraphParameters.OverrideXAxisStep(step, format);            
        }

        public void SetOverrideXAxisString(string str)
        {
            GraphParameters.SetOverrideXAxisString(str);            
        }

        internal void SetLoadFile(string name)
        {
            if (LoadDataCallback != null)
            {
                var ev = new LoadFileEventArgs { FileName = name };
                LoadDataCallback(this, ev);
            }
        }
        internal bool ValidFileExtension(string extension)
        {
            foreach (var t in _fileExtensions)
            {
                if (t.Equals(extension))
                    return true;
            }

            return false;
        }

        public void ClearExtensions()
        {            
            _fileExtensions.Clear();
        }
        public void AddLoadFileExtension(string extension)
        {
            
            _fileExtensions.Add(extension);
        }

        public double GetDouble(string value)
        {
            try
            {
                return double.Parse(value, _numberFormat);
            }
            catch
            {
                return 0;
            }
        }

        public float GetFloat(string value)
        {
            try
            {
                return float.Parse(value, _numberFormat);
            }
            catch
            {
                return 0;
            }
        }

        public bool IsLineNumeric(string line)
        {
            foreach (var t in line)
            {
                if (NonNumeric(t))
                    return false;
            }

            return true;
        }

        private static bool NonNumeric(char c)
        {
            switch (c)
            {
                case ',':
                case ' ':
                case '.':
                case '-':
                case '+':
                case 'E':
                    return false;
            }

            return c < '0' || c > '9';
        }

        private void StateButton_Click(object sender, EventArgs e)
        {
            StateButtonCallback?.Invoke();
        }

    }
}