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
            txtData = new System.Windows.Forms.TextBox();
            lblDataDir = new System.Windows.Forms.Label();
            btnSelectData = new System.Windows.Forms.Button();
            txtIpBin = new System.Windows.Forms.TextBox();
            lblIpBin = new System.Windows.Forms.Label();
            btnSelectIP = new System.Windows.Forms.Button();
            lstCdda = new System.Windows.Forms.ListBox();
            lblCdda = new System.Windows.Forms.Label();
            btnAddCdda = new System.Windows.Forms.Button();
            txtOutdir = new System.Windows.Forms.TextBox();
            lblOutDir = new System.Windows.Forms.Label();
            btnSelOutput = new System.Windows.Forms.Button();
            btnMake = new System.Windows.Forms.Button();
            pbProgress = new System.Windows.Forms.ProgressBar();
            btnRemoveCdda = new System.Windows.Forms.Button();
            btnMoveCddaUp = new System.Windows.Forms.Button();
            btnMoveCddaDown = new System.Windows.Forms.Button();
            btnAdvanced = new System.Windows.Forms.Button();
            chkRawMode = new System.Windows.Forms.CheckBox();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // txtData
            // 
            txtData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtData.Location = new System.Drawing.Point(84, 15);
            txtData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtData.Name = "txtData";
            txtData.ReadOnly = true;
            txtData.Size = new System.Drawing.Size(326, 23);
            txtData.TabIndex = 0;
            // 
            // lblDataDir
            // 
            lblDataDir.AutoSize = true;
            lblDataDir.Location = new System.Drawing.Point(14, 18);
            lblDataDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDataDir.Name = "lblDataDir";
            lblDataDir.Size = new System.Drawing.Size(58, 15);
            lblDataDir.TabIndex = 1;
            lblDataDir.Text = "Data files:";
            // 
            // btnSelectData
            // 
            btnSelectData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSelectData.Location = new System.Drawing.Point(423, 14);
            btnSelectData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSelectData.Name = "btnSelectData";
            btnSelectData.Size = new System.Drawing.Size(88, 27);
            btnSelectData.TabIndex = 2;
            btnSelectData.Text = "Browse...";
            btnSelectData.UseVisualStyleBackColor = true;
            btnSelectData.Click += btnSelectData_Click;
            // 
            // txtIpBin
            // 
            txtIpBin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtIpBin.Location = new System.Drawing.Point(84, 46);
            txtIpBin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtIpBin.Name = "txtIpBin";
            txtIpBin.ReadOnly = true;
            txtIpBin.Size = new System.Drawing.Size(326, 23);
            txtIpBin.TabIndex = 3;
            // 
            // lblIpBin
            // 
            lblIpBin.AutoSize = true;
            lblIpBin.Location = new System.Drawing.Point(29, 50);
            lblIpBin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblIpBin.Name = "lblIpBin";
            lblIpBin.Size = new System.Drawing.Size(42, 15);
            lblIpBin.TabIndex = 4;
            lblIpBin.Text = "IP.BIN:";
            // 
            // btnSelectIP
            // 
            btnSelectIP.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSelectIP.Location = new System.Drawing.Point(423, 45);
            btnSelectIP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSelectIP.Name = "btnSelectIP";
            btnSelectIP.Size = new System.Drawing.Size(88, 27);
            btnSelectIP.TabIndex = 5;
            btnSelectIP.Text = "Browse...";
            btnSelectIP.UseVisualStyleBackColor = true;
            btnSelectIP.Click += btnSelectIP_Click;
            // 
            // lstCdda
            // 
            lstCdda.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lstCdda.FormattingEnabled = true;
            lstCdda.IntegralHeight = false;
            lstCdda.ItemHeight = 15;
            lstCdda.Location = new System.Drawing.Point(84, 77);
            lstCdda.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstCdda.Name = "lstCdda";
            lstCdda.Size = new System.Drawing.Size(326, 109);
            lstCdda.TabIndex = 6;
            // 
            // lblCdda
            // 
            lblCdda.AutoSize = true;
            lblCdda.Location = new System.Drawing.Point(30, 77);
            lblCdda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCdda.Name = "lblCdda";
            lblCdda.Size = new System.Drawing.Size(42, 15);
            lblCdda.TabIndex = 7;
            lblCdda.Text = "CDDA:";
            // 
            // btnAddCdda
            // 
            btnAddCdda.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAddCdda.Location = new System.Drawing.Point(423, 78);
            btnAddCdda.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAddCdda.Name = "btnAddCdda";
            btnAddCdda.Size = new System.Drawing.Size(88, 27);
            btnAddCdda.TabIndex = 8;
            btnAddCdda.Text = "Add...";
            btnAddCdda.UseVisualStyleBackColor = true;
            btnAddCdda.Click += btnSelCdda_Click;
            // 
            // txtOutdir
            // 
            txtOutdir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtOutdir.Location = new System.Drawing.Point(84, 192);
            txtOutdir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtOutdir.Name = "txtOutdir";
            txtOutdir.ReadOnly = true;
            txtOutdir.Size = new System.Drawing.Size(326, 23);
            txtOutdir.TabIndex = 9;
            // 
            // lblOutDir
            // 
            lblOutDir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblOutDir.AutoSize = true;
            lblOutDir.Location = new System.Drawing.Point(12, 195);
            lblOutDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutDir.Name = "lblOutDir";
            lblOutDir.Size = new System.Drawing.Size(65, 15);
            lblOutDir.TabIndex = 10;
            lblOutDir.Text = "Output dir:";
            // 
            // btnSelOutput
            // 
            btnSelOutput.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnSelOutput.Location = new System.Drawing.Point(423, 191);
            btnSelOutput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSelOutput.Name = "btnSelOutput";
            btnSelOutput.Size = new System.Drawing.Size(88, 27);
            btnSelOutput.TabIndex = 11;
            btnSelOutput.Text = "Browse...";
            btnSelOutput.UseVisualStyleBackColor = true;
            btnSelOutput.Click += btnSelOutput_Click;
            // 
            // btnMake
            // 
            btnMake.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnMake.Location = new System.Drawing.Point(379, 252);
            btnMake.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnMake.Name = "btnMake";
            btnMake.Size = new System.Drawing.Size(132, 27);
            btnMake.TabIndex = 12;
            btnMake.Text = "Create GD-ROM";
            btnMake.UseVisualStyleBackColor = true;
            btnMake.Click += btnMake_Click;
            // 
            // pbProgress
            // 
            pbProgress.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pbProgress.Location = new System.Drawing.Point(14, 286);
            pbProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pbProgress.Name = "pbProgress";
            pbProgress.Size = new System.Drawing.Size(415, 21);
            pbProgress.TabIndex = 13;
            // 
            // btnRemoveCdda
            // 
            btnRemoveCdda.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRemoveCdda.Location = new System.Drawing.Point(423, 112);
            btnRemoveCdda.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRemoveCdda.Name = "btnRemoveCdda";
            btnRemoveCdda.Size = new System.Drawing.Size(88, 27);
            btnRemoveCdda.TabIndex = 15;
            btnRemoveCdda.Text = "Remove";
            btnRemoveCdda.UseVisualStyleBackColor = true;
            btnRemoveCdda.Click += btnRemoveCdda_Click;
            // 
            // btnMoveCddaUp
            // 
            btnMoveCddaUp.Location = new System.Drawing.Point(24, 96);
            btnMoveCddaUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnMoveCddaUp.Name = "btnMoveCddaUp";
            btnMoveCddaUp.Size = new System.Drawing.Size(52, 27);
            btnMoveCddaUp.TabIndex = 16;
            btnMoveCddaUp.Text = "Up";
            btnMoveCddaUp.UseVisualStyleBackColor = true;
            btnMoveCddaUp.Click += btnMoveCddaUp_Click;
            // 
            // btnMoveCddaDown
            // 
            btnMoveCddaDown.Location = new System.Drawing.Point(24, 129);
            btnMoveCddaDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnMoveCddaDown.Name = "btnMoveCddaDown";
            btnMoveCddaDown.Size = new System.Drawing.Size(52, 27);
            btnMoveCddaDown.TabIndex = 17;
            btnMoveCddaDown.Text = "Down";
            btnMoveCddaDown.UseVisualStyleBackColor = true;
            btnMoveCddaDown.Click += btnMoveCddaDown_Click;
            // 
            // btnAdvanced
            // 
            btnAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnAdvanced.Location = new System.Drawing.Point(84, 222);
            btnAdvanced.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnAdvanced.Name = "btnAdvanced";
            btnAdvanced.Size = new System.Drawing.Size(158, 27);
            btnAdvanced.TabIndex = 19;
            btnAdvanced.Text = "Advanced Options...";
            btnAdvanced.UseVisualStyleBackColor = true;
            btnAdvanced.Click += btnAdvanced_Click;
            // 
            // chkRawMode
            // 
            chkRawMode.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            chkRawMode.AutoSize = true;
            chkRawMode.Checked = true;
            chkRawMode.CheckState = System.Windows.Forms.CheckState.Checked;
            chkRawMode.Location = new System.Drawing.Point(248, 227);
            chkRawMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chkRawMode.Name = "chkRawMode";
            chkRawMode.Size = new System.Drawing.Size(195, 19);
            chkRawMode.TabIndex = 20;
            chkRawMode.Text = "Output raw sectors (2352 mode)";
            chkRawMode.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCancel.Enabled = false;
            btnCancel.Location = new System.Drawing.Point(437, 285);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 21;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // GDIBuilderForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(524, 321);
            Controls.Add(btnCancel);
            Controls.Add(chkRawMode);
            Controls.Add(btnAdvanced);
            Controls.Add(btnMoveCddaDown);
            Controls.Add(btnMoveCddaUp);
            Controls.Add(btnRemoveCdda);
            Controls.Add(pbProgress);
            Controls.Add(btnMake);
            Controls.Add(btnSelOutput);
            Controls.Add(lblOutDir);
            Controls.Add(txtOutdir);
            Controls.Add(btnAddCdda);
            Controls.Add(lblCdda);
            Controls.Add(lstCdda);
            Controls.Add(btnSelectIP);
            Controls.Add(lblIpBin);
            Controls.Add(txtIpBin);
            Controls.Add(btnSelectData);
            Controls.Add(lblDataDir);
            Controls.Add(txtData);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(540, 360);
            Name = "GDIBuilderForm";
            Text = "Build GDI tracks";
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button btnCancel;
    }
}

