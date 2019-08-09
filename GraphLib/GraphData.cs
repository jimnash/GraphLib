using System;
using System.Windows.Forms;
using System.Drawing;

namespace GraphLib
{
    public delegate void AddNewGraphCallback(DPoint[] pts, string title, string source,
                                             GraphType gType, GraphManipulationTag tag);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    internal class GraphData : IDisposable
    {
        private readonly GraphPanel _graphPanel;
        private readonly GrTabPanel _graphTabPanel;
        private readonly Params _graphParameters;
        private readonly ToolTip _graphToolTip = new ToolTip();

        private int _lastTx;
        private int _lastTy;
        private int _offsetTagValue;
        private bool _useOffsetTag;

        public int AbsoluteGraphTag { private set; get; }
        public int InitialAllocation { private get; set; }


        private readonly Pen[] _presetColours =
        {
            Pens.Red,
            Pens.Blue,
            Pens.Green,
            Pens.Brown,
            Pens.Purple,
            Pens.Yellow,
            Pens.DarkCyan,
            Pens.DeepSkyBlue
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public GraphData(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphTabPanel = _graphPanel.GraphParameters.GraphTabPanel;
            InitialAllocation = 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            _graphToolTip.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useIt"></param>
        /// <param name="val"></param>
        public void UseOffsetTagValue(bool useIt, int val)
        {
            _useOffsetTag = useIt;
            _offsetTagValue = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bPoints"></param>
        /// <param name="title"></param>
        /// <param name="source"></param>
        /// <param name="gType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int AddNewGraph(double[] bPoints, string title, string source, GraphType gType,
            GraphManipulationTag tag)
        {

            var pts = new DPoint[bPoints.Length];
            for (var i = 0; i < bPoints.Length; i++)
            {
                pts[i].X = (float) bPoints[i];
                pts[i].Y = 0;
                pts[i].StartPt = false;
            }
            pts[0].StartPt = true;
            if (tag == null)
                tag = new GraphManipulationTag();

            return AddNewGraph(pts, title, source, gType, tag);

        }
     
        /// <summary>
        /// Reduced for simplicity
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="title"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public int AddNewGraph(DPoint[] pts, string title, string source)           
        {
            // ReSharper disable once IntroduceOptionalParameters.Global
            return AddNewGraph(pts, title, source, GraphType.Graph, null);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="title"></param>
        /// <param name="source"></param>
        /// <param name="gType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int AddNewGraph(DPoint[] pts, string title, string source, GraphType gType, 
                               GraphManipulationTag tag)
        {
            if (tag == null)
                tag = new GraphManipulationTag();
                
            if (_graphPanel.InvokeRequired)
            {
                var d = new AddNewGraphCallback(AddTNewGraph);
                _graphPanel.Invoke(d, pts, title, source, gType, tag);
            }
            else
                AddTNewGraph(pts, title, source, gType, tag);

            if (_useOffsetTag)
                return _offsetTagValue - 1;

            return AbsoluteGraphTag - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="title"></param>
        /// <param name="source"></param>
        /// <param name="gType"></param>
        /// <param name="objectTag"></param>
        private void AddTNewGraph(DPoint[] pts, string title, string source, 
                                  GraphType gType, GraphManipulationTag objectTag)
        {
            var tid = 0;
            var id = 0;           
            var remTag = 0;

            if (_graphTabPanel.Cst.MainPan.TagList != null)         
                tid = _graphTabPanel.Cst.MainPan.TagList.Length;

            if (_useOffsetTag)
            {
                remTag = AbsoluteGraphTag;
                AbsoluteGraphTag = _offsetTagValue;
            }

            Array.Resize(ref _graphTabPanel.Cst.MainPan.TagList, tid + 1);
            _graphTabPanel.Cst.MainPan.TagList[tid] = new GraphControl(AbsoluteGraphTag, _presetColours[tid % _presetColours.Length])
            {
                Visible = _graphParameters.InitiallyVisible,
                CanBeVisible = _graphParameters.InitiallyVisible,
                Master = false
            };


            if (_graphTabPanel.Cst.Graphs != null)          
                id = _graphTabPanel.Cst.Graphs.Length;

            objectTag.GraphTag = AbsoluteGraphTag;
            Array.Resize(ref _graphTabPanel.Cst.Graphs, id + 1);
            _graphTabPanel.Cst.Graphs[id] = new GraphSurface
            {
                PtCount = 0,
                DyScale = 1.0,              
                MaxPtCount = Math.Max(pts.Length, InitialAllocation),                
                GType = gType,
                ObjectTag = objectTag,
                Name = title,
                Source = source,
                TagId = AbsoluteGraphTag,
                GxAxisTitle = _graphTabPanel.Cst.MainPan.XAxisTitle,
                GyAxisTitle = _graphTabPanel.Cst.MainPan.YAxisTitle,              
            };
            _graphTabPanel.Cst.Graphs[id].DPts = new DPoint[_graphTabPanel.Cst.Graphs[id].MaxPtCount];
                      
            for (var i = 0; i < pts.Length; i++)
                _graphTabPanel.Cst.Graphs[id].DPts[i] = pts[i];
 
            _graphTabPanel.Cst.Graphs[id].DPts[0].StartPt = true;

            _graphTabPanel.Cst.Graphs[id].PtCount = pts.Length;
            if (_graphParameters.ResetPointCount)
                _graphTabPanel.Cst.Graphs[id].PtCount = 0;

            double max = pts[0].Y;
            var min = 0.0;
            if (max < min)
                min = max;

            if (GrBoundary.IsAnyMarkerOrBoundary(gType))
                max = 1.0;
            else
            {
                switch (_graphTabPanel.Cst.MainPan.YAxisType)
                {
                    case YAxisType.Free:
                        for (var i = 1; i < pts.Length; i++)
                        {
                            if (pts[i].Y > max)
                                max = pts[i].Y;
                            if (pts[i].Y < min)
                                min = pts[i].Y;
                        }
                        break;
                    case YAxisType.MinMax:
                        min = _graphTabPanel.Cst.MainPan.YAxisMin;
                        max = _graphTabPanel.Cst.MainPan.YAxisMax;
                        break;
                    default:
                        max = _graphPanel.GetMaxOnTab(1, _graphTabPanel.Cst.MainPan);
                        min = _graphPanel.GetMinOnTab(1, _graphTabPanel.Cst.MainPan);
                        break;
                }
            }
            if ((max - min) < 1E-06)
                max = min + 1.0;
       

            _graphTabPanel.Cst.Graphs[id].MaxD = max;
            _graphTabPanel.Cst.Graphs[id].MinD = min;
            if (!GrBoundary.IsAnyMarkerOrBoundary(gType))
            {
                SetMasterGraph(ref _graphTabPanel.Cst.MainPan, 0);
                SetMasterGraph(ref _graphTabPanel.Cst.MainPan, id);
            }

            _graphParameters.ContextMenu.UpdateContextMenu(gType, title);
            _graphParameters.ContextMenu.SetGraphMenuColourTick(AbsoluteGraphTag, _graphTabPanel.Cst.MainPan.TagList[tid].Colour);
            for (var i = 0; i <= tid; i++)
                _graphTabPanel.Cst.MainPan.TagList[i].Master = false;
            for (var i = 0; i <= tid; i++)
            {
                if (!_graphTabPanel.Cst.MainPan.TagList[i].Visible) 
                    continue;
                GpContextMenu.SetMaster(_graphTabPanel.Cst.MainPan.ConMenu, _graphTabPanel.Cst.MainPan.TagList[i].Tag);
                _graphTabPanel.Cst.MainPan.TagList[i].Master = true;
                _graphParameters.MasterCallBack();
                break;
            }
            if (_useOffsetTag)
            {
                AbsoluteGraphTag = remTag;
                _offsetTagValue += 1;
            }
            else
                AbsoluteGraphTag += 1;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="id"></param>
        private void SetMasterGraph(ref PanelControl pan, int id)
        {  
            if (pan.TagList == null)
                return;
           
            foreach (var t in pan.TagList)
                t.Master = false;
          
            pan.TagList[id].Master = true;
            _graphParameters.MasterCallBack();
        }
       


        public string LastNearString { get; private set; }
        // ReSharper disable once IdentifierTypo
        public int LastNearGtag { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool NearGraph(PanelControl pan, int x, int y)
        {
            LastNearString = null;
            LastNearGtag = -1;
            if (pan.TagList == null)
                return false;

            foreach (var t in pan.TagList)           
            {
                if (!t.Visible)
                    continue;

                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);

                if (gs == null)
                    continue;

                if (gs.GType != GraphType.Graph && gs.GType != GraphType.Live)
                    continue;

                for (var j = 0; j < t.ScrPCount - 1; j++)
                {
                    var x1 = t.ScrPoints[j].X - 5;
                    var x2 = t.ScrPoints[j + 1].X + 5;

                    if (x < x1 - 5)
                        break; 

                    if (x > x2 + 5)
                        continue;

                    var y1 = t.ScrPoints[j].Y;
                    var y2 = t.ScrPoints[j + 1].Y;

                    if (y1 > y2)
                    {
                        var tempY = y1;
                        y1 = y2;
                        y2 = tempY;
                    }

                    if (y < y1 - 5 || y > y2 + 5)
                        continue;

                    y1 = t.ScrPoints[j].Y;
                    y2 = t.ScrPoints[j + 1].Y;
                    x1 = t.ScrPoints[j].X;
                    x2 = t.ScrPoints[j + 1].X;
                    if (!(ShortestDistance(x, y, x1, y1, x2, y2) < 10)) 
                        continue;
                    if (gs.Source.Length > 0)
                        LastNearString = gs.Name + " [" + gs.Source + "]";
                    else
                        LastNearString = gs.Name;
                    LastNearGtag = t.Tag;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="lineX1"></param>
        /// <param name="lineY1"></param>
        /// <param name="lineX2"></param>
        /// <param name="lineY2"></param>
        /// <returns></returns>
        private static double ShortestDistance(double pointX, double pointY, double lineX1, double lineY1, double lineX2, double lineY2)
        {
            var a = pointX - lineX1;
            var b = pointY - lineY1;
            var c = lineX2 - lineX1;
            var d = lineY2 - lineY1;

            var dist = Math.Abs(a * d - c * b) / Math.Sqrt(c * c + d * d);

            return dist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="gString"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ToolTipGraph(PanelControl pan, string gString, int x, int y)
        {
            x = pan.GPan.Left + x;
            y = pan.GPan.Top + y + 50;
            if (Math.Abs(x - _lastTx) < 5 && Math.Abs(y - _lastTy) < 5)
                return;

            _graphToolTip.BackColor = Color.LightGray;
            _graphToolTip.ForeColor = Color.Black;
            _graphToolTip.IsBalloon = false;
            _graphToolTip.Show(gString, _graphPanel, x, y, 2000);

            _lastTx = x;
            _lastTy = y;
        }   
    }
}
