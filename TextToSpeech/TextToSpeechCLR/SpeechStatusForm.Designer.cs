﻿namespace TextToSpeechCLR
{
    partial class SpeechStatusForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.stopButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.statusTabControl = new System.Windows.Forms.TabControl();
            this.outputTabPage = new System.Windows.Forms.TabPage();
            this.queueTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.queueDataGridView = new System.Windows.Forms.DataGridView();
            this.textDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteQueueEntryButton = new System.Windows.Forms.Button();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.logDataGridView = new System.Windows.Forms.DataGridView();
            this.pauseButton = new System.Windows.Forms.Button();
            this.resumeButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.visemeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.phonemeToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.positionToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.voiceToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.spokenTextTextBox = new System.Windows.Forms.TextBox();
            this.currentTextTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.statusTabControl.SuspendLayout();
            this.outputTabPage.SuspendLayout();
            this.queueTabPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.queueDataGridView)).BeginInit();
            this.logTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.stopButton, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.nextButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.backButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.statusTabControl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pauseButton, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.resumeButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.startButton, 4, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(597, 464);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(519, 416);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(195, 416);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 6;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Location = new System.Drawing.Point(114, 416);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 7;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // statusStrip1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 6);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripStatusLabel,
            this.voiceToolStripStatusLabel,
            this.positionToolStripStatusLabel,
            this.toolStripProgressBar1,
            this.visemeToolStripStatusLabel,
            this.phonemeToolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(597, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusToolStripStatusLabel
            // 
            this.statusToolStripStatusLabel.Name = "statusToolStripStatusLabel";
            this.statusToolStripStatusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusToolStripStatusLabel.Text = "Ready";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // statusTabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.statusTabControl, 6);
            this.statusTabControl.Controls.Add(this.outputTabPage);
            this.statusTabControl.Controls.Add(this.queueTabPage);
            this.statusTabControl.Controls.Add(this.logTabPage);
            this.statusTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusTabControl.Location = new System.Drawing.Point(3, 3);
            this.statusTabControl.Name = "statusTabControl";
            this.statusTabControl.SelectedIndex = 0;
            this.statusTabControl.Size = new System.Drawing.Size(591, 407);
            this.statusTabControl.TabIndex = 9;
            // 
            // outputTabPage
            // 
            this.outputTabPage.Controls.Add(this.splitContainer1);
            this.outputTabPage.Location = new System.Drawing.Point(4, 22);
            this.outputTabPage.Name = "outputTabPage";
            this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.outputTabPage.Size = new System.Drawing.Size(583, 381);
            this.outputTabPage.TabIndex = 1;
            this.outputTabPage.Text = "Output";
            this.outputTabPage.UseVisualStyleBackColor = true;
            // 
            // queueTabPage
            // 
            this.queueTabPage.Controls.Add(this.tableLayoutPanel2);
            this.queueTabPage.Location = new System.Drawing.Point(4, 22);
            this.queueTabPage.Name = "queueTabPage";
            this.queueTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.queueTabPage.Size = new System.Drawing.Size(583, 381);
            this.queueTabPage.TabIndex = 0;
            this.queueTabPage.Text = "Queue";
            this.queueTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.queueDataGridView, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.deleteQueueEntryButton, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(577, 375);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // queueDataGridView
            // 
            this.queueDataGridView.AllowUserToAddRows = false;
            this.queueDataGridView.AllowUserToDeleteRows = false;
            this.queueDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.queueDataGridView.ColumnHeadersVisible = false;
            this.queueDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.textDataGridViewTextBoxColumn});
            this.queueDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueDataGridView.Location = new System.Drawing.Point(3, 3);
            this.queueDataGridView.Name = "queueDataGridView";
            this.queueDataGridView.ReadOnly = true;
            this.queueDataGridView.RowHeadersVisible = false;
            this.queueDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.queueDataGridView.Size = new System.Drawing.Size(571, 340);
            this.queueDataGridView.TabIndex = 0;
            // 
            // textDataGridViewTextBoxColumn
            // 
            this.textDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.textDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.textDataGridViewTextBoxColumn.HeaderText = "Text";
            this.textDataGridViewTextBoxColumn.Name = "textDataGridViewTextBoxColumn";
            this.textDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // deleteQueueEntryButton
            // 
            this.deleteQueueEntryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteQueueEntryButton.Location = new System.Drawing.Point(499, 349);
            this.deleteQueueEntryButton.Name = "deleteQueueEntryButton";
            this.deleteQueueEntryButton.Size = new System.Drawing.Size(75, 23);
            this.deleteQueueEntryButton.TabIndex = 1;
            this.deleteQueueEntryButton.Text = "Delete";
            this.deleteQueueEntryButton.UseVisualStyleBackColor = true;
            this.deleteQueueEntryButton.Click += new System.EventHandler(this.deleteQueueEntryButton_Click);
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.logDataGridView);
            this.logTabPage.Location = new System.Drawing.Point(4, 22);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.logTabPage.Size = new System.Drawing.Size(583, 381);
            this.logTabPage.TabIndex = 2;
            this.logTabPage.Text = "Log";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // logDataGridView
            // 
            this.logDataGridView.AllowUserToAddRows = false;
            this.logDataGridView.AllowUserToDeleteRows = false;
            this.logDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logDataGridView.Location = new System.Drawing.Point(3, 3);
            this.logDataGridView.Name = "logDataGridView";
            this.logDataGridView.ReadOnly = true;
            this.logDataGridView.Size = new System.Drawing.Size(577, 375);
            this.logDataGridView.TabIndex = 0;
            // 
            // pauseButton
            // 
            this.pauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseButton.Location = new System.Drawing.Point(357, 416);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(75, 23);
            this.pauseButton.TabIndex = 3;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // resumeButton
            // 
            this.resumeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resumeButton.Location = new System.Drawing.Point(276, 416);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(75, 23);
            this.resumeButton.TabIndex = 4;
            this.resumeButton.Text = "Resume";
            this.resumeButton.UseVisualStyleBackColor = true;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(438, 416);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // visemeToolStripStatusLabel
            // 
            this.visemeToolStripStatusLabel.Name = "visemeToolStripStatusLabel";
            this.visemeToolStripStatusLabel.Size = new System.Drawing.Size(13, 17);
            this.visemeToolStripStatusLabel.Text = "0";
            // 
            // phonemeToolStripStatusLabel
            // 
            this.phonemeToolStripStatusLabel.Name = "phonemeToolStripStatusLabel";
            this.phonemeToolStripStatusLabel.Size = new System.Drawing.Size(12, 17);
            this.phonemeToolStripStatusLabel.Text = "?";
            // 
            // positionToolStripStatusLabel
            // 
            this.positionToolStripStatusLabel.Name = "positionToolStripStatusLabel";
            this.positionToolStripStatusLabel.Size = new System.Drawing.Size(43, 17);
            this.positionToolStripStatusLabel.Text = "0:00:00";
            // 
            // voiceToolStripStatusLabel
            // 
            this.voiceToolStripStatusLabel.Name = "voiceToolStripStatusLabel";
            this.voiceToolStripStatusLabel.Size = new System.Drawing.Size(58, 17);
            this.voiceToolStripStatusLabel.Text = "Unknown";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spokenTextTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.currentTextTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(577, 375);
            this.splitContainer1.SplitterDistance = 212;
            this.splitContainer1.TabIndex = 0;
            // 
            // spokenTextTextBox
            // 
            this.spokenTextTextBox.AcceptsReturn = true;
            this.spokenTextTextBox.AcceptsTab = true;
            this.spokenTextTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spokenTextTextBox.Location = new System.Drawing.Point(0, 0);
            this.spokenTextTextBox.Multiline = true;
            this.spokenTextTextBox.Name = "spokenTextTextBox";
            this.spokenTextTextBox.ReadOnly = true;
            this.spokenTextTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.spokenTextTextBox.Size = new System.Drawing.Size(577, 212);
            this.spokenTextTextBox.TabIndex = 1;
            // 
            // currentTextTextBox
            // 
            this.currentTextTextBox.AcceptsReturn = true;
            this.currentTextTextBox.AcceptsTab = true;
            this.currentTextTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentTextTextBox.Location = new System.Drawing.Point(0, 0);
            this.currentTextTextBox.Multiline = true;
            this.currentTextTextBox.Name = "currentTextTextBox";
            this.currentTextTextBox.ReadOnly = true;
            this.currentTextTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.currentTextTextBox.Size = new System.Drawing.Size(577, 159);
            this.currentTextTextBox.TabIndex = 12;
            // 
            // SpeechStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 464);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SpeechStatusForm";
            this.Text = "Speech Status";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpeechStatusForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SpeechStatusForm_FormClosed);
            this.Shown += new System.EventHandler(this.SpeechStatusForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusTabControl.ResumeLayout(false);
            this.outputTabPage.ResumeLayout(false);
            this.queueTabPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.queueDataGridView)).EndInit();
            this.logTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logDataGridView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button resumeButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusToolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.TabControl statusTabControl;
        private System.Windows.Forms.TabPage queueTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TabPage outputTabPage;
        private System.Windows.Forms.DataGridView queueDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn textDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button deleteQueueEntryButton;
        private System.Windows.Forms.DataGridView logDataGridView;
        private System.Windows.Forms.ToolStripStatusLabel visemeToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel phonemeToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel positionToolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel voiceToolStripStatusLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox spokenTextTextBox;
        private System.Windows.Forms.TextBox currentTextTextBox;
    }
}