namespace GraphLib
{
    partial class AxisForm
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AxisCancelButton = new System.Windows.Forms.Button();
            this.AxisApplyButton = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.ApplyAllY = new System.Windows.Forms.CheckBox();
            this.YAxisMax = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.YAxisMin = new System.Windows.Forms.NumericUpDown();
            this.YAXisFixedMinMax = new System.Windows.Forms.RadioButton();
            this.YAxisAutoFree = new System.Windows.Forms.RadioButton();
            this.YAxisAutoFixed = new System.Windows.Forms.RadioButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.ApplyAllX = new System.Windows.Forms.CheckBox();
            this.XAxisMax = new System.Windows.Forms.NumericUpDown();
            this.Label3 = new System.Windows.Forms.Label();
            this.XAxisMin = new System.Windows.Forms.NumericUpDown();
            this.SpanLabel = new System.Windows.Forms.Label();
            this.XAxisTimeSpan = new System.Windows.Forms.NumericUpDown();
            this.XAxisFixedMinMax = new System.Windows.Forms.RadioButton();
            this.XAxisFixed = new System.Windows.Forms.RadioButton();
            this.XAxisAuto = new System.Windows.Forms.RadioButton();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMin)).BeginInit();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisTimeSpan)).BeginInit();
            this.SuspendLayout();
            // 
            // AxisCancelButton
            // 
            this.AxisCancelButton.Location = new System.Drawing.Point(247, 281);
            this.AxisCancelButton.Name = "AxisCancelButton";
            this.AxisCancelButton.Size = new System.Drawing.Size(75, 23);
            this.AxisCancelButton.TabIndex = 10;
            this.AxisCancelButton.Text = "Cancel";
            this.AxisCancelButton.UseVisualStyleBackColor = true;
            this.AxisCancelButton.Click += new System.EventHandler(this.AxisCancelButton_Click);
            // 
            // AxisApplyButton
            // 
            this.AxisApplyButton.Location = new System.Drawing.Point(10, 281);
            this.AxisApplyButton.Name = "AxisApplyButton";
            this.AxisApplyButton.Size = new System.Drawing.Size(75, 23);
            this.AxisApplyButton.TabIndex = 9;
            this.AxisApplyButton.Text = "OK";
            this.AxisApplyButton.UseVisualStyleBackColor = true;
            this.AxisApplyButton.Click += new System.EventHandler(this.AxisApplyButton_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.ApplyAllY);
            this.GroupBox2.Controls.Add(this.YAxisMax);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Controls.Add(this.YAxisMin);
            this.GroupBox2.Controls.Add(this.YAXisFixedMinMax);
            this.GroupBox2.Controls.Add(this.YAxisAutoFree);
            this.GroupBox2.Controls.Add(this.YAxisAutoFixed);
            this.GroupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(5, 149);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(327, 123);
            this.GroupBox2.TabIndex = 7;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Y Axis Configuration";
            // 
            // ApplyAllY
            // 
            this.ApplyAllY.AutoSize = true;
            this.ApplyAllY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyAllY.Location = new System.Drawing.Point(14, 100);
            this.ApplyAllY.Name = "ApplyAllY";
            this.ApplyAllY.Size = new System.Drawing.Size(215, 17);
            this.ApplyAllY.TabIndex = 11;
            this.ApplyAllY.Text = "Apply Changes to ALL Tabs And Panels";
            this.ApplyAllY.UseVisualStyleBackColor = true;
            this.ApplyAllY.CheckedChanged += new System.EventHandler(this.ApplyAllY_CheckedChanged);
            // 
            // YAxisMax
            // 
            this.YAxisMax.DecimalPlaces = 2;
            this.YAxisMax.Enabled = false;
            this.YAxisMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAxisMax.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.YAxisMax.Location = new System.Drawing.Point(110, 66);
            this.YAxisMax.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.YAxisMax.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.YAxisMax.Name = "YAxisMax";
            this.YAxisMax.Size = new System.Drawing.Size(83, 20);
            this.YAxisMax.TabIndex = 5;
            this.YAxisMax.ValueChanged += new System.EventHandler(this.YAxisMax_ValueChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(34, 70);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(73, 13);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "And Maximum";
            // 
            // YAxisMin
            // 
            this.YAxisMin.DecimalPlaces = 2;
            this.YAxisMin.Enabled = false;
            this.YAxisMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAxisMin.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.YAxisMin.Location = new System.Drawing.Point(110, 44);
            this.YAxisMin.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.YAxisMin.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.YAxisMin.Name = "YAxisMin";
            this.YAxisMin.Size = new System.Drawing.Size(83, 20);
            this.YAxisMin.TabIndex = 3;
            this.YAxisMin.ValueChanged += new System.EventHandler(this.YAxisMin_ValueChanged);
            // 
            // YAXisFixedMinMax
            // 
            this.YAXisFixedMinMax.AutoSize = true;
            this.YAXisFixedMinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAXisFixedMinMax.Location = new System.Drawing.Point(13, 44);
            this.YAXisFixedMinMax.Name = "YAXisFixedMinMax";
            this.YAXisFixedMinMax.Size = new System.Drawing.Size(94, 17);
            this.YAXisFixedMinMax.TabIndex = 2;
            this.YAXisFixedMinMax.Text = "Fixed Minimum";
            this.YAXisFixedMinMax.UseVisualStyleBackColor = true;
            this.YAXisFixedMinMax.CheckedChanged += new System.EventHandler(this.YAXisFixedMinMax_CheckedChanged);
            // 
            // YAxisAutoFree
            // 
            this.YAxisAutoFree.AutoSize = true;
            this.YAxisAutoFree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAxisAutoFree.Location = new System.Drawing.Point(107, 21);
            this.YAxisAutoFree.Name = "YAxisAutoFree";
            this.YAxisAutoFree.Size = new System.Drawing.Size(71, 17);
            this.YAxisAutoFree.TabIndex = 1;
            this.YAxisAutoFree.Text = "Auto Free";
            this.YAxisAutoFree.UseVisualStyleBackColor = true;
            this.YAxisAutoFree.Visible = false;
            this.YAxisAutoFree.CheckedChanged += new System.EventHandler(this.YAxisAutoFree_CheckedChanged);
            // 
            // YAxisAutoFixed
            // 
            this.YAxisAutoFixed.AutoSize = true;
            this.YAxisAutoFixed.Checked = true;
            this.YAxisAutoFixed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAxisAutoFixed.Location = new System.Drawing.Point(13, 21);
            this.YAxisAutoFixed.Name = "YAxisAutoFixed";
            this.YAxisAutoFixed.Size = new System.Drawing.Size(77, 17);
            this.YAxisAutoFixed.TabIndex = 0;
            this.YAxisAutoFixed.TabStop = true;
            this.YAxisAutoFixed.Text = "Auto Scale";
            this.YAxisAutoFixed.UseVisualStyleBackColor = true;
            this.YAxisAutoFixed.CheckedChanged += new System.EventHandler(this.YAxisAutoFixed_CheckedChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.ApplyAllX);
            this.GroupBox1.Controls.Add(this.XAxisMax);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.XAxisMin);
            this.GroupBox1.Controls.Add(this.SpanLabel);
            this.GroupBox1.Controls.Add(this.XAxisTimeSpan);
            this.GroupBox1.Controls.Add(this.XAxisFixedMinMax);
            this.GroupBox1.Controls.Add(this.XAxisFixed);
            this.GroupBox1.Controls.Add(this.XAxisAuto);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(3, 5);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(329, 136);
            this.GroupBox1.TabIndex = 6;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "X Axis Configuration";
            // 
            // ApplyAllX
            // 
            this.ApplyAllX.AutoSize = true;
            this.ApplyAllX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApplyAllX.Location = new System.Drawing.Point(15, 114);
            this.ApplyAllX.Name = "ApplyAllX";
            this.ApplyAllX.Size = new System.Drawing.Size(215, 17);
            this.ApplyAllX.TabIndex = 10;
            this.ApplyAllX.Text = "Apply Changes to ALL Tabs And Panels";
            this.ApplyAllX.UseVisualStyleBackColor = true;
            this.ApplyAllX.CheckedChanged += new System.EventHandler(this.ApplyAllX_CheckedChanged);
            // 
            // XAxisMax
            // 
            this.XAxisMax.DecimalPlaces = 2;
            this.XAxisMax.Enabled = false;
            this.XAxisMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisMax.Location = new System.Drawing.Point(112, 91);
            this.XAxisMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.XAxisMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XAxisMax.Name = "XAxisMax";
            this.XAxisMax.Size = new System.Drawing.Size(83, 20);
            this.XAxisMax.TabIndex = 9;
            this.XAxisMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XAxisMax.ValueChanged += new System.EventHandler(this.XAxisChanged);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(36, 94);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(73, 13);
            this.Label3.TabIndex = 8;
            this.Label3.Text = "And Maximum";
            // 
            // XAxisMin
            // 
            this.XAxisMin.DecimalPlaces = 2;
            this.XAxisMin.Enabled = false;
            this.XAxisMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisMin.Location = new System.Drawing.Point(112, 65);
            this.XAxisMin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.XAxisMin.Name = "XAxisMin";
            this.XAxisMin.Size = new System.Drawing.Size(83, 20);
            this.XAxisMin.TabIndex = 7;
            this.XAxisMin.ValueChanged += new System.EventHandler(this.XAxisChanged);
            // 
            // SpanLabel
            // 
            this.SpanLabel.AutoSize = true;
            this.SpanLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpanLabel.Location = new System.Drawing.Point(200, 41);
            this.SpanLabel.Name = "SpanLabel";
            this.SpanLabel.Size = new System.Drawing.Size(79, 13);
            this.SpanLabel.TabIndex = 6;
            this.SpanLabel.Text = "Units From End";
            // 
            // XAxisTimeSpan
            // 
            this.XAxisTimeSpan.DecimalPlaces = 2;
            this.XAxisTimeSpan.Enabled = false;
            this.XAxisTimeSpan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisTimeSpan.Location = new System.Drawing.Point(112, 39);
            this.XAxisTimeSpan.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.XAxisTimeSpan.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XAxisTimeSpan.Name = "XAxisTimeSpan";
            this.XAxisTimeSpan.Size = new System.Drawing.Size(82, 20);
            this.XAxisTimeSpan.TabIndex = 5;
            this.XAxisTimeSpan.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.XAxisTimeSpan.ValueChanged += new System.EventHandler(this.XAxisChanged);
            // 
            // XAxisFixedMinMax
            // 
            this.XAxisFixedMinMax.AutoSize = true;
            this.XAxisFixedMinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisFixedMinMax.Location = new System.Drawing.Point(15, 64);
            this.XAxisFixedMinMax.Name = "XAxisFixedMinMax";
            this.XAxisFixedMinMax.Size = new System.Drawing.Size(94, 17);
            this.XAxisFixedMinMax.TabIndex = 3;
            this.XAxisFixedMinMax.Text = "Fixed Minimum";
            this.XAxisFixedMinMax.UseVisualStyleBackColor = true;
            this.XAxisFixedMinMax.CheckedChanged += new System.EventHandler(this.XAxisFixedMinMax_CheckedChanged);
            // 
            // XAxisFixed
            // 
            this.XAxisFixed.AutoSize = true;
            this.XAxisFixed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisFixed.Location = new System.Drawing.Point(15, 39);
            this.XAxisFixed.Name = "XAxisFixed";
            this.XAxisFixed.Size = new System.Drawing.Size(91, 17);
            this.XAxisFixed.TabIndex = 2;
            this.XAxisFixed.Text = "Fixed Span At";
            this.XAxisFixed.UseVisualStyleBackColor = true;
            this.XAxisFixed.CheckedChanged += new System.EventHandler(this.XAxisFixed_CheckedChanged);
            // 
            // XAxisAuto
            // 
            this.XAxisAuto.AutoSize = true;
            this.XAxisAuto.Checked = true;
            this.XAxisAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XAxisAuto.Location = new System.Drawing.Point(15, 18);
            this.XAxisAuto.Name = "XAxisAuto";
            this.XAxisAuto.Size = new System.Drawing.Size(47, 17);
            this.XAxisAuto.TabIndex = 1;
            this.XAxisAuto.TabStop = true;
            this.XAxisAuto.Text = "Auto";
            this.XAxisAuto.UseVisualStyleBackColor = true;
            this.XAxisAuto.CheckedChanged += new System.EventHandler(this.XAxisAuto_CheckedChanged);
            // 
            // AxisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(342, 314);
            this.ControlBox = false;
            this.Controls.Add(this.AxisCancelButton);
            this.Controls.Add(this.AxisApplyButton);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AxisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Axis Configuration";
            this.Load += new System.EventHandler(this.AxisForm_Load);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMin)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisTimeSpan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button AxisCancelButton;
        internal System.Windows.Forms.Button AxisApplyButton;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.CheckBox ApplyAllY;
        internal System.Windows.Forms.NumericUpDown YAxisMax;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.NumericUpDown YAxisMin;
        internal System.Windows.Forms.RadioButton YAXisFixedMinMax;
        internal System.Windows.Forms.RadioButton YAxisAutoFree;
        internal System.Windows.Forms.RadioButton YAxisAutoFixed;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.CheckBox ApplyAllX;
        internal System.Windows.Forms.NumericUpDown XAxisMax;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.NumericUpDown XAxisMin;
        internal System.Windows.Forms.Label SpanLabel;
        internal System.Windows.Forms.NumericUpDown XAxisTimeSpan;
        internal System.Windows.Forms.RadioButton XAxisFixedMinMax;
        internal System.Windows.Forms.RadioButton XAxisFixed;
        internal System.Windows.Forms.RadioButton XAxisAuto;
    }
}