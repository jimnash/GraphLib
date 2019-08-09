using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
// ReSharper disable EmptyGeneralCatchClause

namespace GraphLib
{
    public class GrTabPanel
    {
        public GrTabControl[] GraphTabs;
        private bool _canSetPanel;
        private GrBoundary _graphBoundary;
        private GraphPanel _graphPanel;
        private Params _graphParameters;

        private int _graphTabSelected;
        private MouseOps _graphicMouseOps;

        public int GraphTabSelected
        {
            set => _graphTabSelected = value;
            get => _graphTabSelected;
        }

        public int TabCount => GraphTabs?.Length ?? 0;

        public double XAxisMin => Cst.MainPan.XAxisMin;

        public double XAxisMax => Cst.MainPan.XAxisMax;

        public double XAxisSpan => Cst.MainPan.XAxisSpan;

        public string XAxisSpanLabel => Cst.MainPan.XAxisSpanLabel;

        public XAxisType XAxisType => Cst.MainPan.XAxisType;

        public void Setup(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphicMouseOps = _graphPanel.Gmo;
            _graphBoundary = _graphParameters.GraphBoundary;
        }

        public void Initialise(TabPage tabPage1)
        {
            GraphTabs = new GrTabControl[1];
            GraphTabs[0] = new GrTabControl {Page = tabPage1};

            InitialiseTabData(0);

            GraphTabs[0].TabSurface = tabPage1.CreateGraphics();
            GraphTabs[0].DrawingImage = new Bitmap(tabPage1.Width, tabPage1.Height);
            GraphTabs[0].GrSurface = Graphics.FromImage(GraphTabs[0].DrawingImage);

 
            SetSelectedTab(0);
            _graphPanel.SetSelectedTab(0);
        }

        public GrTabControl Cst => GraphTabs?[_graphTabSelected];

        //// ReSharper disable once InconsistentNaming
        //public GrTabControl CST()
        //{
        //    return GraphTabs == null ? null : GraphTabs[_graphTabSelected];
        //}

        public void RenameCst(string nm)
        {
            GraphTabs[_graphTabSelected].Page.Text = nm;
            GraphTabs[_graphTabSelected].Page.Update();
        }


        public GrTabControl GetTab(int id)
        {
            if (GraphTabs == null)
                return null;
            return id >= GraphTabs.Length ? null : GraphTabs[id];
        }

        public bool SetSelectedTab(int id)
        {
            if (id >= GraphTabs.Length)
                return false;

            _graphTabSelected = id;
            _graphPanel.RemovePanelItem.Visible = Cst.SubPan != null;
            return true;
        }

        private void SetAxisValue(XAxisType xType, double value, int vtype, bool allTabs)
        {
            SetXAxis(Cst.MainPan, Cst.SubPan, xType, value, vtype);

            if (!allTabs)
                return;

            var now = _graphTabSelected;
            for (var i = 0; i < GraphTabs.Length; i++)
            {
                _graphTabSelected = i;
                if(_graphPanel.Gmo.InTimeList2())
                    SetXAxis(Cst.MainPan, Cst.SubPan, xType, value, vtype);
            }
            _graphTabSelected = now;
        }

        private static void SetXAxis(PanelControl pan, IEnumerable<PanelControl> subPan, XAxisType xType, double value, int vtype)
        {
            SetXAxis2(pan, xType, value, vtype);
            if (subPan == null) 
                return;
            foreach (var t in subPan)
                SetXAxis2(t, xType, value, vtype);
        }

        private static void SetXAxis2(PanelControl pan, XAxisType xType, double value, int vtype)
        {
            if (xType != XAxisType.None)
                pan.XAxisType = xType;
            else switch (vtype)
            {
                case 0:
                    pan.XAxisMin = value;
                    break;
                case 1:
                    pan.XAxisMax = value;
                    break;
                case 2:
                    pan.XAxisSpan = value;
                    break;
            }
        }

        public void SetXAxisMin(double value, bool allTabs)
        {
            SetAxisValue(XAxisType.None, value, 0, allTabs);
        }

        public void SetXAxisMax(double value, bool allTabs)
        {
            SetAxisValue(XAxisType.None, value, 1, allTabs);
        }

        public void SetXAxisSpan(double value, bool allTabs)
        {
            SetAxisValue(XAxisType.None, value, 2, allTabs);
        }

