using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace GraphLib
{
    public class Grid : IDisposable
    {
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParameters;
        private readonly GrTabPanel _graphTabPanel;
        private Font _axisFont;
        private int _yAxisLabelOffset;


        public Grid(GraphPanel g)
        {
           
            try
            {
                var myScreen = Screen.PrimaryScreen;
                var area = myScreen.WorkingArea;
                if (area.Size.Height < 900)
                    SetSmallAxisFont();
                else
                    SetLargeAxisFont();
            }
            catch
            {
                SetSmallAxisFont();
            }
           
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphTabPanel = _graphParameters.GraphTabPanel;
            
        }

        public void SetSmallAxisFont()
        {
            _axisFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
            _yAxisLabelOffset = 1;
        }

        public void SetLargeAxisFont()
        {
            _axisFont = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
            _yAxisLabelOffset = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _axisFont.Dispose();
        }

        internal void DrawGrid(PanelControl pan)
        {          
            var gs = _graphPanel.GetMasterGraph(pan);

            if (gs == null)
                return;

            if (double.IsInfinity(gs.MaxD))
                gs.MaxD = gs.MinD + 100.0;

            if (double.IsNaN(gs.MaxD))
                gs.MaxD = gs.MinD + 100.0;

            var xStep = 50.0;
            while (xStep*pan.XScale < 75)
                xStep = xStep*2;
            while (xStep*pan.XScale > 150)
                xStep = xStep/2;

            var val = xStep;
            var cnt = 0;
            if (val <= 0.0)
                val = 1.0;

            if (val < 1.0)
            {
                while (val < 1.0)
                {
                    val *= 10;
                    cnt -= 1;
                }
            }

            if (val > 10)
            {
                while (val > 10)
                {
                    val = val/10;
                    cnt += 1;
                }
            }

            if (val > 5.001)
                xStep = 5.0;
            else if (val > 2.001)
                xStep = 2.0;
            else
                xStep = 1.0;

            if (cnt < 0)
            {
                cnt = Math.Abs(cnt);
                for (var i = 0; i < cnt; i++)
                    xStep /= 10.0;
            }
            else
            {
                for (var i = 0; i < cnt; i++)
                    xStep *= 10.0;
            }


            var minValue = gs.MinD;

            var maxValue = (gs.MaxD - minValue)/10.0;
// ReSharper disable once CompareOfFloatsByEqualityOperator
            if (maxValue == 0.0)
                maxValue = 10.0;

            var yStep = (pan.GPan.Height - 5)/10.0;
            while (yStep*pan.YScale < 50)
            {
                yStep = yStep*2.0;
                maxValue = maxValue*2.0;
            }

            while (yStep*pan.YScale > 100)
            {
                yStep = yStep/2.0;
                maxValue = maxValue/2.0;
            }

            val = maxValue;
            cnt = 0;

            if (val < 1.0 && val > 0.0)
            {
                while (val < 1.0)
                {
                    val *= 10;
                    cnt -= 1;
                }
            }

            while (val > 10)
            {
                val = val/10;
                cnt += 1;
            }

            if (val > 5.001)
                val = 5.0;
            else if (val > 2.001)
                val = 2.0;
            else
                val = 1.0;

            if (cnt < 0)
            {
                cnt = Math.Abs(cnt);
                for (var i = 0; i < cnt; i++)
                    val /= 10.0;
            }
            else
            {
                for (var i = 0; i < cnt; i++)
                    val *= 10.0;
            }

            if (_stepOverride && xStep > 1)           
                xStep = 1;

            if (_xStepOverride)
                xStep = 1;
           
            if (_stepOverride && val > 1)               
                val = 1;
            
            
            var scl = val/maxValue;
            maxValue = val;
            yStep = yStep*scl;

           
            try
            {
                DrawXGrid(pan, xStep);
                yStep = yStep*pan.YScale;
                DrawYGrid(pan, yStep, gs, minValue, maxValue);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        private bool _stepOverride;
        private bool _xStepOverride;
        public bool UseGreyGrid = false;

        public void SetStepOverride(bool overRide)
        {
            _stepOverride = overRide;
        }
        public void SetXStepOverride(bool overRide)
        {
            _xStepOverride = overRide;
        }
        private void DrawXGrid(PanelControl pan, double xStep)
        {
            double xPosition;
            int gridYPosition;

            var f = new Font("timesnewroman", 10);

            if (_graphTabPanel.Cst.SubPan == null)
                gridYPosition = pan.GPan.Top + pan.GPan.Height;
            else
            {
                var id = _graphTabPanel.Cst.SubPan.Length - 1;
                gridYPosition = _graphTabPanel.Cst.SubPan[id].GPan.Top + _graphTabPanel.Cst.SubPan[id].GPan.Height;
            }

            var pp = new Pen(Color.Black) {Width = 1};
            if (UseGreyGrid)
                pp.Color = Color.Gainsboro;
            if (_graphParameters.OriginalZeroAxis)
            {
                pp.Color = Color.Gainsboro;
                pp.Width = 3;
            }

           

            var hei = pan.GPan.Height;
            var i = 0;

            while (true)
            {
                xPosition = (i*xStep);
                xPosition *= pan.XScale;
                xPosition -= pan.XOffset;
                if (xPosition < 0)
                    break;
                i -= 1;
                if (i < -10000)
                    break;
            }

            i -= 1;
            do
            {
                xPosition = (i*xStep);
                xPosition *= pan.XScale;
                xPosition -= pan.XOffset;
                var ix = (int) xPosition;
                if (xPosition >= -5)
                {
                    if (_graphParameters.DrawTheGrid)
                    {
                        if (UseGreyGrid)
                        {
                            Pen p = new Pen(Color.DarkSlateBlue) {DashStyle = DashStyle.Dot};
                            pan.GrSurface.DrawLine(p, ix, 0, ix, hei);
                        }
                           
                        else
                            pan.GrSurface.DrawLine(Pens.Gainsboro, ix, 0, ix, hei);
                        
                    }
                        

                    if (_graphParameters.DrawTheXAxis && !_graphParameters.XGridDrawn)
                    {
                        var newPosition = xPosition + pan.GPan.Left + 2;
                        ix = (int) newPosition;

                        if (ix < pan.GPan.Right)
                        {
                            _graphTabPanel.Cst.GrSurface.DrawLine(Pens.Black, ix, gridYPosition, ix, (gridYPosition + 7));
                            string tString;
                            if (_graphParameters.TimeAxisStep == 1.0)
                                tString = "" + (i*xStep/_graphParameters.TimeAxisStep);
                            else
                                tString = (i*xStep/_graphParameters.TimeAxisStep).ToString(_graphParameters.XAxisFormat);
                            _graphTabPanel.Cst
                                .GrSurface.DrawString(tString, f, Brushes.Black, (int) (newPosition) - 15, gridYPosition + 8);

                            if ((i*xStep/_graphParameters.TimeAxisStep).Equals(0.0))
                            {                            
                                pan.GrSurface.DrawLine(_graphParameters.OriginalZeroAxis ? Pens.Black : pp, 
                                    (int) xPosition, 0, (int) xPosition, pan.GPan.Height);
                            }
                                
                        }
                    }
                }
                i += 1;
            } while (xPosition < (pan.GPan.Right - pan.GPan.Left));


            if (_graphParameters.DrawTheXAxis && !_graphParameters.XGridDrawn && _graphParameters.DrawTheXAxisLegend)
            {           
                var gs = _graphPanel.GetMasterGraph(pan);
                var title = gs.GxAxisTitle;
                if (_graphParameters.OverrideXAxisTitle)
                    title = _graphParameters.OverrideXAxisString;
                var dx = GetTextWidth(title);
                _graphTabPanel.Cst
                    .GrSurface.DrawString(title, _axisFont, Brushes.Black, pan.GPan.Left + (pan.GPan.Width/2) - dx/2, gridYPosition + 18);
            }

            _graphParameters.XGridDrawn = true;
        }
       
        private int GetTextWidth(string message)
        {
            var sz = _graphPanel.NetGraph.MeasureString(message, _axisFont);
            return (int) sz.Width;
        }
        
        private void DrawYGrid(PanelControl pan, double yStep, GraphSurface gs, double minValue, double maxValue)
        {            
            var pp = new Pen(Color.Black) {Width = 1};
            if (UseGreyGrid)
                pp.Color = Color.Gainsboro;

            var dyPos = (int) (pan.GPan.Height - 5 - pan.YOffset + ((minValue*yStep)/maxValue));
            DrawYAbove(pan, yStep, dyPos, gs, minValue, maxValue);
            DrawYBelow(pan, yStep, dyPos, gs, minValue, maxValue);


            var yPos = (int) (pan.GPan.Height - 5 - pan.YOffset + ((minValue*yStep)/maxValue));

            if (_graphParameters.OriginalZeroAxis)
            {
                pp.Color = Color.Gainsboro;
                pp.Width = 3;
            }

            pan.GrSurface.DrawLine(pp, 0, yPos, pan.GPan.Width, yPos);
       
            _graphTabPanel.Cst.GrSurface.ResetTransform();
            _graphTabPanel.Cst.GrSurface.RotateTransform(-90);
            var y = pan.GPan.Top + pan.GPan.Height / 2;
            var len = GetTextWidth(gs.GyAxisTitle);
        
            y = y + len / 2;
            _graphTabPanel.Cst.GrSurface.TranslateTransform(_yAxisLabelOffset, y, MatrixOrder.Append);
            _graphTabPanel.Cst.GrSurface.DrawString(gs.GyAxisTitle, _axisFont, Brushes.Black, 0, 0);        
            _graphTabPanel.Cst.GrSurface.ResetTransform();
           
        }

        private void DrawYAxisBits(PanelControl pan, int yPosition, GraphSurface gs, double value, double checkValue)
        {
            var f = new Font("timesnewroman", 10);

            if (_graphParameters.DrawTheGrid)
            {
                if (UseGreyGrid)
                {
                    Pen p = new Pen(Color.DarkSlateBlue) {DashStyle = DashStyle.Dot};
                    pan.GrSurface.DrawLine(p, 0, yPosition, pan.GPan.Width, yPosition);
                }                    
                else
                    pan.GrSurface.DrawLine(Pens.Gainsboro, 0, yPosition, pan.GPan.Width, yPosition);                
            }
               

            if (!_graphParameters.DrawTheYAxis) 
                return;

            var str = Params.GetValueString(checkValue, gs, value);
            var newPosition = yPosition + pan.GPan.Top + 2;
            var top = pan.GPan.Top + 2;
            var bot = top + pan.GPan.Height;

            if (newPosition >= bot || newPosition <= top) 
                return;
            _graphTabPanel.Cst.GrSurface.DrawLine(Pens.Black, pan.GPan.Left - 5, newPosition, pan.GPan.Left, newPosition);
            if ((newPosition - 5) <= bot)
                _graphTabPanel.Cst.GrSurface.DrawString(str, f, Brushes.Black, 15, newPosition - 7);
        }

        private void DrawYAbove(PanelControl pan, double yStep, int yPosition, GraphSurface gs, double minValue, double maxValue)
        {
            double value = 0;
            double dyPos = yPosition;
            var checkValue = Math.Abs(minValue + (20.0 * maxValue));
            do
            {
                dyPos = dyPos - yStep;
                value = value + maxValue;
                DrawYAxisBits(pan, (int)dyPos, gs, value, checkValue);
            } while (dyPos > 0);
        }

        private void DrawYBelow(PanelControl pan, double yStep, int yPosition, GraphSurface gs, double minValue, double maxValue)
        {
            double dyPos = yPosition;
            var checkValue = Math.Abs(minValue + (20.0*maxValue));

            dyPos = dyPos - yStep;
            var value = maxValue;

            do
            {
                dyPos = dyPos + yStep;
                value = value - maxValue;

                DrawYAxisBits(pan, (int) dyPos, gs, value, checkValue);
            } while (dyPos < pan.GPan.Height);
        }
    }
}