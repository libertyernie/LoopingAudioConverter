namespace LoopingAudioConverter.Brawl {
	partial class MultipleProgressTrackerRow {
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
			this.lblName = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// lblName
			// 
			this.lblName.AutoEllipsis = true;
			this.lblName.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblName.Location = new System.Drawing.Point(0, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(150, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 13);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(150, 37);
			this.panel1.TabIndex = 1;
			// 
			// MultipleProgressTrackerRow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblName);
			this.Name = "MultipleProgressTrackerRow";
			this.Size = new System.Drawing.Size(150, 50);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Panel panel1;
	}
}