        public void SetXAxisType(XAxisType value, bool allTabs)
        {
            SetAxisValue(value, 0, 0, allTabs);
        }

        public int AddNewTab(string title)
        {
            AddATab(title, _graphPanel.TabControl1);
            _graphPanel.SetSize(_graphPanel.Width, _graphPanel.Height);
            _graphPanel.Reset();
            return _graphTabSelected;
        }

        private void AddATab(string title, TabControl tabControl1)
        {
            var tt = new TabPage {Text = title};

            _graphPanel.HideLegend();

            var cnt = GraphTabs.Length;
            Array.Resize(ref GraphTabs, cnt + 1);
            GraphTabs[cnt] = new GrTabControl();

            tabControl1.Controls.Add(tt);
            GraphTabs[cnt].Page = tt;

            InitialiseTabData(cnt);

            GraphTabs[cnt].TabSurface = tt.CreateGraphics();
            GraphTabs[cnt].DrawingImage = new Bitmap(tt.Width, tt.Height);
            GraphTabs[cnt].GrSurface = Graphics.FromImage(GraphTabs[cnt].DrawingImage);

            SetSelectedTab(cnt);
            _graphPanel.SetSelectedTab(cnt);
        }

        public void SetSizes(int width, int height)
        {           
            var now = _graphTabSelected;
            for (var i = 0; i < GraphTabs.Length; i++)
            {
                _graphTabSelected = i;
                SetWidth(width);
                SetHeight(height);
                SetButtons();
            }
            SetPanel();         
            _graphPanel.Reset();
            _graphTabSelected = now;
        }

        private void SetWidth(int wid)
        {

            if (wid < 200)
                wid = 200;
            _graphPanel.TabControl1.Width = wid - 10;
            Cst.MainPan.GPan.Width = _graphPanel.TabControl1.Width - 145;
            Cst.MainPan.GPan.Left = 75;
            _graphPanel.Width = wid;
            if (Cst.SubPan == null) 
                return;
            foreach (var t in Cst.SubPan)
            {
                t.GPan.Width = Cst.MainPan.GPan.Width;
                t.GPan.Left = Cst.MainPan.GPan.Left;
            }
        }

        private void SetHeight(int hei)
        {
            if (hei < 200)
                hei = 200;

            _graphPanel.TabControl1.Height = hei - 45;
            var gHeight = _graphPanel.TabControl1.Height - 70;

            Cst.MainPan.GPan.Height = gHeight;

            if (Cst.SubPan != null)
            {
                var cnt = Cst.SubPan.Length;

                gHeight -= 5*(cnt + 2);
                Cst.MainPan.GPan.Height = (int) (gHeight*Cst.MainPan.PercentHeight/100.0);

                for (var i = 0; i < Cst.SubPan.Length; i++)
                {
                    if (i == 0)
                        Cst.SubPan[i].GPan.Top = Cst.MainPan.GPan.Top + Cst.MainPan.GPan.Height + 5;
                    else
                        Cst.SubPan[i].GPan.Top = Cst.SubPan[i - 1].GPan.Top + Cst.SubPan[i - 1].GPan.Height + 5;
                    Cst.SubPan[i].GPan.Height = (int) (gHeight*Cst.SubPan[i].PercentHeight/100.0);
                }
            }

            _graphPanel.Height = hei;
            SetPageHeights(_graphPanel.TabControl1.Height - 25);
        }

        private void SetButtons()
        {
            if (!_graphParameters.AllReadyToGo)
                return;

            var xPosition = Cst.MainPan.GPan.Right;
            var yPosition = _graphPanel.TabControl1.Bottom - 75;

            Cst.ExpandX.Top = yPosition;
            Cst.ContractX.Top = yPosition;
            Cst.ResetX.Top = yPosition;

            Cst.ContractX.Left = xPosition;
            Cst.ResetX.Left = Cst.ContractX.Right + 3;
            Cst.ExpandX.Left = Cst.ResetX.Right + 3;

            if (Cst.SubPan != null)
            {
                for (var i = 0; i < Cst.SubPan.Length; i++)
                    SetPanelButtons(ref Cst.SubPan[i]);
            }
            SetPanelButtons(ref Cst.MainPan);
        }

