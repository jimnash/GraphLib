using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
// ReSharper disable UnusedMember.Global

namespace GraphLib
{
    public class BoundaryEventArgs : EventArgs
    {
        public int BoundarySetId { get; set; }
    }

    public class GrBoundary
    {
        private GraphPanel _graphPanel;
        private Params _graphParameters;
        private GrTabPanel _graphTabPanel;

        private InputBox _inputBox;
        private double _zeroPoint1;
        private double _zeroPoint2;

        public Marker Markers { get; private set; }
        public bool CanMoveMarker { get;  set; }

        public event EventHandler<BoundaryEventArgs> BoundaryCallback;

        public GrBoundary()
        {
            CanMoveMarker = true;
        }

        internal void Setup(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphTabPanel = _graphParameters.GraphTabPanel;
            Markers = new Marker(_graphPanel);
        }     

        public void GetZeroPoints(out double p1, out double p2)
        {
            p1 = _zeroPoint1;
            p2 = _zeroPoint2;
        }

        internal static bool IsAnyMarkerOrBoundary(GraphType gType)
        {
            return gType == GraphType.MoveableMarker || gType == GraphType.Boundary || gType == GraphType.FixedMarker;
        }

        private static bool IsGraphSurfaceBoundaryOk(GraphSurface gs)
        {
            if (gs?.GType != GraphType.Boundary)
                return false;
            return gs.ObjectTag != null;
        }

        internal void FillBoundaries(PanelControl pan)
        {
            if (_graphTabPanel.Cst.Graphs == null)
                return;
            if (pan.TagList == null)
                return;

            var pp = new Pen(Color.Black);
            var hbr = new HatchBrush(HatchStyle.Percent50, Color.Wheat, Color.Transparent);

            foreach (var t in pan.TagList)
            {
                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);
                if (!IsGraphSurfaceBoundaryOk(gs))
                    continue;

                if (gs.ObjectTag.TypeId == GraphManipulationType.MeasurementMarker)
                {
                    if (gs.ObjectTag.Values == null)
                        continue;
                    if (gs.ObjectTag.Values[0] < 0.0)
                        continue;
                }

                pp.Color = t.Colour.Color;
                pp.Width = t.Highlight ? 2 : 1;

                for (var j = 0; j < gs.PtCount - 1; j += 2)
                {
                    var xx1 = gs.DPts[j].X;
                    var xx2 = gs.DPts[j + 1].X;
                    var x1 = Params.GetXScreenPoint(pan, gs, xx1);
                    int x2;
                    if (xx2 > xx1)
                        x2 = Params.GetXScreenPoint(pan, gs, xx2);
                    else
                    {
                        x2 = Params.GetXScreenPoint(pan, gs, 360);
                        pan.GrSurface.FillRectangle(hbr, x1, 0, x2 - x1, pan.GPan.Height);
                        x1 = Params.GetXScreenPoint(pan, gs, 0);
                        x2 = Params.GetXScreenPoint(pan, gs, xx2);
                    }
                    pan.GrSurface.FillRectangle(hbr, x1, 0, x2 - x1, pan.GPan.Height);
                }
            }
        }

        internal void DrawBoundaries(PanelControl pan)
        {
            var pp = new Pen(Color.Black);

            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList)
            {
                if (!t.Visible)
                    continue;

                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);
                if (!IsGraphSurfaceBoundaryOk(gs))
                    continue;

                if (gs.ObjectTag.TypeId == GraphManipulationType.MeasurementMarker)
                {
                    if (gs.ObjectTag.Values?[0] < 0.0)
                        continue;
                }

                pp.Color = t.Colour.Color;
                pp.Width = t.Highlight ? 2 : 1;

