using System;
using System.Windows.Forms;

namespace GraphLib
{
    public partial class AxisForm : Form
    {
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParams;  
        private bool _xChanged;
        private bool _yChanged;      
        private bool _applyAllXChanges;
        private bool _applyAllYChanges;
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gp"></param>
        public AxisForm(GraphPanel gp)
        {
            InitializeComponent();
            _graphPanel = gp;
            _graphParams = _graphPanel.GraphParameters;
        }

        /// <summary>
        ///  
        /// </summary>
        internal void Start()
        {
            Show();
            FillDataItems();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changed"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="span"></param>
        private void SetMinMaxSpan(bool changed,bool min, bool max, bool span)
        {
            if (_graphPanel == null)
                return;
            _xChanged = true;
            if (!changed)
                return;
            XAxisMin.Enabled = min;
            XAxisMax.Enabled = max;
            XAxisTimeSpan.Enabled = span;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAxisAuto_CheckedChanged(object sender, EventArgs e)
        {          
            SetMinMaxSpan(XAxisAuto.Checked,false, false, false);         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAxisFixed_CheckedChanged(object sender, EventArgs e)
        {          
            SetMinMaxSpan(XAxisFixed.Checked,false, false, true);                   
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAxisFixedMinMax_CheckedChanged(object sender, EventArgs e)
        {          
            SetMinMaxSpan(XAxisFixedMinMax.Checked,true, true, false);                      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAxisChanged(object sender, EventArgs e)
        {
            _xChanged = true;
        }       

        private void ApplyAllX_CheckedChanged(object sender, EventArgs e)
        {
            _applyAllXChanges = ApplyAllX.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YAxisAutoFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null)
                return;

            if (YAxisAutoFixed.Checked)
            {
                YAxisMin.Enabled = false;
                YAxisMax.Enabled = false;
            }
            _yChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YAxisAutoFree_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null)
                return;

            if (YAxisAutoFree.Checked)
            {
                YAxisMin.Enabled = false;
                YAxisMax.Enabled = false;
            }
            _yChanged = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YAXisFixedMinMax_CheckedChanged(object sender, EventArgs e)
        {
            if (_graphPanel == null)
                return;

            if (YAXisFixedMinMax.Checked)
            {
                YAxisMin.Enabled = true;
                YAxisMax.Enabled = true;
            }
            _yChanged = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YAxisMin_ValueChanged(object sender, EventArgs e)
        {
            _yChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YAxisMax_ValueChanged(object sender, EventArgs e)
        {
            _yChanged = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyAllY_CheckedChanged(object sender, EventArgs e)
        {
            _applyAllYChanges = ApplyAllY.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisCancelButton_Click(object sender, EventArgs e)
        {
              FillDataItems();
              Hide();
        }

        /// <summary>
        /// 
        /// </summary>
        private void AxisApplyAll()
        {
            var now = _graphParams.GraphTabPanel.GraphTabSelected;

            for (var i = 0; i < _graphParams.GraphTabPanel.GraphTabs.Length; i++)
            {
                _graphParams.GraphTabPanel.GraphTabSelected = i;
                var pControl = _graphParams.GraphTabPanel.Cst.MainPan;
                pControl.YAxisMin = (double)YAxisMin.Value;
                pControl.YAxisMax = (double)YAxisMax.Value;

                if (YAxisAutoFixed.Checked)
                    pControl.YAxisType = YAxisType.Auto;
                else if (YAxisAutoFree.Checked)
                    pControl.YAxisType = YAxisType.Free;
                else if (YAXisFixedMinMax.Checked)
                    pControl.YAxisType = YAxisType.MinMax;

                if (_graphParams.GraphTabPanel.Cst.SubPan == null)
                    continue;

                foreach (var t in _graphParams.GraphTabPanel.Cst.SubPan)
                {
                    pControl = t;
                    pControl.YAxisMin = (double)YAxisMin.Value;
                    pControl.YAxisMax = (double)YAxisMax.Value;

                    if (YAxisAutoFixed.Checked)
                        pControl.YAxisType = YAxisType.Auto;
                    else if (YAxisAutoFree.Checked)
                        pControl.YAxisType = YAxisType.Free;
                    else if (YAXisFixedMinMax.Checked)
                        pControl.YAxisType = YAxisType.MinMax;
                }
            }
            _graphParams.GraphTabPanel.GraphTabSelected = now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisApplyButton_Click(object sender, EventArgs e)
        {
            Hide();

            if (_graphPanel == null)
                return;

            if (_applyAllYChanges)
                AxisApplyAll();
            

            _graphParams.GraphTabPanel.Cst.MainPan.YAxisMin = (double)YAxisMin.Value;
            _graphParams.GraphTabPanel.Cst.MainPan.YAxisMax = (double)YAxisMax.Value;
           
            _graphParams.GraphTabPanel.SetXAxisMin((double)XAxisMin.Value, _applyAllXChanges);
            _graphParams.GraphTabPanel.SetXAxisMax((double)XAxisMax.Value, _applyAllXChanges);
            _graphParams.GraphTabPanel.SetXAxisSpan((double)XAxisTimeSpan.Value, _applyAllXChanges);

            if (_xChanged)
            {
                if (XAxisAuto.Checked)
                    _graphParams.GraphTabPanel.SetXAxisType(XAxisType.Auto, _applyAllXChanges);
                else if (XAxisFixed.Checked)
                    _graphParams.GraphTabPanel.SetXAxisType(XAxisType.SetSpan, _applyAllXChanges);
                else if (XAxisFixedMinMax.Checked)
                    _graphParams.GraphTabPanel.SetXAxisType(XAxisType.MinMax, _applyAllXChanges);
                _graphPanel.ResetX();
            }

            if (_yChanged)
            {
                if (YAxisAutoFixed.Checked)
                    _graphParams.GraphTabPanel.Cst.MainPan.YAxisType = YAxisType.Auto;
                else if (YAxisAutoFree.Checked)
                    _graphParams.GraphTabPanel.Cst.MainPan.YAxisType = YAxisType.Free;
                else if (YAXisFixedMinMax.Checked)
                    _graphParams.GraphTabPanel.Cst.MainPan.YAxisType = YAxisType.MinMax;
                _graphPanel.ResetY(_graphParams.GraphTabPanel.Cst.MainPan);
            }

            _graphPanel.RedrawAll();
            _xChanged = false;
            _yChanged = false;     
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillDataItems()
        {
            YAxisMin.Value = (decimal)_graphParams.GraphTabPanel.Cst.MainPan.YAxisMin;
            YAxisMax.Value = (decimal)_graphParams.GraphTabPanel.Cst.MainPan.YAxisMax;          
            XAxisMin.Value = (decimal)_graphParams.GraphTabPanel.XAxisMin;
            XAxisMax.Value = (decimal)_graphParams.GraphTabPanel.XAxisMax;
            XAxisTimeSpan.Value = (decimal)_graphParams.GraphTabPanel.XAxisSpan;
            SpanLabel.Text = _graphParams.GraphTabPanel.XAxisSpanLabel;

            FillYItems();
            FillXItems();

            _xChanged = false;
            _yChanged = false;
            
            ApplyAllX.Checked = false;
            ApplyAllY.Checked = false;
           
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillXItems()
        {
            var xType = _graphParams.GraphTabPanel.XAxisType;
            XAxisMin.Enabled = false;
            XAxisMax.Enabled = false;
            XAxisTimeSpan.Enabled = false;
            XAxisFixedMinMax.Checked = false;
            XAxisFixed.Checked = false;

            switch (xType)
            {
                case XAxisType.Auto:
                    XAxisAuto.Checked = true;
                    break;
                case XAxisType.MinMax:
                    XAxisFixedMinMax.Checked = true;
                    XAxisMin.Enabled = true;
                    XAxisMax.Enabled = true;
                    break;
                case XAxisType.SetSpan:
                    XAxisFixed.Checked = true;
                    XAxisTimeSpan.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillYItems()
        {
            var yType = _graphParams.GraphTabPanel.Cst.MainPan.YAxisType;
            YAxisAutoFree.Checked = false;
            YAxisAutoFixed.Checked = false;
            YAXisFixedMinMax.Checked = false;
            YAxisMin.Enabled = false;
            YAxisMax.Enabled = false;
            switch (yType)
            {
                case YAxisType.Auto:
                    YAxisAutoFixed.Checked = true;
                    break;
                case YAxisType.Free:
                    YAxisAutoFree.Checked = true;
                    break;
                case YAxisType.MinMax:
                    YAXisFixedMinMax.Checked = true;
                    YAxisMin.Enabled = true;
                    YAxisMax.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisForm_Load(object sender, EventArgs e)
        {
            TopMost = true;
        }

    }
}