        private void SetPanelButtons(ref PanelControl pan)
        {
            var xPosition = Cst.MainPan.GPan.Right + 45;

            pan.ContractYBottom.Left = xPosition;
            pan.ExpandYBottom.Left = xPosition;
            pan.ContractYTop.Left = xPosition;
            pan.ExpandYTop.Left = xPosition;

            pan.ExpandYTop.Top = pan.GPan.Top + 5;
            pan.ContractYTop.Top = pan.ExpandYTop.Top + 22;

            pan.ExpandYBottom.Top = pan.ContractYTop.Top + pan.GPan.Height - 44;
            pan.ContractYBottom.Top = pan.ExpandYBottom.Top - 22;

            var yPosition = pan.ContractYTop.Top + ((pan.ExpandYBottom.Top - 10 - pan.ContractYTop.Top)/2);
            pan.Reset.Left = xPosition;
            pan.Reset.Top = yPosition;
        }

        private void DisposeOfPanels()
        {
            for (var i = 0; i < GraphTabs.Length; i++)
                DisposeOfPanel(i);
        }

        public void InitialisePanel()
        {
            _canSetPanel = true;
            SetPanel();
        }

        private void SetPanel()
        {
            if (!_canSetPanel)
                return;

            DisposeOfPanels();

            foreach (var t in GraphTabs)
            {
                if (t.Page != null)
                {
                    if (t.TabSurface != null)
                    {
                        t.TabSurface.Dispose();
                        t.TabSurface = null;
                    }
                    if (t.DrawingImage != null)
                    {
                        t.DrawingImage.Dispose();
                        t.DrawingImage = null;
                    }
                    if (t.GrSurface != null)
                    {
                        t.GrSurface.Dispose();
                        t.GrSurface = null;
                    }
                    t.TabSurface = t.Page.CreateGraphics();
                    t.DrawingImage = new Bitmap(t.Page.Width, t.Page.Height);
                    t.GrSurface = Graphics.FromImage(t.DrawingImage);
                }
                SetupSinglePanel(t.MainPan);
                if (t.SubPan != null)
                {
                    foreach (var t1 in t.SubPan)
                        SetupSinglePanel(t1);
                }
            }
        }

        private static void SetupSinglePanel(PanelControl pan)
        {
            pan.GPan.Cursor = Cursors.Cross;
            pan.GPan.AllowDrop = true;
            var wid = pan.GPan.Width;
            var hei = pan.GPan.Height;

            pan.PanSurface = pan.GPan.CreateGraphics();
            pan.DrawingImage = new Bitmap(wid, hei);
            pan.GrSurface = Graphics.FromImage(pan.DrawingImage);

            pan.XOffset = 0;
            pan.YOffset = 0;

            pan.HCursorImage = new Bitmap(wid, 1);
            pan.HCursorSurface = Graphics.FromImage(pan.HCursorImage);
            pan.HCursorSurface.DrawLine(Pens.SlateGray, 0, 0, hei, 0);

            pan.VCursorImage = new Bitmap(1, hei);
            pan.VCursorSurface = Graphics.FromImage(pan.VCursorImage);
            pan.VCursorSurface.DrawLine(Pens.SlateGray, 0, 0, 0, hei);

            pan.RectHImage = new Bitmap(wid, 1);
            pan.RectHSurface = Graphics.FromImage(pan.RectHImage);

            pan.RectVImage = new Bitmap(1, hei);
            pan.RectVSurface = Graphics.FromImage(pan.RectVImage);

            pan.CrossImage = new Bitmap(6, 6);
            pan.CrossSurface = Graphics.FromImage(pan.CrossImage);
            pan.CrossSurface.FillRectangle(Brushes.Blue, 0, 0, 6, 6);
        }

