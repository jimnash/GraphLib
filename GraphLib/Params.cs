using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
// ReSharper disable UnusedMember.Global

namespace GraphLib
{
    public class Params
    {
        private readonly GraphPanel _graphPanel;


        private GpContextMenu _contextMenu;

        public MethodInvoker BoundaryChangeCallback;
        public MethodInvoker DoubleClickCallback;
        public MethodInvoker LegendChangeCallback;
        public LiveData[] LiveGraphs;
        public MethodInvoker MarkerChangeCallback;
        public MethodInvoker MarkerTextCallback;
        public MethodInvoker MasterClickCallback;
        public MethodInvoker MoveCallback;
     

        public MethodInvoker TabChangedCallback;
        public MethodInvoker YAxisCallback;

        public Params(GraphPanel g)
        {
            LastDoubleClickTime = 0.0;
         
            XGridDrawn = false;
            CheckGraphTooltip = false;
            MouseY = 0;
            MouseX = 0;
            LastXPosition = 0.0;
            CursorPan = null;
            ClearListCount = 0;
            ResetPointCount = false;
            AllReadyToGo = false;
            DisplayActiveValues = false;
            HaltUpdateFromSplit = false;
            Panning = false;
            DisplayCursor = false;
            BoxCursor = false;
            BoxBotY = 0;
            BoxBotX = 0;
            BoxTopY = 0;
            BoxTopX = 0;
            CrossY = 0;
            CrossX = 0;
            CursorY = 0;
            CursorX = 0;            
            DrawBoundaryStrings = false;
            ShowDifferenceMarkers = false;
            MarkerX = 0.0;
            MoveFromRight = false;
            MoveFromLeft = false;
            _graphPanel = g;
        }

        public double LastDoubleClickTime { get; set; }


// ReSharper disable once ConvertToAutoProperty
        public GpContextMenu ContextMenu
        {
            get => _contextMenu;
            set => _contextMenu = value;
        }

        public GrTabPanel GraphTabPanel { get; set; }

        public GrBoundary GraphBoundary { get; set; }

        public bool MoveFromLeft { get; set; }

        public bool MoveFromRight { get; set; }

        public double MarkerX { get; set; }

        public bool CanSetVisible { get; set; } = true;

        public bool ShowDifferenceMarkers { get; set; }

        public int BoundaryMoveId { get; set; } = -1;

        public int LastBoundaryMoveId { get; set; } = -1;


        public int BoundaryIndexId { get; set; } = -1;

        public bool DrawBoundaryStrings { get; set; }

        public int LastMarkerMoveId { get; set; } = -1;

        public bool AllowTextMarker { get; set; } = true;

        public bool AllowRemoveMarker { get; set; } = true;

        public bool OriginalZeroAxis { get; set; } = true;

        public int CursorX { get; set; }

        public int CursorY { get; set; }

        public int CrossX { get; set; }

        public int CrossY { get; set; }

        public int BoxTopX { get; set; }

        public int BoxTopY { get; set; }

        public int BoxBotX { get; set; }

        public int BoxBotY { get; set; }

        public bool BoxCursor { get; set; }

        public bool DisplayCursor { get; set; }

        public bool CursorActive { get; set; } = true;

        public bool DisplayGraphMover { get; set; } = true;

        public bool GraphicsSet { get; set; }

        public bool Panning { get; set; }

        public bool HaltUpdateFromSplit { get; set; }

        public bool DrawTheGrid { get; set; } = true;

        public bool DrawTheXAxis { get; set; } = true;

        public bool DrawTheYAxis { get; set; } = true;

        public bool DrawTheXAxisLegend { get; set; } = true;

        public bool DrawTheYAxisLegend { get; set; } = true;

        public double LastMarkerValue { get; set; }

        public double LastMarkerTime { get; set; }

        public bool DisplayActiveValues { get; set; }

        public bool AllReadyToGo { get; set; }

        public bool ResetPointCount { get; set; }

        public PanelControl LastPanel { get; set; }

        public int HighlightWidth { get; set; } = 2;

        public int ClearListCount { get; set; }

        public bool InitiallyVisible { get; set; } = true;

        public bool MissFromContextMenu { get; set; }

        public PanelControl CursorPan { get; set; }

        public double LastXScale { get; set; } = 1.0;

        public double LastYScale { get; set; } = 1.0;

