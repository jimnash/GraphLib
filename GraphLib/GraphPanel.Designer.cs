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
                if (Gdata != null)
                    Gdata.Dispose();
                if (GraphOptions != null)
                    GraphOptions.Dispose();
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
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.UseFollowingCursoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AxisDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowXAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowYAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowYAxisLegendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowXAxisLegendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AxisConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemovePanelItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowDifferenceMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowGraphTooltipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowBoundaryValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeHighlightWidthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X6ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X7ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.X9ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseAlternateZeroYAxisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showWaferBoundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundariesOnAllPanelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UseDarkBackgroundItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.YValueLabel = new System.Windows.Forms.Label();
            this.XValueLabel = new System.Windows.Forms.Label();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.TriggerPanel = new System.Windows.Forms.Panel();
            this.RPMLabel = new System.Windows.Forms.Label();
            this.BlueStatePanel = new System.Windows.Forms.Panel();
            this.BlueStateName = new System.Windows.Forms.Label();
            this.RedStatePanel = new System.Windows.Forms.Panel();
            this.RedStateName = new System.Windows.Forms.Label();
            this.RedExtraLabel = new System.Windows.Forms.Label();
            this.BlueExtraLabel = new System.Windows.Forms.Label();
            this.StateButton = new System.Windows.Forms.Button();
            this.ToolStrip1.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.BlueStatePanel.SuspendLayout();
            this.RedStatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripDropDownButton1});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(63, 25);
            this.ToolStrip1.TabIndex = 17;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // ToolStripDropDownButton1
            // 
            this.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UseFollowingCursoToolStripMenuItem,
            this.AxisDisplayToolStripMenuItem,
            this.AxisConfigurationToolStripMenuItem,
            this.AddPanelToolStripMenuItem,
            this.RemovePanelItem,
            this.ShowDifferenceMarkersToolStripMenuItem,
            this.ShowGraphTooltipToolStripMenuItem,
            this.ShowBoundaryValuesToolStripMenuItem,
            this.ChangeHighlightWidthToolStripMenuItem,
            this.UseAlternateZeroYAxisToolStripMenuItem,
            this.showWaferBoundariesToolStripMenuItem,
            this.boundariesOnAllPanelsToolStripMenuItem,
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem,
            this.UseDarkBackgroundItem});
            this.ToolStripDropDownButton1.Image = global::GraphLib.Properties.Resources.graph;
            this.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1";
            this.ToolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.ToolStripDropDownButton1.Text = "ToolStripDropDownButton1";
            this.ToolStripDropDownButton1.ToolTipText = "Display Options";
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
            // UseAlternateZeroYAxisToolStripMenuItem
            // 
            this.UseAlternateZeroYAxisToolStripMenuItem.Name = "UseAlternateZeroYAxisToolStripMenuItem";
            this.UseAlternateZeroYAxisToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.UseAlternateZeroYAxisToolStripMenuItem.Text = "Use Alternate Zero Y Axis";
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
            this.showWaferBoundariesToolStripMenuItem.Click += new System.EventHandler(this.ShowWaferBoundariesToolStripMenuItem_Click);
            // 
            // boundariesOnAllPanelsToolStripMenuItem
            // 
            this.boundariesOnAllPanelsToolStripMenuItem.CheckOnClick = true;
            this.boundariesOnAllPanelsToolStripMenuItem.Name = "boundariesOnAllPanelsToolStripMenuItem";
            this.boundariesOnAllPanelsToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.boundariesOnAllPanelsToolStripMenuItem.Text = "Zone Boundaries On All Panels";
            this.boundariesOnAllPanelsToolStripMenuItem.Visible = false;
            this.boundariesOnAllPanelsToolStripMenuItem.Click += new System.EventHandler(this.BoundariesOnAllPanelsToolStripMenuItem_Click);
            // 
            // zoomAndPanAllTimePanelsTogetherToolStripMenuItem
            // 
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Checked = true;
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Name = "zoomAndPanAllTimePanelsTogetherToolStripMenuItem";
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Text = "Zoom And Pan All Time Panels Together";
            this.zoomAndPanAllTimePanelsTogetherToolStripMenuItem.Click += new System.EventHandler(this.ZoomAndPanAllTimePanelsTogetherToolStripMenuItem_Click);
            // 
            // UseDarkBackgroundItem
            // 
            this.UseDarkBackgroundItem.CheckOnClick = true;
            this.UseDarkBackgroundItem.Name = "UseDarkBackgroundItem";
            this.UseDarkBackgroundItem.Size = new System.Drawing.Size(289, 22);
            this.UseDarkBackgroundItem.Text = "Use Dark Background";
            // 
            // TabControl1
            // 
            this.TabControl1.AllowDrop = true;
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
            this.TabPage1.AllowDrop = true;
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
            // BlueStatePanel
            // 
            this.BlueStatePanel.BackColor = System.Drawing.Color.Red;
            this.BlueStatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BlueStatePanel.Controls.Add(this.BlueStateName);
            this.BlueStatePanel.Location = new System.Drawing.Point(483, 5);
            this.BlueStatePanel.Name = "BlueStatePanel";
            this.BlueStatePanel.Size = new System.Drawing.Size(130, 17);
            this.BlueStatePanel.TabIndex = 31;
            this.BlueStatePanel.Visible = false;
            // 
            // BlueStateName
            // 
            this.BlueStateName.AutoSize = true;
            this.BlueStateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlueStateName.ForeColor = System.Drawing.Color.Black;
            this.BlueStateName.Location = new System.Drawing.Point(3, 1);
            this.BlueStateName.Name = "BlueStateName";
            this.BlueStateName.Size = new System.Drawing.Size(56, 13);
            this.BlueStateName.TabIndex = 31;
            this.BlueStateName.Text = "Blue State";
            this.BlueStateName.Visible = false;
            // 
            // RedStatePanel
            // 
            this.RedStatePanel.BackColor = System.Drawing.Color.Red;
            this.RedStatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RedStatePanel.Controls.Add(this.RedStateName);
            this.RedStatePanel.Location = new System.Drawing.Point(117, 5);
            this.RedStatePanel.Name = "RedStatePanel";
            this.RedStatePanel.Size = new System.Drawing.Size(130, 17);
            this.RedStatePanel.TabIndex = 32;
            this.RedStatePanel.Visible = false;
            // 
            // RedStateName
            // 
            this.RedStateName.AutoSize = true;
            this.RedStateName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RedStateName.ForeColor = System.Drawing.Color.Black;
            this.RedStateName.Location = new System.Drawing.Point(3, 1);
            this.RedStateName.Name = "RedStateName";
            this.RedStateName.Size = new System.Drawing.Size(122, 13);
            this.RedStateName.TabIndex = 31;
            this.RedStateName.Text = "Red State: Signal Rising";
            this.RedStateName.Visible = false;
            // 
            // RedExtraLabel
            // 
            this.RedExtraLabel.AutoSize = true;
            this.RedExtraLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RedExtraLabel.ForeColor = System.Drawing.Color.Black;
            this.RedExtraLabel.Location = new System.Drawing.Point(248, 7);
            this.RedExtraLabel.Name = "RedExtraLabel";
            this.RedExtraLabel.Size = new System.Drawing.Size(215, 13);
            this.RedExtraLabel.TabIndex = 33;
            this.RedExtraLabel.Text = "Start Time: Unknown, Finish Time:Unknown";
            this.RedExtraLabel.Visible = false;
            // 
            // BlueExtraLabel
            // 
            this.BlueExtraLabel.AutoSize = true;
            this.BlueExtraLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BlueExtraLabel.ForeColor = System.Drawing.Color.Black;
            this.BlueExtraLabel.Location = new System.Drawing.Point(616, 7);
            this.BlueExtraLabel.Name = "BlueExtraLabel";
            this.BlueExtraLabel.Size = new System.Drawing.Size(218, 13);
            this.BlueExtraLabel.TabIndex = 34;
            this.BlueExtraLabel.Text = "Start Time: Unknown, Finish Time: Unknown";
            this.BlueExtraLabel.Visible = false;
            // 
            // StateButton
            // 
            this.StateButton.Location = new System.Drawing.Point(38, 1);
            this.StateButton.Name = "StateButton";
            this.StateButton.Size = new System.Drawing.Size(65, 23);
            this.StateButton.TabIndex = 35;
            this.StateButton.Text = "STATE";
            this.StateButton.UseVisualStyleBackColor = true;
            this.StateButton.Visible = false;
            this.StateButton.Click += new System.EventHandler(this.StateButton_Click);
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StateButton);
            this.Controls.Add(this.BlueExtraLabel);
            this.Controls.Add(this.RedExtraLabel);
            this.Controls.Add(this.RedStatePanel);
            this.Controls.Add(this.BlueStatePanel);
            this.Controls.Add(this.RPMLabel);
            this.Controls.Add(this.TriggerPanel);
            this.Controls.Add(this.YValueLabel);
            this.Controls.Add(this.XValueLabel);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.ToolStrip1);
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(893, 471);
            this.Load += new System.EventHandler(this.GraphPanel_Load);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.TabControl1.ResumeLayout(false);
            this.BlueStatePanel.ResumeLayout(false);
            this.BlueStatePanel.PerformLayout();
            this.RedStatePanel.ResumeLayout(false);
            this.RedStatePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripDropDownButton ToolStripDropDownButton1;
        internal System.Windows.Forms.ToolStripMenuItem UseFollowingCursoToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AxisDisplayToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowGridToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowXAxisToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowYAxisToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowYAxisLegendToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowXAxisLegendToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AxisConfigurationToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AddPanelToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem RemovePanelItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowDifferenceMarkersToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowGraphTooltipToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ShowBoundaryValuesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ChangeHighlightWidthToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X2ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X3ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X4ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X5ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X6ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X7ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X8ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem X9ToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem UseAlternateZeroYAxisToolStripMenuItem;
        internal System.Windows.Forms.TabControl TabControl1;
        internal System.Windows.Forms.TabPage TabPage1;
        internal System.Windows.Forms.Label YValueLabel;
        internal System.Windows.Forms.Label XValueLabel;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        private System.Windows.Forms.Panel TriggerPanel;
        private System.Windows.Forms.ToolStripMenuItem showWaferBoundariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boundariesOnAllPanelsToolStripMenuItem;
        internal System.Windows.Forms.Label RPMLabel;
        private System.Windows.Forms.ToolStripMenuItem zoomAndPanAllTimePanelsTogetherToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem UseDarkBackgroundItem;
        private System.Windows.Forms.Panel BlueStatePanel;
        internal System.Windows.Forms.Label BlueStateName;
        private System.Windows.Forms.Panel RedStatePanel;
        internal System.Windows.Forms.Label RedStateName;
        internal System.Windows.Forms.Label BlueExtraLabel;
        internal System.Windows.Forms.Label RedExtraLabel;
        private System.Windows.Forms.Button StateButton;
    }
}