        private void InitialiseTabData(int id)
        {
            
            GraphTabs[id].SubPan = null;
            GraphTabs[id].MainPan = new PanelControl(_graphPanel, 0);
            GraphTabs[id].Page.Controls.Add(GraphTabs[id].MainPan.GPan);

            SetPanelActions(GraphTabs[id].MainPan.GPan);

            GraphTabs[id].Page.MouseMove += _graphicMouseOps.TabMouseMove;
            GraphTabs[id].Page.MouseUp += _graphicMouseOps.TabMouseUp;
            GraphTabs[id].Page.MouseDown += _graphicMouseOps.TabMouseDown;
         

            _graphTabSelected = id;


            _graphicMouseOps.AddYExpandContractButtons(GraphTabs[id].MainPan, 0);
            _graphicMouseOps.AddXExpandContractButtons(GraphTabs[id]);

            GraphTabs[id].MainPan.BoundaryConMenu.MenuItems.Add("Add New Zone Start/Stop Boundary Pair",
                _graphBoundary.AddWaferBoundaryClick);
            GraphTabs[id].MainPan.BoundaryConMenu.MenuItems.Add("Remove This Boundary",
                _graphBoundary.RemoveBoundaryClick);
            GraphTabs[id].MainPan.BoundaryConMenu.MenuItems.Add("Set The Exact Start/Stop Angle",
                _graphBoundary.SetStartStopAngle);
            GraphTabs[id].MainPan.BoundaryConMenu.MenuItems.Add("Zoom Detail This Boundary Pair ",
                _graphBoundary.WaferBoundaryZoom);
            if (_graphParameters.AllowRemoveMarker)
                GraphTabs[id].MainPan.MarkerConMenu.MenuItems.Add("Remove This Marker", _graphBoundary.RemoveMarkerClick);

            if (_graphParameters.AllowTextMarker)
                GraphTabs[id].MainPan.MarkerConMenu.MenuItems.Add("Add Text To This Marker",
                    _graphBoundary.TextMarkerClick);

            var mxt = new MenuTag { ItemTag = GraphOption.AsYAxis, PanelTag = 0 };
            var mx = new MenuItem("Y Axis Control") {Tag = mxt};
            mx.Click += AxisMenuContextClick;
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx);

            var mx1T = new MenuTag { ItemTag = GraphOption.Legend, PanelTag = 0 };
            var mx1 = new MenuItem("Show Graph Legend") {Tag = mx1T};
            mx1.Click += LegendMenuContextClick;
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx1);