                for (var j = 0; j < gs.PtCount; j++)
                {
                    var x = Params.GetXScreenPoint(pan, gs, gs.DPts[j].X);
                    pan.GrSurface.DrawLine(pp, x, 0, x, pan.GPan.Height);
                }

                


            }
        }

        internal void DrawToolTips(PanelControl pan)
        {
            if (pan.TagList == null)
                return;

            var yOffset = 0;
            var f = new Font("timesnewroman", 8);
            var pp = new Pen(Color.Black);

            foreach (var t in pan.TagList)
            {
                if (!t.Visible)
                    continue;

                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);
                if (!IsGraphSurfaceBoundaryOk(gs))
                    continue;

                if (gs.ObjectTag.TypeId == GraphManipulationType.MeasurementMarker)
                {
                    if (gs.ObjectTag.Values?[0] < 0.0)
                        continue;
                }

                pp.Color = t.Colour.Color;
                pp.Width = t.Highlight ? 2 : 1;

                for (var j = 0; j < gs.PtCount; j++)
                {
                    var x = Params.GetXScreenPoint(pan, gs, gs.DPts[j].X);

                    double val = gs.DPts[j].X;
                    string valueString;
                    if (val > 1000)
                        valueString = ((int) val).ToString(CultureInfo.InvariantCulture);
                    else
                    {
                        var str1 = Params.SetPlaces(val);
                        valueString = val.ToString(str1);
                    }

                    string str;
                    if (_graphParameters.DrawBoundaryStrings)
                    {
                        str = gs.Name + " " + valueString;
                        pan.GrSurface.FillRectangle(Brushes.LightGray, x, yOffset*25, str.Length*8, 25);
                        pan.GrSurface.DrawRectangle(Pens.Black, x, yOffset*25, str.Length*8, 25);
                        pan.GrSurface.DrawString(str, f, Brushes.Black, x + 5, yOffset*25);
                    }
                    else
                    {
                        if (_graphParameters.BoundaryMoveId == gs.TagId && _graphParameters.BoundaryIndexId == j)
                        {
                            str = gs.Name + " " + valueString;
                            pan.GrSurface.FillRectangle(Brushes.LightGray, x, 5, str.Length*8, 25);
                            pan.GrSurface.DrawRectangle(Pens.Black, x, 5, str.Length*8, 25);
                            pan.GrSurface.DrawString(str, f, Brushes.Black, x + 5, 10);
                        }
                    }

                    yOffset += 1;
                    if (yOffset > 3)
                        yOffset = 0;
                }
            }
        }

        internal void RemoveBoundaryClick(object sender, EventArgs e)
        {
            var id = -1;
            var cnt = 0;
            if (_graphTabPanel.Cst.Graphs == null)
                return;

            var len = _graphTabPanel.Cst.Graphs.Length;

            foreach (var t in _graphTabPanel.Cst.Graphs)
            {
                if (t.ObjectTag.TypeId == GraphManipulationType.WbWaferStartStop)
                    ++cnt;
            }
            
            if (cnt == 1)
            {
                MessageBox.Show(@"You Can't Remove The Last Zone

There Has To Be At Least One");
                return;
            }
           
            for (var i = 0; i < len; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].TagId != _graphParameters.LastBoundaryMoveId) 
                    continue;
                id = i;
                break;
            }

            if (id < 0 || id >= len)
                return;

            if (_graphTabPanel.Cst.Graphs[id].ObjectTag == null)
                return;


            _graphPanel.RemoveGraph(_graphTabPanel.Cst.Graphs[id].TagId);
            _graphPanel.RedrawAll();
            _graphParameters.BoundaryChangeCallback?.Invoke();
            if (BoundaryCallback == null) 
                return;
            try
            {
                var ev = new BoundaryEventArgs {BoundarySetId = -1};
                BoundaryCallback(this, ev);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }                   
        }

        internal void AddWaferBoundaryClick(object sender, EventArgs e)
        {
            var myId = 0;
            var pts1 = new double[2];
            var error = false;
            var gid = -1;

            pts1[0] = 0.0;

            if (_graphTabPanel.Cst.Graphs == null)
                return;

            foreach (var t in _graphTabPanel.Cst.Graphs.Where(t => t.ObjectTag != null))
            {
                if (t.ObjectTag.GroupId > gid)
                    gid = t.ObjectTag.GroupId;

                if (t.ObjectTag.TypeId != GraphManipulationType.WbWaferStartStop)
                    continue;

                if (Math.Abs(_graphParameters.LastXPosition - t.DPts[0].X) < 3.0)
                {
                    pts1[0] = _graphParameters.LastXPosition - 15.0;
                    pts1[1] = pts1[0] + 10.0;
                    myId = t.ObjectTag.MyId;
                    break;
                }
                if (Math.Abs(_graphParameters.LastXPosition - t.DPts[1].X) < 3.0)
                {
                    pts1[0] = _graphParameters.LastXPosition + 5.0;
                    pts1[1] = pts1[0] + 10.0;
                    myId = t.ObjectTag.MyId;
                    break;
                }
                gid = -1;
            }

            if (gid == -1)
                return;


            foreach (var t in _graphTabPanel.Cst.Graphs.Where(t => t.ObjectTag != null).Where(t => t.ObjectTag.TypeId == GraphManipulationType.WbWaferStartStop))
            {
                if (pts1[0] > t.DPts[0].X && pts1[0] < t.DPts[1].X)
                    error = true;
                else if (pts1[1] > t.DPts[0].X && pts1[1] < t.DPts[1].X)
                    error = true;

                if (!error) 
                    continue;
                MessageBox.Show(@"Not Enough Space To Add A Zone At This Point");
                return;
            }
      
            _graphParameters.DontAddToContextMenu(true);           
            var objectTag = new GraphManipulationTag
            {
                GroupId = -1,
                MyId = myId,
                TypeId = GraphManipulationType.WbWaferStartStop
            };
            var id = _graphPanel.Gdata.AddNewGraph(pts1, "Zone: Start/Stop ", "BOU", GraphType.Boundary, objectTag);
            _graphPanel.SetGraphColour(id, GraphColour.Red);
            _graphParameters.DontAddToContextMenu(false);
            _graphPanel.RedrawAll();
            ForceBoundariesOnPan();
            SortAngleNames(GraphManipulationType.WbWaferStartStop, "Zone: ");
            _graphParameters.BoundaryChangeCallback?.Invoke();
            if (BoundaryCallback == null) 
                return;
            var ev = new BoundaryEventArgs {BoundarySetId = myId};
            BoundaryCallback(this, ev);
        }

        internal void WaferBoundaryZoom(object sender, EventArgs e)
        {
            var id = -1;
            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].TagId != _graphParameters.LastBoundaryMoveId) 
                    continue;
                id = i;
                break;
            }

            if (id < 0 || id >= _graphTabPanel.Cst.Graphs.Length)
                return;

            if (_graphTabPanel.Cst.Graphs[id].ObjectTag == null)
                return;

            if (_graphTabPanel.Cst.Graphs[id].ObjectTag.TypeId != GraphManipulationType.WbWaferStartStop)
                return;

            var x1 = (int)
                ((_graphTabPanel.Cst.Graphs[id].DPts[0].X*_graphTabPanel.Cst.MainPan.XScale) -
                 _graphTabPanel.Cst.MainPan.XOffset);
            var x2 = (int)
                ((_graphTabPanel.Cst.Graphs[id].DPts[1].X*_graphTabPanel.Cst.MainPan.XScale) -
                 _graphTabPanel.Cst.MainPan.XOffset);
            _graphPanel.SetXScaleByXPos(_graphTabPanel.Cst.MainPan, x1 - 5, x2 + 5);
            _graphPanel.RedrawAll();
        }

        internal void TextMarkerClick(object sender, EventArgs e)
        {
            if (!CheckMarkerClick())
                return;

            _graphParameters.MarkerTextCallback?.Invoke();
        }

        private bool CheckMarkerClick()
        {
            var id = _graphParameters.LastMarkerMoveId;
            if (id < 0 || id >= _graphTabPanel.Cst.Graphs.Length)
                return false;
            if (_graphTabPanel.Cst.Graphs[id].ObjectTag == null)
                return false;

            if (_graphTabPanel.Cst.Graphs[id].ObjectTag.TypeId != GraphManipulationType.GeneralMarker) 
                return false;

            _zeroPoint1 = _graphTabPanel.Cst.Graphs[id].DPts[0].X;
            _zeroPoint2 = _graphTabPanel.Cst.Graphs[id].DPts[0].X;
            return true;
        }

        internal void RemoveMarkerClick(object sender, EventArgs e)
        {
            if (!_graphParameters.AllowRemoveMarker)
                return;
            var id = _graphParameters.LastMarkerMoveId;

            if (id < 0 || id >= _graphTabPanel.Cst.Graphs.Length)
                return;
            if (_graphTabPanel.Cst.Graphs[id].ObjectTag == null)
                return;
            if (_graphTabPanel.Cst.Graphs[id].ObjectTag.TypeId != GraphManipulationType.GeneralMarker) 
                return;

            _graphPanel.RemoveGraph(_graphTabPanel.Cst.Graphs[id].TagId);
            _graphPanel.RedrawAll();
        }

        private void ForceBoundariesOnPan()
        {
            foreach (var t in _graphTabPanel.Cst.MainPan.TagList)
            {
                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);

                if (gs?.GType != GraphType.Boundary)
                    continue;

                if (_graphTabPanel.Cst.SubPan == null) 
                    continue;
                foreach (var t1 in _graphTabPanel.Cst.SubPan)
                {
                    if (!_graphParameters.ContextMenu.ResetAsubtag(t1,t.Tag, true))
                        _graphParameters.ContextMenu.SetAsubtag(t1,t);
                }
            }
        }

        private void ForceMarkersOnPan()
        {
            const GraphType gType = GraphType.MoveableMarker;
            foreach (var t in _graphTabPanel.Cst.MainPan.TagList)
            {
                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);
                if (gs?.GType != gType)
                    continue;

                if (_graphTabPanel.Cst.SubPan == null) 
                    continue;
                foreach (var t1 in _graphTabPanel.Cst.SubPan)
                {
                    if (!_graphParameters.ContextMenu.ResetAsubtag(t1,t.Tag, true))
                        _graphParameters.ContextMenu.SetAsubtag(t1,t);
                }
            }
        }

        public int AddFixedMarker(double tim)
        {
            return AddFixedMarker(tim, null);
        }

        public int AddFixedMarker(double tim, string value)
        {
            return AddGeneralMarker(tim, value, GraphType.FixedMarker);
        }

        public int AddGeneralMarker(double tim)
        {
            return AddGeneralMarker(tim, null);
        }

        public int AddGeneralMarker(double tim, string value)
        {
            return AddGeneralMarker(tim, null, GraphType.MoveableMarker);
        }

        private int AddGeneralMarker(double tim,string value,GraphType gType)
        { 
            var tagId = 0;
            var bPoints = new double[1];
            var tag = new GraphManipulationTag {TypeId = GraphManipulationType.GeneralMarker};

            if (_graphTabPanel.Cst.MainPan.TagList != null)
            {
                foreach (var t in _graphTabPanel.Cst.MainPan.TagList)
                {
                    var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);

                    if (gs.ObjectTag.TypeId != GraphManipulationType.GeneralMarker) 
                        continue;
                    if (gs.ObjectTag.MyId > tagId)
                        tagId = gs.ObjectTag.MyId;
                }
            }
            tag.MyId = tagId + 1;
            tag.Pts = new double[1];

            tag.Pts[0] = 0.0;
            bPoints[0] = tim;

            var lab = "(M" + tag.MyId + ")";
            if (value != null)                          
                lab = "(" + value + ")";

            tagId = _graphPanel.Gdata.AddNewGraph(bPoints, lab, "TAG", gType, tag);

            _graphPanel.SetGraphColour(tagId, GraphColour.Brown);

            _graphParameters.DontAddToContextMenu(true);
            ForceMarkersOnPan();
            _graphParameters.DontAddToContextMenu(false);
            return tagId;
        }

        public GraphManipulationTag[] GetBoundaryData()
        {
            double maxA = 0;

            var cnt = _graphTabPanel.Cst.Graphs.Count(t => t.ObjectTag != null);
            if (cnt == 0)
                return null;
            var gmt = new GraphManipulationTag[cnt];
            cnt = -1;
            foreach (var t in _graphTabPanel.Cst.Graphs.Where(t => t.ObjectTag != null))
            {
                ++cnt;

                gmt[cnt] = new GraphManipulationTag
                {
                    GroupId = t.ObjectTag.GroupId,
                    MyId = t.ObjectTag.MyId,
                    TypeId = t.ObjectTag.TypeId,
                    GraphTag = t.ObjectTag.GraphTag,
                    Pts = null
                };

                switch (gmt[cnt].TypeId)
                {
                    case GraphManipulationType.GeneralMarker:
                        gmt[cnt].Pts = new double[2];
                        gmt[cnt].Pts[0] = t.DPts[0].X;
                        gmt[cnt].Pts[1] = t.DPts[0].Y;
                        break;
                    case GraphManipulationType.MeasurementMarker:
                        gmt[cnt].Pts = new double[1];
                        gmt[cnt].Pts[0] = t.DPts[0].X;
                        if (t.ObjectTag.Values != null)
                        {
// ReSharper disable once CompareOfFloatsByEqualityOperator
                            if (maxA == 0)
                                maxA = t.ObjectTag.Values[0];
                            else
                            {
                                // ReSharper disable once CompareOfFloatsByEqualityOperator
                                if (t.ObjectTag.Values[0] == 0)
                                    t.ObjectTag.Values[0] = maxA;
                            }
                        }
                        break;
                    case GraphManipulationType.WbWaferStartStop:
                        gmt[cnt].Pts = new double[2];
                        gmt[cnt].Pts[0] = t.DPts[0].X;
                        gmt[cnt].Pts[1] = t.DPts[1].X;
                        gmt[cnt].GraphTag = t.TagId;
                        break;
                }
            }
            return gmt;
        }
 

        internal void SetStartStopAngle(object sender, EventArgs e)
        {

            if (_inputBox == null)
                _inputBox = new InputBox();

            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].ObjectTag == null)
                    continue;
                if (_graphTabPanel.Cst.Graphs[i].ObjectTag.TypeId != GraphManipulationType.WbWaferStartStop) 
                    continue;
                if (!(Math.Abs(_graphTabPanel.Cst.Graphs[i].DPts[0].X - _graphParameters.LastXPosition) < 3.0) &&
                    !(Math.Abs(_graphTabPanel.Cst.Graphs[i].DPts[1].X - _graphParameters.LastXPosition) < 3.0))
                    continue;

                _inputBox.Start(_graphTabPanel.Cst.Graphs[i].DPts[0].X,
                    _graphTabPanel.Cst.Graphs[i].DPts[1].X);

                if (!CheckMoveIsOk(i, (float) _inputBox.TheStartAngle, (float) _inputBox.TheStopAngle))
                    return;

                _graphTabPanel.Cst.Graphs[i].DPts[0].X = (float) _inputBox.TheStartAngle;
                _graphTabPanel.Cst.Graphs[i].DPts[1].X = (float) _inputBox.TheStopAngle;
                SortAngleNames(GraphManipulationType.WbWaferStartStop, "Zone: ");
                _graphParameters.BoundaryChangeCallback?.Invoke();
                if (BoundaryCallback == null) 
                    continue;
                var ev = new BoundaryEventArgs
                {
                    BoundarySetId = _graphTabPanel.Cst.Graphs[i].ObjectTag.MyId
                };
                BoundaryCallback(this, ev);
            }
        }

        private bool MoveBowWafer(int gid, double newX)
        {
            var dx = _graphTabPanel.Cst.Graphs[gid].DPts[1].X - _graphTabPanel.Cst.Graphs[gid].DPts[0].X;
            var dx1 = Math.Abs(newX - _graphTabPanel.Cst.Graphs[gid].DPts[0].X);
            var dx2 = Math.Abs(newX - _graphTabPanel.Cst.Graphs[gid].DPts[1].X);
            if (!_graphParameters.MoveFromLeft && !_graphParameters.MoveFromRight)
            {
                if (dx1 < dx2)
                    _graphParameters.MoveFromLeft = true;
                else
                    _graphParameters.MoveFromRight = true;
            }
            if (dx1 < dx2 && _graphParameters.MoveFromLeft)
            {
                if (!MoveFromLeft(gid, newX, dx))
                    return false;
            }
            else if (_graphParameters.MoveFromRight)
            {
                if (!MoveFromRight(gid, newX))
                    return false;
            }

            SortAngleNames(GraphManipulationType.WbWaferStartStop, "Zone: ");
            return true;
        }

        public bool Check360 = true;
        private bool MoveFromRight(int gid, double newX)
        {
            if (Check360)
            {
                if (newX < 1.0)
                    newX = 350;

                if (newX > 358.0)
                    newX = 10.0;
            }
           

            if (!CheckMoveIsOk(gid, _graphTabPanel.Cst.Graphs[gid].DPts[0].X, (float) newX))
                return false;

            var dx2 = newX - _graphTabPanel.Cst.Graphs[gid].DPts[0].X;
            if (dx2 < 2)
                return false;

            _graphTabPanel.Cst.Graphs[gid].DPts[1].X = (float) newX;

            return true;
        }

        private bool MoveFromLeft(int gid, double newX, double dx)
        {
            if (Check360)
            {
                if (newX < 1.0)
                    newX = 1.0;
                if (newX > 358.0)
                    newX = 358.0;

                if (newX + dx > 358.0)
                    newX = 358.0 - dx;
            }
           
            if (!CheckMoveIsOk(gid, (float) newX, (float) (newX + dx)))
                return false;

            _graphTabPanel.Cst.Graphs[gid].DPts[0].X = (float) newX;
            if ((newX + dx) <= 0)
                _graphTabPanel.Cst.Graphs[gid].DPts[1].X = 358;
            else
                _graphTabPanel.Cst.Graphs[gid].DPts[1].X = (float) (newX + dx);

            return true;
        }

        private bool CheckMoveIsOk(int id, float startX, float stopX)
        {
            var ok = true;
            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (i == id)
                    continue;

                if (_graphTabPanel.Cst.Graphs[i].ObjectTag == null)
                    continue;
                if (_graphTabPanel.Cst.Graphs[i].ObjectTag.TypeId != GraphManipulationType.WbWaferStartStop) 
                    continue;

                if (startX > _graphTabPanel.Cst.Graphs[i].DPts[0].X &&
                    startX < _graphTabPanel.Cst.Graphs[i].DPts[1].X)
                {
                    ok = false;
                    break;
                }

                if (stopX > _graphTabPanel.Cst.Graphs[i].DPts[0].X &&
                    stopX < _graphTabPanel.Cst.Graphs[i].DPts[1].X)
                {
                    ok = false;
                    break;
                }

                if (startX < _graphTabPanel.Cst.Graphs[i].DPts[0].X &&
                    stopX > _graphTabPanel.Cst.Graphs[i].DPts[0].X)
                {
                    ok = false;
                    break;
                }

                if (!(stopX < startX)) 
                    continue;
                if (startX < _graphTabPanel.Cst.Graphs[i].DPts[0].X)
                {
                    ok = false;
                    break;
                }
                if (stopX > _graphTabPanel.Cst.Graphs[i].DPts[0].X)
                {
                    ok = false;
                    break;
                }
            }
            if (!ok)
                MessageBox.Show(@"Inconsistent Wafer Boundary Action

Operation Ignored");
            return ok;
        }


        public void MoveBoundary(PanelControl pan, int gid, int pid, int x)
        {
            var ok = false;
            if (gid == -1 || pid == -1)
                return;

            if (_graphTabPanel.Cst.Graphs == null)
                return;
    
            var newX = (x + pan.XOffset)/pan.XScale;
            var found = false;

            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].TagId != gid) 
                    continue;
                gid = i;
                found = true;
                break;
            }
            if (!found)
                return;

            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag == null)
                return;


            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag.TypeId == GraphManipulationType.WbWaferStartStop)
                ok = MoveBowWafer(gid, newX);
            else
                _graphTabPanel.Cst.Graphs[gid].DPts[pid].X = (float) newX;
            if (ok && BoundaryCallback != null)
            {
                var ev = new BoundaryEventArgs {BoundarySetId = _graphTabPanel.Cst.Graphs[gid].ObjectTag.MyId};
                BoundaryCallback(this, ev);
            }
            _graphPanel.RedrawAll();
        }

        internal int NearMarker(PanelControl pan, int xPosition)
        {
            if (!CanMoveMarker)
                return -1;
            if (_graphTabPanel.Cst.Graphs == null)
                return -1;

            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].GType != GraphType.MoveableMarker)
                    continue;

                var x = Params.GetXScreenPoint(pan, _graphTabPanel.Cst.Graphs[i], _graphTabPanel.Cst.Graphs[i].DPts[0].X);

                if (Math.Abs(x - xPosition) < 3)
                    return i;
            }
            return -1;
        }


        internal int NearHMarker(PanelControl pan, int yPosition)
        {
            if (!CanMoveMarker)
                return -1;
            if (_graphTabPanel.Cst.Graphs == null)
                return -1;

            for (var i = 0; i < _graphTabPanel.Cst.Graphs.Length; i++)
            {
                if (_graphTabPanel.Cst.Graphs[i].GType != GraphType.MoveableMarker)
                    continue;

                var y = Params.GetYScreenPoint(pan,  _graphTabPanel.Cst.Graphs[i].DPts[0].Y);

                if (Math.Abs(y - yPosition) < 3)
                    return i;
            }
            return -1;
        }


        internal void MoveMarker(PanelControl pan, int gid, int x)
        {
            if (!CanMoveMarker)
                return;
            if (gid == -1)
                return;

            var newX = (x + pan.XOffset)/pan.XScale;

            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag == null)
                return;

            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag.TypeId == GraphManipulationType.GeneralMarker)
            {
                _graphTabPanel.Cst.Graphs[gid].DPts[0].X = (float) newX;
                _graphPanel.RedrawAll();              
                return;
            }
            _graphPanel.RedrawAll();
        }

        internal void MoveHMarker(PanelControl pan, int gid, int y)
        {
            if (!CanMoveMarker)
                return;
            if (gid == -1)
                return;

            var newY = _graphParameters.GetYFromScr(pan, y);

            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag == null)
                return;

            if (_graphTabPanel.Cst.Graphs[gid].ObjectTag.TypeId == GraphManipulationType.GeneralMarker)
            {
                _graphTabPanel.Cst.Graphs[gid].DPts[0].Y = (float)newY;
                _graphPanel.RedrawAll();
                return;
            }
            _graphPanel.RedrawAll();
        }

        internal int NearBoundary(PanelControl pan, out int indexId, ref GraphManipulationType typeId, int xPosition)
        {          
            indexId = -1;
            if (_graphTabPanel.Cst.Graphs == null)
                return -1;

            foreach (var t in _graphTabPanel.Cst.Graphs)
            {
                if (t.GType != GraphType.Boundary)
                    continue;
                if (t.ObjectTag.TypeId == GraphManipulationType.NotSet)
                    continue;

                if (t.ObjectTag.TypeId == GraphManipulationType.MeasurementMarker)
                    continue;

                for (var j = 0; j < t.PtCount; j++)
                {
                    var x = Params.GetXScreenPoint(pan, t, t.DPts[j].X);

                    if (Math.Abs(x - xPosition) >= 3) 
                        continue;
                    indexId = j;
                    typeId = t.ObjectTag.TypeId;
                    return t.TagId;
                }
            }

            return -1;
        }

        public void SortAngleNames(GraphManipulationType typ, string tit)
        {
            var sorted = false;
            var cnt = _graphTabPanel.Cst.Graphs.Count(t => t.ObjectTag.TypeId == typ);

            if (cnt == 0)
                return;

            var nms = new Anam[cnt];
            cnt = 0;            

            for (var j = 0; j < _graphTabPanel.Cst.Graphs.Length; j++)
            {
                if (_graphTabPanel.Cst.Graphs[j].ObjectTag.TypeId != typ)
                    continue;
                nms[cnt].X = _graphTabPanel.Cst.Graphs[j].DPts[0].X;
                nms[cnt].Gid = j;
                ++cnt;
            }

            while (!sorted)
            {
                sorted = true;
                for (var i = 0; i < nms.Length - 1; i++)
                {
                    if (!(nms[i].X > nms[i + 1].X)) 
                        continue;
                    Anam temp;
                    temp.X = nms[i + 1].X;
                    temp.Gid = nms[i + 1].Gid;
                    nms[i + 1].X = nms[i].X;
                    nms[i + 1].Gid = nms[i].Gid;

                    nms[i].X = temp.X;
                    nms[i].Gid = temp.Gid;
                    sorted = false;
                }
            }
            for (var i = 0; i < cnt; i++)
                _graphTabPanel.Cst.Graphs[nms[i].Gid].Name = tit + (i + 1);
        }
    }
}