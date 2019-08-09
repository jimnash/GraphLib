using System.Drawing;
using System.Globalization;

namespace GraphLib
{
    public class Marker
    {
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParameters;

        public bool Hide { get; set; }

        public Marker(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
        }

        internal void DrawMarkers(PanelControl pan)
        {
            if (Hide)
                return;
            if (pan.TagList == null)
                return;

            var yOffset = 0;
            var pp = new Pen(Color.Black);
            int y = 2;
            foreach (var t in pan.TagList)
            {
                if (!t.Visible)
                    continue;
                var gs = _graphPanel.GetGraphSurfaceFromTag(t.Tag);
                if (gs == null)
                    continue;
                if (!IsAnyMarker(gs.GType))
                    continue;

                pp.Color = t.Colour.Color;
                pp.Width = t.Highlight ? 2 : 1;
                
                DrawBoundaryMarkers(gs, pan, pp,y, ref yOffset);
                y = y == 2 ? 16 : 2;
            }
        }

        private void DrawBoundaryMarkers(GraphSurface gs, PanelControl pan, Pen pp,int y, ref int yOffset)
        {
            var f = new Font("timesnewroman", 8);
           
            for (var j = 0; j < gs.PtCount; j++)
            {
                var x = Params.GetXScreenPoint(pan, gs, gs.DPts[j].X);
                if (x > 0)
                    pan.GrSurface.DrawLine(pp, x, 0, x, pan.GPan.Height);
                if (gs.DPts[j].Y > 0)
                {
                    var yy = Params.GetYScreenPoint(pan, gs.DPts[j].Y);
                    if (yy > 0)
                        pan.GrSurface.DrawLine(pp, 0, yy, pan.GPan.Width, yy);
                }
               


                double val = gs.DPts[j].X;
                string valueString;
                if (val > 1000)
                    valueString = ((int) val).ToString(CultureInfo.InvariantCulture);
                else
                {
                    var str1 = Params.SetPlaces(val);
                    valueString = val.ToString(str1);
                }

                if (_graphParameters.DrawBoundaryStrings)
                {
                    var str = gs.Name + " " + valueString;
                    pan.GrSurface.DrawString(str, f, pp.Brush, x + 1, y);
                                     
                }

                if (_graphParameters.ShowDifferenceMarkers)
                    DrawDifferenceMarkers(gs, pan, x, f, pp, ref yOffset);
            }
        }

        private void DrawDifferenceMarkers(GraphSurface gs, PanelControl pan, int x, Font f, Pen pp, ref int yOffset)
        {
            foreach (var t in pan.TagList)
            {
                if (!t.Visible)
                    continue;

                var gs2 = _graphPanel.GetGraphSurfaceFromTag(t.Tag);

                if (gs2 == null)
                    continue;
                if (gs.TagId == gs2.TagId)
                    continue;

                DrawDifferenceMarkerSet(gs2, pan, gs.DPts[0].X, x, f, pp, ref yOffset);
            }
        }

        private static void DrawDifferenceMarkerSet(GraphSurface gs2, PanelControl pan, float refX, int x, Font f, Pen pp, ref int yOffset)
        {
            var typ = gs2.GType;
            if (!IsAnyMarkerOrBoundary(typ))
                return;

            for (var n = 0; n < gs2.PtCount; n++)
            {
                var x2 = Params.GetXScreenPoint(pan, gs2, gs2.DPts[n].X);
                if ((!IsAnyMarker(typ) || x2 <= x) && typ != GraphType.Boundary) 
                    continue;
                var y = 30 + (yOffset*12);
                pan.GrSurface.DrawLine(pp, x, y, x2, y);
                DrawLeftRightArrowHeads(pan, pp, y, x, x2);

                double val = gs2.DPts[n].X - refX;
                string valueString;
                if (val > 1000)
                    valueString = ((int) val).ToString(CultureInfo.InvariantCulture);
                else
                {
                    var str1 = Params.SetPlaces(val);
                    valueString = val.ToString(str1);
                }
                if (x2 > x)
                    pan.GrSurface.DrawString(valueString, f, pp.Brush, x2 + 5, y - 5);
                else
                    pan.GrSurface.DrawString(valueString, f, pp.Brush, x + 5, y - 5);
                yOffset += 1;
            }
        }

        private static bool IsAnyMarker(GraphType gType)
        {
            return gType == GraphType.MoveableMarker || gType == GraphType.FixedMarker;
        }

        private static void DrawLeftRightArrowHeads(PanelControl pan, Pen p, int y, int x1, int x2)
        {
            const int siz = 16;
            var sign = 1;

            if (x1 > x2)
                sign = -1;

            DrawArrowHead(pan, p, x1, y, siz*sign);
            DrawArrowHead(pan, p, x2, y, siz*sign*-1);
        }

        private static void DrawArrowHead(PanelControl pan, Pen p, int x, int y, int dx)
        {
            var pts = new Point[4];
            pts[0].X = x;
            pts[0].Y = y;
            pts[1].X = x + dx;
            pts[1].Y = y + 4;
            pts[2].X = pts[1].X;
            pts[2].Y = pts[1].Y - 8;
            pts[3].X = pts[0].X;
            pts[3].Y = pts[0].Y;

            pan.GrSurface.FillPolygon(p.Brush, pts);
        }


        private static bool IsAnyMarkerOrBoundary(GraphType gType)
        {
            return gType == GraphType.MoveableMarker || gType == GraphType.Boundary || gType == GraphType.FixedMarker;
        }
    }
}