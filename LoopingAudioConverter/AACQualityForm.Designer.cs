namespace LoopingAudioConverter {
	partial class AACQualityForm {
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
			this.radTVBR = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.ddlTVBRSetting = new System.Windows.Forms.ComboBox();
			this.radCBR = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.numCBRBitrate = new System.Windows.Forms.NumericUpDown();
			this.radCustom = new System.Windows.Forms.RadioButton();
			this.txtParameters = new System.Windows.Forms.TextBox();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.radCVBR = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.numCVBRBitrate = new System.Windows.Forms.NumericUpDown();
			this.radABR = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.numABRBitrate = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numCBRBitrate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numCVBRBitrate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numABRBitrate)).BeginInit();
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
			// radTVBR
			// 
			this.radTVBR.AutoSize = true;
			this.radTVBR.Checked = true;
			this.radTVBR.Location = new System.Drawing.Point(12, 28);
			this.radTVBR.Name = "radTVBR";
			this.radTVBR.Size = new System.Drawing.Size(160, 17);
			this.radTVBR.TabIndex = 1;
			this.radTVBR.TabStop = true;
			this.radTVBR.Text = "True variable bit rate (TVBR)";
			this.radTVBR.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(30, 54);
			this.label2.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Quality:";
			// 
			// ddlTVBRSetting
			// 
			this.ddlTVBRSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.ddlTVBRSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlTVBRSetting.FormattingEnabled = true;
			this.ddlTVBRSetting.Items.AddRange(new object[] {
			"0",
			"9",
			"18",
			"27",
			"36",
			"45",
			"54",
			"63",
			"73",
			"82",
			"91",
			"100",
			"109",
			"118",
			"127"});
			this.ddlTVBRSetting.Location = new System.Drawing.Point(78, 51);
			this.ddlTVBRSetting.Name = "ddlTVBRSetting";
			this.ddlTVBRSetting.Size = new System.Drawing.Size(194, 21);
			this.ddlTVBRSetting.TabIndex = 3;
			// 
			// radCBR
			// 
			this.radCBR.AutoSize = true;
			this.radCBR.Location = new System.Drawing.Point(11, 176);
			this.radCBR.Name = "radCBR";
			this.radCBR.Size = new System.Drawing.Size(133, 17);
			this.radCBR.TabIndex = 10;
			this.radCBR.Text = "Constant bit rate (CBR)";
			this.radCBR.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(30, 201);
			this.label3.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(157, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Bitrate (kbps) or bits per sample:";
			// 
			// numCBRBitrate
			// 
			this.numCBRBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numCBRBitrate.Location = new System.Drawing.Point(208, 199);
			this.numCBRBitrate.Maximum = new decimal(new int[] {
			320,
			0,
			0,
			0});
			this.numCBRBitrate.Name = "numCBRBitrate";
			this.numCBRBitrate.Size = new System.Drawing.Size(64, 20);
			this.numCBRBitrate.TabIndex = 12;
			this.numCBRBitrate.Value = new decimal(new int[] {
			320,
			0,
			0,
			0});
			// 
			// radCustom
			// 
			this.radCustom.AutoSize = true;
			this.radCustom.Location = new System.Drawing.Point(11, 225);
			this.radCustom.Name = "radCustom";
			this.radCustom.Size = new System.Drawing.Size(115, 17);
			this.radCustom.TabIndex = 13;
			this.radCustom.Text = "Custom parameters";
			this.radCustom.UseVisualStyleBackColor = true;
			// 
			// txtParameters
			// 
			this.txtParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtParameters.Location = new System.Drawing.Point(33, 248);
			this.txtParameters.Name = "txtParameters";
			this.txtParameters.Size = new System.Drawing.Size(239, 20);
			this.txtParameters.TabIndex = 14;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(116, 276);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(75, 23);
			this.btnOkay.TabIndex = 15;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(197, 276);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// radCVBR
			// 
			this.radCVBR.AutoSize = true;
			this.radCVBR.Location = new System.Drawing.Point(11, 78);
			this.radCVBR.Name = "radCVBR";
			this.radCVBR.Size = new System.Drawing.Size(194, 17);
			this.radCVBR.TabIndex = 4;
			this.radCVBR.Text = "Constrained variable bit rate (CVBR)";
			this.radCVBR.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(30, 103);
			this.label4.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(157, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Bitrate (kbps) or bits per sample:";
			// 
			// numCVBRBitrate
			// 
			this.numCVBRBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numCVBRBitrate.Location = new System.Drawing.Point(208, 101);
			this.numCVBRBitrate.Maximum = new decimal(new int[] {
			320,
			0,
			0,
			0});
			this.numCVBRBitrate.Name = "numCVBRBitrate";
			this.numCVBRBitrate.Size = new System.Drawing.Size(64, 20);
			this.numCVBRBitrate.TabIndex = 6;
			this.numCVBRBitrate.Value = new decimal(new int[] {
			320,
			0,
			0,
			0});
			// 
			// radABR
			// 
			this.radABR.AutoSize = true;
			this.radABR.Location = new System.Drawing.Point(11, 127);
			this.radABR.Name = "radABR";
			this.radABR.Size = new System.Drawing.Size(131, 17);
			this.radABR.TabIndex = 7;
			this.radABR.Text = "Average bit rate (ABR)";
			this.radABR.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(30, 152);
			this.label5.Margin = new System.Windows.Forms.Padding(22, 3, 3, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(157, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Bitrate (kbps) or bits per sample:";
			// 
			// numABRBitrate
			// 
			this.numABRBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numABRBitrate.Location = new System.Drawing.Point(208, 150);
			this.numABRBitrate.Maximum = new decimal(new int[] {
			320,
			0,
			0,
			0});
			this.numABRBitrate.Name = "numABRBitrate";
			this.numABRBitrate.Size = new System.Drawing.Size(64, 20);
			this.numABRBitrate.TabIndex = 9;
			this.numABRBitrate.Value = new decimal(new int[] {
			320,
			0,
			0,
			0});
			// 
			// AACQualityForm
			// 
			this.AcceptButton = this.btnOkay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(284, 311);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOkay);
			this.Controls.Add(this.txtParameters);
			this.Controls.Add(this.radCustom);
			this.Controls.Add(this.numCVBRBitrate);
			this.Controls.Add(this.numABRBitrate);
			this.Controls.Add(this.numCBRBitrate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.radCVBR);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.radABR);
			this.Controls.Add(this.radCBR);
			this.Controls.Add(this.ddlTVBRSetting);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.radTVBR);
			this.Controls.Add(this.label1);
			this.Name = "AACQualityForm";
			this.Text = "AAC Quality";
			((System.ComponentModel.ISupportInitialize)(this.numCBRBitrate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numCVBRBitrate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numABRBitrate)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radTVBR;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox ddlTVBRSetting;
		private System.Windows.Forms.RadioButton radCBR;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numCBRBitrate;
		private System.Windows.Forms.RadioButton radCustom;
		private System.Windows.Forms.TextBox txtParameters;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton radCVBR;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numCVBRBitrate;
		private System.Windows.Forms.RadioButton radABR;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numABRBitrate;
	}
}