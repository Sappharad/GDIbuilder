namespace GDIbuilder
{
    partial class AdvancedDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.txtSystem = new System.Windows.Forms.TextBox();
            this.txtVolumeSet = new System.Windows.Forms.TextBox();
            this.txtPublisher = new System.Windows.Forms.TextBox();
            this.txtDataPrep = new System.Windows.Forms.TextBox();
            this.txtApplication = new System.Windows.Forms.TextBox();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.lblVolSet = new System.Windows.Forms.Label();
            this.lblPublisher = new System.Windows.Forms.Label();
            this.lblDataPrep = new System.Windows.Forms.Label();
            this.lblApplication = new System.Windows.Forms.Label();
            this.chkTruncateMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(705, 210);
            this.btnOK.Margin = new System.Windows.Forms.Padding(6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 46);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(867, 210);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 46);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtVolume
            // 
            this.txtVolume.Location = new System.Drawing.Point(192, 15);
            this.txtVolume.Margin = new System.Windows.Forms.Padding(6);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(296, 39);
            this.txtVolume.TabIndex = 2;
            // 
            // txtSystem
            // 
            this.txtSystem.Location = new System.Drawing.Point(192, 66);
            this.txtSystem.Margin = new System.Windows.Forms.Padding(6);
            this.txtSystem.Name = "txtSystem";
            this.txtSystem.Size = new System.Drawing.Size(296, 39);
            this.txtSystem.TabIndex = 3;
            // 
            // txtVolumeSet
            // 
            this.txtVolumeSet.Location = new System.Drawing.Point(192, 117);
            this.txtVolumeSet.Margin = new System.Windows.Forms.Padding(6);
            this.txtVolumeSet.Name = "txtVolumeSet";
            this.txtVolumeSet.Size = new System.Drawing.Size(296, 39);
            this.txtVolumeSet.TabIndex = 4;
            // 
            // txtPublisher
            // 
            this.txtPublisher.Location = new System.Drawing.Point(713, 15);
            this.txtPublisher.Margin = new System.Windows.Forms.Padding(6);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(296, 39);
            this.txtPublisher.TabIndex = 5;
            // 
            // txtDataPrep
            // 
            this.txtDataPrep.Location = new System.Drawing.Point(713, 66);
            this.txtDataPrep.Margin = new System.Windows.Forms.Padding(6);
            this.txtDataPrep.Name = "txtDataPrep";
            this.txtDataPrep.Size = new System.Drawing.Size(296, 39);
            this.txtDataPrep.TabIndex = 6;
            // 
            // txtApplication
            // 
            this.txtApplication.Location = new System.Drawing.Point(713, 117);
            this.txtApplication.Margin = new System.Windows.Forms.Padding(6);
            this.txtApplication.Name = "txtApplication";
            this.txtApplication.Size = new System.Drawing.Size(296, 39);
            this.txtApplication.TabIndex = 7;
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(50, 18);
            this.lblVolume.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(130, 32);
            this.lblVolume.TabIndex = 8;
            this.lblVolume.Text = "Volume ID:";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(55, 69);
            this.lblSystem.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(125, 32);
            this.lblSystem.TabIndex = 9;
            this.lblSystem.Text = "System ID:";
            // 
            // lblVolSet
            // 
            this.lblVolSet.AutoSize = true;
            this.lblVolSet.Location = new System.Drawing.Point(9, 120);
            this.lblVolSet.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblVolSet.Name = "lblVolSet";
            this.lblVolSet.Size = new System.Drawing.Size(171, 32);
            this.lblVolSet.TabIndex = 10;
            this.lblVolSet.Text = "Volume Set ID:";
            // 
            // lblPublisher
            // 
            this.lblPublisher.AutoSize = true;
            this.lblPublisher.Location = new System.Drawing.Point(554, 18);
            this.lblPublisher.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblPublisher.Name = "lblPublisher";
            this.lblPublisher.Size = new System.Drawing.Size(147, 32);
            this.lblPublisher.TabIndex = 11;
            this.lblPublisher.Text = "Publisher ID:";
            // 
            // lblDataPrep
            // 
            this.lblDataPrep.AutoSize = true;
            this.lblDataPrep.Location = new System.Drawing.Point(507, 69);
            this.lblDataPrep.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblDataPrep.Name = "lblDataPrep";
            this.lblDataPrep.Size = new System.Drawing.Size(194, 32);
            this.lblDataPrep.TabIndex = 12;
            this.lblDataPrep.Text = "Data Preparer ID:";
            // 
            // lblApplication
            // 
            this.lblApplication.AutoSize = true;
            this.lblApplication.Location = new System.Drawing.Point(532, 120);
            this.lblApplication.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblApplication.Name = "lblApplication";
            this.lblApplication.Size = new System.Drawing.Size(169, 32);
            this.lblApplication.TabIndex = 13;
            this.lblApplication.Text = "Application ID:";
            // 
            // chkTruncateMode
            // 
            this.chkTruncateMode.AutoSize = true;
            this.chkTruncateMode.Location = new System.Drawing.Point(192, 168);
            this.chkTruncateMode.Margin = new System.Windows.Forms.Padding(6);
            this.chkTruncateMode.Name = "chkTruncateMode";
            this.chkTruncateMode.Size = new System.Drawing.Size(375, 36);
            this.chkTruncateMode.TabIndex = 14;
            this.chkTruncateMode.Text = "Generate truncated track03.bin";
            this.chkTruncateMode.UseVisualStyleBackColor = true;
            // 
            // AdvancedDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1042, 303);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblApplication);
            this.Controls.Add(this.txtApplication);
            this.Controls.Add(this.lblDataPrep);
            this.Controls.Add(this.txtDataPrep);
            this.Controls.Add(this.lblPublisher);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.lblSystem);
            this.Controls.Add(this.lblVolSet);
            this.Controls.Add(this.chkTruncateMode);
            this.Controls.Add(this.txtVolumeSet);
            this.Controls.Add(this.txtSystem);
            this.Controls.Add(this.txtVolume);
            this.Controls.Add(this.txtPublisher);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AdvancedDialog";
            this.Text = "Advanced";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.TextBox txtSystem;
        private System.Windows.Forms.TextBox txtVolumeSet;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.TextBox txtDataPrep;
        private System.Windows.Forms.TextBox txtApplication;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.Label lblVolSet;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblDataPrep;
        private System.Windows.Forms.Label lblApplication;
        private System.Windows.Forms.CheckBox chkTruncateMode;
    }
}