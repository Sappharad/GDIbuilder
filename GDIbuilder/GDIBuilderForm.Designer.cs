namespace GDIbuilder
{
    partial class GDIBuilderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GDIBuilderForm));
            this.txtData = new System.Windows.Forms.TextBox();
            this.lblDataDir = new System.Windows.Forms.Label();
            this.btnSelectData = new System.Windows.Forms.Button();
            this.txtIpBin = new System.Windows.Forms.TextBox();
            this.lblIpBin = new System.Windows.Forms.Label();
            this.btnSelectIP = new System.Windows.Forms.Button();
            this.lstCdda = new System.Windows.Forms.ListBox();
            this.lblCdda = new System.Windows.Forms.Label();
            this.btnAddCdda = new System.Windows.Forms.Button();
            this.txtOutdir = new System.Windows.Forms.TextBox();
            this.lblOutDir = new System.Windows.Forms.Label();
            this.btnSelOutput = new System.Windows.Forms.Button();
            this.btnMake = new System.Windows.Forms.Button();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnRemoveCdda = new System.Windows.Forms.Button();
            this.btnMoveCddaUp = new System.Windows.Forms.Button();
            this.btnMoveCddaDown = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.chkRawMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtData.Location = new System.Drawing.Point(72, 13);
            this.txtData.Name = "txtData";
            this.txtData.ReadOnly = true;
            this.txtData.Size = new System.Drawing.Size(283, 20);
            this.txtData.TabIndex = 0;
            // 
            // lblDataDir
            // 
            this.lblDataDir.AutoSize = true;
            this.lblDataDir.Location = new System.Drawing.Point(12, 16);
            this.lblDataDir.Name = "lblDataDir";
            this.lblDataDir.Size = new System.Drawing.Size(54, 13);
            this.lblDataDir.TabIndex = 1;
            this.lblDataDir.Text = "Data files:";
            // 
            // btnSelectData
            // 
            this.btnSelectData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectData.Location = new System.Drawing.Point(366, 12);
            this.btnSelectData.Name = "btnSelectData";
            this.btnSelectData.Size = new System.Drawing.Size(75, 23);
            this.btnSelectData.TabIndex = 2;
            this.btnSelectData.Text = "Browse...";
            this.btnSelectData.UseVisualStyleBackColor = true;
            this.btnSelectData.Click += new System.EventHandler(this.btnSelectData_Click);
            // 
            // txtIpBin
            // 
            this.txtIpBin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIpBin.Location = new System.Drawing.Point(72, 40);
            this.txtIpBin.Name = "txtIpBin";
            this.txtIpBin.ReadOnly = true;
            this.txtIpBin.Size = new System.Drawing.Size(283, 20);
            this.txtIpBin.TabIndex = 3;
            // 
            // lblIpBin
            // 
            this.lblIpBin.AutoSize = true;
            this.lblIpBin.Location = new System.Drawing.Point(25, 43);
            this.lblIpBin.Name = "lblIpBin";
            this.lblIpBin.Size = new System.Drawing.Size(41, 13);
            this.lblIpBin.TabIndex = 4;
            this.lblIpBin.Text = "IP.BIN:";
            // 
            // btnSelectIP
            // 
            this.btnSelectIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectIP.Location = new System.Drawing.Point(366, 39);
            this.btnSelectIP.Name = "btnSelectIP";
            this.btnSelectIP.Size = new System.Drawing.Size(75, 23);
            this.btnSelectIP.TabIndex = 5;
            this.btnSelectIP.Text = "Browse...";
            this.btnSelectIP.UseVisualStyleBackColor = true;
            this.btnSelectIP.Click += new System.EventHandler(this.btnSelectIP_Click);
            // 
            // lstCdda
            // 
            this.lstCdda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCdda.FormattingEnabled = true;
            this.lstCdda.Location = new System.Drawing.Point(72, 67);
            this.lstCdda.Name = "lstCdda";
            this.lstCdda.Size = new System.Drawing.Size(283, 95);
            this.lstCdda.TabIndex = 6;
            // 
            // lblCdda
            // 
            this.lblCdda.AutoSize = true;
            this.lblCdda.Location = new System.Drawing.Point(26, 67);
            this.lblCdda.Name = "lblCdda";
            this.lblCdda.Size = new System.Drawing.Size(40, 13);
            this.lblCdda.TabIndex = 7;
            this.lblCdda.Text = "CDDA:";
            // 
            // btnAddCdda
            // 
            this.btnAddCdda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCdda.Location = new System.Drawing.Point(366, 68);
            this.btnAddCdda.Name = "btnAddCdda";
            this.btnAddCdda.Size = new System.Drawing.Size(75, 23);
            this.btnAddCdda.TabIndex = 8;
            this.btnAddCdda.Text = "Add...";
            this.btnAddCdda.UseVisualStyleBackColor = true;
            this.btnAddCdda.Click += new System.EventHandler(this.btnSelCdda_Click);
            // 
            // txtOutdir
            // 
            this.txtOutdir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutdir.Location = new System.Drawing.Point(72, 169);
            this.txtOutdir.Name = "txtOutdir";
            this.txtOutdir.ReadOnly = true;
            this.txtOutdir.Size = new System.Drawing.Size(283, 20);
            this.txtOutdir.TabIndex = 9;
            // 
            // lblOutDir
            // 
            this.lblOutDir.AutoSize = true;
            this.lblOutDir.Location = new System.Drawing.Point(10, 172);
            this.lblOutDir.Name = "lblOutDir";
            this.lblOutDir.Size = new System.Drawing.Size(56, 13);
            this.lblOutDir.TabIndex = 10;
            this.lblOutDir.Text = "Output dir:";
            // 
            // btnSelOutput
            // 
            this.btnSelOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelOutput.Location = new System.Drawing.Point(366, 168);
            this.btnSelOutput.Name = "btnSelOutput";
            this.btnSelOutput.Size = new System.Drawing.Size(75, 23);
            this.btnSelOutput.TabIndex = 11;
            this.btnSelOutput.Text = "Browse...";
            this.btnSelOutput.UseVisualStyleBackColor = true;
            this.btnSelOutput.Click += new System.EventHandler(this.btnSelOutput_Click);
            // 
            // btnMake
            // 
            this.btnMake.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMake.Location = new System.Drawing.Point(329, 233);
            this.btnMake.Name = "btnMake";
            this.btnMake.Size = new System.Drawing.Size(113, 23);
            this.btnMake.TabIndex = 12;
            this.btnMake.Text = "Create GD-ROM";
            this.btnMake.UseVisualStyleBackColor = true;
            this.btnMake.Click += new System.EventHandler(this.btnMake_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(12, 236);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(310, 18);
            this.pbProgress.TabIndex = 13;
            // 
            // btnRemoveCdda
            // 
            this.btnRemoveCdda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveCdda.Location = new System.Drawing.Point(366, 97);
            this.btnRemoveCdda.Name = "btnRemoveCdda";
            this.btnRemoveCdda.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveCdda.TabIndex = 15;
            this.btnRemoveCdda.Text = "Remove";
            this.btnRemoveCdda.UseVisualStyleBackColor = true;
            this.btnRemoveCdda.Click += new System.EventHandler(this.btnRemoveCdda_Click);
            // 
            // btnMoveCddaUp
            // 
            this.btnMoveCddaUp.Location = new System.Drawing.Point(21, 83);
            this.btnMoveCddaUp.Name = "btnMoveCddaUp";
            this.btnMoveCddaUp.Size = new System.Drawing.Size(45, 23);
            this.btnMoveCddaUp.TabIndex = 16;
            this.btnMoveCddaUp.Text = "Up";
            this.btnMoveCddaUp.UseVisualStyleBackColor = true;
            this.btnMoveCddaUp.Click += new System.EventHandler(this.btnMoveCddaUp_Click);
            // 
            // btnMoveCddaDown
            // 
            this.btnMoveCddaDown.Location = new System.Drawing.Point(21, 112);
            this.btnMoveCddaDown.Name = "btnMoveCddaDown";
            this.btnMoveCddaDown.Size = new System.Drawing.Size(45, 23);
            this.btnMoveCddaDown.TabIndex = 17;
            this.btnMoveCddaDown.Text = "Down";
            this.btnMoveCddaDown.UseVisualStyleBackColor = true;
            this.btnMoveCddaDown.Click += new System.EventHandler(this.btnMoveCddaDown_Click);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(72, 195);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(135, 23);
            this.btnAdvanced.TabIndex = 19;
            this.btnAdvanced.Text = "Advanced Options...";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // chkRawMode
            // 
            this.chkRawMode.AutoSize = true;
            this.chkRawMode.Checked = true;
            this.chkRawMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRawMode.Location = new System.Drawing.Point(213, 199);
            this.chkRawMode.Name = "chkRawMode";
            this.chkRawMode.Size = new System.Drawing.Size(177, 17);
            this.chkRawMode.TabIndex = 20;
            this.chkRawMode.Text = "Output raw sectors (2352 mode)";
            this.chkRawMode.UseVisualStyleBackColor = true;
            // 
            // GDIBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 266);
            this.Controls.Add(this.chkRawMode);
            this.Controls.Add(this.btnAdvanced);
            this.Controls.Add(this.btnMoveCddaDown);
            this.Controls.Add(this.btnMoveCddaUp);
            this.Controls.Add(this.btnRemoveCdda);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnMake);
            this.Controls.Add(this.btnSelOutput);
            this.Controls.Add(this.lblOutDir);
            this.Controls.Add(this.txtOutdir);
            this.Controls.Add(this.btnAddCdda);
            this.Controls.Add(this.lblCdda);
            this.Controls.Add(this.lstCdda);
            this.Controls.Add(this.btnSelectIP);
            this.Controls.Add(this.lblIpBin);
            this.Controls.Add(this.txtIpBin);
            this.Controls.Add(this.btnSelectData);
            this.Controls.Add(this.lblDataDir);
            this.Controls.Add(this.txtData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GDIBuilderForm";
            this.Text = "Build GDI tracks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label lblDataDir;
        private System.Windows.Forms.Button btnSelectData;
        private System.Windows.Forms.TextBox txtIpBin;
        private System.Windows.Forms.Label lblIpBin;
        private System.Windows.Forms.Button btnSelectIP;
        private System.Windows.Forms.ListBox lstCdda;
        private System.Windows.Forms.Label lblCdda;
        private System.Windows.Forms.Button btnAddCdda;
        private System.Windows.Forms.TextBox txtOutdir;
        private System.Windows.Forms.Label lblOutDir;
        private System.Windows.Forms.Button btnSelOutput;
        private System.Windows.Forms.Button btnMake;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnRemoveCdda;
        private System.Windows.Forms.Button btnMoveCddaUp;
        private System.Windows.Forms.Button btnMoveCddaDown;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.CheckBox chkRawMode;
    }
}

