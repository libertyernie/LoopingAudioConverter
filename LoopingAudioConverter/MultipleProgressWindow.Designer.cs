namespace LoopingAudioConverter {
	partial class MultipleProgressWindow {
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblDecoding = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.pnlEncoding = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlCenter = new System.Windows.Forms.Panel();
			this.pnlTotalProgress = new System.Windows.Forms.Panel();
			this.pnlTotalProgress2 = new System.Windows.Forms.Panel();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.pnlCenter.SuspendLayout();
			this.pnlTotalProgress.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lblDecoding);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(260, 40);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Decoding";
			// 
			// lblDecoding
			// 
			this.lblDecoding.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDecoding.Location = new System.Drawing.Point(3, 16);
			this.lblDecoding.Name = "lblDecoding";
			this.lblDecoding.Size = new System.Drawing.Size(254, 21);
			this.lblDecoding.TabIndex = 0;
			this.lblDecoding.Text = "filename 4";
			this.lblDecoding.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.pnlEncoding);
			this.groupBox2.Location = new System.Drawing.Point(12, 58);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(260, 161);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Encoding";
			// 
			// pnlEncoding
			// 
			this.pnlEncoding.AutoSize = true;
			this.pnlEncoding.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlEncoding.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlEncoding.Location = new System.Drawing.Point(3, 16);
			this.pnlEncoding.Name = "pnlEncoding";
			this.pnlEncoding.Size = new System.Drawing.Size(254, 0);
			this.pnlEncoding.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(197, 225);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pnlCenter
			// 
			this.pnlCenter.Controls.Add(this.groupBox1);
			this.pnlCenter.Controls.Add(this.btnCancel);
			this.pnlCenter.Controls.Add(this.groupBox2);
			this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCenter.Location = new System.Drawing.Point(0, 51);
			this.pnlCenter.Name = "pnlCenter";
			this.pnlCenter.Size = new System.Drawing.Size(284, 260);
			this.pnlCenter.TabIndex = 3;
			// 
			// pnlTotalProgress
			// 
			this.pnlTotalProgress.Controls.Add(this.pnlTotalProgress2);
			this.pnlTotalProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTotalProgress.Location = new System.Drawing.Point(0, 0);
			this.pnlTotalProgress.Name = "pnlTotalProgress";
			this.pnlTotalProgress.Size = new System.Drawing.Size(284, 51);
			this.pnlTotalProgress.TabIndex = 1;
			// 
			// pnlTotalProgress2
			// 
			this.pnlTotalProgress2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTotalProgress2.Location = new System.Drawing.Point(12, 12);
			this.pnlTotalProgress2.Name = "pnlTotalProgress2";
			this.pnlTotalProgress2.Size = new System.Drawing.Size(260, 33);
			this.pnlTotalProgress2.TabIndex = 2;
			// 
			// MultipleProgressWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 311);
			this.Controls.Add(this.pnlCenter);
			this.Controls.Add(this.pnlTotalProgress);
			this.Name = "MultipleProgressWindow";
			this.Text = "Progress";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultipleProgressWindow_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.pnlCenter.ResumeLayout(false);
			this.pnlTotalProgress.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblDecoding;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Panel pnlEncoding;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlCenter;
		private System.Windows.Forms.Panel pnlTotalProgress;
		private System.Windows.Forms.Panel pnlTotalProgress2;
	}
}