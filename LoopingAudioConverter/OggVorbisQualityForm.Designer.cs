namespace LoopingAudioConverter {
	partial class OggVorbisQualityForm {
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
			this.numQuality = new System.Windows.Forms.NumericUpDown();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtParameters = new System.Windows.Forms.TextBox();
			this.radCustom = new System.Windows.Forms.RadioButton();
			this.radQuality = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.numQuality)).BeginInit();
			this.SuspendLayout();
			// 
			// numQuality
			// 
			this.numQuality.DecimalPlaces = 1;
			this.numQuality.Increment = new decimal(new int[] {
			2,
			0,
			0,
			65536});
			this.numQuality.Location = new System.Drawing.Point(75, 28);
			this.numQuality.Maximum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.numQuality.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			-2147483648});
			this.numQuality.Name = "numQuality";
			this.numQuality.Size = new System.Drawing.Size(64, 20);
			this.numQuality.TabIndex = 4;
			this.numQuality.Value = new decimal(new int[] {
			3,
			0,
			0,
			0});
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
			// txtParameters
			// 
			this.txtParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtParameters.Location = new System.Drawing.Point(34, 77);
			this.txtParameters.Name = "txtParameters";
			this.txtParameters.Size = new System.Drawing.Size(238, 20);
			this.txtParameters.TabIndex = 6;
			// 
			// radCustom
			// 
			this.radCustom.AutoSize = true;
			this.radCustom.Location = new System.Drawing.Point(12, 54);
			this.radCustom.Name = "radCustom";
			this.radCustom.Size = new System.Drawing.Size(115, 17);
			this.radCustom.TabIndex = 5;
			this.radCustom.Text = "Custom parameters";
			this.radCustom.UseVisualStyleBackColor = true;
			// 
			// radQuality
			// 
			this.radQuality.AutoSize = true;
			this.radQuality.Checked = true;
			this.radQuality.Location = new System.Drawing.Point(12, 28);
			this.radQuality.Name = "radQuality";
			this.radQuality.Size = new System.Drawing.Size(57, 17);
			this.radQuality.TabIndex = 1;
			this.radQuality.TabStop = true;
			this.radQuality.Text = "Quality";
			this.radQuality.UseVisualStyleBackColor = true;
			// 
			// VorbisQualityForm
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
			this.Controls.Add(this.numQuality);
			this.Controls.Add(this.radQuality);
			this.Controls.Add(this.label1);
			this.Name = "VorbisQualityForm";
			this.Text = "Ogg Vorbis Quality";
			((System.ComponentModel.ISupportInitialize)(this.numQuality)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.NumericUpDown numQuality;
		private System.Windows.Forms.Button btnOkay;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtParameters;
		private System.Windows.Forms.RadioButton radCustom;
		private System.Windows.Forms.RadioButton radQuality;
	}
}