using System;
using System.Windows.Forms;

namespace GraphLib
{
    /// <summary>
    /// Mouse events just for the panel, used for re-sizing sub panels
    /// </summary>
    public class TabMouseOps
    {             
        private readonly GrTabPanel _graphicsTabPanel;
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParameters;

        private double _panPercent;
        private bool _tabMDown;
        private int _edgePanel = -1;
        private int _bottomY;
        private int _topY;
        
        public TabMouseOps(GraphPanel g)
        {
            _graphPanel = g;
            _graphParameters = _graphPanel.GraphParameters;
            _graphicsTabPanel = _graphPanel.GraphParameters.GraphTabPanel;            
        }

        internal void TabMouseMove(int x, int y)
        {
            if (!_tabMDown)
            {
                _graphicsTabPanel.Cst.Page.Cursor = AtEdgeOfPanel(x, y) ? Cursors.HSplit : Cursors.Arrow;
                return;
            }

            if (_edgePanel == -1)
                return;

            var mainPan = _graphicsTabPanel.Cst.MainPan;
            var subPan = _graphicsTabPanel.Cst.SubPan;

            var p1 = _panPercent * (y - _topY) / (_bottomY - _topY);
            var p2 = _panPercent * (_bottomY - y) / (_bottomY - _topY);
            if (p1 < 5 || p2 < 5)
                return;

            if (_edgePanel == 0)
            {
                if (Math.Abs(p1 - mainPan.PercentHeight) <= 1.0)
                    return;
                mainPan.PercentHeight = p1;
                subPan[0].PercentHeight = p2;                
            }
            else
            {
                if (Math.Abs(p1 - subPan[_edgePanel - 1].PercentHeight) <= 1.0)
                    return;
                subPan[_edgePanel - 1].PercentHeight = p1;
                subPan[_edgePanel].PercentHeight = p2;                
            }         
            _graphPanel.SetSize(_graphPanel.Width, _graphPanel.Height);
        }

        internal void TabMouseUp()
        {
            _tabMDown = false;
            _graphParameters.HaltUpdateFromSplit = false;
        }

        internal void TabMouseDown(int x, int y)
        {
            if (!AtEdgeOfPanel(x, y))
                return;

            var mainPan = _graphicsTabPanel.Cst.MainPan;
            var subPan = _graphicsTabPanel.Cst.SubPan;

            switch (_edgePanel)
            {
                case -1:
                    return;
                case 0:
                    _panPercent = mainPan.PercentHeight +
                                  subPan[0].PercentHeight;
                    _topY = mainPan.GPan.Top;
                    _bottomY = subPan[0].GPan.Bottom;
                    break;
                default:
                    _panPercent = subPan[_edgePanel - 1].PercentHeight +
                                  subPan[_edgePanel].PercentHeight;
                    _topY = subPan[_edgePanel - 1].GPan.Top;
                    _bottomY = subPan[_edgePanel].GPan.Bottom;
                    break;
            }

            _tabMDown = true;
            _graphParameters.HaltUpdateFromSplit = true;
        }

        private bool AtEdgeOfPanel(int x, int y)
        {
            _edgePanel = -1;
            var mainPan = _graphicsTabPanel.Cst.MainPan;
            var subPan = _graphicsTabPanel.Cst.SubPan;

            if (subPan == null)
                return false;

            if (y < mainPan.GPan.Top)
                return false;

            var cnt = subPan.Length;

            if (y >= subPan[cnt - 1].GPan.Bottom)
                return false;
            if (x < mainPan.GPan.Left)
                return false;
            if (x > mainPan.GPan.Right)
                return false;

            if (y > mainPan.GPan.Bottom && y < subPan[0].GPan.Top)
            {
                _edgePanel = 0;
                return true;
            }

            for (var i = 1; i < cnt; i++)
                if (y > subPan[i - 1].GPan.Bottom && y < subPan[i].GPan.Top)
                {
                    _edgePanel = i;
                    return true;
                }
            return false;
        }
    }
}