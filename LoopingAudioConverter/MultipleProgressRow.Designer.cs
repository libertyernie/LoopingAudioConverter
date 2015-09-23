namespace LoopingAudioConverter {
	partial class MultipleProgressRow {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.pnlProgress = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "label1";
			// 
			// pnlProgress
			// 
			this.pnlProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlProgress.Location = new System.Drawing.Point(0, 13);
			this.pnlProgress.MinimumSize = new System.Drawing.Size(100, 20);
			this.pnlProgress.Name = "pnlProgress";
			this.pnlProgress.Size = new System.Drawing.Size(100, 20);
			this.pnlProgress.TabIndex = 1;
			this.pnlProgress.Visible = false;
			// 
			// MultipleProgressRow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.pnlProgress);
			this.Controls.Add(this.label1);
			this.MinimumSize = new System.Drawing.Size(100, 0);
			this.Name = "MultipleProgressRow";
			this.Size = new System.Drawing.Size(100, 33);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlProgress;
	}
}
