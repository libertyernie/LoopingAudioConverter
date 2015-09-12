namespace LoopingAudioConverter
{
    partial class MainForm
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblOutputFormat = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.chk0End = new System.Windows.Forms.CheckBox();
			this.txt0EndFilenamePattern = new System.Windows.Forms.TextBox();
			this.txt0StartFilenamePattern = new System.Windows.Forms.TextBox();
			this.chk0Start = new System.Windows.Forms.CheckBox();
			this.txtStartEndFilenamePattern = new System.Windows.Forms.TextBox();
			this.chkStartEnd = new System.Windows.Forms.CheckBox();
			this.lblNumberLoops = new System.Windows.Forms.Label();
			this.lblFadeOutTime = new System.Windows.Forms.Label();
			this.numNumberLoops = new System.Windows.Forms.NumericUpDown();
			this.numFadeOutTime = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblSeconds = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBox1);
			this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lblSeconds);
			this.splitContainer1.Panel2.Controls.Add(this.label3);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Panel2.Controls.Add(this.label1);
			this.splitContainer1.Panel2.Controls.Add(this.numFadeOutTime);
			this.splitContainer1.Panel2.Controls.Add(this.numNumberLoops);
			this.splitContainer1.Panel2.Controls.Add(this.lblFadeOutTime);
			this.splitContainer1.Panel2.Controls.Add(this.lblNumberLoops);
			this.splitContainer1.Panel2.Controls.Add(this.txtStartEndFilenamePattern);
			this.splitContainer1.Panel2.Controls.Add(this.chkStartEnd);
			this.splitContainer1.Panel2.Controls.Add(this.txt0StartFilenamePattern);
			this.splitContainer1.Panel2.Controls.Add(this.chk0Start);
			this.splitContainer1.Panel2.Controls.Add(this.txt0EndFilenamePattern);
			this.splitContainer1.Panel2.Controls.Add(this.chk0End);
			this.splitContainer1.Panel2.Controls.Add(this.comboBox1);
			this.splitContainer1.Panel2.Controls.Add(this.lblOutputFormat);
			this.splitContainer1.Size = new System.Drawing.Size(484, 361);
			this.splitContainer1.SplitterDistance = 161;
			this.splitContainer1.TabIndex = 0;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(3, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Location = new System.Drawing.Point(4, 32);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(74, 23);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// listBox1
			// 
			this.listBox1.AllowDrop = true;
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.IntegralHeight = false;
			this.listBox1.Location = new System.Drawing.Point(3, 3);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBox1.Size = new System.Drawing.Size(398, 155);
			this.listBox1.TabIndex = 1;
			this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
			this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.btnAdd);
			this.flowLayoutPanel1.Controls.Add(this.btnRemove);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(403, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(81, 161);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// lblOutputFormat
			// 
			this.lblOutputFormat.Location = new System.Drawing.Point(3, 3);
			this.lblOutputFormat.Margin = new System.Windows.Forms.Padding(3);
			this.lblOutputFormat.Name = "lblOutputFormat";
			this.lblOutputFormat.Size = new System.Drawing.Size(80, 21);
			this.lblOutputFormat.TabIndex = 0;
			this.lblOutputFormat.Text = "Output format:";
			this.lblOutputFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "BRSTM",
            "WAV",
            "FLAC",
            "MP3",
            "Ogg Vorbis"});
			this.comboBox1.Location = new System.Drawing.Point(89, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 1;
			// 
			// chk0End
			// 
			this.chk0End.Checked = true;
			this.chk0End.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk0End.Location = new System.Drawing.Point(6, 30);
			this.chk0End.Name = "chk0End";
			this.chk0End.Size = new System.Drawing.Size(180, 20);
			this.chk0End.TabIndex = 2;
			this.chk0End.Text = "Export whole song as:";
			this.chk0End.UseVisualStyleBackColor = true;
			this.chk0End.CheckedChanged += new System.EventHandler(this.chk0End_CheckedChanged);
			// 
			// txt0EndFilenamePattern
			// 
			this.txt0EndFilenamePattern.Location = new System.Drawing.Point(235, 30);
			this.txt0EndFilenamePattern.Name = "txt0EndFilenamePattern";
			this.txt0EndFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txt0EndFilenamePattern.TabIndex = 3;
			// 
			// txt0StartFilenamePattern
			// 
			this.txt0StartFilenamePattern.Enabled = false;
			this.txt0StartFilenamePattern.Location = new System.Drawing.Point(235, 108);
			this.txt0StartFilenamePattern.Name = "txt0StartFilenamePattern";
			this.txt0StartFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txt0StartFilenamePattern.TabIndex = 5;
			this.txt0StartFilenamePattern.Text = " (beginning)";
			// 
			// chk0Start
			// 
			this.chk0Start.Location = new System.Drawing.Point(6, 108);
			this.chk0Start.Name = "chk0Start";
			this.chk0Start.Size = new System.Drawing.Size(180, 20);
			this.chk0Start.TabIndex = 4;
			this.chk0Start.Text = "Export segment before loop as:";
			this.chk0Start.UseVisualStyleBackColor = true;
			this.chk0Start.CheckedChanged += new System.EventHandler(this.chk0Start_CheckedChanged);
			// 
			// txtStartEndFilenamePattern
			// 
			this.txtStartEndFilenamePattern.Enabled = false;
			this.txtStartEndFilenamePattern.Location = new System.Drawing.Point(235, 134);
			this.txtStartEndFilenamePattern.Name = "txtStartEndFilenamePattern";
			this.txtStartEndFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txtStartEndFilenamePattern.TabIndex = 7;
			this.txtStartEndFilenamePattern.Text = " (loop)";
			// 
			// chkStartEnd
			// 
			this.chkStartEnd.Location = new System.Drawing.Point(6, 134);
			this.chkStartEnd.Name = "chkStartEnd";
			this.chkStartEnd.Size = new System.Drawing.Size(180, 20);
			this.chkStartEnd.TabIndex = 6;
			this.chkStartEnd.Text = "Export loop segment as:";
			this.chkStartEnd.UseVisualStyleBackColor = true;
			this.chkStartEnd.CheckedChanged += new System.EventHandler(this.chkStartEnd_CheckedChanged);
			// 
			// lblNumberLoops
			// 
			this.lblNumberLoops.Location = new System.Drawing.Point(43, 56);
			this.lblNumberLoops.Margin = new System.Windows.Forms.Padding(3);
			this.lblNumberLoops.Name = "lblNumberLoops";
			this.lblNumberLoops.Size = new System.Drawing.Size(96, 20);
			this.lblNumberLoops.TabIndex = 8;
			this.lblNumberLoops.Text = "Number of loops:";
			this.lblNumberLoops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFadeOutTime
			// 
			this.lblFadeOutTime.Location = new System.Drawing.Point(43, 82);
			this.lblFadeOutTime.Margin = new System.Windows.Forms.Padding(3);
			this.lblFadeOutTime.Name = "lblFadeOutTime";
			this.lblFadeOutTime.Size = new System.Drawing.Size(96, 20);
			this.lblFadeOutTime.TabIndex = 9;
			this.lblFadeOutTime.Text = "Fade-out time";
			this.lblFadeOutTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numNumberLoops
			// 
			this.numNumberLoops.Location = new System.Drawing.Point(192, 56);
			this.numNumberLoops.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numNumberLoops.Name = "numNumberLoops";
			this.numNumberLoops.Size = new System.Drawing.Size(53, 20);
			this.numNumberLoops.TabIndex = 10;
			this.numNumberLoops.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// numFadeOutTime
			// 
			this.numFadeOutTime.DecimalPlaces = 3;
			this.numFadeOutTime.Location = new System.Drawing.Point(192, 82);
			this.numFadeOutTime.Name = "numFadeOutTime";
			this.numFadeOutTime.Size = new System.Drawing.Size(80, 20);
			this.numFadeOutTime.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(189, 30);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "Suffix:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(189, 108);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 20);
			this.label2.TabIndex = 13;
			this.label2.Text = "Suffix:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(189, 134);
			this.label3.Margin = new System.Windows.Forms.Padding(3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 20);
			this.label3.TabIndex = 14;
			this.label3.Text = "Suffix:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSeconds
			// 
			this.lblSeconds.Location = new System.Drawing.Point(275, 82);
			this.lblSeconds.Margin = new System.Windows.Forms.Padding(3);
			this.lblSeconds.Name = "lblSeconds";
			this.lblSeconds.Size = new System.Drawing.Size(40, 20);
			this.lblSeconds.TabIndex = 15;
			this.lblSeconds.Text = "sec";
			this.lblSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 361);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MainForm";
			this.Text = "Looping Audio Converter";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label lblOutputFormat;
		private System.Windows.Forms.TextBox txtStartEndFilenamePattern;
		private System.Windows.Forms.CheckBox chkStartEnd;
		private System.Windows.Forms.TextBox txt0StartFilenamePattern;
		private System.Windows.Forms.CheckBox chk0Start;
		private System.Windows.Forms.TextBox txt0EndFilenamePattern;
		private System.Windows.Forms.CheckBox chk0End;
		private System.Windows.Forms.NumericUpDown numFadeOutTime;
		private System.Windows.Forms.NumericUpDown numNumberLoops;
		private System.Windows.Forms.Label lblFadeOutTime;
		private System.Windows.Forms.Label lblNumberLoops;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblSeconds;
    }
}