        public double LastXPosition { get; set; }

        public int MouseX { get; set; }

        public int MouseY { get; set; }

        public bool CheckGraphTooltip { get; set; }

        public bool DarkBackground { get; set; }

        public bool XGridDrawn { get; set; }

        public double YScale { get; set; } = 1.0;

        public bool HaltLiveUpdate { get; set; }

        public bool OverrideXAxisTitle { get; set; }

        public string OverrideXAxisString { get; set; } = "";

        public double TimeAxisStep { get; set; } = 1.0;

        public string XAxisFormat { get; set; } = "0.00";

        internal static string GetValueString(double checkValue, GraphSurface gs, double value)
        {
            string str;

            if (checkValue > 1000)
            {
                str = ((int) value).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                var str1 = SetPlaces(checkValue);
                str = value.ToString(str1);
            }
            
            return str;
        }

        internal static string SetPlaces(double val)
        {
            var checkValue = Math.Abs(val);
            var cnt = 0;
            var str = "0.0";
            if (checkValue < 1E-06)
                return str;

            while (checkValue < 90)
            {
                checkValue *= 10;
                cnt += 1;
            }
            for (var i = 0; i < cnt; i++)
                str = str + "0";

            return str;
        }

        public void GetDataValueFromPosition(int x, ref int tim, ref string valueString)
        {
            if (LastPanel == null)
                GetDataValueFromPosition(GraphTabPanel.Cst.MainPan, x, ref tim, ref valueString);
            else
                GetDataValueFromPosition(LastPanel, x, ref tim, ref valueString);
        }

        private void GetDataValueFromPosition(PanelControl pan, int x, ref int tim, ref string valueString)
        {
            double value = 0;
            var gs = _graphPanel.GetMasterGraph(pan);
            if (gs == null)
                return;

            GetDataValueFromPosition(pan, x, ref tim, ref value);

            valueString = value > 1000
                ? ((int) value).ToString(CultureInfo.InvariantCulture)
                : value.ToString(Math.Abs(value) < 1.0 ? "0.00" : "0.0");
        }

        internal void GetDataValueFromPosition(PanelControl panel, int x, ref int tim, ref double value)
        {
            PanelControl pan;
            if (panel == null)
                pan = LastPanel ?? GraphTabPanel.Cst.MainPan;
            else
                pan = panel;


            var tid = GraphPanel.GetMasterTag(pan);
            if (tid < 0)
                return;

            try
            {
                if (!GraphicsSet)
                    return;

                tim = -1;
                var xValue = (x + GraphTabPanel.Cst.MainPan.XOffset) / GraphTabPanel.Cst.MainPan.XScale;
                for (var i = 0; i < pan.TagList[tid].ScrPCount; i++)
                {
                    if (!(pan.TagList[tid].DisplayPoints[i].X >= xValue))
                        continue;
                    tim = i - 1;
                    break;
                }

                if (tim == -1)
                {
                    tim = 0;
                    if (DisplayGraphMover)
                        _graphPanel.DrawCross(pan, x, 0.0);

                    return;
                }

                value = pan.TagList[tid].DisplayPoints[tim].Y;
                if (tim != pan.TagList[tid].ScrPCount - 1)
                {
                    var dx = xValue - pan.TagList[tid].DisplayPoints[tim].X;
                    if (dx > 0)
                    {
                        double dx2 = pan.TagList[tid].DisplayPoints[tim + 1].X - pan.TagList[tid].DisplayPoints[tim].X;
                        double dy = pan.TagList[tid].DisplayPoints[tim + 1].Y - pan.TagList[tid].DisplayPoints[tim].Y;
                        value = value + dx * dy / dx2;
                    }
                }

                if (DisplayGraphMover)
                    _graphPanel.DrawCross(pan, x, value);

                value = value / pan.TagList[tid].DisplayYScale;
                LastMarkerValue = value;
                LastMarkerTime = xValue;
            }
            catch
            {
                tim = 0;
            }
        }

        internal void SetMoverBox(PanelControl pan, int x)
        {
            int tim = 0;
            double value = 0;
            GetDataValueFromPosition(pan, x, ref tim, ref value);           
        }

