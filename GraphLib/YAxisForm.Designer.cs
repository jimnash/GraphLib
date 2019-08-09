namespace GraphLib
{
    partial class YAxisForm
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
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.SetFixed = new System.Windows.Forms.Button();
            this.YAxisMax = new System.Windows.Forms.NumericUpDown();
            this.Label1 = new System.Windows.Forms.Label();
            this.YAxisMin = new System.Windows.Forms.NumericUpDown();
            this.YAXisFixedMinMax = new System.Windows.Forms.RadioButton();
            this.YAxisAutoFree = new System.Windows.Forms.RadioButton();
            this.YAxisAutoFixed = new System.Windows.Forms.RadioButton();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMin)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Location = new System.Drawing.Point(12, 176);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(64, 23);
            this.OK.TabIndex = 14;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(157, 176);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(64, 23);
            this.Cancel.TabIndex = 13;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.SetFixed);
            this.GroupBox2.Controls.Add(this.YAxisMax);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Controls.Add(this.YAxisMin);
            this.GroupBox2.Controls.Add(this.YAXisFixedMinMax);
            this.GroupBox2.Controls.Add(this.YAxisAutoFree);
            this.GroupBox2.Controls.Add(this.YAxisAutoFixed);
            this.GroupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(2, 12);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(229, 136);
            this.GroupBox2.TabIndex = 11;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Y Axis Configuration";
            // 
            // SetFixed
            // 
            this.SetFixed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetFixed.Location = new System.Drawing.Point(48, 107);
            this.SetFixed.Name = "SetFixed";
            this.SetFixed.Size = new System.Drawing.Size(131, 23);
            this.SetFixed.TabIndex = 6;
            this.SetFixed.Text = "Set Fixed at +/- 10%";
            this.SetFixed.UseVisualStyleBackColor = true;
            this.SetFixed.Click += new System.EventHandler(this.SetFixed_Click);
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
            this.YAxisMax.Location = new System.Drawing.Point(125, 51);
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
            this.Label1.Location = new System.Drawing.Point(35, 80);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(70, 13);
            this.Label1.TabIndex = 4;
            this.Label1.Text = "And Minimum";
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
            this.YAxisMin.Location = new System.Drawing.Point(125, 78);
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
            this.YAXisFixedMinMax.Location = new System.Drawing.Point(14, 54);
            this.YAXisFixedMinMax.Name = "YAXisFixedMinMax";
            this.YAXisFixedMinMax.Size = new System.Drawing.Size(97, 17);
            this.YAXisFixedMinMax.TabIndex = 2;
            this.YAXisFixedMinMax.Text = "Fixed Maximum";
            this.YAXisFixedMinMax.UseVisualStyleBackColor = true;
            this.YAXisFixedMinMax.CheckedChanged += new System.EventHandler(this.YAXisFixedMinMax_CheckedChanged);
            // 
            // YAxisAutoFree
            // 
            this.YAxisAutoFree.AutoSize = true;
            this.YAxisAutoFree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YAxisAutoFree.Location = new System.Drawing.Point(108, 31);
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
            this.YAxisAutoFixed.Location = new System.Drawing.Point(14, 31);
            this.YAxisAutoFixed.Name = "YAxisAutoFixed";
            this.YAxisAutoFixed.Size = new System.Drawing.Size(77, 17);
            this.YAxisAutoFixed.TabIndex = 0;
            this.YAxisAutoFixed.TabStop = true;
            this.YAxisAutoFixed.Text = "Auto Scale";
            this.YAxisAutoFixed.UseVisualStyleBackColor = true;
            this.YAxisAutoFixed.CheckedChanged += new System.EventHandler(this.YAxisAutoFixed_CheckedChanged);
            // 
            // YAxisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(233, 207);
            this.ControlBox = false;
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.GroupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YAxisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Y Axis Panel Configuration";
            this.Load += new System.EventHandler(this.YAxisForm_Load);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YAxisMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button OK;
        internal System.Windows.Forms.Button Cancel;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Button SetFixed;
        internal System.Windows.Forms.NumericUpDown YAxisMax;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.NumericUpDown YAxisMin;
        internal System.Windows.Forms.RadioButton YAXisFixedMinMax;
        internal System.Windows.Forms.RadioButton YAxisAutoFree;
        internal System.Windows.Forms.RadioButton YAxisAutoFixed;
    }
}