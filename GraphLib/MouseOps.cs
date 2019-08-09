using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphLib.Properties;

namespace GraphLib
{
    public class MouseOps : IDisposable
    {
        private bool _alwaysMoveBoundary;    
        private int _currentX;
        private int _currentY;
        private int _firstBoundaryX;
        private int _firstBoundaryY;
        private bool _movingHorizontal;

        private GraphPanel _graphPanel;
        private Params _graphParameters;
        private GrTabPanel _graphicsTabPanel;
        private GrBoundary _graphBoundary;
        private TabMouseOps _tabMouse;      
        private int _markerMoveId = -1;

        private bool _mouseIsDown;
             
        private const double XExpander = 1.1;

       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
           
        }

        internal void Setup(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphicsTabPanel = _graphPanel.GraphParameters.GraphTabPanel;
            _graphBoundary = _graphPanel.GraphParameters.GraphBoundary;
            _tabMouse = new TabMouseOps(g);
            
        }
    
        internal void GrPan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _currentX = e.X;
            _currentY = e.Y;
            var tim = (e.X + _graphicsTabPanel.Cst.MainPan.XOffset) / _graphicsTabPanel.Cst.MainPan.XScale;
            _graphParameters.LastDoubleClickTime = tim;
            if (_graphParameters.DoubleClickCallback != null)
                _graphParameters.DoubleClickCallback();
            else
            {
                CancelRectCursor();
                _graphParameters.SetInitVisible(true);
                
                _graphParameters.DontAddToContextMenu(true);
                _graphBoundary.AddGeneralMarker(tim);
                _graphParameters.DontAddToContextMenu(false);
                _graphPanel.RedrawAll();
            }
        }

        internal void GrPan_MouseMove(object sender, MouseEventArgs e)
        {         
            var p = (Panel) sender;
            var pan = _graphParameters.GraphTabPanel.GetPanelControlFromTag((int) p.Tag);

            _currentX = e.X;
            _currentY = e.Y;

            if (_graphParameters.Panning)
            {
                DealWithPanMove(pan);
                return;
            }
          
            _graphParameters.MoveCallback?.Invoke();

            DrawCursor(pan, e.X, e.Y);
            _graphParameters.SetMoverBox(pan, e.X);
            _graphPanel.TheLastPanel = pan;
            _graphPanel.LastXyVx = e.X;

            if (_mouseIsDown)
                DealWithMouseDownMove(pan, e);
            else
                DealWithMouseUpMove(pan, e);
            

            DisplayActive();          
        }

        private void DisplayActive()
        {
            if (!_graphParameters.DisplayActiveValues)
                return;
            var str1 = Params.SetPlaces(_graphParameters.LastMarkerTime);
            var str = _graphParameters.LastMarkerTime.ToString(str1);

            _graphPanel.XValueLabel.Text = @"Time: " + str;
            str1 = Params.SetPlaces(_graphParameters.LastMarkerValue);
            str = _graphParameters.LastMarkerValue.ToString(str1);
            _graphPanel.YValueLabel.Text = @"Value: " + str;
        }
        
        internal void GrPan_MouseUp(object sender, MouseEventArgs e)
        {
            var p = (Panel) sender;
            var pan = _graphParameters.GraphTabPanel.GetPanelControlFromTag((int) p.Tag);
                 
            if (_mouseIsDown && _graphParameters.BoundaryMoveId != -1)
            {
                if (e.Button != MouseButtons.Right)
                {
                    _graphBoundary.MoveBoundary(pan, _graphParameters.BoundaryMoveId,
                        _graphParameters.BoundaryIndexId, e.X);
                    _graphParameters.BoundaryChangeCallback?.Invoke();
                }
            }

            if (_mouseIsDown && _markerMoveId != -1)
            {
                if(_movingHorizontal)
                     _graphBoundary.MoveHMarker(pan, _markerMoveId, e.Y);
                else
                    _graphBoundary.MoveMarker(pan, _markerMoveId, e.X);
                if (_graphParameters.MarkerChangeCallback != null)
                {
                    _graphParameters.MarkerX = (e.X + pan.XOffset)/pan.XScale;
                    _graphParameters.MarkerChangeCallback();
                }
            }

            if (_mouseIsDown && e.Button.Equals(MouseButtons.Left))
            {
                var dx = (_graphParameters.BoxTopX - _graphParameters.BoxBotX);
                var dy = (_graphParameters.BoxTopY - _graphParameters.BoxBotY);

                if (Math.Abs(dx) < 30 || Math.Abs(dy) < 30)
                {
                    
                    if (_graphPanel.Gdata.NearGraph(pan, e.X, e.Y))                     
                        _graphParameters.ReverseGraphHighlight(pan, _graphPanel.Gdata.LastNearGtag);                    
                }
            }

            _markerMoveId = -1;
            _graphParameters.BoundaryMoveId = -1;

            _graphParameters.MoveFromLeft = false;
            _graphParameters.MoveFromRight = false;
            _mouseIsDown = false;
            if (EndRectCursor(ref pan, e.X, e.Y))
                _graphPanel.RedrawAll();

            if (_graphParameters.Panning)
            {
                if (pan.XPanStart == e.X && pan.YPanStart == e.Y)
                    _graphParameters.Panning = false;
            }

            UpPanning(pan);
           

            pan.GPan.Cursor = Cursors.Cross;
            _graphParameters.PauseLiveData(false);
        }

        private void UpPanning(PanelControl pan)
        {
            
            if (!_graphParameters.Panning)
                return;
            
            pan.XPanStart = 0;
            pan.YPanStart = 0;
            pan.XOffset = pan.XOffset - pan.XPan;
            pan.YOffset = pan.YOffset - pan.YPan;
            pan.XPan = 0;
            pan.YPan = 0;
            if (InTimeList())
            {
                int current = _graphicsTabPanel.GraphTabSelected;
                for (var i = 0; i < _graphicsTabPanel.TabCount; ++i)
                {
                    _graphicsTabPanel.SetSelectedTab(i);
                    if (!InTimeList())
                        continue;
                    PanAll(pan.XOffset);
                }
                _graphicsTabPanel.SetSelectedTab(current);
            }
            else
                PanAll(pan.XOffset);

            _graphParameters.Panning = false;
            _graphicsTabPanel.Cst.MainPan.GPan.ContextMenu = pan.ConMenu;
            pan.GPan.ContextMenu = pan.ConMenu;
            _graphPanel.RedrawAll();
            
        }
        private void PanAll(double xOffset)
        {
            if (_graphicsTabPanel.Cst.SubPan == null)
                return;
            _graphicsTabPanel.Cst.MainPan.XOffset = xOffset;
            foreach (var t in _graphicsTabPanel.Cst.SubPan)
                t.XOffset = xOffset;
        }

        private void DownLeftButton(MouseEventArgs e)
        {
           
            if (_graphParameters.BoundaryMoveId == -1 && _markerMoveId == -1)
                StartRectCursor(e.X, e.Y);
            
        }

        private void DownRightButton(PanelControl pan, MouseEventArgs e)
        {
            if (_graphParameters.BoundaryMoveId != -1)
                return;
            pan.XPanStart = e.X;
            pan.YPanStart = e.Y;
            _graphParameters.Panning = true;
        }

        internal void GrPan_MouseDown(object sender, MouseEventArgs e)
        {
            var p = (Panel) sender;
            var pan = _graphicsTabPanel.GetPanelControlFromTag((int) p.Tag);

            _mouseIsDown = true;
            _graphParameters.MouseX = e.X;
            _graphParameters.MouseY = e.Y;
            _firstBoundaryX = e.X;
            _firstBoundaryY = e.Y;
            _alwaysMoveBoundary = false;
            _graphParameters.PauseLiveData(true);

            if (e.Button.Equals(MouseButtons.Left))
                DownLeftButton(e);
            else if (e.Button.Equals(MouseButtons.Right))
                DownRightButton(pan, e);

            _graphParameters.LastXPosition = (e.X + pan.XOffset)/pan.XScale;
        }

        internal void GrPan_MouseEnter(object sender, EventArgs e)
        {
            var p = (Panel) sender;
            var pan = _graphicsTabPanel.GetPanelControlFromTag((int) p.Tag);
            SetCursorActive(pan, true);

            if (_graphParameters.LiveGraphs != null)
            {
                for (var i = 0; i < _graphParameters.LiveGraphs.Length; i++)
                {
                    if (_graphParameters.LiveGraphs[i].TabId == _graphicsTabPanel.GraphTabSelected)
                        return;
                }
            }

            _graphParameters.LastPanel = pan;
        }

        internal void GrPan_MouseLeave(object sender, EventArgs e)
        {
            var p = (Panel) sender;
            var pan = _graphicsTabPanel.GetPanelControlFromTag((int) p.Tag);
            if (_graphParameters.DisplayCursor)
                _graphPanel.ClearCursors(pan);

            SetCursorActive(pan, false);
        }

        

        internal void GrPan_Paint(object sender, PaintEventArgs e)
        {
            _graphPanel.RedrawAll();
        }
    

        private static Button CreateXyButton(int tag, int dx, int dy, Image image, string tTip)
        {                        
            var button = new Button
            {
                Tag = tag,             
                BackgroundImage = image,
                BackgroundImageLayout = ImageLayout.Stretch,
                Location = new Point(777, 78),
                Size = new Size(dx, dy),
                UseVisualStyleBackColor = true
            };

            var tt = new ToolTip();
            tt.SetToolTip(button, tTip);

            return button;
        }

        internal void AddYExpandContractButtons(PanelControl pan, int tag)
        {
            pan.ExpandYTop = CreateXyButton(tag, 15, 20, Resources.DownArrow, "Expand Y Axis From Top");
            pan.ExpandYTop.Click += ExpandYTop_Click;

            pan.ContractYTop = CreateXyButton(tag, 15, 20, Resources.UpArrow, "Contract Y Axis To Top");
            pan.ContractYTop.Click += ContractYTop_Click;

            pan.ExpandYBottom = CreateXyButton(tag, 15, 20, Resources.UpArrow, "Expand Y Axis From Bottom");
            pan.ExpandYBottom.Click += ExpandYBottom_Click;

            pan.ContractYBottom = CreateXyButton(tag, 15, 20, Resources.DownArrow, "Contract Y Axis To Bottom");
            pan.ContractYBottom.Click += ContractYBottom_Click;

            pan.Reset = CreateXyButton(tag, 15, 15, Resources.Reset, "Reset Y Axis");
            pan.Reset.Click += Reset_Click;

            _graphicsTabPanel.Cst.Page.Controls.Add(pan.ExpandYTop);
            _graphicsTabPanel.Cst.Page.Controls.Add(pan.ContractYTop);
            _graphicsTabPanel.Cst.Page.Controls.Add(pan.ExpandYBottom);
            _graphicsTabPanel.Cst.Page.Controls.Add(pan.ContractYBottom);
            _graphicsTabPanel.Cst.Page.Controls.Add(pan.Reset);
        }

        internal void AddXExpandContractButtons(GrTabControl tab)
        {
            tab.ExpandX = CreateXyButton(0, 20, 15, Resources.RightArrow, "Expand the X Axis");
            tab.ExpandX.Click += ExpandButton_Click;

            tab.ContractX = CreateXyButton(0, 20, 15, Resources.LeftArrow, "Contract the X Axis");
            tab.ContractX.Click += ContractButton_Click;

            tab.ResetX = CreateXyButton(0, 15, 15, Resources.Reset, "Reset All X & Y Axes");
            tab.ResetX.Click += ResetButton_Click;

            tab.Page.Controls.Add(tab.ExpandX);
            tab.Page.Controls.Add(tab.ContractX);
            tab.Page.Controls.Add(tab.ResetX);
        }

        private PanelControl GetPanelFromButton(object sender)
        {
            var p = (Button) sender;
            return _graphicsTabPanel.GetPanelControlFromTag((int) p.Tag);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            var pan = GetPanelFromButton(sender);

            pan.YOffset = 0;
            pan.YPan = 0;
            pan.YScale = 1.0;
            pan.Zooming = false;
            _graphPanel.RedrawAll();
        }

       

        private void ExpandYBottom_Click(object sender, EventArgs e)
        {
            var pan = GetPanelFromButton(sender);

            pan.YScale = pan.YScale*1.5;
            pan.YOffset = (pan.YOffset*1.5);
            _graphPanel.RedrawAll();
        }

        private void ContractYBottom_Click(object sender, EventArgs e)
        {
            var pan = GetPanelFromButton(sender);

            pan.YOffset = (pan.YOffset/1.5);
            pan.YScale = pan.YScale/1.5;
            _graphPanel.RedrawAll();
        }

        private void ExpandYTop_Click(object sender, EventArgs e)
        {
            var pan = GetPanelFromButton(sender);

            pan.YScale = pan.YScale*1.5;
            pan.YOffset = (pan.YOffset*1.5);
            pan.YOffset -= (pan.GPan.Height/2.0);
            _graphPanel.RedrawAll();
        }

        private void ContractYTop_Click(object sender, EventArgs e)
        {
            var pan = GetPanelFromButton(sender);

            pan.YOffset += (pan.GPan.Height/2.0);
            pan.YOffset = (pan.YOffset/1.5);
            pan.YScale = pan.YScale/1.5;
            _graphPanel.RedrawAll();
        }

        private void ExpandButton_Click(object sender, EventArgs e)
        {
            if (_graphicsTabPanel.Cst.SubPan != null)
            {
                foreach (var value in _graphicsTabPanel.Cst.SubPan)
                {
                    ScaleX(value, XExpander);
                    value.XOffset *= XExpander;
                }
            }
            ScaleX(_graphicsTabPanel.Cst.MainPan, XExpander);
            _graphicsTabPanel.Cst.MainPan.XOffset *= XExpander;
            _graphPanel.RedrawAll();
        }

        private void ContractButton_Click(object sender, EventArgs e)
        {
            if (_graphicsTabPanel.Cst.SubPan != null)
            {
                foreach (var value in _graphicsTabPanel.Cst.SubPan)
                {
                    value.XOffset /= XExpander;
                    ScaleX(value, 1.0/XExpander);
                }
            }
            _graphicsTabPanel.Cst.MainPan.XOffset /= XExpander;
            ScaleX(_graphicsTabPanel.Cst.MainPan, 1.0/XExpander);
            _graphPanel.RedrawAll();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void ResetAll()
        {
            if (InTimeList())
            {
                var current = _graphicsTabPanel.GraphTabSelected;
                for (var i = 0; i < _graphicsTabPanel.TabCount; ++i)
                {
                    _graphicsTabPanel.SetSelectedTab(i);
                    if (!InTimeList())
                        continue;
                    _graphPanel.Reset();
                }
                _graphicsTabPanel.SetSelectedTab(current);
            }
            else
                _graphPanel.Reset();
        }

       
        private void CancelRectCursor()
        {
            _graphParameters.BoxCursor = false;
        }
    

        private void DealWithPanMove(PanelControl pan)
        {
            pan.XPan = _currentX - pan.XPanStart;
            pan.YPan = _currentY - pan.YPanStart;
            if (Math.Abs(pan.XPan) > 10 || Math.Abs(pan.YPan) > 10)
            {
                pan.GPan.ContextMenu = null;
                pan.GPan.Cursor = Cursors.NoMove2D;
                _graphPanel.RedrawAll();
            }
            else
            {
                pan.XPan = 0;
                pan.YPan = 0;
            }
        }

        private void DealWithMouseDownMove(PanelControl pan, MouseEventArgs e)
        {
            if (!e.Button.Equals(MouseButtons.Left))
                return;
            if(_movingHorizontal)
                DealWithYMove(pan,e.Y);
            else
                DealWithXMove(pan, e.X);

        }

        private void DealWithXMove(PanelControl pan,int x)
        {
            if (Math.Abs(_firstBoundaryX - x) > 5 || _alwaysMoveBoundary)
            {
                _graphParameters.PauseLiveData(true);
                _alwaysMoveBoundary = true;
                if (_graphParameters.BoundaryMoveId != -1)
                {
                    pan.GPan.ContextMenu = null;
                    _graphBoundary.MoveBoundary(pan, _graphParameters.BoundaryMoveId,
                        _graphParameters.BoundaryIndexId, x);
                }
                else if (_markerMoveId != -1)
                {
                    pan.GPan.ContextMenu = null;
                    _graphBoundary.MoveMarker(pan, _markerMoveId, x);
                }
            }
        }
        private void DealWithYMove(PanelControl pan, int y)
        {
            if (Math.Abs(_firstBoundaryY - y) > 5 || _alwaysMoveBoundary)
            {
                _graphParameters.PauseLiveData(true);
                _alwaysMoveBoundary = true;
                
                if (_markerMoveId != -1)
                {
                    pan.GPan.ContextMenu = null;
                    _graphBoundary.MoveHMarker(pan, _markerMoveId, y);
                }
            }
        }

        private bool CheckBoundaryMove(PanelControl pan,int xPosition)
        {
            if (!_graphPanel.ShowBoundaries)
                return false;
        
            var typeId = GraphManipulationType.NotSet;
            _graphParameters.BoundaryMoveId = _graphBoundary.NearBoundary(pan, out var id, ref typeId, xPosition);
            _graphParameters.BoundaryIndexId = id;
            if (_graphParameters.BoundaryMoveId == -1)
                return false;

            pan.GPan.Cursor = Cursors.SizeWE;
            pan.GPan.ContextMenu = typeId == GraphManipulationType.WbWaferStartStop ? pan.BoundaryConMenu : null;
            _graphParameters.LastBoundaryMoveId = _graphParameters.BoundaryMoveId;
            return true;
        }

        private bool CheckMarkerMove(PanelControl pan, int xPosition, int yPosition)
        {
            if (!_graphPanel.ShowBoundaries)
                return false;
            _movingHorizontal = false;
            _markerMoveId = _graphBoundary.NearMarker(pan, xPosition);
            if (_markerMoveId == -1)
            {
                _markerMoveId = _graphBoundary.NearHMarker(pan, yPosition);
                if (_markerMoveId == -1)
                    return false;
                _movingHorizontal = true;
            }
               
            if (!_graphBoundary.CanMoveMarker)
                return true;
            if(_movingHorizontal)
                pan.GPan.Cursor = Cursors.SizeNS;
            else
                pan.GPan.Cursor = Cursors.SizeWE;
            pan.GPan.ContextMenu = pan.MarkerConMenu;
            _graphParameters.LastMarkerMoveId = _markerMoveId;
            return true;
        }

        private void DealWithMouseUpMove(PanelControl pan, MouseEventArgs e)
        {
            int tag = (int) pan.GPan.Tag;
            if (tag == 0)
            {
                if (CheckBoundaryMove(pan, e.X))
                    return;

                if (CheckMarkerMove(pan, e.X,e.Y))
                    return;
            }

            if (_graphParameters.CheckGraphTooltip)
            {
                            
                if (_graphPanel.Gdata.NearGraph(pan, e.X, e.Y))
                    _graphPanel.Gdata.ToolTipGraph(pan, _graphPanel.Gdata.LastNearString, e.X, e.Y);
            }           
            pan.GPan.ContextMenu = pan.ConMenu;
            pan.GPan.Cursor = Cursors.Cross;
        }

        private void DrawCursor(PanelControl pan, int x, int y)
        {
            _graphParameters.CursorY = y;
            _graphParameters.CursorX = x;
            _graphParameters.BoxBotX = x;
            _graphParameters.BoxBotY = y;
            _graphParameters.CursorPan = pan;
            _graphPanel.RefreshDrawing(pan);
        }

        private void StartRectCursor(int x, int y)
        {
            if (_graphicsTabPanel.Cst.MainPan.TagList == null)
                return;

            _graphParameters.BoxCursor = true;
            _graphParameters.BoxTopX = x;
            _graphParameters.BoxTopY = y;
        }

        private bool EndRectCursor(ref PanelControl pan, int x, int y)
        {
            int temp;
            var dx = (_graphParameters.BoxTopX - _graphParameters.BoxBotX);
            var dy = (_graphParameters.BoxTopY - _graphParameters.BoxBotY);
            if (!_graphParameters.BoxCursor)
            {
                _graphPanel.RefreshDrawing(pan);
                return false;
            }
            _graphParameters.BoxCursor = false;

            if (Math.Abs(dx) < 30 || Math.Abs(dy) < 30)
            {
                _graphPanel.RefreshDrawing(pan);
                return false;
            }

            _graphParameters.BoxBotX = x;
            _graphParameters.BoxBotY = y;

            if (dx > 0)
            {
                temp = _graphParameters.BoxBotX;
                _graphParameters.BoxBotX = _graphParameters.BoxTopX;
                _graphParameters.BoxTopX = temp;
            }
            if (dy > 0)
            {
                temp = _graphParameters.BoxBotY;
                _graphParameters.BoxBotY = _graphParameters.BoxTopY;
                _graphParameters.BoxTopY = temp;
            }

            var wid = pan.GPan.Width;
            var hei = pan.GPan.Height;
            var scl = (double)wid/Math.Abs(dx);

            _graphParameters.BoxTopX =
                (int) (_graphParameters.BoxTopX/_graphParameters.LastXScale + (pan.XOffset/_graphParameters.LastXScale));
            pan.XScale = pan.XScale*scl;
            pan.XOffset = (_graphParameters.BoxTopX*scl*_graphParameters.LastXScale);
            _graphParameters.LastXScale = scl*_graphParameters.LastXScale;

            scl = (double)hei/Math.Abs(dy);
            pan.YScale = pan.YScale*scl;
            pan.YOffset = (pan.YOffset + _graphParameters.BoxTopY - hei)*scl;
            pan.YOffset += hei;
            pan.Zooming = true;
            _graphParameters.LastYScale = scl*_graphParameters.LastYScale;

            if (InTimeList())
            {
                int current = _graphicsTabPanel.GraphTabSelected;
                for (var i = 0; i < _graphicsTabPanel.TabCount; ++i)
                {
                    _graphicsTabPanel.SetSelectedTab(i);
                    if (!InTimeList())
                        continue;
                    ScaleAllPans(pan.XOffset, pan.XScale);
                }
                _graphicsTabPanel.SetSelectedTab(current);
            }
            else
                ScaleAllPans(pan.XOffset, pan.XScale);

           
            return true;
        }

        private readonly List<int> _zoomList = new List<int>(); 
        internal void AddZoomPan(int id)
        {
            _zoomList.Add(id);
        }
        private bool InTimeList()
        {
            if (!_graphPanel.ZoomPanAll)
                return false;
            if (_zoomList == null)
                return false;
            if (_zoomList.Count <= 0)
                return false;
            foreach (var t in _zoomList)
            {
                if (_graphicsTabPanel.GraphTabSelected == t)
                    return true;
            }
          
            return false;
        }

        internal bool InTimeList2()
        {
            if (_zoomList == null)
                return false;
            if (_zoomList.Count <= 0)
                return false;
            foreach (var t in _zoomList)
            {
                if (_graphicsTabPanel.GraphTabSelected == t)
                    return true;
            }

            return false;
        }

        private void ScaleAllPans(double xOffset,double xScale)
        {                      
            _graphicsTabPanel.Cst.MainPan.NeedToSetScale = false;

            if (_graphicsTabPanel.Cst.SubPan == null)
                return;

            _graphicsTabPanel.Cst.MainPan.XScale = xScale;
            _graphicsTabPanel.Cst.MainPan.XOffset = xOffset;

            foreach (var t in _graphicsTabPanel.Cst.SubPan)
            {
                t.NeedToSetScale = false;
                t.XScale = xScale;
                t.XOffset = xOffset;
            }
        }
        private static void ScaleX(PanelControl pan, double val)
        {
            pan.XScale = pan.XScale*val;
            pan.NeedToSetScale = false;
        }
      
        private void SetCursorActive(PanelControl pan, bool active)
        {
            _graphParameters.CursorActive = active;
            if (!active)
            {
                _graphParameters.CrossX = 0;
                _graphParameters.CrossY = 0;
                _graphParameters.CursorY = 0;
                _graphParameters.CursorX = 0;
            }
            _graphPanel.RefreshDrawing(pan);
        }

        internal void TabMouseMove(object sender, MouseEventArgs e)
        {
            _tabMouse.TabMouseMove(e.X, e.Y);
        }

        internal void TabMouseUp(object sender, MouseEventArgs e)
        {
            _tabMouse.TabMouseUp();
        }

        internal void TabMouseDown(object sender, MouseEventArgs e)
        {
            _tabMouse.TabMouseDown(e.X, e.Y);
        }
    }
}