namespace PaimanImportFileIntoRDS
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboRDSType = new System.Windows.Forms.ComboBox();
            this.cmdTestConnectionString = new System.Windows.Forms.LinkLabel();
            this.txtRDSConnectionString = new System.Windows.Forms.TextBox();
            this.cmdAddFile = new System.Windows.Forms.LinkLabel();
            this.txtImportFiles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaxNoDataConnections = new System.Windows.Forms.TextBox();
            this.txtErrorFileLog = new System.Windows.Forms.TextBox();
            this.cmdRunProcess = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIndexOfDataColumn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtColumnOverride = new System.Windows.Forms.TextBox();
            this.txtDeliminator = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdClearFileList = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.chkCreateTable = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDBDefaultColumnDataType = new System.Windows.Forms.TextBox();
            this.chkIsExcel = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtExceWorkSheetIndex = new System.Windows.Forms.TextBox();
            this.txt_SQLLineLimit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RDS Type:";
            // 
            // cboRDSType
            // 
            this.cboRDSType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboRDSType.FormattingEnabled = true;
            this.cboRDSType.Location = new System.Drawing.Point(197, 18);
            this.cboRDSType.Name = "cboRDSType";
            this.cboRDSType.Size = new System.Drawing.Size(378, 21);
            this.cboRDSType.TabIndex = 1;
            this.cboRDSType.SelectedIndexChanged += new System.EventHandler(this.CboRDSType_SelectedIndexChanged);
            // 
            // cmdTestConnectionString
            // 
            this.cmdTestConnectionString.AutoSize = true;
            this.cmdTestConnectionString.Location = new System.Drawing.Point(74, 48);
            this.cmdTestConnectionString.Name = "cmdTestConnectionString";
            this.cmdTestConnectionString.Size = new System.Drawing.Size(117, 13);
            this.cmdTestConnectionString.TabIndex = 3;
            this.cmdTestConnectionString.TabStop = true;
            this.cmdTestConnectionString.Text = "RDS connection string:";
            this.cmdTestConnectionString.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CmdTestConnectionString_LinkClicked);
            // 
            // txtRDSConnectionString
            // 
            this.txtRDSConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRDSConnectionString.Location = new System.Drawing.Point(197, 45);
            this.txtRDSConnectionString.Name = "txtRDSConnectionString";
            this.txtRDSConnectionString.Size = new System.Drawing.Size(378, 20);
            this.txtRDSConnectionString.TabIndex = 4;
            // 
            // cmdAddFile
            // 
            this.cmdAddFile.AutoSize = true;
            this.cmdAddFile.Location = new System.Drawing.Point(132, 74);
            this.cmdAddFile.Name = "cmdAddFile";
            this.cmdAddFile.Size = new System.Drawing.Size(59, 13);
            this.cmdAddFile.TabIndex = 5;
            this.cmdAddFile.TabStop = true;
            this.cmdAddFile.Text = "Add File(s):";
            this.cmdAddFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CmdAddFile_LinkClicked);
            // 
            // txtImportFiles
            // 
            this.txtImportFiles.AcceptsReturn = true;
            this.txtImportFiles.AcceptsTab = true;
            this.txtImportFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImportFiles.Location = new System.Drawing.Point(197, 71);
            this.txtImportFiles.Multiline = true;
            this.txtImportFiles.Name = "txtImportFiles";
            this.txtImportFiles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtImportFiles.Size = new System.Drawing.Size(378, 85);
            this.txtImportFiles.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 240);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Max No. of data connections to use:";
            // 
            // txtMaxNoDataConnections
            // 
            this.txtMaxNoDataConnections.Location = new System.Drawing.Point(197, 237);
            this.txtMaxNoDataConnections.Name = "txtMaxNoDataConnections";
            this.txtMaxNoDataConnections.Size = new System.Drawing.Size(100, 20);
            this.txtMaxNoDataConnections.TabIndex = 8;
            this.txtMaxNoDataConnections.Text = "3";
            // 
            // txtErrorFileLog
            // 
            this.txtErrorFileLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtErrorFileLog.Location = new System.Drawing.Point(197, 496);
            this.txtErrorFileLog.Multiline = true;
            this.txtErrorFileLog.Name = "txtErrorFileLog";
            this.txtErrorFileLog.ReadOnly = true;
            this.txtErrorFileLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErrorFileLog.Size = new System.Drawing.Size(378, 106);
            this.txtErrorFileLog.TabIndex = 17;
            // 
            // cmdRunProcess
            // 
            this.cmdRunProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRunProcess.Location = new System.Drawing.Point(15, 393);
            this.cmdRunProcess.Name = "cmdRunProcess";
            this.cmdRunProcess.Size = new System.Drawing.Size(176, 23);
            this.cmdRunProcess.TabIndex = 14;
            this.cmdRunProcess.Text = "Run Process";
            this.cmdRunProcess.UseVisualStyleBackColor = true;
            this.cmdRunProcess.Click += new System.EventHandler(this.CmdRunProcess_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdExit.Location = new System.Drawing.Point(12, 584);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 15;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.CmdExit_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(197, 393);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(378, 97);
            this.txtLog.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 292);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Index of First Row of Data:";
            // 
            // txtIndexOfDataColumn
            // 
            this.txtIndexOfDataColumn.Location = new System.Drawing.Point(197, 289);
            this.txtIndexOfDataColumn.Name = "txtIndexOfDataColumn";
            this.txtIndexOfDataColumn.Size = new System.Drawing.Size(100, 20);
            this.txtIndexOfDataColumn.TabIndex = 19;
            this.txtIndexOfDataColumn.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 315);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Column Header Override:";
            // 
            // txtColumnOverride
            // 
            this.txtColumnOverride.AcceptsReturn = true;
            this.txtColumnOverride.AcceptsTab = true;
            this.txtColumnOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtColumnOverride.Location = new System.Drawing.Point(197, 315);
            this.txtColumnOverride.Multiline = true;
            this.txtColumnOverride.Name = "txtColumnOverride";
            this.txtColumnOverride.Size = new System.Drawing.Size(378, 46);
            this.txtColumnOverride.TabIndex = 21;
            // 
            // txtDeliminator
            // 
            this.txtDeliminator.Location = new System.Drawing.Point(197, 367);
            this.txtDeliminator.Name = "txtDeliminator";
            this.txtDeliminator.Size = new System.Drawing.Size(100, 20);
            this.txtDeliminator.TabIndex = 23;
            this.txtDeliminator.Text = "|";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 370);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Deliminator:";
            // 
            // cmdClearFileList
            // 
            this.cmdClearFileList.AutoSize = true;
            this.cmdClearFileList.Location = new System.Drawing.Point(161, 143);
            this.cmdClearFileList.Name = "cmdClearFileList";
            this.cmdClearFileList.Size = new System.Drawing.Size(30, 13);
            this.cmdClearFileList.TabIndex = 24;
            this.cmdClearFileList.TabStop = true;
            this.cmdClearFileList.Text = "clear";
            this.cmdClearFileList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CmdClearFileList_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(125, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Table name:";
            // 
            // txtTableName
            // 
            this.txtTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableName.Location = new System.Drawing.Point(197, 162);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(378, 20);
            this.txtTableName.TabIndex = 26;
            // 
            // chkCreateTable
            // 
            this.chkCreateTable.AutoSize = true;
            this.chkCreateTable.Location = new System.Drawing.Point(197, 188);
            this.chkCreateTable.Name = "chkCreateTable";
            this.chkCreateTable.Size = new System.Drawing.Size(112, 17);
            this.chkCreateTable.TabIndex = 27;
            this.chkCreateTable.Text = "Create New Table";
            this.chkCreateTable.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(78, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Data type for columns:";
            // 
            // txtDBDefaultColumnDataType
            // 
            this.txtDBDefaultColumnDataType.Location = new System.Drawing.Point(197, 211);
            this.txtDBDefaultColumnDataType.Name = "txtDBDefaultColumnDataType";
            this.txtDBDefaultColumnDataType.Size = new System.Drawing.Size(378, 20);
            this.txtDBDefaultColumnDataType.TabIndex = 29;
            this.txtDBDefaultColumnDataType.Text = "nvarchar(MAX)";
            // 
            // chkIsExcel
            // 
            this.chkIsExcel.AutoSize = true;
            this.chkIsExcel.Location = new System.Drawing.Point(304, 368);
            this.chkIsExcel.Name = "chkIsExcel";
            this.chkIsExcel.Size = new System.Drawing.Size(115, 17);
            this.chkIsExcel.TabIndex = 30;
            this.chkIsExcel.Text = "Is Excel Document";
            this.chkIsExcel.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(426, 369);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Work sheet index:";
            // 
            // txtExceWorkSheetIndex
            // 
            this.txtExceWorkSheetIndex.Location = new System.Drawing.Point(525, 367);
            this.txtExceWorkSheetIndex.Name = "txtExceWorkSheetIndex";
            this.txtExceWorkSheetIndex.Size = new System.Drawing.Size(50, 20);
            this.txtExceWorkSheetIndex.TabIndex = 32;
            this.txtExceWorkSheetIndex.Text = "0";
            // 
            // txt_SQLLineLimit
            // 
            this.txt_SQLLineLimit.Location = new System.Drawing.Point(197, 263);
            this.txt_SQLLineLimit.Name = "txt_SQLLineLimit";
            this.txt_SQLLineLimit.Size = new System.Drawing.Size(100, 20);
            this.txt_SQLLineLimit.TabIndex = 34;
            this.txt_SQLLineLimit.Text = "25000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(108, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "SQL Line Limits:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 614);
            this.Controls.Add(this.txt_SQLLineLimit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtExceWorkSheetIndex);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkIsExcel);
            this.Controls.Add(this.txtDBDefaultColumnDataType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkCreateTable);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmdClearFileList);
            this.Controls.Add(this.txtDeliminator);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtColumnOverride);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtIndexOfDataColumn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtErrorFileLog);
            this.Controls.Add(this.cmdRunProcess);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtMaxNoDataConnections);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtImportFiles);
            this.Controls.Add(this.cmdAddFile);
            this.Controls.Add(this.txtRDSConnectionString);
            this.Controls.Add(this.cmdTestConnectionString);
            this.Controls.Add(this.cboRDSType);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Paiman\'s Import File Into RDS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRDSType;
        private System.Windows.Forms.LinkLabel cmdTestConnectionString;
        private System.Windows.Forms.TextBox txtRDSConnectionString;
        private System.Windows.Forms.LinkLabel cmdAddFile;
        private System.Windows.Forms.TextBox txtImportFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaxNoDataConnections;
        private System.Windows.Forms.TextBox txtErrorFileLog;
        private System.Windows.Forms.Button cmdRunProcess;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIndexOfDataColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtColumnOverride;
        private System.Windows.Forms.TextBox txtDeliminator;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel cmdClearFileList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.CheckBox chkCreateTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDBDefaultColumnDataType;
        private System.Windows.Forms.CheckBox chkIsExcel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtExceWorkSheetIndex;
        private System.Windows.Forms.TextBox txt_SQLLineLimit;
        private System.Windows.Forms.Label label9;
    }
}

