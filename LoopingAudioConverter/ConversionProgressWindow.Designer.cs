namespace LoopingAudioConverter {
	partial class ConversionProgressWindow {
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
			this.lblDecoding = new System.Windows.Forms.Label();
			this.progDecoding = new System.Windows.Forms.ProgressBar();
			this.progEncoding = new System.Windows.Forms.ProgressBar();
			this.lblEncoding = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblDecoding
			// 
			this.lblDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDecoding.Location = new System.Drawing.Point(12, 38);
			this.lblDecoding.Name = "lblDecoding";
			this.lblDecoding.Size = new System.Drawing.Size(260, 32);
			this.lblDecoding.TabIndex = 0;
			this.lblDecoding.Text = "Decoding:\r\n    file1";
			// 
			// progDecoding
			// 
			this.progDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progDecoding.Location = new System.Drawing.Point(12, 12);
			this.progDecoding.Name = "progDecoding";
			this.progDecoding.Size = new System.Drawing.Size(260, 23);
			this.progDecoding.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progDecoding.TabIndex = 1;
			// 
			// progEncoding
			// 
			this.progEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progEncoding.Location = new System.Drawing.Point(12, 73);
			this.progEncoding.Name = "progEncoding";
			this.progEncoding.Size = new System.Drawing.Size(260, 23);
			this.progEncoding.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progEncoding.TabIndex = 3;
			// 
			// lblEncoding
			// 
			this.lblEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEncoding.Location = new System.Drawing.Point(12, 99);
			this.lblEncoding.Name = "lblEncoding";
			this.lblEncoding.Size = new System.Drawing.Size(260, 124);
			this.lblEncoding.TabIndex = 2;
			this.lblEncoding.Text = "Encoding:\r\n    file2\r\n    file3\r\n    file4";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.Location = new System.Drawing.Point(12, 226);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ConversionProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.progEncoding);
			this.Controls.Add(this.lblEncoding);
			this.Controls.Add(this.progDecoding);
			this.Controls.Add(this.lblDecoding);
			this.Name = "ConversionProgressWindow";
			this.Text = "Progress";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblDecoding;
		private System.Windows.Forms.ProgressBar progDecoding;
		private System.Windows.Forms.ProgressBar progEncoding;
		private System.Windows.Forms.Label lblEncoding;
		private System.Windows.Forms.Button btnCancel;
	}
}