namespace LoopingAudioConverter
{
    partial class OptionsForm
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.lblSeconds = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.numFadeOutTime = new System.Windows.Forms.NumericUpDown();
			this.numNumberLoops = new System.Windows.Forms.NumericUpDown();
			this.lblFadeOutTime = new System.Windows.Forms.Label();
			this.lblNumberLoops = new System.Windows.Forms.Label();
			this.txtStartEndFilenamePattern = new System.Windows.Forms.TextBox();
			this.chkStartEnd = new System.Windows.Forms.CheckBox();
			this.txt0StartFilenamePattern = new System.Windows.Forms.TextBox();
			this.chk0Start = new System.Windows.Forms.CheckBox();
			this.txt0EndFilenamePattern = new System.Windows.Forms.TextBox();
			this.chk0End = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.lblOutputFormat = new System.Windows.Forms.Label();
			this.lblMoreThanOneChannel = new System.Windows.Forms.Label();
			this.radChannelsTogether = new System.Windows.Forms.RadioButton();
			this.radChannelsPairs = new System.Windows.Forms.RadioButton();
			this.radChannelsSeparate = new System.Windows.Forms.RadioButton();
			this.pnlOutputFormat = new System.Windows.Forms.Panel();
			this.pnlExportSegments = new System.Windows.Forms.Panel();
			this.pnlExportChannels = new System.Windows.Forms.Panel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnOkay = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).BeginInit();
			this.pnlOutputFormat.SuspendLayout();
			this.pnlExportSegments.SuspendLayout();
			this.pnlExportChannels.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
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
			this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
			this.splitContainer1.Panel2.Controls.Add(this.panel1);
			this.splitContainer1.Size = new System.Drawing.Size(534, 361);
			this.splitContainer1.SplitterDistance = 161;
			this.splitContainer1.TabIndex = 0;
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
			this.listBox1.Size = new System.Drawing.Size(449, 155);
			this.listBox1.TabIndex = 0;
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
			this.flowLayoutPanel1.Location = new System.Drawing.Point(454, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(80, 161);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(3, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(74, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Location = new System.Drawing.Point(3, 32);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(74, 23);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// lblSeconds
			// 
			this.lblSeconds.Location = new System.Drawing.Point(255, 55);
			this.lblSeconds.Margin = new System.Windows.Forms.Padding(3);
			this.lblSeconds.Name = "lblSeconds";
			this.lblSeconds.Size = new System.Drawing.Size(40, 20);
			this.lblSeconds.TabIndex = 8;
			this.lblSeconds.Text = "sec";
			this.lblSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(169, 107);
			this.label3.Margin = new System.Windows.Forms.Padding(3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 20);
			this.label3.TabIndex = 13;
			this.label3.Text = "Suffix:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(169, 81);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 20);
			this.label2.TabIndex = 10;
			this.label2.Text = "Suffix:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(169, 3);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Suffix:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// numFadeOutTime
			// 
			this.numFadeOutTime.DecimalPlaces = 3;
			this.numFadeOutTime.Location = new System.Drawing.Point(172, 55);
			this.numFadeOutTime.Name = "numFadeOutTime";
			this.numFadeOutTime.Size = new System.Drawing.Size(80, 20);
			this.numFadeOutTime.TabIndex = 7;
			// 
			// numNumberLoops
			// 
			this.numNumberLoops.Location = new System.Drawing.Point(172, 29);
			this.numNumberLoops.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numNumberLoops.Name = "numNumberLoops";
			this.numNumberLoops.Size = new System.Drawing.Size(53, 20);
			this.numNumberLoops.TabIndex = 4;
			this.numNumberLoops.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// lblFadeOutTime
			// 
			this.lblFadeOutTime.Location = new System.Drawing.Point(40, 55);
			this.lblFadeOutTime.Margin = new System.Windows.Forms.Padding(3);
			this.lblFadeOutTime.Name = "lblFadeOutTime";
			this.lblFadeOutTime.Size = new System.Drawing.Size(96, 20);
			this.lblFadeOutTime.TabIndex = 6;
			this.lblFadeOutTime.Text = "Fade-out time";
			this.lblFadeOutTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNumberLoops
			// 
			this.lblNumberLoops.Location = new System.Drawing.Point(40, 29);
			this.lblNumberLoops.Margin = new System.Windows.Forms.Padding(3);
			this.lblNumberLoops.Name = "lblNumberLoops";
			this.lblNumberLoops.Size = new System.Drawing.Size(96, 20);
			this.lblNumberLoops.TabIndex = 3;
			this.lblNumberLoops.Text = "Number of loops:";
			this.lblNumberLoops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtStartEndFilenamePattern
			// 
			this.txtStartEndFilenamePattern.Enabled = false;
			this.txtStartEndFilenamePattern.Location = new System.Drawing.Point(215, 107);
			this.txtStartEndFilenamePattern.Name = "txtStartEndFilenamePattern";
			this.txtStartEndFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txtStartEndFilenamePattern.TabIndex = 0;
			this.txtStartEndFilenamePattern.Text = " (loop)";
			// 
			// chkStartEnd
			// 
			this.chkStartEnd.Location = new System.Drawing.Point(3, 107);
			this.chkStartEnd.Name = "chkStartEnd";
			this.chkStartEnd.Size = new System.Drawing.Size(160, 20);
			this.chkStartEnd.TabIndex = 12;
			this.chkStartEnd.Text = "Export loop segment";
			this.chkStartEnd.UseVisualStyleBackColor = true;
			this.chkStartEnd.CheckedChanged += new System.EventHandler(this.chkStartEnd_CheckedChanged);
			// 
			// txt0StartFilenamePattern
			// 
			this.txt0StartFilenamePattern.Enabled = false;
			this.txt0StartFilenamePattern.Location = new System.Drawing.Point(215, 81);
			this.txt0StartFilenamePattern.Name = "txt0StartFilenamePattern";
			this.txt0StartFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txt0StartFilenamePattern.TabIndex = 11;
			this.txt0StartFilenamePattern.Text = " (beginning)";
			// 
			// chk0Start
			// 
			this.chk0Start.Location = new System.Drawing.Point(3, 81);
			this.chk0Start.Name = "chk0Start";
			this.chk0Start.Size = new System.Drawing.Size(168, 20);
			this.chk0Start.TabIndex = 9;
			this.chk0Start.Text = "Export segment before loop";
			this.chk0Start.UseVisualStyleBackColor = true;
			this.chk0Start.CheckedChanged += new System.EventHandler(this.chk0Start_CheckedChanged);
			// 
			// txt0EndFilenamePattern
			// 
			this.txt0EndFilenamePattern.Location = new System.Drawing.Point(215, 3);
			this.txt0EndFilenamePattern.Name = "txt0EndFilenamePattern";
			this.txt0EndFilenamePattern.Size = new System.Drawing.Size(80, 20);
			this.txt0EndFilenamePattern.TabIndex = 2;
			// 
			// chk0End
			// 
			this.chk0End.Checked = true;
			this.chk0End.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chk0End.Location = new System.Drawing.Point(3, 3);
			this.chk0End.Name = "chk0End";
			this.chk0End.Size = new System.Drawing.Size(160, 20);
			this.chk0End.TabIndex = 0;
			this.chk0End.Text = "Export whole song";
			this.chk0End.UseVisualStyleBackColor = true;
			this.chk0End.CheckedChanged += new System.EventHandler(this.chk0End_CheckedChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.DisplayMember = "Name";
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(89, 3);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.ValueMember = "Value";
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
			// lblMoreThanOneChannel
			// 
			this.lblMoreThanOneChannel.Location = new System.Drawing.Point(3, 0);
			this.lblMoreThanOneChannel.Name = "lblMoreThanOneChannel";
			this.lblMoreThanOneChannel.Size = new System.Drawing.Size(224, 21);
			this.lblMoreThanOneChannel.TabIndex = 0;
			this.lblMoreThanOneChannel.Text = "For files with more than one audio channel:";
			this.lblMoreThanOneChannel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radChannelsTogether
			// 
			this.radChannelsTogether.AutoSize = true;
			this.radChannelsTogether.Checked = true;
			this.radChannelsTogether.Location = new System.Drawing.Point(6, 24);
			this.radChannelsTogether.Name = "radChannelsTogether";
			this.radChannelsTogether.Size = new System.Drawing.Size(148, 17);
			this.radChannelsTogether.TabIndex = 1;
			this.radChannelsTogether.TabStop = true;
			this.radChannelsTogether.Text = "Put all channels in one file";
			this.radChannelsTogether.UseVisualStyleBackColor = true;
			// 
			// radChannelsPairs
			// 
			this.radChannelsPairs.AutoSize = true;
			this.radChannelsPairs.Location = new System.Drawing.Point(6, 47);
			this.radChannelsPairs.Name = "radChannelsPairs";
			this.radChannelsPairs.Size = new System.Drawing.Size(209, 17);
			this.radChannelsPairs.TabIndex = 2;
			this.radChannelsPairs.Text = "Put each pair of channels in its own file";
			this.radChannelsPairs.UseVisualStyleBackColor = true;
			// 
			// radChannelsSeparate
			// 
			this.radChannelsSeparate.AutoSize = true;
			this.radChannelsSeparate.Location = new System.Drawing.Point(6, 70);
			this.radChannelsSeparate.Name = "radChannelsSeparate";
			this.radChannelsSeparate.Size = new System.Drawing.Size(172, 17);
			this.radChannelsSeparate.TabIndex = 3;
			this.radChannelsSeparate.Text = "Put each channel in its own file";
			this.radChannelsSeparate.UseVisualStyleBackColor = true;
			// 
			// pnlOutputFormat
			// 
			this.pnlOutputFormat.AutoSize = true;
			this.pnlOutputFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlOutputFormat.Controls.Add(this.lblOutputFormat);
			this.pnlOutputFormat.Controls.Add(this.comboBox1);
			this.pnlOutputFormat.Location = new System.Drawing.Point(3, 3);
			this.pnlOutputFormat.Name = "pnlOutputFormat";
			this.pnlOutputFormat.Size = new System.Drawing.Size(213, 27);
			this.pnlOutputFormat.TabIndex = 20;
			// 
			// pnlExportSegments
			// 
			this.pnlExportSegments.AutoSize = true;
			this.pnlExportSegments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlExportSegments.Controls.Add(this.chk0End);
			this.pnlExportSegments.Controls.Add(this.txt0EndFilenamePattern);
			this.pnlExportSegments.Controls.Add(this.chk0Start);
			this.pnlExportSegments.Controls.Add(this.txt0StartFilenamePattern);
			this.pnlExportSegments.Controls.Add(this.chkStartEnd);
			this.pnlExportSegments.Controls.Add(this.txtStartEndFilenamePattern);
			this.pnlExportSegments.Controls.Add(this.lblSeconds);
			this.pnlExportSegments.Controls.Add(this.lblNumberLoops);
			this.pnlExportSegments.Controls.Add(this.label3);
			this.pnlExportSegments.Controls.Add(this.lblFadeOutTime);
			this.pnlExportSegments.Controls.Add(this.label2);
			this.pnlExportSegments.Controls.Add(this.numNumberLoops);
			this.pnlExportSegments.Controls.Add(this.label1);
			this.pnlExportSegments.Controls.Add(this.numFadeOutTime);
			this.pnlExportSegments.Location = new System.Drawing.Point(0, 33);
			this.pnlExportSegments.Margin = new System.Windows.Forms.Padding(0);
			this.pnlExportSegments.Name = "pnlExportSegments";
			this.pnlExportSegments.Size = new System.Drawing.Size(298, 130);
			this.pnlExportSegments.TabIndex = 21;
			// 
			// pnlExportChannels
			// 
			this.pnlExportChannels.AutoSize = true;
			this.pnlExportChannels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlExportChannels.Controls.Add(this.lblMoreThanOneChannel);
			this.pnlExportChannels.Controls.Add(this.radChannelsTogether);
			this.pnlExportChannels.Controls.Add(this.radChannelsPairs);
			this.pnlExportChannels.Controls.Add(this.radChannelsSeparate);
			this.pnlExportChannels.Location = new System.Drawing.Point(298, 0);
			this.pnlExportChannels.Margin = new System.Windows.Forms.Padding(0);
			this.pnlExportChannels.Name = "pnlExportChannels";
			this.pnlExportChannels.Size = new System.Drawing.Size(230, 90);
			this.pnlExportChannels.TabIndex = 22;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoScroll = true;
			this.flowLayoutPanel2.Controls.Add(this.pnlOutputFormat);
			this.flowLayoutPanel2.Controls.Add(this.pnlExportSegments);
			this.flowLayoutPanel2.Controls.Add(this.pnlExportChannels);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(534, 166);
			this.flowLayoutPanel2.TabIndex = 23;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Controls.Add(this.btnOkay);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 166);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(534, 30);
			this.panel1.TabIndex = 23;
			// 
			// btnOkay
			// 
			this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOkay.Location = new System.Drawing.Point(377, 4);
			this.btnOkay.Name = "btnOkay";
			this.btnOkay.Size = new System.Drawing.Size(74, 23);
			this.btnOkay.TabIndex = 0;
			this.btnOkay.Text = "OK";
			this.btnOkay.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(457, 4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(74, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.btnOkay;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(534, 361);
			this.Controls.Add(this.splitContainer1);
			this.Name = "OptionsForm";
			this.Text = "Looping Audio Converter";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).EndInit();
			this.pnlOutputFormat.ResumeLayout(false);
			this.pnlExportSegments.ResumeLayout(false);
			this.pnlExportSegments.PerformLayout();
			this.pnlExportChannels.ResumeLayout(false);
			this.pnlExportChannels.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.panel1.ResumeLayout(false);
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
		private System.Windows.Forms.Label lblMoreThanOneChannel;
		private System.Windows.Forms.RadioButton radChannelsSeparate;
		private System.Windows.Forms.RadioButton radChannelsPairs;
		private System.Windows.Forms.RadioButton radChannelsTogether;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Panel pnlOutputFormat;
		private System.Windows.Forms.Panel pnlExportSegments;
		private System.Windows.Forms.Panel pnlExportChannels;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOkay;
    }
}