        public double GetDataValueFromTime(int tagId, double tim)
        {
            var tid = -1;
            var pan = GraphTabPanel.Cst.MainPan;

            for (var i = 0; i < pan.TagList.Length; i++)
            {
                if (pan.TagList[i].Tag != tagId)
                    continue;
                tid = i;
                break;
            }

            if (tid == -1)
                return 0.0;

            try
            {
                var t1 = -1;
                var t2 = -1;
                for (var i = 0; i < pan.TagList[tid].ScrPCount; i++)
                {
                    if (tim <= pan.TagList[tid].DisplayPoints[i].X)
                    {
                        t2 = i;
                        break;
                    }
                    t1 = i;
                }
                if (t1 == -1 || t2 == -1)
                    return 0.0;

                double value = pan.TagList[tid].DisplayPoints[t1].Y;
                var dx = tim - pan.TagList[tid].DisplayPoints[t1].X;
                if (!(dx > 0))
                    return value;
                double dx2 = pan.TagList[tid].DisplayPoints[t2].X - pan.TagList[tid].DisplayPoints[t1].X;
                if (!(Math.Abs(dx2) > 0.0001))
                    return value;
                double dy = pan.TagList[tid].DisplayPoints[t2].Y - pan.TagList[tid].DisplayPoints[t1].Y;
                value = value + dx * dy / dx2;

                return value;
            }
            catch
            {
                return 0.0;
            }
        }

        internal static int GetXScreenPoint(PanelControl pan, GraphSurface gs, double xp)
        {
            var xScale = pan.XScale;

            if (pan.NeedToSetScale)
                xScale = gs.XScale;

            return (int) (xp * xScale - pan.XOffset);
        }

        internal static int GetYScreenPoint(PanelControl pan,  double yp)
        {
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
                var tempP = yp - min;
                tempP = tempP / ((max - min) / (hei));
                tempP *= pan.YScale;
                yp = hei - tempP - pan.YOffset;


                return (int)yp;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                return 0;
            }
        }

        internal double GetYFromScr(PanelControl pan,int pt)
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

            double val = hei - pt - pan.YOffset;
            double yp = val * (max - min) / (hei * pan.YScale);
            yp += min;
           