            var mx8T = new MenuTag { ItemTag = GraphOption.None, PanelTag = 0 };
            var mx8 = new MenuItem("-") { Tag = mx8T };
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx8);

            var mx2T = new MenuTag { ItemTag = GraphOption.ShowAllGraphs, PanelTag = 0 };
            var mx2 = new MenuItem("Show All Graphs") { Tag = mx2T };
            mx2.Click += ShowAllMenuContextClick;
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx2);

            var mx3T = new MenuTag { ItemTag = GraphOption.HideAllGraphs, PanelTag = 0 };
            var mx3 = new MenuItem("Hide All Graphs") { Tag = mx3T };
            mx3.Click += HideAllMenuContextClick;
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx3);

            var mx9T = new MenuTag { ItemTag = GraphOption.None, PanelTag = 0 };
            var mx9 = new MenuItem("-") { Tag = mx9T };
            GraphTabs[id].MainPan.ConMenu.MenuItems.Add(mx9);

            GraphTabs[id].MainPan.GPan.ContextMenu = GraphTabs[id].MainPan.ConMenu;
        }

        private void SetPageHeights(int hei)
        {
            foreach (var t in GraphTabs.Where(t => t.Page != null))
            {
                t.Page.Height = hei;
            }
        }

        private void DisposeOfPanel(int i)
        {
           
            if (i < 0 || i >= GraphTabs.Length)
                return;

            DisposePanelItems(ref GraphTabs[i].MainPan);

            if (GraphTabs[i].SubPan == null)
                return;

            for (var j = 0; j < GraphTabs[i].SubPan.Length; j++)
                DisposePanelItems(ref GraphTabs[i].SubPan[j]);
        }

        private static void DisposePanelItems(ref PanelControl pan)
        {
            if (pan.DrawingImage != null)
            {
                pan.DrawingImage.Dispose();
                pan.DrawingImage = null;
            }
            if (pan.PanSurface != null)
            {
                pan.PanSurface.Dispose();
                pan.PanSurface = null;
            }
            if (pan.GrSurface != null)
            {
                pan.GrSurface.Dispose();
                pan.GrSurface = null;
            }
            if (pan.HCursorImage != null)
            {
                pan.HCursorImage.Dispose();
                pan.HCursorImage = null;
            }
            if (pan.HCursorSurface != null)
            {
                pan.HCursorSurface.Dispose();
                pan.HCursorSurface = null;
            }
            if (pan.VCursorImage != null)
            {
                pan.VCursorImage.Dispose();
                pan.VCursorImage = null;
            }
            if (pan.VCursorSurface != null)
            {
                pan.VCursorSurface.Dispose();
                pan.VCursorSurface = null;
            }
            if (pan.RectHImage != null)
            {
                pan.RectHImage.Dispose();
                pan.RectHImage = null;
            }
            if (pan.RectHSurface != null)
            {
                pan.RectHSurface.Dispose();
                pan.RectHSurface = null;
            }
            if (pan.RectVImage != null)
            {
                pan.RectVImage.Dispose();
                pan.RectVImage = null;
            }
            if (pan.RectVSurface != null)
            {
                pan.RectVSurface.Dispose();
                pan.RectVSurface = null;
            }
            if (pan.CrossImage != null)
            {
                pan.CrossImage.Dispose();
                pan.CrossImage = null;
            }
            if (pan.CrossSurface != null)
            {
                pan.CrossSurface.Dispose();
                pan.CrossSurface = null;
            }
        }

        public PanelControl GetPanelControlFromTag(int tagId)
        {
            if (tagId == 0)
                return Cst.MainPan;

            if (Cst.SubPan == null)
                return Cst.MainPan;
            tagId -= 1;
            if (tagId >= 0 && tagId < Cst.SubPan.Length)
                return Cst.SubPan[tagId];
            return Cst.MainPan;
        }

        private PanelControl GetPanFromSender(Object sender)
        {
            var it = (MenuItem)sender;
            PanelControl pan = null;
            var menuTag = (MenuTag)it.Tag;
            if (menuTag.PanelTag == 0)
            {
                pan = GraphTabs[_graphTabSelected].MainPan;
            }
            else
            {
                var id = menuTag.PanelTag - 1;
                if (id >= 0 && GraphTabs[_graphTabSelected].SubPan != null)
                {
                    if (id >= GraphTabs[_graphTabSelected].SubPan.Length)
                        return null;
                    pan = GraphTabs[_graphTabSelected].SubPan[id];
                }
            }
            return pan;
        }
        public void HideAllMenuContextClick(Object sender, EventArgs e)
        {
            PanelControl pan = GetPanFromSender(sender);
            if (pan == null)
                return;
            ShowHide(false,pan);
            if (pan.Legend.Visible)
                _graphPanel.UpdateLegend(pan);
           
        }

        public void ShowAllMenuContextClick(Object sender, EventArgs e)
        {

            PanelControl pan = GetPanFromSender(sender);
            if (pan == null)
                return;
            ShowHide(true,pan);
            if(pan.Legend.Visible)
                _graphPanel.UpdateLegend(pan);
        }

        public void ShowHide(bool show)
        {
            ShowHide(show, GraphTabs[_graphTabSelected].MainPan);
        }
        public void ShowHide(bool show, PanelControl pan)
        {
            if (pan.TagList == null)
                return;
            for (var i = 0; i < pan.ConMenu.MenuItems.Count; ++i)
            {
                Menu m = pan.ConMenu.MenuItems[i];
                if (m.MenuItems.Count <= 0)
                    continue;
                m.MenuItems[0].Checked = show;
               
                //for (var j = 0; j < m.MenuItems.Count; ++j)
                //{

                //    m.MenuItems[j].Checked = show;
                //}
            }
            foreach (var t in pan.TagList)
            {
                if (t.Tag >= 1000)
                    continue; 
                //if(t.CanBeVisible)
                    t.Visible = show;                
            }
            _graphPanel.RedrawGraphics();

        }

        public void AxisMenuContextClick(object sender, EventArgs e)
        {
            var it = (MenuItem)sender;
            var menuTag = (MenuTag)it.Tag;
            _graphPanel.DoYAxisMenu(menuTag.PanelTag);
        }

        public void LegendMenuContextClick(object sender, EventArgs e)
        {
            var it = (MenuItem) sender;
            PanelControl pan = null;
            var menuTag = (MenuTag) it.Tag;
            if (menuTag.PanelTag == 0)
            {
                pan = GraphTabs[_graphTabSelected].MainPan;
                pan.Legend.SetTitle(pan.LegendTitle.Length <= 0 ? "Main Panel Legend" : pan.LegendTitle);
            }
            else
            {
                var id = menuTag.PanelTag - 1;
                if (id >= 0 && GraphTabs[_graphTabSelected].SubPan != null)
                {
                    if (id >= GraphTabs[_graphTabSelected].SubPan.Length)
                        return;
                    pan = GraphTabs[_graphTabSelected].SubPan[id];
                    pan.Legend.SetTitle(pan.LegendTitle.Length <= 0 ? "Sub Panel " + (id + 1) + " Legend" : pan.LegendTitle);
                                    
                }
            }

            if (it.Checked)
            {
                it.Checked = false;
                if (pan != null)
                {
                    pan.Legend.Hide();
                    pan.Legend.EmptyLegend();
                }
            }
            else
            {
                it.Checked = true;
                if (pan != null)
                {
                    var x = pan.GPan.Left + _legendDx - 200;
                    var y = pan.GPan.Top + _graphPanel.Top + _legendDy + 100;

                    if (x < 20)
                        x = 20;
                    pan.Legend.Location = new Point(x, y);
                   
                    try
                    {
                        pan.Legend.Show(_graphPanel);
                    }
                    catch
                    {
                        pan.Legend.Show();
                    }
                    pan.Legend.UpdateLegend(pan);

                }
            }
            _graphPanel.RedrawGraphics();
        }

        private int _legendDx;
        private int _legendDy;

        public void SetLegendOffset(int x, int y)
        {
            _legendDx = x;
            _legendDy = y;
        }
        public void RemoveLastGraphicsPanel(int width, int height)
        {
            if (Cst.SubPan == null)
                return;

            var cnt = Cst.SubPan.Length - 1;
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].GPan);
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].ExpandYTop);
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].ContractYTop);
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].ExpandYBottom);
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].ContractYBottom);
            Cst.Page.Controls.Remove(Cst.SubPan[cnt].Reset);

            Cst.SubPan[cnt].GPan.Dispose();
            Cst.SubPan[cnt].ExpandYTop.Dispose();
            Cst.SubPan[cnt].ContractYTop.Dispose();
            Cst.SubPan[cnt].ExpandYBottom.Dispose();
            Cst.SubPan[cnt].ContractYBottom.Dispose();
            Cst.SubPan[cnt].Reset.Dispose();

            cnt -= 1;
            Array.Resize(ref Cst.SubPan, cnt + 1);
            if (cnt < 0)
            {
                Cst.SubPan = null;
                Cst.MainPan.PercentHeight = 100.0;
                _graphPanel.RemovePanelItem.Visible = false;
            }
            else
            {
                var percentHeight = 100.0/(cnt + 2);
                Cst.MainPan.PercentHeight = percentHeight;
                for (var i = 0; i <= cnt; i++)
                    Cst.SubPan[i].PercentHeight = percentHeight;               
            }

            _graphPanel.SetSize(width, height);
            _graphPanel.Reset();
        }

        public void AddPanel(int[] list)
        {
            _graphParameters.PauseLiveData(true);
            if(list == null)
                AddPanelFromMain();
            else
                AddNewGraphicsPanel(_graphPanel.Width, _graphPanel.Height, list);
            _graphParameters.PauseLiveData(false);
        }

        public void AddPanel(int[] startId, int cnt)
        {
            var list = new int[cnt];
            for (var i = 0; i < cnt; i++)
                list[i] = startId[i];
            AddPanel(list);
        }

        private void AddPanelFromMain()
        {
            if (Cst.MainPan?.TagList == null)
                return;
            if (Cst.MainPan.TagList.Length <= 0)
                return;
            var list = new int[Cst.MainPan.TagList.Length];
            for (var i = 0; i < Cst.MainPan.TagList.Length; i++)
                list[i] = Cst.MainPan.TagList[i].Tag;
            AddPanel(list);
        }

        public void ResetPanelZoomPan()
        {
            ResetSinglePan(Cst.MainPan);
            if(Cst.SubPan == null)
                return;
            foreach (var t in Cst.SubPan)
                ResetSinglePan(t);
            
        }

        private static void ResetSinglePan(PanelControl pan)
        {
            pan.Zooming = false;
            pan.YScale = 1.0;
            pan.XScale = 1.0;
            pan.XOffset = 0;
            pan.YOffset = 0;
            pan.XPan = 0;
            pan.YPan = 0;
            pan.XPanStart = 0;
            pan.YPanStart = 0;
            pan.YAxisType = YAxisType.Auto;            
            pan.NeedToSetScale = true;
        }

        private void AddNewGraphicsPanel(int width, int height, int[] newPanelStartId)
        {
            var cnt = 0;
            if (Cst.SubPan != null)
                cnt = Cst.SubPan.Length;

            Array.Resize(ref Cst.SubPan, cnt + 1);

            _graphPanel.RemovePanelItem.Visible = true;

            Cst.SubPan[cnt] = new PanelControl(_graphPanel, cnt + 1);
            Cst.Page.Controls.Add(Cst.SubPan[cnt].GPan);

            var percentHeight = 100.0/(cnt + 2);
            Cst.MainPan.PercentHeight = percentHeight;
            for (var i = 0; i <= cnt; i++)
                Cst.SubPan[i].PercentHeight = percentHeight;

            Cst.SubPan[cnt].GPan.ContextMenu = Cst.SubPan[cnt].ConMenu;
           
            _graphParameters.ContextMenu.CopyMenu(Cst.SubPan[cnt].ConMenu, Cst.MainPan.ConMenu, cnt);
                              
            if (newPanelStartId != null)
            {
                foreach (var value in Cst.MainPan.TagList)
                {
                    _graphParameters.ContextMenu.SetAsubtag(Cst.SubPan[cnt], value);
                    _graphParameters.ContextMenu.ResetAsubtag(Cst.SubPan[cnt], value.Tag, false);
                }
                for (var i = 0; i < newPanelStartId.Length; i++)
                {
                    var visible = true;
                    var sid = newPanelStartId[i];
                    if (sid < 0)
                    {
                        sid *= -1;
                        visible = false;
                    }

                    if (sid >= Cst.MainPan.TagList.Length) 
                        continue;
                    var tag = Cst.MainPan.TagList[sid].Tag;
                    if (i == 0)
                        tag = tag*-1; // set the first as the master

                    _graphParameters.ContextMenu.ResetAsubtag(Cst.SubPan[cnt], tag, visible);
                }

                foreach (var t in Cst.MainPan.TagList)
                {
                    var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);

                    if (gs?.GType != GraphType.Boundary)
                        continue;
                    _graphParameters.ContextMenu.ResetAsubtag(Cst.SubPan[cnt], t.Tag, true);
                }

               
            }

            SetPanelActions(Cst.SubPan[cnt].GPan);

            _graphPanel.Gmo.AddYExpandContractButtons(Cst.SubPan[cnt], cnt + 1);
            _graphPanel.SetSize(width, height);

            if (Cst.Page != null)
                Cst.TabSurface = Cst.Page.CreateGraphics();
        }

      
        private void SetPanelActions(Panel pan)
        {
            if (pan == null)
                return;
          
            pan.MouseMove += _graphicMouseOps.GrPan_MouseMove;
            pan.MouseDown += _graphicMouseOps.GrPan_MouseDown;
            pan.MouseUp += _graphicMouseOps.GrPan_MouseUp;
            pan.MouseEnter += _graphicMouseOps.GrPan_MouseEnter;
            pan.MouseLeave += _graphicMouseOps.GrPan_MouseLeave;
            pan.MouseDoubleClick += _graphicMouseOps.GrPan_MouseDoubleClick;
            pan.Paint += _graphicMouseOps.GrPan_Paint;
            pan.AllowDrop = true;
            pan.DragEnter += Pan_DragEnter;
            pan.DragDrop += Pan_DragDrop;
        }

      
        private string _dropFilename;
        private void Pan_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (!_graphPanel.AllowFileDrop)
                    return;
                _graphPanel.SetLoadFile(_dropFilename);
            }
            catch
            {             
            }                       
        }

        private void Pan_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (!_graphPanel.AllowFileDrop)
                    return;
                if (GetFilename(out _dropFilename, e))
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.None;
                
            }
            catch 
            {
                e.Effect = DragDropEffects.None;
            }
            
        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
          
            filename = string.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) != DragDropEffects.Copy)
                return false;

            if (!(e.Data.GetData("FileName") is Array data))
                return false;
            if ((data.Length == 1) && (data.GetValue(0) is string))
            {
                filename = ((string[])data)[0];
                // ReSharper disable once PossibleNullReferenceException
                var ext = Path.GetExtension(filename).ToLower();
                if(_graphPanel.ValidFileExtension(ext))
                    return true;

            }
            return false;
        }

    }
}