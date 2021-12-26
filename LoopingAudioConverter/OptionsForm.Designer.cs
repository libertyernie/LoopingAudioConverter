namespace LoopingAudioConverter {
    partial class OptionsForm {
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtInputDir = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSuffixFilter = new System.Windows.Forms.Button();
            this.txtSuffixFilter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.lblOutputDir = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnAddDir = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblEnumerationStatus = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numTempo = new System.Windows.Forms.NumericUpDown();
            this.chkTempo = new System.Windows.Forms.CheckBox();
            this.numPitch = new System.Windows.Forms.NumericUpDown();
            this.chkPitch = new System.Windows.Forms.CheckBox();
            this.numAmplifyRatio = new System.Windows.Forms.NumericUpDown();
            this.chkAmplifyRatio = new System.Windows.Forms.CheckBox();
            this.numAmplifydB = new System.Windows.Forms.NumericUpDown();
            this.chkAmplifydB = new System.Windows.Forms.CheckBox();
            this.numMaxSampleRate = new System.Windows.Forms.NumericUpDown();
            this.chkSampleRate = new System.Windows.Forms.CheckBox();
            this.chkMono = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ddlUnknownLoopBehavior = new System.Windows.Forms.ComboBox();
            this.lblUnknownLoopBehavior = new System.Windows.Forms.Label();
            this.pnlExportChannels = new System.Windows.Forms.Panel();
            this.lblMoreThanOneChannel = new System.Windows.Forms.Label();
            this.radChannelsTogether = new System.Windows.Forms.RadioButton();
            this.radChannelsPairs = new System.Windows.Forms.RadioButton();
            this.radChannelsSeparate = new System.Windows.Forms.RadioButton();
            this.pnlExportSegments = new System.Windows.Forms.Panel();
            this.chkWriteLoopingMetadata = new System.Windows.Forms.CheckBox();
            this.btnEncodingOptions = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblOutputFormat = new System.Windows.Forms.Label();
            this.chk0End = new System.Windows.Forms.CheckBox();
            this.txt0EndFilenamePattern = new System.Windows.Forms.TextBox();
            this.chk0Start = new System.Windows.Forms.CheckBox();
            this.txt0StartFilenamePattern = new System.Windows.Forms.TextBox();
            this.chkStartEnd = new System.Windows.Forms.CheckBox();
            this.txtStartEndFilenamePattern = new System.Windows.Forms.TextBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.lblNumberLoops = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFadeOutTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numNumberLoops = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numFadeOutTime = new System.Windows.Forms.NumericUpDown();
            this.chkShortCircuit = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoadOptions = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnSaveOptions = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmplifyRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmplifydB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSampleRate)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlExportChannels.SuspendLayout();
            this.pnlExportSegments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.txtInputDir);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.btnSuffixFilter);
            this.splitContainer1.Panel1.Controls.Add(this.txtSuffixFilter);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.btnBrowse);
            this.splitContainer1.Panel1.Controls.Add(this.txtOutputDir);
            this.splitContainer1.Panel1.Controls.Add(this.lblOutputDir);
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
            this.splitContainer1.Size = new System.Drawing.Size(534, 481);
            this.splitContainer1.SplitterDistance = 161;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtInputDir
            // 
            this.txtInputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputDir.Location = new System.Drawing.Point(383, 112);
            this.txtInputDir.Name = "txtInputDir";
            this.txtInputDir.Size = new System.Drawing.Size(148, 20);
            this.txtInputDir.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(296, 112);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Input directory:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSuffixFilter
            // 
            this.btnSuffixFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSuffixFilter.Location = new System.Drawing.Point(245, 112);
            this.btnSuffixFilter.Name = "btnSuffixFilter";
            this.btnSuffixFilter.Size = new System.Drawing.Size(45, 19);
            this.btnSuffixFilter.TabIndex = 3;
            this.btnSuffixFilter.Text = "Filter";
            this.btnSuffixFilter.UseVisualStyleBackColor = true;
            this.btnSuffixFilter.Click += new System.EventHandler(this.btnSuffixFilter_Click);
            // 
            // txtSuffixFilter
            // 
            this.txtSuffixFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSuffixFilter.Location = new System.Drawing.Point(127, 112);
            this.txtSuffixFilter.Name = "txtSuffixFilter";
            this.txtSuffixFilter.Size = new System.Drawing.Size(112, 20);
            this.txtSuffixFilter.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(3, 111);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Only files ending with:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(499, 137);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 20);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDir.Location = new System.Drawing.Point(99, 137);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(394, 20);
            this.txtOutputDir.TabIndex = 5;
            this.txtOutputDir.Text = "./output";
            // 
            // lblOutputDir
            // 
            this.lblOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOutputDir.Location = new System.Drawing.Point(3, 137);
            this.lblOutputDir.Margin = new System.Windows.Forms.Padding(3);
            this.lblOutputDir.Name = "lblOutputDir";
            this.lblOutputDir.Size = new System.Drawing.Size(90, 20);
            this.lblOutputDir.TabIndex = 4;
            this.lblOutputDir.Text = "Output directory:";
            this.lblOutputDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.listBox1.Size = new System.Drawing.Size(448, 102);
            this.listBox1.TabIndex = 0;
            this.listBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBox1_DragDrop);
            this.listBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBox1_DragEnter);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnAdd);
            this.flowLayoutPanel1.Controls.Add(this.btnAddDir);
            this.flowLayoutPanel1.Controls.Add(this.btnRemove);
            this.flowLayoutPanel1.Controls.Add(this.lblEnumerationStatus);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(454, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(80, 105);
            this.flowLayoutPanel1.TabIndex = 0;
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
            // btnAddDir
            // 
            this.btnAddDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDir.Location = new System.Drawing.Point(3, 32);
            this.btnAddDir.Name = "btnAddDir";
            this.btnAddDir.Size = new System.Drawing.Size(74, 23);
            this.btnAddDir.TabIndex = 1;
            this.btnAddDir.Text = "Add Folder";
            this.btnAddDir.UseVisualStyleBackColor = true;
            this.btnAddDir.Click += new System.EventHandler(this.btnAddDir_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(3, 61);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(74, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblEnumerationStatus
            // 
            this.lblEnumerationStatus.AutoSize = true;
            this.lblEnumerationStatus.Location = new System.Drawing.Point(3, 87);
            this.lblEnumerationStatus.Name = "lblEnumerationStatus";
            this.lblEnumerationStatus.Size = new System.Drawing.Size(10, 13);
            this.lblEnumerationStatus.TabIndex = 3;
            this.lblEnumerationStatus.Text = "\t ";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoScroll = true;
            this.flowLayoutPanel2.Controls.Add(this.panel2);
            this.flowLayoutPanel2.Controls.Add(this.panel4);
            this.flowLayoutPanel2.Controls.Add(this.pnlExportChannels);
            this.flowLayoutPanel2.Controls.Add(this.pnlExportSegments);
            this.flowLayoutPanel2.Controls.Add(this.chkShortCircuit);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(534, 316);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.numTempo);
            this.panel2.Controls.Add(this.chkTempo);
            this.panel2.Controls.Add(this.numPitch);
            this.panel2.Controls.Add(this.chkPitch);
            this.panel2.Controls.Add(this.numAmplifyRatio);
            this.panel2.Controls.Add(this.chkAmplifyRatio);
            this.panel2.Controls.Add(this.numAmplifydB);
            this.panel2.Controls.Add(this.chkAmplifydB);
            this.panel2.Controls.Add(this.numMaxSampleRate);
            this.panel2.Controls.Add(this.chkSampleRate);
            this.panel2.Controls.Add(this.chkMono);
            this.panel2.Location = new System.Drawing.Point(0, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 156);
            this.panel2.TabIndex = 0;
            // 
            // numTempo
            // 
            this.numTempo.DecimalPlaces = 3;
            this.numTempo.Enabled = false;
            this.numTempo.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numTempo.Location = new System.Drawing.Point(158, 133);
            this.numTempo.Name = "numTempo";
            this.numTempo.Size = new System.Drawing.Size(66, 20);
            this.numTempo.TabIndex = 10;
            // 
            // chkTempo
            // 
            this.chkTempo.Location = new System.Drawing.Point(3, 133);
            this.chkTempo.Name = "chkTempo";
            this.chkTempo.Size = new System.Drawing.Size(149, 20);
            this.chkTempo.TabIndex = 9;
            this.chkTempo.Text = "Adjust tempo:";
            this.chkTempo.UseVisualStyleBackColor = true;
            this.chkTempo.CheckedChanged += new System.EventHandler(this.chkTempo_CheckedChanged);
            // 
            // numPitch
            // 
            this.numPitch.DecimalPlaces = 2;
            this.numPitch.Enabled = false;
            this.numPitch.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPitch.Location = new System.Drawing.Point(158, 107);
            this.numPitch.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numPitch.Name = "numPitch";
            this.numPitch.Size = new System.Drawing.Size(66, 20);
            this.numPitch.TabIndex = 8;
            // 
            // chkPitch
            // 
            this.chkPitch.Location = new System.Drawing.Point(3, 107);
            this.chkPitch.Name = "chkPitch";
            this.chkPitch.Size = new System.Drawing.Size(149, 20);
            this.chkPitch.TabIndex = 7;
            this.chkPitch.Text = "Adjust pitch (semitones):";
            this.chkPitch.UseVisualStyleBackColor = true;
            this.chkPitch.CheckedChanged += new System.EventHandler(this.chkPitch_CheckedChanged);
            // 
            // numAmplifyRatio
            // 
            this.numAmplifyRatio.DecimalPlaces = 3;
            this.numAmplifyRatio.Enabled = false;
            this.numAmplifyRatio.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numAmplifyRatio.Location = new System.Drawing.Point(158, 81);
            this.numAmplifyRatio.Name = "numAmplifyRatio";
            this.numAmplifyRatio.Size = new System.Drawing.Size(66, 20);
            this.numAmplifyRatio.TabIndex = 6;
            this.numAmplifyRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAmplifyRatio
            // 
            this.chkAmplifyRatio.Location = new System.Drawing.Point(3, 81);
            this.chkAmplifyRatio.Name = "chkAmplifyRatio";
            this.chkAmplifyRatio.Size = new System.Drawing.Size(149, 20);
            this.chkAmplifyRatio.TabIndex = 5;
            this.chkAmplifyRatio.Text = "Amplify (amplitude ratio):";
            this.chkAmplifyRatio.UseVisualStyleBackColor = true;
            this.chkAmplifyRatio.CheckedChanged += new System.EventHandler(this.chkAmplifyRatio_CheckedChanged);
            // 
            // numAmplifydB
            // 
            this.numAmplifydB.Enabled = false;
            this.numAmplifydB.Location = new System.Drawing.Point(158, 55);
            this.numAmplifydB.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numAmplifydB.Name = "numAmplifydB";
            this.numAmplifydB.Size = new System.Drawing.Size(66, 20);
            this.numAmplifydB.TabIndex = 4;
            // 
            // chkAmplifydB
            // 
            this.chkAmplifydB.Location = new System.Drawing.Point(3, 55);
            this.chkAmplifydB.Name = "chkAmplifydB";
            this.chkAmplifydB.Size = new System.Drawing.Size(149, 20);
            this.chkAmplifydB.TabIndex = 3;
            this.chkAmplifydB.Text = "Amplify (dB):";
            this.chkAmplifydB.UseVisualStyleBackColor = true;
            this.chkAmplifydB.CheckedChanged += new System.EventHandler(this.chkAmplifydB_CheckedChanged);
            // 
            // numMaxSampleRate
            // 
            this.numMaxSampleRate.Enabled = false;
            this.numMaxSampleRate.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMaxSampleRate.Location = new System.Drawing.Point(158, 29);
            this.numMaxSampleRate.Maximum = new decimal(new int[] {
            48000,
            0,
            0,
            0});
            this.numMaxSampleRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxSampleRate.Name = "numMaxSampleRate";
            this.numMaxSampleRate.Size = new System.Drawing.Size(66, 20);
            this.numMaxSampleRate.TabIndex = 2;
            this.numMaxSampleRate.Value = new decimal(new int[] {
            32000,
            0,
            0,
            0});
            // 
            // chkSampleRate
            // 
            this.chkSampleRate.Location = new System.Drawing.Point(3, 29);
            this.chkSampleRate.Name = "chkSampleRate";
            this.chkSampleRate.Size = new System.Drawing.Size(149, 20);
            this.chkSampleRate.TabIndex = 1;
            this.chkSampleRate.Text = "New sample rate (Hz):";
            this.chkSampleRate.UseVisualStyleBackColor = true;
            this.chkSampleRate.CheckedChanged += new System.EventHandler(this.chkMaxSampleRate_CheckedChanged);
            // 
            // chkMono
            // 
            this.chkMono.Location = new System.Drawing.Point(3, 3);
            this.chkMono.Name = "chkMono";
            this.chkMono.Size = new System.Drawing.Size(221, 20);
            this.chkMono.TabIndex = 0;
            this.chkMono.Text = "Convert to mono";
            this.chkMono.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Controls.Add(this.ddlUnknownLoopBehavior);
            this.panel4.Controls.Add(this.lblUnknownLoopBehavior);
            this.panel4.Location = new System.Drawing.Point(0, 165);
            this.panel4.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(227, 48);
            this.panel4.TabIndex = 1;
            // 
            // ddlUnknownLoopBehavior
            // 
            this.ddlUnknownLoopBehavior.DisplayMember = "Name";
            this.ddlUnknownLoopBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlUnknownLoopBehavior.FormattingEnabled = true;
            this.ddlUnknownLoopBehavior.Location = new System.Drawing.Point(6, 24);
            this.ddlUnknownLoopBehavior.Name = "ddlUnknownLoopBehavior";
            this.ddlUnknownLoopBehavior.Size = new System.Drawing.Size(218, 21);
            this.ddlUnknownLoopBehavior.TabIndex = 1;
            this.ddlUnknownLoopBehavior.ValueMember = "Value";
            // 
            // lblUnknownLoopBehavior
            // 
            this.lblUnknownLoopBehavior.Location = new System.Drawing.Point(3, 0);
            this.lblUnknownLoopBehavior.Name = "lblUnknownLoopBehavior";
            this.lblUnknownLoopBehavior.Size = new System.Drawing.Size(221, 21);
            this.lblUnknownLoopBehavior.TabIndex = 0;
            this.lblUnknownLoopBehavior.Text = "For files with no loop information:";
            this.lblUnknownLoopBehavior.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlExportChannels
            // 
            this.pnlExportChannels.AutoSize = true;
            this.pnlExportChannels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlExportChannels.Controls.Add(this.lblMoreThanOneChannel);
            this.pnlExportChannels.Controls.Add(this.radChannelsTogether);
            this.pnlExportChannels.Controls.Add(this.radChannelsPairs);
            this.pnlExportChannels.Controls.Add(this.radChannelsSeparate);
            this.pnlExportChannels.Location = new System.Drawing.Point(0, 219);
            this.pnlExportChannels.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pnlExportChannels.Name = "pnlExportChannels";
            this.pnlExportChannels.Size = new System.Drawing.Size(230, 90);
            this.pnlExportChannels.TabIndex = 2;
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
            this.radChannelsTogether.Location = new System.Drawing.Point(6, 24);
            this.radChannelsTogether.Name = "radChannelsTogether";
            this.radChannelsTogether.Size = new System.Drawing.Size(148, 17);
            this.radChannelsTogether.TabIndex = 1;
            this.radChannelsTogether.Text = "Put all channels in one file";
            this.radChannelsTogether.UseVisualStyleBackColor = true;
            // 
            // radChannelsPairs
            // 
            this.radChannelsPairs.AutoSize = true;
            this.radChannelsPairs.Checked = true;
            this.radChannelsPairs.Location = new System.Drawing.Point(6, 47);
            this.radChannelsPairs.Name = "radChannelsPairs";
            this.radChannelsPairs.Size = new System.Drawing.Size(209, 17);
            this.radChannelsPairs.TabIndex = 2;
            this.radChannelsPairs.TabStop = true;
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
            // pnlExportSegments
            // 
            this.pnlExportSegments.AutoSize = true;
            this.pnlExportSegments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlExportSegments.Controls.Add(this.chkWriteLoopingMetadata);
            this.pnlExportSegments.Controls.Add(this.btnEncodingOptions);
            this.pnlExportSegments.Controls.Add(this.comboBox1);
            this.pnlExportSegments.Controls.Add(this.lblOutputFormat);
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
            this.pnlExportSegments.Location = new System.Drawing.Point(230, 3);
            this.pnlExportSegments.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pnlExportSegments.Name = "pnlExportSegments";
            this.pnlExportSegments.Size = new System.Drawing.Size(298, 183);
            this.pnlExportSegments.TabIndex = 3;
            // 
            // chkWriteLoopingMetadata
            // 
            this.chkWriteLoopingMetadata.Checked = true;
            this.chkWriteLoopingMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWriteLoopingMetadata.Location = new System.Drawing.Point(41, 108);
            this.chkWriteLoopingMetadata.Name = "chkWriteLoopingMetadata";
            this.chkWriteLoopingMetadata.Size = new System.Drawing.Size(184, 20);
            this.chkWriteLoopingMetadata.TabIndex = 11;
            this.chkWriteLoopingMetadata.Text = "Write looping metadata";
            this.chkWriteLoopingMetadata.UseVisualStyleBackColor = true;
            // 
            // btnEncodingOptions
            // 
            this.btnEncodingOptions.Location = new System.Drawing.Point(216, 3);
            this.btnEncodingOptions.Name = "btnEncodingOptions";
            this.btnEncodingOptions.Size = new System.Drawing.Size(76, 21);
            this.btnEncodingOptions.TabIndex = 2;
            this.btnEncodingOptions.Text = "Options";
            this.btnEncodingOptions.UseVisualStyleBackColor = true;
            this.btnEncodingOptions.Click += new System.EventHandler(this.btnEncodingOptions_Click);
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
            // chk0End
            // 
            this.chk0End.Checked = true;
            this.chk0End.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk0End.Location = new System.Drawing.Point(3, 30);
            this.chk0End.Name = "chk0End";
            this.chk0End.Size = new System.Drawing.Size(160, 20);
            this.chk0End.TabIndex = 3;
            this.chk0End.Text = "Export whole song";
            this.chk0End.UseVisualStyleBackColor = true;
            this.chk0End.CheckedChanged += new System.EventHandler(this.chk0End_CheckedChanged);
            // 
            // txt0EndFilenamePattern
            // 
            this.txt0EndFilenamePattern.Location = new System.Drawing.Point(215, 30);
            this.txt0EndFilenamePattern.Name = "txt0EndFilenamePattern";
            this.txt0EndFilenamePattern.Size = new System.Drawing.Size(80, 20);
            this.txt0EndFilenamePattern.TabIndex = 5;
            // 
            // chk0Start
            // 
            this.chk0Start.Location = new System.Drawing.Point(3, 134);
            this.chk0Start.Name = "chk0Start";
            this.chk0Start.Size = new System.Drawing.Size(168, 20);
            this.chk0Start.TabIndex = 12;
            this.chk0Start.Text = "Export segment before loop";
            this.chk0Start.UseVisualStyleBackColor = true;
            this.chk0Start.CheckedChanged += new System.EventHandler(this.chk0Start_CheckedChanged);
            // 
            // txt0StartFilenamePattern
            // 
            this.txt0StartFilenamePattern.Enabled = false;
            this.txt0StartFilenamePattern.Location = new System.Drawing.Point(215, 134);
            this.txt0StartFilenamePattern.Name = "txt0StartFilenamePattern";
            this.txt0StartFilenamePattern.Size = new System.Drawing.Size(80, 20);
            this.txt0StartFilenamePattern.TabIndex = 14;
            this.txt0StartFilenamePattern.Text = " (beginning)";
            // 
            // chkStartEnd
            // 
            this.chkStartEnd.Location = new System.Drawing.Point(3, 160);
            this.chkStartEnd.Name = "chkStartEnd";
            this.chkStartEnd.Size = new System.Drawing.Size(160, 20);
            this.chkStartEnd.TabIndex = 15;
            this.chkStartEnd.Text = "Export loop segment";
            this.chkStartEnd.UseVisualStyleBackColor = true;
            this.chkStartEnd.CheckedChanged += new System.EventHandler(this.chkStartEnd_CheckedChanged);
            // 
            // txtStartEndFilenamePattern
            // 
            this.txtStartEndFilenamePattern.Enabled = false;
            this.txtStartEndFilenamePattern.Location = new System.Drawing.Point(215, 160);
            this.txtStartEndFilenamePattern.Name = "txtStartEndFilenamePattern";
            this.txtStartEndFilenamePattern.Size = new System.Drawing.Size(80, 20);
            this.txtStartEndFilenamePattern.TabIndex = 17;
            this.txtStartEndFilenamePattern.Text = " (loop)";
            // 
            // lblSeconds
            // 
            this.lblSeconds.Location = new System.Drawing.Point(231, 81);
            this.lblSeconds.Margin = new System.Windows.Forms.Padding(3);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(40, 21);
            this.lblSeconds.TabIndex = 10;
            this.lblSeconds.Text = "sec";
            this.lblSeconds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNumberLoops
            // 
            this.lblNumberLoops.Location = new System.Drawing.Point(40, 56);
            this.lblNumberLoops.Margin = new System.Windows.Forms.Padding(3);
            this.lblNumberLoops.Name = "lblNumberLoops";
            this.lblNumberLoops.Size = new System.Drawing.Size(96, 20);
            this.lblNumberLoops.TabIndex = 6;
            this.lblNumberLoops.Text = "Number of loops:";
            this.lblNumberLoops.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(169, 160);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "Suffix:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFadeOutTime
            // 
            this.lblFadeOutTime.Location = new System.Drawing.Point(40, 82);
            this.lblFadeOutTime.Margin = new System.Windows.Forms.Padding(3);
            this.lblFadeOutTime.Name = "lblFadeOutTime";
            this.lblFadeOutTime.Size = new System.Drawing.Size(96, 20);
            this.lblFadeOutTime.TabIndex = 8;
            this.lblFadeOutTime.Text = "Fade-out time";
            this.lblFadeOutTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(169, 134);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "Suffix:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numNumberLoops
            // 
            this.numNumberLoops.Location = new System.Drawing.Point(172, 56);
            this.numNumberLoops.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumberLoops.Name = "numNumberLoops";
            this.numNumberLoops.Size = new System.Drawing.Size(53, 20);
            this.numNumberLoops.TabIndex = 7;
            this.numNumberLoops.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(169, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Suffix:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numFadeOutTime
            // 
            this.numFadeOutTime.DecimalPlaces = 3;
            this.numFadeOutTime.Location = new System.Drawing.Point(172, 82);
            this.numFadeOutTime.Name = "numFadeOutTime";
            this.numFadeOutTime.Size = new System.Drawing.Size(53, 20);
            this.numFadeOutTime.TabIndex = 9;
            // 
            // chkShortCircuit
            // 
            this.chkShortCircuit.AutoSize = true;
            this.chkShortCircuit.Location = new System.Drawing.Point(233, 192);
            this.chkShortCircuit.Name = "chkShortCircuit";
            this.chkShortCircuit.Size = new System.Drawing.Size(259, 17);
            this.chkShortCircuit.TabIndex = 4;
            this.chkShortCircuit.Text = "Skip re-encoding for similar formats when possible";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnLoadOptions);
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.btnOkay);
            this.panel1.Controls.Add(this.btnSaveOptions);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 481);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(534, 30);
            this.panel1.TabIndex = 1;
            // 
            // btnLoadOptions
            // 
            this.btnLoadOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadOptions.Location = new System.Drawing.Point(84, 4);
            this.btnLoadOptions.Name = "btnLoadOptions";
            this.btnLoadOptions.Size = new System.Drawing.Size(100, 23);
            this.btnLoadOptions.TabIndex = 1;
            this.btnLoadOptions.Text = "Load Options";
            this.btnLoadOptions.UseVisualStyleBackColor = true;
            this.btnLoadOptions.Click += new System.EventHandler(this.btnLoadOptions_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(3, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 0;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(457, 4);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(74, 23);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "Start";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnSaveOptions
            // 
            this.btnSaveOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveOptions.Location = new System.Drawing.Point(190, 4);
            this.btnSaveOptions.Name = "btnSaveOptions";
            this.btnSaveOptions.Size = new System.Drawing.Size(100, 23);
            this.btnSaveOptions.TabIndex = 2;
            this.btnSaveOptions.Text = "Save Options";
            this.btnSaveOptions.UseVisualStyleBackColor = true;
            this.btnSaveOptions.Click += new System.EventHandler(this.btnSaveOptions_Click);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 511);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "OptionsForm";
            this.Text = "Looping Audio Converter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPitch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmplifyRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAmplifydB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSampleRate)).EndInit();
            this.panel4.ResumeLayout(false);
            this.pnlExportChannels.ResumeLayout(false);
            this.pnlExportChannels.PerformLayout();
            this.pnlExportSegments.ResumeLayout(false);
            this.pnlExportSegments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumberLoops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeOutTime)).EndInit();
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
        private System.Windows.Forms.Panel pnlExportSegments;
        private System.Windows.Forms.Panel pnlExportChannels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkSampleRate;
        private System.Windows.Forms.CheckBox chkMono;
        private System.Windows.Forms.NumericUpDown numAmplifydB;
        private System.Windows.Forms.CheckBox chkAmplifydB;
        private System.Windows.Forms.NumericUpDown numMaxSampleRate;
        private System.Windows.Forms.NumericUpDown numAmplifyRatio;
        private System.Windows.Forms.CheckBox chkAmplifyRatio;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.Label lblOutputDir;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.TextBox txtSuffixFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddDir;
        private System.Windows.Forms.Button btnSuffixFilter;
        private System.Windows.Forms.Label lblEnumerationStatus;
        private System.Windows.Forms.CheckBox chkShortCircuit;
        private System.Windows.Forms.Button btnSaveOptions;
        private System.Windows.Forms.Button btnLoadOptions;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox ddlUnknownLoopBehavior;
        private System.Windows.Forms.Label lblUnknownLoopBehavior;
        private System.Windows.Forms.Button btnEncodingOptions;
        private System.Windows.Forms.CheckBox chkWriteLoopingMetadata;
		private System.Windows.Forms.NumericUpDown numPitch;
		private System.Windows.Forms.CheckBox chkPitch;
		private System.Windows.Forms.NumericUpDown numTempo;
		private System.Windows.Forms.CheckBox chkTempo;
		private System.Windows.Forms.TextBox txtInputDir;
		private System.Windows.Forms.Label label5;
	}
}