            return yp;
        }

        internal void SetGraphHighlightPan(PanelControl pan, int tag, bool setIt)
        {
            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList.Where(t => t.Tag == tag))
            {
                t.Highlight = setIt;              
                GpContextMenu.SetHighlight(pan.ConMenu, t.Tag, setIt);
            }
        }

        internal void SetGraphSolidPan(PanelControl pan, int tag, bool setIt)
        {
            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList.Where(t => t.Tag == tag))
            {
                t.AsPoint = setIt;
               
                GpContextMenu.SetSolid(pan.ConMenu, t.Tag, setIt);
            }
        }

        internal static void SetGraphMasterPan(PanelControl pan, int tag, bool setIt)
        {
            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList)
                t.Master = false;
            if (setIt)
                foreach (var t in pan.TagList.Where(t => t.Tag == tag))
                {
                    t.Master = true;
                    return;
                }
            else
                pan.TagList[0].Master = true;
        }

        internal void MasterCallBack()
        {
            MasterClickCallback?.Invoke();
        }

        internal static bool IsNumeric(string s)
        {
            var ct = s.ToCharArray();
            foreach (var t in ct)
            {
                if (t == '.' || t == '+' || t == '-')
                    continue;
                if (!char.IsNumber(t))
                    return false;
            }
            return true;
        }

        // ReSharper disable once IdentifierTypo
        internal void SetArgbGraphColour(int tag, int rgb)
        {
            SetGraphColour(tag, Color.FromArgb(rgb));
        }

        private static Color GetDarker(Color c)
        {
            int r = c.R;
            r = (int) (r * 0.65);

            int g = c.G;
            g = (int) (g * 0.65);

            int b = c.B;
            b = (int) (b * 0.65);

            return Color.FromArgb(r, g, b);
        }

        public void ForceGraphColour(GraphColour col, int tag, bool darker)
        {
            switch (col)
            {
                case GraphColour.Red:
                    ForceColour(tag, darker, Color.Red);
                    break;
                case GraphColour.Blue:
                    ForceColour(tag, darker, Color.Blue);
                    break;
                case GraphColour.Green:
                    ForceColour(tag, darker, Color.Green);
                    break;
                case GraphColour.Purple:
                    ForceColour(tag, darker, Color.Purple);
                    break;
                case GraphColour.Brown:
                    ForceColour(tag, darker, Color.Brown);
                    break;
                case GraphColour.Gray:
                    ForceColour(tag, darker, Color.Gray);
                    break;
                case GraphColour.Black:
                    ForceColour(tag, false, Color.Black);
                    break;
                case GraphColour.Yellow:
                    ForceColour(tag, false, Color.Yellow);
                    break;
                case GraphColour.GoldenRod:
                    ForceColour(tag, false, Color.Goldenrod);
                    break;
            }
        }

        private void ForceColour(int tag, bool darker, Color col)
        {
            SetGraphColour(tag, darker ? GetDarker(col) : col);
        }

        private void SetGraphColour(int tag, Color c)
        {
            var p = new Pen(c);

            if (GraphTabPanel.Cst.MainPan.TagList == null)
                return;

            foreach (var t in GraphTabPanel.Cst.MainPan.TagList.Where(t => t.Tag == tag))
            {
                t.Colour = p;
                return;
            }
            if (GraphTabPanel.Cst.SubPan == null)
                return;

            foreach (var t in GraphTabPanel.Cst.SubPan.Where(t => t.TagList != null))
            foreach (var t1 in t.TagList.Where(t1 => t1.Tag == tag))
            {
                t1.Colour = p;
                return;
            }
        }

        public void OverrideXAxisStep(double step, string format)
        {
            TimeAxisStep = step;
            XAxisFormat = format;
        }

        public void SetOverrideXAxisString(string str)
        {
            if (str == null)
            {
                OverrideXAxisTitle = false;
            }
            else
            {
                OverrideXAxisTitle = true;
                OverrideXAxisString = str;
            }
        }

        public void DontAddToContextMenu(bool setIt)
        {
            MissFromContextMenu = setIt;
        }

        internal static Pen GetPenColour(GraphColour tag)
        {
            switch (tag)
            {
                case GraphColour.Black:
                    return Pens.Black;
                case GraphColour.Blue:
                    return Pens.Blue;
                case GraphColour.Brown:
                    return Pens.Brown;
                case GraphColour.Green:
                    return Pens.Green;
                case GraphColour.LightGreen:
                    return Pens.LightGreen;
                case GraphColour.Purple:
                    return Pens.Purple;
                case GraphColour.Red:
                    return Pens.Red;
                case GraphColour.White:
                    return Pens.White;
                case GraphColour.Yellow:
                    return Pens.Yellow;
                case GraphColour.GoldenRod:
                    return Pens.Goldenrod;
            }

            return Pens.Black;
        }

        public void SetInitVisible(bool setIt)
        {
            if (CanSetVisible)
                InitiallyVisible = setIt;
        }

        public void SetInitVisible2(bool setIt, bool can)
        {
            CanSetVisible = can;
            InitiallyVisible = setIt;
        }

        internal void PauseLiveData(bool setIt)
        {
            HaltLiveUpdate = setIt;
        }

        internal void ReverseGraphHighlight(PanelControl pan, int tag)
        {
            if (pan.TagList == null)
                return;

            foreach (var t in pan.TagList.Where(t => t.Tag == tag))
            {
                t.Highlight = !t.Highlight;
                GpContextMenu.SetHighlight(pan.ConMenu, t.Tag, t.Highlight);
            }
        }
     
        public void AddYAxisCallback(MethodInvoker cbk)
        {
            YAxisCallback = cbk;
        }

        public void AddMarkerTextCallback(MethodInvoker cbk)
        {
            MarkerTextCallback = cbk;
        }

        public void AddBoundaryChangeFunction(MethodInvoker cbk)
        {
            BoundaryChangeCallback = cbk;
        }

        public void AddMarkerChangeFunction(MethodInvoker cbk)
        {
            MarkerChangeCallback = cbk;
        }

        public void AddLegendChangeFunction(MethodInvoker cbk)
        {
            LegendChangeCallback = cbk;
        }

        public void AddMoveFunction(MethodInvoker cbk)
        {
            MoveCallback = cbk;
        }

        public void AddDoubleClickFunction(MethodInvoker cbk)
        {
            DoubleClickCallback = cbk;
        }

        public void AddTabChangedFunction(MethodInvoker cbk)
        {
            TabChangedCallback = cbk;
        }

        public void AddMasterClickFunction(MethodInvoker cbk)
        {
            MasterClickCallback = cbk;
        }

    }
}