namespace GraphLib
{
    partial class Legend
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
            this.Lscalelab = new System.Windows.Forms.Label();
            this.YMaster = new System.Windows.Forms.Label();
            this.LHLLab = new System.Windows.Forms.Label();
            this.Lnamelab = new System.Windows.Forms.Label();
            this.Llastslab = new System.Windows.Forms.Label();
            this.Lcollab = new System.Windows.Forms.Label();
            this.Lmaxlab = new System.Windows.Forms.Label();
            this.Lminlab = new System.Windows.Forms.Label();
            this.ColorDialog1 = new System.Windows.Forms.ColorDialog();
            this.LabSolid = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Lscalelab
            // 
            this.Lscalelab.AutoSize = true;
            this.Lscalelab.Location = new System.Drawing.Point(330, 4);
            this.Lscalelab.Name = "Lscalelab";
            this.Lscalelab.Size = new System.Drawing.Size(71, 13);
            this.Lscalelab.TabIndex = 17;
            this.Lscalelab.Text = "Display Scale";
            // 
            // YMaster
            // 
            this.YMaster.AutoSize = true;
            this.YMaster.Location = new System.Drawing.Point(34, 4);
            this.YMaster.Name = "YMaster";
            this.YMaster.Size = new System.Drawing.Size(50, 13);
            this.YMaster.TabIndex = 15;
            this.YMaster.Text = "Y Control";
            // 
            // LHLLab
            // 
            this.LHLLab.AutoSize = true;
            this.LHLLab.Location = new System.Drawing.Point(7, 4);
            this.LHLLab.Name = "LHLLab";
            this.LHLLab.Size = new System.Drawing.Size(21, 13);
            this.LHLLab.TabIndex = 14;
            this.LHLLab.Text = "HL";
            // 
            // Lnamelab
            // 
            this.Lnamelab.AutoSize = true;
            this.Lnamelab.Location = new System.Drawing.Point(441, 4);
            this.Lnamelab.Name = "Lnamelab";
            this.Lnamelab.Size = new System.Drawing.Size(35, 13);
            this.Lnamelab.TabIndex = 13;
            this.Lnamelab.Text = "Name";
            // 
            // Llastslab
            // 
            this.Llastslab.AutoSize = true;
            this.Llastslab.Location = new System.Drawing.Point(267, 4);
            this.Llastslab.Name = "Llastslab";
            this.Llastslab.Size = new System.Drawing.Size(57, 13);
            this.Llastslab.TabIndex = 12;
            this.Llastslab.Text = "Last Value";
            // 
            // Lcollab
            // 
            this.Lcollab.AutoSize = true;
            this.Lcollab.Location = new System.Drawing.Point(406, 4);
            this.Lcollab.Name = "Lcollab";
            this.Lcollab.Size = new System.Drawing.Size(37, 13);
            this.Lcollab.TabIndex = 11;
            this.Lcollab.Text = "Colour";
            // 
            // Lmaxlab
            // 
            this.Lmaxlab.AutoSize = true;
            this.Lmaxlab.Location = new System.Drawing.Point(198, 4);
            this.Lmaxlab.Name = "Lmaxlab";
            this.Lmaxlab.Size = new System.Drawing.Size(51, 13);
            this.Lmaxlab.TabIndex = 10;
            this.Lmaxlab.Text = "Maximum";
            // 
            // Lminlab
            // 
            this.Lminlab.AutoSize = true;
            this.Lminlab.Location = new System.Drawing.Point(131, 4);
            this.Lminlab.Name = "Lminlab";
            this.Lminlab.Size = new System.Drawing.Size(48, 13);
            this.Lminlab.TabIndex = 9;
            this.Lminlab.Text = "Minimum";
            // 
            // LabSolid
            // 
            this.LabSolid.AutoSize = true;
            this.LabSolid.Location = new System.Drawing.Point(91, 4);
            this.LabSolid.Name = "LabSolid";
            this.LabSolid.Size = new System.Drawing.Size(30, 13);
            this.LabSolid.TabIndex = 18;
            this.LabSolid.Text = "Solid";
            // 
            // Legend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Wheat;
            this.ClientSize = new System.Drawing.Size(778, 67);
            this.ControlBox = false;
            this.Controls.Add(this.LabSolid);
            this.Controls.Add(this.Lscalelab);
            this.Controls.Add(this.YMaster);
            this.Controls.Add(this.LHLLab);
            this.Controls.Add(this.Lnamelab);
            this.Controls.Add(this.Llastslab);
            this.Controls.Add(this.Lcollab);
            this.Controls.Add(this.Lmaxlab);
            this.Controls.Add(this.Lminlab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Legend";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "    ";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Legend_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Lscalelab;
        internal System.Windows.Forms.Label YMaster;
        internal System.Windows.Forms.Label LHLLab;
        internal System.Windows.Forms.Label Lnamelab;
        internal System.Windows.Forms.Label Llastslab;
        internal System.Windows.Forms.Label Lcollab;
        internal System.Windows.Forms.Label Lmaxlab;
        internal System.Windows.Forms.Label Lminlab;
        internal System.Windows.Forms.ColorDialog ColorDialog1;
        internal System.Windows.Forms.Label LabSolid;
    }
}