namespace LoopingAudioConverter {
	partial class MP3QualityForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.radVBR = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.ddlVBRSetting = new System.Windows.Forms.ComboBox();
			this.radCBR = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.numBitrate = new System.Windows.Forms.NumericUpDown();
			this.radCustom = new System.Windows.Forms.RadioButton();
			this.txtParameters = new System.Windows.Forms.TextBox();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numBitrate)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select an encoding type:";
			// 
			// radVBR
			// 
			this.radVBR.AutoSize = true;
			this.radVBR.Checked = true;
			this.radVBR.Location = new System.Drawing.Point(12, 28);
			this.radVBR.Name = "radVBR";
			this.radVBR.Size = new System.Drawing.Size(129, 17);
			this.radVBR.TabIndex = 1;
			this.radVBR.TabStop = true;
			this.radVBR.Text = "Variable bit rate (VBR)";
			this.radVBR.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(31, 54);
			this.label2.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Quality:";
			// 
			// ddlVBRSetting
			// 
			this.ddlVBRSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlVBRSetting.FormattingEnabled = true;
			this.ddlVBRSetting.Location = new System.Drawing.Point(79, 51);
			this.ddlVBRSetting.Name = "ddlVBRSetting";
			this.ddlVBRSetting.Size = new System.Drawing.Size(193, 21);
			this.ddlVBRSetting.TabIndex = 1;
			// 
			// radCBR
			// 
			this.radCBR.AutoSize = true;
			this.radCBR.Location = new System.Drawing.Point(12, 78);
			this.radCBR.Name = "radCBR";
			this.radCBR.Size = new System.Drawing.Size(133, 17);
			this.radCBR.TabIndex = 2;
			this.radCBR.Text = "Constant bit rate (CBR)";
			this.radCBR.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(31, 103);
			this.label3.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Bitrate (kbps):";
			// 
			// numBitrate
			// 
			this.numBitrate.Location = new System.Drawing.Point(109, 101);
			this.numBitrate.Maximum = new decimal(new int[] {
			320,
			0,
			0,
			0});
			this.numBitrate.Minimum = new decimal(new int[] {
			8,
			0,
			0,
			0});
			this.numBitrate.Name = "numBitrate";
			this.numBitrate.Size = new System.Drawing.Size(64, 20);
			this.numBitrate.TabIndex = 4;
			this.numBitrate.Value = new decimal(new int[] {
			320,
			0,
			0,
			0});
			// 
			// radCustom
			// 
			this.radCustom.AutoSize = true;
			this.radCustom.Location = new System.Drawing.Point(12, 127);
			this.radCustom.Name = "radCustom";
			this.radCustom.Size = new System.Drawing.Size(115, 17);
			this.radCustom.TabIndex = 5;
			this.radCustom.Text = "Custom parameters";
			this.radCustom.UseVisualStyleBackColor = true;
			// 
			// txtParameters
			// 
			this.txtParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtParameters.Location = new System.Drawing.Point(34, 150);
			this.txtParameters.Name = "txtParameters";
			this.txtParameters.Size = new System.Drawing.Size(238, 20);
			this.txtParameters.TabIndex = 6;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(116, 226);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 7;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(197, 226);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// MP3QualityForm
			// 
			this.AcceptButton = this.btnOkay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.txtParameters);
			this.Controls.Add(this.radCustom);
			this.Controls.Add(this.numBitrate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.radCBR);
			this.Controls.Add(this.ddlVBRSetting);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.radVBR);
			this.Controls.Add(this.label1);
			this.Name = "MP3QualityForm";
			this.Text = "MP3 Quality";
			((System.ComponentModel.ISupportInitialize)(this.numBitrate)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radVBR;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox ddlVBRSetting;
		private System.Windows.Forms.RadioButton radCBR;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numBitrate;
		private System.Windows.Forms.RadioButton radCustom;
		private System.Windows.Forms.TextBox txtParameters;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
	}
}