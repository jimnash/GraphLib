namespace GraphLib
{
    partial class GraphPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if(disposing)
            {
                if (Gmo != null)
                    Gmo.Dispose();
                if (Gpg != null)
                    Gpg.Dispose();
                if (Gopt != null)
                    Gopt.Dispose();
                if (_grid != null)
                    _grid.Dispose();
              
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphPanel));
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.YValueLabel = new System.Windows.Forms.Label();
            this.XValueLabel = new System.Windows.Forms.Label();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.TriggerPanel = new System.Windows.Forms.Panel();
            this.RPMLabel = new System.Windows.Forms.Label();
            this.UseFollowingCursoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AxisDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowXAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowYAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowYAxisLegendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowXAxisLegendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AxisConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowImprtedGraphManipulationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MoveImportedGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Set360DegreePassCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetPassCountTo1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetPassCountTo3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetPassCountTo5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemovePanelItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowDifferenceMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowGraphTooltipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowBoundaryValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetOthersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeHighlightWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveAllImportedFileGraphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseAlternateZeroYAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EndValueScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NormalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.X3ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.X5ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.X10ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showWaferBoundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundariesOnAllPanelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolNewTabButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.TabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(102, 25);
            this.ToolStrip1.TabIndex = 17;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl1.ItemSize = new System.Drawing.Size(120, 25);
            this.TabControl1.Location = new System.Drawing.Point(0, 28);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(772, 443);
            this.TabControl1.TabIndex = 18;
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage1.Location = new System.Drawing.Point(4, 29);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage1.Size = new System.Drawing.Size(764, 410);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "TabPage1";
            // 
            // YValueLabel
            // 
            this.YValueLabel.AutoSize = true;
            this.YValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YValueLabel.Location = new System.Drawing.Point(805, 35);
            this.YValueLabel.Name = "YValueLabel";
            this.YValueLabel.Size = new System.Drawing.Size(17, 13);
            this.YValueLabel.TabIndex = 26;
            this.YValueLabel.Text = "Y:";
            // 
            // XValueLabel
            // 
            this.XValueLabel.AutoSize = true;
            this.XValueLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XValueLabel.Location = new System.Drawing.Point(708, 35);
            this.XValueLabel.Name = "XValueLabel";
            this.XValueLabel.Size = new System.Drawing.Size(17, 13);
            this.XValueLabel.TabIndex = 25;
            this.XValueLabel.Text = "X:";
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "openFileDialog1";
            // 
            // TriggerPanel
            // 
            this.TriggerPanel.BackColor = System.Drawing.Color.Red;
            this.TriggerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TriggerPanel.Location = new System.Drawing.Point(668, 5);
            this.TriggerPanel.Name = "TriggerPanel";
            this.TriggerPanel.Size = new System.Drawing.Size(57, 17);
            this.TriggerPanel.TabIndex = 29;
            this.TriggerPanel.Visible = false;
            // 
            // RPMLabel
            // 
            this.RPMLabel.AutoSize = true;
            this.RPMLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RPMLabel.ForeColor = System.Drawing.Color.DarkBlue;
            this.RPMLabel.Location = new System.Drawing.Point(739, 7);
            this.RPMLabel.Name = "RPMLabel";
            this.RPMLabel.Size = new System.Drawing.Size(105, 13);
            this.RPMLabel.TabIndex = 30;
            this.RPMLabel.Text = "Current RPM: 0.0";
            this.RPMLabel.Visible = false;
            // 
            // UseFollowingCursoToolStripMenuItem
            // 
            this.UseFollowingCursoToolStripMenuItem.Name = "UseFollowingCursoToolStripMenuItem";
            this.UseFollowingCursoToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.UseFollowingCursoToolStripMenuItem.Text = "Use Following Cursor";
            // 
            // AxisDisplayToolStripMenuItem
            // 
            this.AxisDisplayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowGridToolStripMenuItem,
            this.ShowXAxisToolStripMenuItem,
            this.ShowYAxisToolStripMenuItem,
            this.ShowYAxisLegendToolStripMenuItem,
            this.ShowXAxisLegendToolStripMenuItem});
            this.AxisDisplayToolStripMenuItem.Name = "AxisDisplayToolStripMenuItem";
            this.AxisDisplayToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.AxisDisplayToolStripMenuItem.Text = "Axis Display";
            // 
            // ShowGridToolStripMenuItem
            // 
            this.ShowGridToolStripMenuItem.Checked = true;
            this.ShowGridToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGridToolStripMenuItem.Name = "ShowGridToolStripMenuItem";
            this.ShowGridToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ShowGridToolStripMenuItem.Text = "Show Grid";
            // 
            // ShowXAxisToolStripMenuItem
            // 
            this.ShowXAxisToolStripMenuItem.Checked = true;
            this.ShowXAxisToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowXAxisToolStripMenuItem.Name = "ShowXAxisToolStripMenuItem";
            this.ShowXAxisToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ShowXAxisToolStripMenuItem.Text = "Show X Axis";
            // 
            // ShowYAxisToolStripMenuItem
            // 
            this.ShowYAxisToolStripMenuItem.Checked = true;
            this.ShowYAxisToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowYAxisToolStripMenuItem.Name = "ShowYAxisToolStripMenuItem";
            this.ShowYAxisToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ShowYAxisToolStripMenuItem.Text = "Show Y Axis";
            // 
            // ShowYAxisLegendToolStripMenuItem
            // 
            this.ShowYAxisLegendToolStripMenuItem.Checked = true;
            this.ShowYAxisLegendToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowYAxisLegendToolStripMenuItem.Name = "ShowYAxisLegendToolStripMenuItem";
            this.ShowYAxisLegendToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ShowYAxisLegendToolStripMenuItem.Text = "Show Y Axis Legend";
            // 
            // ShowXAxisLegendToolStripMenuItem
            // 
            this.ShowXAxisLegendToolStripMenuItem.Checked = true;
            this.ShowXAxisLegendToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowXAxisLegendToolStripMenuItem.Name = "ShowXAxisLegendToolStripMenuItem";
            this.ShowXAxisLegendToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ShowXAxisLegendToolStripMenuItem.Text = "Show X Axis Legend";
            // 
            // AxisConfigurationToolStripMenuItem
            // 
            this.AxisConfigurationToolStripMenuItem.Name = "AxisConfigurationToolStripMenuItem";
            this.AxisConfigurationToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.AxisConfigurationToolStripMenuItem.Text = "Axis Configuration";
            // 
            // allowImprtedGraphManipulationToolStripMenuItem
            // 
            this.allowImprtedGraphManipulationToolStripMenuItem.Name = "allowImprtedGraphManipulationToolStripMenuItem";
            this.allowImprtedGraphManipulationToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.allowImprtedGraphManipulationToolStripMenuItem.Text = "Allow Imported Graph Manipulation";
            // 
            // MoveImportedGraphToolStripMenuItem
            // 
            this.MoveImportedGraphToolStripMenuItem.CheckOnClick = true;
            this.MoveImportedGraphToolStripMenuItem.Name = "MoveImportedGraphToolStripMenuItem";
            this.MoveImportedGraphToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.MoveImportedGraphToolStripMenuItem.Text = "Move Imported Graph";
            // 
            // Set360DegreePassCountToolStripMenuItem
            // 
            this.Set360DegreePassCountToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetPassCountTo1ToolStripMenuItem,
            this.SetPassCountTo3ToolStripMenuItem,
            this.SetPassCountTo5ToolStripMenuItem,
            this.ClearGraphToolStripMenuItem});
            this.Set360DegreePassCountToolStripMenuItem.Name = "Set360DegreePassCountToolStripMenuItem";
            this.Set360DegreePassCountToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.Set360DegreePassCountToolStripMenuItem.Text = "Number Of Lines Drawn Before Refresh";
            this.Set360DegreePassCountToolStripMenuItem.Visible = false;
            // 
            // SetPassCountTo1ToolStripMenuItem
            // 
            this.SetPassCountTo1ToolStripMenuItem.Checked = true;
            this.SetPassCountTo1ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SetPassCountTo1ToolStripMenuItem.Name = "SetPassCountTo1ToolStripMenuItem";
            this.SetPassCountTo1ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SetPassCountTo1ToolStripMenuItem.Text = "1";
            // 
            // SetPassCountTo3ToolStripMenuItem
            // 
            this.SetPassCountTo3ToolStripMenuItem.Name = "SetPassCountTo3ToolStripMenuItem";
            this.SetPassCountTo3ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SetPassCountTo3ToolStripMenuItem.Text = "3";
            // 
            // SetPassCountTo5ToolStripMenuItem
            // 
            this.SetPassCountTo5ToolStripMenuItem.Name = "SetPassCountTo5ToolStripMenuItem";
            this.SetPassCountTo5ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.SetPassCountTo5ToolStripMenuItem.Text = "5";
            // 
            // ClearGraphToolStripMenuItem
            // 
            this.ClearGraphToolStripMenuItem.Name = "ClearGraphToolStripMenuItem";
            this.ClearGraphToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ClearGraphToolStripMenuItem.Text = "Clear Graph";
            // 
            // AddPanelToolStripMenuItem
            // 
            this.AddPanelToolStripMenuItem.Name = "AddPanelToolStripMenuItem";
            this.AddPanelToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.AddPanelToolStripMenuItem.Text = "Add Panel";
            // 
            // RemovePanelItem
            // 
            this.RemovePanelItem.Name = "RemovePanelItem";
            this.RemovePanelItem.Size = new System.Drawing.Size(289, 22);
            this.RemovePanelItem.Text = "Remove Panel";
            this.RemovePanelItem.Visible = false;
            // 
            // ShowDifferenceMarkersToolStripMenuItem
            // 
            this.ShowDifferenceMarkersToolStripMenuItem.Name = "ShowDifferenceMarkersToolStripMenuItem";
            this.ShowDifferenceMarkersToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.ShowDifferenceMarkersToolStripMenuItem.Text = "Show Difference Markers";
            // 
            // ShowGraphTooltipToolStripMenuItem
            // 
            this.ShowGraphTooltipToolStripMenuItem.CheckOnClick = true;
            this.ShowGraphTooltipToolStripMenuItem.Name = "ShowGraphTooltipToolStripMenuItem";
            this.ShowGraphTooltipToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.ShowGraphTooltipToolStripMenuItem.Text = "Show Graph Tooltip";
            // 
            // ShowBoundaryValuesToolStripMenuItem
            // 
            this.ShowBoundaryValuesToolStripMenuItem.Name = "ShowBoundaryValuesToolStripMenuItem";
            this.ShowBoundaryValuesToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.ShowBoundaryValuesToolStripMenuItem.Text = "Show Marker Values";
            // 
            // ResetOthersToolStripMenuItem
            // 
            this.ResetOthersToolStripMenuItem.Name = "ResetOthersToolStripMenuItem";
            this.ResetOthersToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.ResetOthersToolStripMenuItem.Text = "Reset Other Tool Menus";
            // 
            // ChangeHighlightWidthToolStripMenuItem
            // 
            this.ChangeHighlightWidthToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.X2ToolStripMenuItem,
            this.X3ToolStripMenuItem,
            this.X4ToolStripMenuItem,
            this.X5ToolStripMenuItem,
            this.X6ToolStripMenuItem,
            this.X7ToolStripMenuItem,
            this.X8ToolStripMenuItem,
            this.X9ToolStripMenuItem});
            this.ChangeHighlightWidthToolStripMenuItem.Name = "ChangeHighlightWidthToolStripMenuItem";
            this.ChangeHighlightWidthToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.ChangeHighlightWidthToolStripMenuItem.Text = "Change Highlight Width";
            // 
            // X2ToolStripMenuItem
            // 
            this.X2ToolStripMenuItem.Checked = true;
            this.X2ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.X2ToolStripMenuItem.Name = "X2ToolStripMenuItem";
            this.X2ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X2ToolStripMenuItem.Tag = "2";
            this.X2ToolStripMenuItem.Text = "x2";
            // 
            // X3ToolStripMenuItem
            // 
            this.X3ToolStripMenuItem.Name = "X3ToolStripMenuItem";
            this.X3ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X3ToolStripMenuItem.Tag = "3";
            this.X3ToolStripMenuItem.Text = "x3";
            // 
            // X4ToolStripMenuItem
            // 
            this.X4ToolStripMenuItem.Name = "X4ToolStripMenuItem";
            this.X4ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X4ToolStripMenuItem.Tag = "4";
            this.X4ToolStripMenuItem.Text = "x4";
            // 
            // X5ToolStripMenuItem
            // 
            this.X5ToolStripMenuItem.Name = "X5ToolStripMenuItem";
            this.X5ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X5ToolStripMenuItem.Tag = "5";
            this.X5ToolStripMenuItem.Text = "x5";
            // 
            // X6ToolStripMenuItem
            // 
            this.X6ToolStripMenuItem.Name = "X6ToolStripMenuItem";
            this.X6ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X6ToolStripMenuItem.Tag = "6";
            this.X6ToolStripMenuItem.Text = "x6";
            // 
            // X7ToolStripMenuItem
            // 
            this.X7ToolStripMenuItem.Name = "X7ToolStripMenuItem";
            this.X7ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X7ToolStripMenuItem.Tag = "7";
            this.X7ToolStripMenuItem.Text = "x7";
            // 
            // X8ToolStripMenuItem
            // 
            this.X8ToolStripMenuItem.Name = "X8ToolStripMenuItem";
            this.X8ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X8ToolStripMenuItem.Tag = "8";
            this.X8ToolStripMenuItem.Text = "x8";
            // 
            // X9ToolStripMenuItem
            // 
            this.X9ToolStripMenuItem.Name = "X9ToolStripMenuItem";
            this.X9ToolStripMenuItem.Size = new System.Drawing.Size(85, 22);
            this.X9ToolStripMenuItem.Tag = "9";
            this.X9ToolStripMenuItem.Text = "x9";
            // 
            // RemoveAllImportedFileGraphsToolStripMenuItem
            // 
            this.RemoveAllImportedFileGraphsToolStripMenuItem.Name = "RemoveAllImportedFileGraphsToolStripMenuItem";
            this.RemoveAllImportedFileGraphsToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.RemoveAllImportedFileGraphsToolStripMenuItem.Text = "Remove All Imported File Graphs";
            // 
            // UseAlternateZeroYAxisToolStripMenuItem
            // 
            this.UseAlternateZeroYAxisToolStripMenuItem.Name = "UseAlternateZeroYAxisToolStripMenuItem";
            this.UseAlternateZeroYAxisToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.UseAlternateZeroYAxisToolStripMenuItem.Text = "Use Alternate Zero Y Axis";
            // 
            // EndValueScaleToolStripMenuItem
            // 
            this.EndValueScaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NormalToolStripMenuItem,
            this.X2ToolStripMenuItem1,
            this.X3ToolStripMenuItem1,
            this.X5ToolStripMenuItem1,
            this.X10ToolStripMenuItem});
            this.EndValueScaleToolStripMenuItem.Name = "EndValueScaleToolStripMenuItem";
            this.EndValueScaleToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.EndValueScaleToolStripMenuItem.Text = "End Value Scale";
            // 
            // NormalToolStripMenuItem
            // 
            this.NormalToolStripMenuItem.Checked = true;
            this.NormalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NormalToolStripMenuItem.Name = "NormalToolStripMenuItem";
            this.NormalToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.NormalToolStripMenuItem.Tag = "1";
            this.NormalToolStripMenuItem.Text = "Normal";
            // 
            // X2ToolStripMenuItem1
            // 
            this.X2ToolStripMenuItem1.Name = "X2ToolStripMenuItem1";
            this.X2ToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.X2ToolStripMenuItem1.Tag = "2";
            this.X2ToolStripMenuItem1.Text = "x2";
            // 
            // X3ToolStripMenuItem1
            // 
            this.X3ToolStripMenuItem1.Name = "X3ToolStripMenuItem1";
            this.X3ToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.X3ToolStripMenuItem1.Tag = "3";
            this.X3ToolStripMenuItem1.Text = "x3";
            // 
            // X5ToolStripMenuItem1
            // 
            this.X5ToolStripMenuItem1.Name = "X5ToolStripMenuItem1";
            this.X5ToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.X5ToolStripMenuItem1.Tag = "5";
            this.X5ToolStripMenuItem1.Text = "x5";
            // 
            // X10ToolStripMenuItem
            // 
            this.X10ToolStripMenuItem.Name = "X10ToolStripMenuItem";
            this.X10ToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.X10ToolStripMenuItem.Tag = "10";
            this.X10ToolStripMenuItem.Text = "x10";
            // 
            // showWaferBoundariesToolStripMenuItem
            // 
            this.showWaferBoundariesToolStripMenuItem.Checked = true;
            this.showWaferBoundariesToolStripMenuItem.CheckOnClick = true;
            this.showWaferBoundariesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showWaferBoundariesToolStripMenuItem.Name = "showWaferBoundariesToolStripMenuItem";
            this.showWaferBoundariesToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.showWaferBoundariesToolStripMenuItem.Text = "Show Zone Boundaries";
            this.showWaferBoundariesToolStripMenuItem.Visible = false;
            this.showWaferBoundariesToolStripMenuItem.Click += new System.EventHandler(this.showWaferBoundariesToolStripMenuItem_Click);
            // 
            // boundariesOnAllPanelsToolStripMenuItem
            // 
            this.boundariesOnAllPanelsToolStripMenuItem.CheckOnClick = true;
            this.boundariesOnAllPanelsToolStripMenuItem.Name = "boundariesOnAllPanelsToolStripMenuItem";
            this.boundariesOnAllPanelsToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.boundariesOnAllPanelsToolStripMenuItem.Text = "Zone Boundaries On All Panels";
            this.boundariesOnAllPanelsToolStripMenuItem.Visible = false;
            this.boundariesOnAllPanelsToolStripMenuItem.Click += new System.EventHandler(this.boundariesOnAllPanelsToolStripMenuItem_Click);
            // 
            // zoomAndPanAllTimePanelsTogetherToolStripMenuItem
            // 
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Checked = true;
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Name = "zoomAndPanAllTimePanelsTogetherToolStripMenuItem";
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Text = "Zoom And Pan All Time Panels Together";
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Click += new System.EventHandler(this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem_Click);
            // 
            // ToolNewTabButton
            // 
            this.ToolNewTabButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolNewTabButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolNewTabButton.Image")));
            this.ToolNewTabButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolNewTabButton.Name = "ToolNewTabButton";
            this.ToolNewTabButton.Size = new System.Drawing.Size(23, 22);
            this.ToolNewTabButton.Text = "ToolStripButton4";
            this.ToolNewTabButton.ToolTipText = "Create New Graph Tab";
            // 
            // ToolStripDropDownButton1
            // 
            this.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripDropDownButton1.Image")));
            this.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1";
            this.ToolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1";
            this.ToolStripDropDownButton1.ToolTipText = "Display Options";
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RPMLabel);
            this.Controls.Add(this.TriggerPanel);
            this.Controls.Add(this.YValueLabel);
            this.Controls.Add(this.XValueLabel);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.ToolStrip1);
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(893, 471);
            this.Load += new System.EventHandler(this.GraphPanel_Load);
            this.TabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton ToolNewTabButton;
        internal System.Windows.Forms.ToolStripDropDownButton ToolStripDropDownButton1;
        internal System.Windows.Forms.ToolStripMenuItem UseFollowingCursoToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AxisDisplayToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowGridToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowXAxisToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowYAxisToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowYAxisLegendToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowXAxisLegendToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AxisConfigurationToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem MoveImportedGraphToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem Set360DegreePassCountToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SetPassCountTo1ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SetPassCountTo3ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SetPassCountTo5ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ClearGraphToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddPanelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem RemovePanelItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowDifferenceMarkersToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowGraphTooltipToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowBoundaryValuesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ResetOthersToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ChangeHighlightWidthToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X2ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X3ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X4ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X5ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X6ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X7ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X8ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X9ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem RemoveAllImportedFileGraphsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem UseAlternateZeroYAxisToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem EndValueScaleToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem NormalToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X2ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem X3ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem X5ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem X10ToolStripMenuItem;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Label YValueLabel;
        internal System.Windows.Forms.Label XValueLabel;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        public System.Windows.Forms.ToolStripMenuItem allowImprtedGraphManipulationToolStripMenuItem;
        private System.Windows.Forms.Panel TriggerPanel;
        private System.Windows.Forms.ToolStripMenuItem showWaferBoundariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boundariesOnAllPanelsToolStripMenuItem;
        internal System.Windows.Forms.Label RPMLabel;
        private System.Windows.Forms.ToolStripMenuItem zoomAndPanAllTimePanelsTogetherToolStripMenuItem;
    }
}
