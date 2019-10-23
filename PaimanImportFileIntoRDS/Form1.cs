using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace PaimanImportFileIntoRDS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Build_cboRDSType();
        }

        public void Build_cboRDSType()
        {
            try
            {
                try
                {
                    cboRDSType.Items.Clear();
                }
                catch { }
                int iId = 0;
                ComboBoxItems oItem = new ComboBoxItems();
                oItem.Value = iId.ToString();
                oItem.Text = "MS SQL";

                this.cboRDSType.Items.Add(oItem);

                iId++;
                oItem = new ComboBoxItems();
                oItem.Value = iId.ToString();
                oItem.Text = "POSTGRE";

                this.cboRDSType.Items.Add(oItem);


            }
            catch
            {

            }
        }

        private void CmdExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CmdTestConnectionString_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ComboBoxItems oSelected = (ComboBoxItems)this.cboRDSType.SelectedItem;
                if (oSelected != null)
                {
                    int iConnectionType = System.Convert.ToInt32(oSelected.Value);

                    //validate three is a connection string before testing.
                    if (!String.IsNullOrEmpty(txtRDSConnectionString.Text))
                    {
                        int iValue = System.Convert.ToInt32(oSelected.Value);
                        switch (iValue)
                        {
                            case 0://MS SQL
                                using (SqlConnection connection = new SqlConnection(txtRDSConnectionString.Text))
                                {
                                    connection.Open();

                                    MessageBox.Show("Good Connection!");
                                }
                                break;
                            case 1://Postgre
                                using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(txtRDSConnectionString.Text))
                                {
                                    connection.Open();

                                    MessageBox.Show("Good Connection!");
                                }
                                break;
                            case 2://ODBC
                                using (OdbcConnection connection = new OdbcConnection(txtRDSConnectionString.Text))
                                {
                                    connection.Open();

                                    MessageBox.Show("Good Connection!");
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception s)
            {
                MessageBox.Show("Process Error: " + s.ToString());
            }
        }

        private void CmdAddFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog opImportFile = new System.Windows.Forms.OpenFileDialog();
            opImportFile.ShowDialog();
            if (!String.IsNullOrEmpty(opImportFile.FileName))
            {
                if (String.IsNullOrEmpty(this.txtImportFiles.Text))
                {
                    this.txtImportFiles.Text += opImportFile.FileName;
                }
                else
                {
                    this.txtImportFiles.Text += "," + opImportFile.FileName;
                }
            }
            opImportFile = null;
        }

        int iCount = 0;
        int iCountLimit = 1000000;
        void UpdateTextBoxWithInfo(string _sAddText)
        {
            try
            {
                if (_sAddText != "")
                {
                    if (txtLog.InvokeRequired)
                    {
                        if (iCount > iCountLimit)
                        {
                            txtLog.Invoke(new MethodInvoker(delegate { txtLog.Text = _sAddText; }));
                            iCount = 0;
                        }
                        else
                        {
                            txtLog.Invoke(new MethodInvoker(delegate { txtLog.Text = _sAddText + Environment.NewLine + txtLog.Text; }));
                            iCount++;
                        }
                    }
                }
            }
            catch (Exception s)
            {
            }
        }
        void UpdateTextBoxWithErrorInfo(string _sAddText)
        {
            try
            {
                if (_sAddText != "")
                {
                    if (txtErrorFileLog.InvokeRequired)
                    {
                        txtErrorFileLog.Invoke(new MethodInvoker(delegate { txtErrorFileLog.Text = _sAddText + Environment.NewLine + txtErrorFileLog.Text; }));
                    }
                }
            }
            catch (Exception s)
            {
            }
        }
        void UpdatecmdRunProcess()
        {
            try
            {
                if (cmdRunProcess.InvokeRequired)
                {
                    cmdRunProcess.Invoke(new MethodInvoker(delegate { cmdRunProcess.Enabled = true; }));
                }
            }
            catch (Exception s)
            {
            }
        }
        private void CmdRunProcess_Click(object sender, EventArgs e)
        {
            ComboBoxItems oSelected = (ComboBoxItems)this.cboRDSType.SelectedItem;
            if (oSelected != null)
            {
                cmdRunProcess.Enabled = false;

                int iConnectionType = System.Convert.ToInt32(oSelected.Value);
                string sConnectionString = this.txtRDSConnectionString.Text;

                string sImportFiles = this.txtImportFiles.Text;
                string sNewTableName = this.txtTableName.Text;
                string sDeliminator = this.txtDeliminator.Text;
                string sDBDefaultColumnDataType = this.txtDBDefaultColumnDataType.Text;
                string sColumnOverride = this.txtColumnOverride.Text;

                bool bFileIsExcel = false;

                bFileIsExcel = this.chkIsExcel.Checked;

                int iWorkBookIndex = 0;
                try
                {
                    iWorkBookIndex = System.Convert.ToInt32(txtExceWorkSheetIndex.Text);
                }
                catch { }

                int iSQLLineLimit = 25000;
                try
                {
                    iSQLLineLimit = System.Convert.ToInt32(txt_SQLLineLimit.Text);
                }
                catch { }

                int iMaxNoDataConnections = 3;
                try
                {
                    iMaxNoDataConnections = System.Convert.ToInt32(txtMaxNoDataConnections.Text);
                }
                catch { }

                bool bCreateTable = false;
                try
                {
                    bCreateTable = this.chkCreateTable.Checked;
                }
                catch { }


                int iMaxNoConnections = System.Convert.ToInt32(this.txtMaxNoDataConnections.Text);

                RunImportFilesIntoRDS oSQLRunner = new RunImportFilesIntoRDS(iConnectionType, sConnectionString, bCreateTable, sImportFiles,
                    sNewTableName, sDeliminator, sDBDefaultColumnDataType, sColumnOverride, iMaxNoDataConnections, iSQLLineLimit, bFileIsExcel);
                oSQLRunner.OnProcessCompleted += OSQLRunner_OnProcessCompleted;
                oSQLRunner.OnProgressReport += OSQLRunner_OnProgressReport;
                oSQLRunner.OnRaisError += OSQLRunner_OnRaisError;

                if (bFileIsExcel)
                {
                    new Thread(() =>
                    {
                        oSQLRunner.Import_ExcelFilesIntoRds();
                    }).Start();
                }
                else
                {
                    new Thread(() =>
                    {
                        oSQLRunner.Import_TextFilesIntoRds();
                    }).Start();
                }

            }
            else
            {
                MessageBox.Show("Please select data connection type, and set connection string");
            }
        }


        private void OSQLRunner_OnRaisError(object sender, EventArgs e)
        {
            RunImportFilesIntoRDS.ProgressReport _evArgs = (RunImportFilesIntoRDS.ProgressReport)e;
            if (_evArgs != null)
            {
                UpdateTextBoxWithErrorInfo(_evArgs.Mesage);
            }
        }

        private void OSQLRunner_OnProgressReport(object sender, EventArgs e)
        {
            RunImportFilesIntoRDS.ProgressReport _evArgs = (RunImportFilesIntoRDS.ProgressReport)e;
            if (_evArgs != null)
            {
                UpdateTextBoxWithInfo(_evArgs.Mesage);
            }
        }

        private void OSQLRunner_OnProcessCompleted(object sender, EventArgs e)
        {
            RunImportFilesIntoRDS.ProgressReport _evArgs = (RunImportFilesIntoRDS.ProgressReport)e;
            if (_evArgs != null)
            {
                UpdateTextBoxWithInfo("Process Completed!!");
                UpdatecmdRunProcess();
            }
        }

        private void CmdClearFileList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.txtImportFiles.Text = "";
        }

        private void CboRDSType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxItems oSelected = (ComboBoxItems)this.cboRDSType.SelectedItem;
            if (oSelected != null)
            {
                int iValue = System.Convert.ToInt32(oSelected.Value);
                switch (iValue)
                {
                    case 0://MS SQL
                        this.txtDBDefaultColumnDataType.Text = "nvarchar(MAX)";
                        break;
                    case 1://Postgre
                        this.txtDBDefaultColumnDataType.Text = "text";// or varchar(10485760)
                        break;
                    case 2://ODBC
                        this.txtDBDefaultColumnDataType.Text = "SQL_VARCHAR";
                        break;
                }
            }

            
        }
        RunImportFilesIntoRDS oSQLRunner = null;
        private void Button1_Click(object sender, EventArgs e)
        {
            ComboBoxItems oSelected = (ComboBoxItems)this.cboRDSType.SelectedItem;
            if (oSelected != null)
            {
                cmdRunProcess.Enabled = false;

                int iConnectionType = System.Convert.ToInt32(oSelected.Value);
                string sConnectionString = this.txtRDSConnectionString.Text;

                string sImportFiles = this.txtImportFiles.Text;
                string sNewTableName = this.txtTableName.Text;
                string sDeliminator = this.txtDeliminator.Text;
                string sDBDefaultColumnDataType = this.txtDBDefaultColumnDataType.Text;
                string sColumnOverride = this.txtColumnOverride.Text;

                bool bFileIsExcel = false;

                bFileIsExcel = this.chkIsExcel.Checked;

                int iWorkBookIndex = 0;
                try
                {
                    iWorkBookIndex = System.Convert.ToInt32(txtExceWorkSheetIndex.Text);
                }
                catch { }

                int iSQLLineLimit = 25000;
                try
                {
                    iSQLLineLimit = System.Convert.ToInt32(txt_SQLLineLimit.Text);
                }
                catch { }

                int iMaxNoDataConnections = 3;
                try
                {
                    iMaxNoDataConnections = System.Convert.ToInt32(txtMaxNoDataConnections.Text);
                }
                catch { }

                bool bCreateTable = false;
                try
                {
                    bCreateTable = this.chkCreateTable.Checked;
                }
                catch { }


                int iMaxNoConnections = System.Convert.ToInt32(this.txtMaxNoDataConnections.Text);

                oSQLRunner = new RunImportFilesIntoRDS(iConnectionType, sConnectionString, bCreateTable, sImportFiles,
                    sNewTableName, sDeliminator, sDBDefaultColumnDataType, sColumnOverride, iMaxNoDataConnections, iSQLLineLimit, bFileIsExcel);
                oSQLRunner.OnProcessCompleted += OSQLRunner_OnProcessCompleted;
                oSQLRunner.OnProgressReport += OSQLRunner_OnProgressReport;
                oSQLRunner.OnRaisError += OSQLRunner_OnRaisError;


            }
            else
            {
                MessageBox.Show("Please select data connection type, and set connection string");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            oSQLRunner.DecrementStuff();
        }
    }

    [Serializable]
    public class ComboBoxItems
    {
        object sID = null;
        string sName = "";
        public object Value
        {
            get
            {
                return sID;
            }
            set
            {
                sID = value;
            }
        }
        public string Text
        {
            get
            {
                return sName;
            }
            set
            {
                sName = value;
            }
        }
        public override string ToString()
        {
            return sName;
        }
    }

    public class RunImportFilesIntoRDS
    {
        string InstanceID = "";
        #region Process Events

        /// <summary>
        /// General Class For Reporting Progess via Events
        /// </summary>
        [Serializable]
        public class ProgressReport : System.EventArgs
        {
            string Id = "";
            public string SourceID
            {
                get { return Id; }
                set { Id = value; }
            }

            string _name = "";
            public string SourceName
            {
                get { return _name; }
                set { _name = value; }
            }

            string _sMessage = "";
            public string Mesage
            {
                get
                {
                    return _sMessage;
                }
                set
                {
                    _sMessage = value;
                }
            }

            int _code = 200;
            public int Code
            {
                get { return _code; }
                set { _code = value; }
            }

            int _TotalCount = 0;
            public int TotalCount
            {
                get { return _TotalCount; }
                set { _TotalCount = value; }
            }

            int _CurrentRow = 0;
            public int CurrentRow
            {
                get { return _CurrentRow; }
                set { _CurrentRow = value; }
            }

            public ProgressReport()
            {

            }
        }


        /// <summary>
        /// Event for progress report
        /// </summary>
        [field: NonSerialized]
        public event EventHandler OnProgressReport;

        /// <summary>
        /// Raise Event for progress report
        /// </summary>
        /// <param name="oTick"></param>
        protected void Raise_OnProgressReport(int ProcessCode, string ProcessMessage, int TotalCount = 0, int CurrentRow = 0)
        {
            try
            {
                if (OnProgressReport != null)
                {
                    ProgressReport _evArgs = new ProgressReport();
                    _evArgs.SourceID = InstanceID;
                    _evArgs.Mesage = ProcessMessage;
                    _evArgs.Code = ProcessCode;
                    _evArgs.TotalCount = TotalCount;
                    _evArgs.CurrentRow = CurrentRow;

                    OnProgressReport(this, _evArgs);
                }
            }
            catch (Exception s)
            {
            }
        }


        /// <summary>
        /// Event for process completed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler OnProcessCompleted;

        /// <summary>
        /// Raise Event for process completed
        /// </summary>
        /// <param name="oTick"></param>
        protected void Raise_OnProcessCompleted(int ProcessCode, string ProcessMessage, int TotalCount = 0, int CurrentRow = 0)
        {
            try
            {
                if (OnProcessCompleted != null)
                {
                    ProgressReport _evArgs = new ProgressReport();
                    _evArgs.SourceID = InstanceID;
                    _evArgs.Mesage = ProcessMessage;
                    _evArgs.Code = ProcessCode;
                    _evArgs.TotalCount = TotalCount;
                    _evArgs.CurrentRow = CurrentRow;

                    OnProcessCompleted(this, _evArgs);
                }
            }
            catch (Exception s)
            {
            }
        }

        /// <summary>
        /// Event for process error
        /// </summary>
        [field: NonSerialized]
        public event EventHandler OnRaisError;

        /// <summary>
        /// Raise Event for process error
        /// </summary>
        /// <param name="oTick"></param>
        protected void Raise_OnRaisError(int ProcessCode, string ProcessMessage, int TotalCount = 0, int CurrentRow = 0)
        {
            try
            {
                if (OnProcessCompleted != null)
                {
                    ProgressReport _evArgs = new ProgressReport();
                    _evArgs.SourceID = InstanceID;
                    _evArgs.Mesage = ProcessMessage;
                    _evArgs.Code = ProcessCode;
                    _evArgs.TotalCount = TotalCount;
                    _evArgs.CurrentRow = CurrentRow;

                    OnRaisError(this, _evArgs);
                }
            }
            catch (Exception s)
            {
            }
        }
        #endregion


        int NumOfSQLConnections = 0;
        int MaxNumOfSQLConnectionsAllowed = 3;

        //0 - MS SQL, 1 - Postgre
        int ConnectionType = 0;
        string ConnectionString = "";
        string DataFilesFile = "";
        string[] DataFiles = null;
        bool bCreateNewTable = false;
        string sTableName = "";
        string sColumnDefaultDataType = "";
        string sOverrideColumnHeaders = "";
        string DataSourceFileDeliminator = "";

        bool bIsExcelDoc = false;
        int iWorkBookIndex = 0;

        int iMaxThreadsToBeExecuting = 10;
        int iSQLLineLimit = 25000;


        public RunImportFilesIntoRDS(int _ConnectionType, string _ConnectionString, bool _CreateNewTable, string _DataFiles, string _TableName, string _ColumnDeliminator, 
            string _ColumnDefaultDataType = "", string _OverrideColumnHeaders = "", int _MaxNumberOfConnections = 3, int _SQLLineLimit = 25000, bool IsExcelDoc = false, int _WorkBookIndex = 0)
        {
            iSQLLineLimit = _SQLLineLimit;
            iMaxThreadsToBeExecuting = _MaxNumberOfConnections;
            iWorkBookIndex = _WorkBookIndex;
            bIsExcelDoc = IsExcelDoc;
            DataFilesFile = _DataFiles;
            DataSourceFileDeliminator = _ColumnDeliminator;
            sOverrideColumnHeaders = _OverrideColumnHeaders;
            bCreateNewTable = _CreateNewTable;
            sTableName = _TableName;
            sColumnDefaultDataType = _ColumnDefaultDataType;
            ConnectionType = _ConnectionType;
            ConnectionString = _ConnectionString;
            MaxNumOfSQLConnectionsAllowed = _MaxNumberOfConnections;
        }

        public string sProcessError = "";

        public void Import_TextFilesIntoRds()
        {
            try
            {
                string sMasterSQL = "";
                DateTime dtStartOn = DateTime.Now;
                TimeSpan dtTimeDiff = TimeSpan.MinValue;
                if (!String.IsNullOrEmpty(DataFilesFile))
                {
                    DataFiles = DataFilesFile.Split(',');

                    if (DataFiles != null)
                    {
                        for (int i = 0; i < DataFiles.Length; i++)
                        {
                            int iTotalLines = 0;
                            int iLinesRead = 0;
                            int iLinesProcess = 0;
                            int iErrorsCount = 0;
                            int iSQLLines = 0;
                            bool bSQLExecuted = false;
                            bool bHeaderColumnsSet = false;
                            bool bDBCreated = false;
                            string currentLine = "";

                            iTotalLines = File.ReadLines(DataFiles[i]).Count();

                            using (StreamReader oFile = new StreamReader(DataFiles[i]))
                            {
                                FileToRDS oImporter = new FileToRDS();
                                oImporter.DataDef = new FileToRDS.Tables();

                                string sLocTableName = "";

                                if (String.IsNullOrEmpty(sTableName))
                                {
                                    try
                                    {
                                        sLocTableName = Path.GetFileNameWithoutExtension(DataFiles[i]);
                                        sLocTableName = "dbimp_" + sLocTableName.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_").Replace(" ", "_").Replace(":", "_").Replace("[", "_").Replace("]", "_").Replace("{", "_").Replace("}", "_");
                                        sLocTableName = sLocTableName.TrimEnd('_');
                                        sLocTableName = sLocTableName + "_fl_no_" + i.ToString();
                                        if (sLocTableName.Length > 20)
                                        {
                                            sLocTableName = "dbimp_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString();
                                            sLocTableName = sLocTableName.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_").Replace(" ", "_").Replace(":", "_").Replace("[", "_").Replace("]", "_").Replace("{", "_").Replace("}", "_");
                                            sLocTableName = sLocTableName.TrimEnd('_');
                                            sLocTableName = sLocTableName + "_fl_no_" + i.ToString();
                                        }
                                    }
                                    catch { }
                                }
                                else
                                {
                                    sLocTableName = sTableName + "_fl_no_" + i.ToString();
                                }

                                oImporter.DataDef.TableName = sLocTableName;

                                iLinesRead = 0;
                                iLinesProcess = 0;
                                iErrorsCount = 0;
                                iSQLLines = 0;
                                bSQLExecuted = false;
                                bHeaderColumnsSet = false;
                                bDBCreated = false;
                                currentLine = "";


                                // currentLine will be null when the StreamReader reaches the end of file
                                while ((currentLine = oFile.ReadLine()) != null)
                                {
                                    try
                                    {
                                        if (!bHeaderColumnsSet)
                                        {
                                            if (!String.IsNullOrEmpty(sOverrideColumnHeaders))
                                            {
                                                bHeaderColumnsSet = oImporter.DataDef.CreateHeadersFromString(sOverrideColumnHeaders, DataSourceFileDeliminator, null);
                                            }
                                            else
                                            {
                                                bHeaderColumnsSet = oImporter.DataDef.CreateHeadersFromString(currentLine, DataSourceFileDeliminator, null);
                                            }
                                        }

                                        //first we want to see if we need to build a script to create the table and it's columns
                                        if (bCreateNewTable)
                                        {
                                            if (!bDBCreated)
                                            {
                                                if (bHeaderColumnsSet)
                                                {
                                                    bDBCreated = true;
                                                    string sCreate_SQL = oImporter.DataDef.Build_CreateTable_SQL(sColumnDefaultDataType);

                                                    do
                                                    {
                                                    } while (iThreadsExecuting > iMaxThreadsToBeExecuting);
                                                    System.Threading.Interlocked.Increment(ref iThreadsExecuting);

                                                    EXE_SQL(sCreate_SQL);

                                                }
                                            }
                                        }

                                        if (bHeaderColumnsSet)
                                        {
                                            if (iLinesRead > 0)
                                            {
                                                oImporter.Last_AddRowError = "";
                                                if (oImporter.Add_Row(currentLine, DataSourceFileDeliminator, null))
                                                {
                                                    bSQLExecuted = false;
                                                    string sSQL = "";
                                                    sSQL = oImporter.Build_SQL_Last_Row_Statements(FileToRDS.SQLStatementType.Insert);
                                                    sMasterSQL += sSQL + Environment.NewLine;
                                                    iSQLLines++;
                                                    iLinesProcess++;

                                                    if (!String.IsNullOrEmpty(oImporter.Last_AddRowError))
                                                    {
                                                        Raise_OnRaisError(200, "File: [" + Path.GetFileNameWithoutExtension(DataFiles[i]) + "] row: " + iLinesRead + "  Import process row error: " + oImporter.Last_AddRowError, iLinesRead, iTotalLines);
                                                    }
                                                }
                                                else
                                                {
                                                    iErrorsCount++;
                                                    Raise_OnRaisError(200, "Import process row: " + iLinesRead + "  error: " + oImporter.Last_AddRowError, iLinesRead, iTotalLines);
                                                }


                                                if (iSQLLines >= iSQLLineLimit)
                                                {
                                                    do
                                                    {
                                                    } while (iThreadsExecuting > iMaxThreadsToBeExecuting);

                                                    if (!String.IsNullOrEmpty(sMasterSQL))
                                                    {
                                                        System.Threading.Interlocked.Increment(ref iThreadsExecuting);
                                                        string sTempSql = sMasterSQL;

                                                        new Thread(() =>
                                                        {
                                                            EXE_SQL(sTempSql);
                                                        }).Start();
                                                    }
                                                    sMasterSQL = "";
                                                    bSQLExecuted = true;
                                                    iSQLLines = 0;

                                                    oImporter.DataRows = new List<FileToRDS.TableRows>();
                                                }
                                                dtTimeDiff = DateTime.Now - dtStartOn;
                                                Raise_OnProgressReport(200, "Processed Line " + iLinesRead.ToString() + " | " + iTotalLines.ToString() + Environment.NewLine + "Time Elapsed: " + dtTimeDiff.TotalDays.ToString() + " Days, " + Environment.NewLine + dtTimeDiff.Hours.ToString() + " hours, " + dtTimeDiff.Minutes.ToString() + " min., " + dtTimeDiff.Seconds.ToString(), iLinesRead, iTotalLines);
                                            }
                                        }
                                    }
                                    catch (Exception s1)
                                    {
                                        string sInnderException = "";
                                        if (s1.InnerException != null)
                                        {
                                            sInnderException = s1.InnerException.ToString();
                                        }
                                        sProcessError += "File Read Process Exception :" + s1.ToString();
                                        if (!String.IsNullOrEmpty(sInnderException))
                                        {
                                            sProcessError += " File Read INNER EXCEPTION:" + sInnderException;
                                        }
                                        Raise_OnRaisError(500, sProcessError, 0, 0);
                                        Raise_OnProcessCompleted(500, sProcessError, 0, 0);
                                    }

                                    iLinesRead++;
                                }
                            }
                            if (((iSQLLines > 0) || (!bSQLExecuted)) || (!String.IsNullOrEmpty(sMasterSQL)))
                            {
                                if (!String.IsNullOrEmpty(sMasterSQL))
                                {
                                    System.Threading.Interlocked.Increment(ref iThreadsExecuting);
                                    string sTempSql = sMasterSQL;

                                    //new Thread(() =>
                                    //{
                                        EXE_SQL(sTempSql);
                                    //}).Start();
                                }
                                sMasterSQL = "";
                                bSQLExecuted = true;
                                iSQLLines = 0;
                            }

                            dtTimeDiff = DateTime.Now - dtStartOn;
                        }

                        int iWaitCounter = 0;
                        do
                        {
                            if (iWaitCounter <= 0)
                            {
                                Raise_OnProgressReport(200, "Waiting for all threads to complete!", iThreadsExecuting, iMaxThreadsToBeExecuting);
                                iWaitCounter = 240000000;
                            }

                            iWaitCounter--;
                        } while (iThreadsExecuting > 0);

                        Raise_OnProcessCompleted(200, "Process Completed!", 0, 0);
                    }
                    else
                    {

                        Raise_OnRaisError(500, "Files Is Null or Empty!", 0, 0);
                        Raise_OnProcessCompleted(500, "Files Is Null or Empty!", 0, 0);
                    }
                }
                else
                {

                    Raise_OnRaisError(500, "Files Is Null or Empty!", 0, 0);
                    Raise_OnProcessCompleted(500, "Files Is Null or Empty!", 0, 0);
                }

            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                sProcessError += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    sProcessError += " INNER EXCEPTION:" + sInnderException;
                }
                Raise_OnRaisError(500, sProcessError, 0, 0);
                Raise_OnProcessCompleted(500, sProcessError, 0, 0);
            }
        }

        public void Import_ExcelFilesIntoRds()
        {
            try
            {
                string sMasterSQL = "";
                DateTime dtStartOn = DateTime.Now;
                TimeSpan dtTimeDiff = TimeSpan.MinValue;
                if (!String.IsNullOrEmpty(DataFilesFile))
                {
                    DataFiles = DataFilesFile.Split(',');

                    if (DataFiles != null)
                    {
                        for (int i = 0; i < DataFiles.Length; i++)
                        {
                            FileToRDS oImporter = new FileToRDS();
                            oImporter.DataDef = new FileToRDS.Tables();

                            string sLocTableName = "";

                            if (String.IsNullOrEmpty(sTableName))
                            {
                                try
                                {
                                    sLocTableName = Path.GetFileNameWithoutExtension(DataFiles[i]);
                                    sLocTableName = "dbimp_" + sLocTableName.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_").Replace(" ", "_").Replace(":", "_").Replace("[", "_").Replace("]", "_").Replace("{", "_").Replace("}", "_");
                                    sLocTableName = sLocTableName.TrimEnd('_');
                                    sLocTableName = sLocTableName + "_fl_no_" + i.ToString();
                                    if (sLocTableName.Length > 20)
                                    {
                                        sLocTableName = "dbimp_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString();
                                        sLocTableName = sLocTableName.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_").Replace(" ", "_").Replace(":", "_").Replace("[", "_").Replace("]", "_").Replace("{", "_").Replace("}", "_");
                                        sLocTableName = sLocTableName.TrimEnd('_');
                                        sLocTableName = sLocTableName + "_fl_no_" + i.ToString();
                                    }
                                }
                                catch(Exception s11)
                                {
                                }
                            }
                            else
                            {
                                sLocTableName = sTableName + "_fl_no_" + i.ToString();
                            }

                            oImporter.DataDef.TableName = sLocTableName;

                            int iDataLinesRead = 0;
                            int iLinesProcess = 0;
                            int iTotalLines = 0;
                            int iErrorsCount = 0;
                            int iSQLLines = 0;
                            bool bSQLExecuted = false;
                            bool bHeaderColumnsSet = false;
                            bool bDBCreated = false;
                            string currentLine = "";

                            IWorkbook oFile = new XSSFWorkbook(DataFiles[i]);
                            ISheet oWorkSheet = oFile.GetSheetAt(iWorkBookIndex);

                            iLinesProcess = 0;
                            iErrorsCount = 0;
                            iSQLLines = 0;
                            bSQLExecuted = false;
                            bHeaderColumnsSet = false;
                            bDBCreated = false;
                            currentLine = "";
                            iTotalLines = oWorkSheet.LastRowNum;


                            for ( int iLinesRead = 0; iLinesRead < oWorkSheet.LastRowNum; iLinesRead++)
                            {
                                IRow oRow = oWorkSheet.GetRow(iLinesRead);
                                if (oRow.Cells != null)
                                {
                                    if (oRow.Cells.Count > 0)
                                    {
                                        try
                                        {
                                            if (!bHeaderColumnsSet)
                                            {
                                                if (!String.IsNullOrEmpty(sOverrideColumnHeaders))
                                                {
                                                    bHeaderColumnsSet = oImporter.DataDef.CreateHeadersFromString(sOverrideColumnHeaders, DataSourceFileDeliminator, null);
                                                    iDataLinesRead = 0;
                                                }
                                                else
                                                {
                                                    bHeaderColumnsSet = oImporter.DataDef.CreateHeadersFromExcelCells(oRow, null);
                                                    iDataLinesRead = 0;
                                                }
                                            }

                                            //first we want to see if we need to build a script to create the table and it's columns
                                            if (bCreateNewTable)
                                            {
                                                if (!bDBCreated)
                                                {
                                                    if (bHeaderColumnsSet)
                                                    {
                                                        bDBCreated = true;
                                                        string sCreate_SQL = oImporter.DataDef.Build_CreateTable_SQL(sColumnDefaultDataType);
                                                        do
                                                        {
                                                        } while (iThreadsExecuting > iMaxThreadsToBeExecuting);
                                                        System.Threading.Interlocked.Increment(ref iThreadsExecuting);
                                                        EXE_SQL(sCreate_SQL);
                                                    }
                                                }
                                            }

                                            if (bHeaderColumnsSet)
                                            {
                                                if (iDataLinesRead > 0)
                                                {
                                                    oImporter.Last_AddRowError = "";
                                                    if (oImporter.Add_Row(oRow, null))
                                                    {
                                                        bSQLExecuted = false;
                                                        string sSQL = "";
                                                        sSQL = oImporter.Build_SQL_Last_Row_Statements(FileToRDS.SQLStatementType.Insert);
                                                        sMasterSQL += sSQL + Environment.NewLine;
                                                        iSQLLines++;
                                                        iLinesProcess++;

                                                        if (!String.IsNullOrEmpty(oImporter.Last_AddRowError))
                                                        {
                                                            Raise_OnRaisError(200, "File: [" + Path.GetFileNameWithoutExtension(DataFiles[i]) + "] row: " + iLinesRead + "  Import process row error: " + oImporter.Last_AddRowError, iLinesRead, iTotalLines);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        iErrorsCount++;
                                                        Raise_OnRaisError(200, "Import process row: " + iLinesRead + "  error: " + oImporter.Last_AddRowError, iLinesRead, iTotalLines);
                                                    }


                                                    if (iSQLLines >= iSQLLineLimit)
                                                    {

                                                        do
                                                        {
                                                        } while (iThreadsExecuting > iMaxThreadsToBeExecuting);
                                                        System.Threading.Interlocked.Increment(ref iThreadsExecuting);
                                                        string sTempSql = sMasterSQL;
                                                        new Thread(() =>
                                                        {
                                                            EXE_SQL(sTempSql);
                                                        }).Start();
                                                        sMasterSQL = "";
                                                        bSQLExecuted = true;
                                                        iSQLLines = 0;

                                                        oImporter.DataRows = new List<FileToRDS.TableRows>();
                                                    }
                                                    dtTimeDiff = DateTime.Now - dtStartOn;
                                                    Raise_OnProgressReport(200, "Processed Line " + iLinesRead.ToString() + " | " + iTotalLines.ToString() + Environment.NewLine + "Time Elapsed: " + dtTimeDiff.TotalDays.ToString() + " Days, " + Environment.NewLine + dtTimeDiff.Hours.ToString() + " hours, " + dtTimeDiff.Minutes.ToString() + " min., " + dtTimeDiff.Seconds.ToString(), iLinesRead, iTotalLines);
                                                }

                                                iDataLinesRead++;
                                            }
                                        }
                                        catch (Exception s1)
                                        {
                                            string sInnderException = "";
                                            if (s1.InnerException != null)
                                            {
                                                sInnderException = s1.InnerException.ToString();
                                            }
                                            sProcessError += "File Read Process Exception :" + s1.ToString();
                                            if (!String.IsNullOrEmpty(sInnderException))
                                            {
                                                sProcessError += " File Read INNER EXCEPTION:" + sInnderException;
                                            }
                                            Raise_OnRaisError(500, sProcessError, 0, 0);
                                            Raise_OnProcessCompleted(500, sProcessError, 0, 0);
                                        }
                                    }
                                }
                                iLinesRead++;
                            }
                            
                            if ((iSQLLines > 0) || (!bSQLExecuted))
                            {

                                System.Threading.Interlocked.Increment(ref iThreadsExecuting);
                                string sTempSql = sMasterSQL;
                                //new Thread(() =>
                                //{
                                    EXE_SQL(sTempSql);
                                //}).Start();
                                sMasterSQL = "";
                                bSQLExecuted = true;
                                iSQLLines = 0;
                            }

                            dtTimeDiff = DateTime.Now - dtStartOn;
                        }


                        int iWaitCounter = 0;
                        do
                        {

                            if (iWaitCounter <= 0)
                            {
                                Raise_OnProgressReport(200, "Waiting for all threads to complete!", iThreadsExecuting, iMaxThreadsToBeExecuting);
                                iWaitCounter = 240000000;
                            }

                            iWaitCounter--;
                        } while (iThreadsExecuting > 0);

                        Raise_OnProcessCompleted(200, "Process Completed!", 0, 0);
                    }
                    else
                    {
                        Raise_OnRaisError(500, "Files Is Null or Empty!", 0, 0);
                        Raise_OnProcessCompleted(500, "Files Is Null or Empty!", 0, 0);
                    }
                }
                else
                {
                    Raise_OnRaisError(500, "Files Is Null or Empty!", 0, 0);
                    Raise_OnProcessCompleted(500, "Files Is Null or Empty!", 0, 0);
                }

            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                sProcessError += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    sProcessError += " INNER EXCEPTION:" + sInnderException;
                }
                Raise_OnRaisError(500, sProcessError, 0, 0);
                Raise_OnProcessCompleted(500, sProcessError, 0, 0);
            }
        }

        int iThreadsExecuting = 0;
        private void EXE_SQL(string _SQL)
        {
            try
            {
                if (!String.IsNullOrEmpty(_SQL))
                {
                    if (ConnectionType == 0)
                    {
                        #region MS SQL Connection 
                        //Using the connection strings, we open our data connection and make sure we have access
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            connection.Open();
                            try
                            {
                                //if we were able to open the data connection we continue
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    try
                                    {
                                        try
                                        {
                                            //Pull data for this query
                                            SqlCommand command = new SqlCommand(_SQL, connection);
                                            command.CommandTimeout = 600;
                                            int iResult = command.ExecuteNonQuery();
                                            //if (iResult >= 0)
                                            //{
                                            //File Processed
                                            Raise_OnProgressReport(200, "Processed SQL Results " + iResult.ToString() + Environment.NewLine, 0, 0);
                                            //}
                                            //else
                                            //{
                                            //    Raise_OnRaisError(400, "Error Processing file " + _File + Environment.NewLine, 0, 0);
                                            //}
                                        }
                                        catch (Exception s3)
                                        {
                                            string sInnderException = "";
                                            if (s3.InnerException != null)
                                            {
                                                sInnderException = s3.InnerException.ToString();
                                            }
                                            string sLocalExceptionProcessError = " Process Exception :" + s3.ToString();
                                            if (!String.IsNullOrEmpty(sInnderException))
                                            {
                                                sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                            }
                                            Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                            //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                        }
                                    }
                                    catch (Exception s2)
                                    {
                                        string sInnderException = "";
                                        if (s2.InnerException != null)
                                        {
                                            sInnderException = s2.InnerException.ToString();
                                        }
                                        string sLocalExceptionProcessError = " Process Exception :" + s2.ToString();
                                        if (!String.IsNullOrEmpty(sInnderException))
                                        {
                                            sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                        }
                                        Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                        //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                    }
                                }
                                else
                                {
                                    //Could not open data connection
                                    Raise_OnRaisError(500, "Data Connection Error Could Not Open Data Connection. ", 0, 0);
                                }
                            }
                            catch (Exception s1)
                            {
                                Raise_OnRaisError(500, "Data Connection Error (code 2): " + s1.ToString(), 0, 0);
                            }

                            //we want to make sure we close our connection and clean up
                            connection.Close();
                        }
                        #endregion
                    }
                    else if (ConnectionType == 1)
                    {
                        #region PostGRE Connection 
                        //Using the connection strings, we open our data connection and make sure we have access
                        using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(ConnectionString))
                        {
                            connection.Open();
                            try
                            {
                                //if we were able to open the data connection we continue
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    try
                                    {
                                        try
                                        {
                                            //sSQL = "DO" + Environment.NewLine + "$do$" + Environment.NewLine + "BEGIN" + Environment.NewLine + Environment.NewLine + sSQL + Environment.NewLine + Environment.NewLine + "END" + Environment.NewLine + Environment.NewLine + "$do$";

                                            //Pull data for this query
                                            Npgsql.NpgsqlCommand command = new Npgsql.NpgsqlCommand(_SQL, connection);
                                            command.CommandTimeout = 600;
                                            int iResult = command.ExecuteNonQuery();

                                            //if (iResult >= 0)
                                            //{
                                            //File Processed
                                            Raise_OnProgressReport(200, "Processed SQL Results " + iResult.ToString() + Environment.NewLine, 0, 0);
                                            //}
                                            //else
                                            //{
                                            //    Raise_OnRaisError(400, "Error Processing file " + _File + Environment.NewLine, 0, 0);
                                            //}
                                        }
                                        catch (Exception s3)
                                        {
                                            string sInnderException = "";
                                            if (s3.InnerException != null)
                                            {
                                                sInnderException = s3.InnerException.ToString();
                                            }
                                            string sLocalExceptionProcessError = " Process Exception :" + s3.ToString();
                                            if (!String.IsNullOrEmpty(sInnderException))
                                            {
                                                sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                            }
                                            Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                            //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                        }
                                    }
                                    catch (Exception s2)
                                    {
                                        string sInnderException = "";
                                        if (s2.InnerException != null)
                                        {
                                            sInnderException = s2.InnerException.ToString();
                                        }
                                        string sLocalExceptionProcessError = " Process Exception :" + s2.ToString();
                                        if (!String.IsNullOrEmpty(sInnderException))
                                        {
                                            sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                        }
                                        Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                        //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                    }
                                }
                                else
                                {
                                    //Could not open data connection
                                    Raise_OnRaisError(500, "Data Connection Error Could Not Open Data Connection. ", 0, 0);
                                }
                            }
                            catch (Exception s1)
                            {
                                Raise_OnRaisError(500, "Data Connection Error (code 2): " + s1.ToString(), 0, 0);
                            }

                            //we want to make sure we close our connection and clean up
                            connection.Close();
                        }
                        #endregion
                    }
                    else if (ConnectionType == 2)
                    {
                        #region ODBC Connection 
                        //Using the connection strings, we open our data connection and make sure we have access
                        using (OdbcConnection connection = new OdbcConnection(ConnectionString))
                        {
                            connection.Open();
                            try
                            {
                                //if we were able to open the data connection we continue
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    try
                                    {

                                        try
                                        {
                                            //Pull data for this query
                                            OdbcCommand command = new OdbcCommand(_SQL, connection);
                                            command.CommandTimeout = 600;
                                            int iResult = command.ExecuteNonQuery();
                                            //if (iResult >= 0)
                                            //{
                                            //File Processed
                                            Raise_OnProgressReport(200, "Processed SQL Results " + iResult.ToString() + Environment.NewLine, 0, 0);
                                            //}
                                            //else
                                            //{
                                            //    Raise_OnRaisError(400, "Error Processing file " + _File + Environment.NewLine, 0, 0);
                                            //}
                                        }
                                        catch (Exception s3)
                                        {
                                            string sInnderException = "";
                                            if (s3.InnerException != null)
                                            {
                                                sInnderException = s3.InnerException.ToString();
                                            }
                                            string sLocalExceptionProcessError = " Process Exception :" + s3.ToString();
                                            if (!String.IsNullOrEmpty(sInnderException))
                                            {
                                                sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                            }
                                            Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                            //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                        }
                                    }
                                    catch (Exception s2)
                                    {
                                        string sInnderException = "";
                                        if (s2.InnerException != null)
                                        {
                                            sInnderException = s2.InnerException.ToString();
                                        }
                                        string sLocalExceptionProcessError = " Process Exception :" + s2.ToString();
                                        if (!String.IsNullOrEmpty(sInnderException))
                                        {
                                            sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                                        }
                                        Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                                        //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
                                    }
                                }
                                else
                                {
                                    //Could not open data connection
                                    Raise_OnRaisError(500, "Data Connection Error Could Not Open Data Connection. ", 0, 0);
                                }
                            }
                            catch (Exception s1)
                            {
                                Raise_OnRaisError(500, "Data Connection Error (code 2): " + s1.ToString(), 0, 0);
                            }

                            //we want to make sure we close our connection and clean up
                            connection.Close();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                string sLocalExceptionProcessError = "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    sLocalExceptionProcessError += " INNER EXCEPTION:" + sInnderException;
                }
                Raise_OnRaisError(500, sLocalExceptionProcessError, 0, 0);
                //Raise_OnProcessCompleted(500, sLocalExceptionProcessError, 0, 0);
            }

            System.Threading.Interlocked.Decrement(ref iThreadsExecuting);
        }


        public void DecrementStuff()
        {
            System.Threading.Interlocked.Decrement(ref iThreadsExecuting);
        }
    }

    public class FileToRDS
    {
        public enum SQLStatementType
        {
            Insert = 0,
            InsertIfNotExists = 1,
            InsertOrUpdate = 2
        };
        public class Tables
        {
            public enum PossibleDataTypes
            {
                Number = 0,
                DateTime = 1,
                String = 3
            };

            public Tables()
            {
                TableHeaders = new List<ColumnHeader>();
            }
            public class ColumnHeader
            {
                public string Name { get; set; }
                public bool IsRequired { get; set; }
                public PossibleDataTypes DataType { get; set; }
                public int ColumnIndex { get; set; }
                public bool Skip { get; set; }
            }


            public string TableName { get; set; }

            public List<ColumnHeader> TableHeaders { get; set; }
            public string Last_CopyHeaderTo_Error { get; set; }
            public List<ColumnHeader> CopyHeaderTo()
            {
                Last_CopyHeaderTo_Error = "";
                List<ColumnHeader> oResponse = null;
                try
                {

                    if (TableHeaders != null)
                    {
                        if (TableHeaders.Count > 0)
                        {
                            for (int i = 0; i < TableHeaders.Count; i++)
                            {
                                ColumnHeader oNCH = new ColumnHeader();
                                oNCH.ColumnIndex = TableHeaders[i].ColumnIndex;
                                oNCH.DataType = TableHeaders[i].DataType;
                                oNCH.IsRequired = TableHeaders[i].IsRequired;
                                oNCH.Name = TableHeaders[i].Name;
                                oNCH.Skip = TableHeaders[i].Skip;

                                oResponse.Add(oNCH);
                            }
                        }
                        else
                        {
                            //No Headers
                            Last_CopyHeaderTo_Error += "Error No Headers to copy!";
                        }
                    }
                    else
                    {
                        //Headers null
                        Last_CopyHeaderTo_Error += "Error Headers Are Null!";
                    }
                }
                catch (Exception s)
                {
                    string sInnderException = "";
                    if (s.InnerException != null)
                    {
                        sInnderException = s.InnerException.ToString();
                    }
                    Last_CopyHeaderTo_Error += "Process Exception :" + s.ToString();
                    if (!String.IsNullOrEmpty(sInnderException))
                    {
                        Last_CopyHeaderTo_Error += " INNER EXCEPTION:" + sInnderException;
                    }
                }
                return oResponse;
            }
            public string Last_CopyHeaderToRow_Error { get; set; }
            public List<TableRows.TableRowColumn> CopyHeaderToRow()
            {
                Last_CopyHeaderToRow_Error = "";
                List<TableRows.TableRowColumn> oResponse = null;
                try
                {

                    if (TableHeaders != null)
                    {
                        if (TableHeaders.Count > 0)
                        {
                            oResponse = new List<TableRows.TableRowColumn>();
                            for (int i = 0; i < TableHeaders.Count; i++)
                            {
                                TableRows.TableRowColumn oNCH = new TableRows.TableRowColumn();
                                oNCH.Column = new ColumnHeader();
                                oNCH.Column.ColumnIndex = TableHeaders[i].ColumnIndex;
                                oNCH.Column.DataType = TableHeaders[i].DataType;
                                oNCH.Column.IsRequired = TableHeaders[i].IsRequired;
                                oNCH.Column.Name = TableHeaders[i].Name;
                                oNCH.Column.Skip = TableHeaders[i].Skip;
                                oNCH.Value = "null";
                                oResponse.Add(oNCH);
                            }
                        }
                        else
                        {
                            //No Headers
                            Last_CopyHeaderTo_Error += "Error No Headers to copy!";
                        }
                    }
                    else
                    {
                        //Headers null
                        Last_CopyHeaderTo_Error += "Error Headers Are Null!";
                    }
                }
                catch (Exception s)
                {
                    string sInnderException = "";
                    if (s.InnerException != null)
                    {
                        sInnderException = s.InnerException.ToString();
                    }
                    Last_CopyHeaderTo_Error += "Process Exception :" + s.ToString();
                    if (!String.IsNullOrEmpty(sInnderException))
                    {
                        Last_CopyHeaderTo_Error += " INNER EXCEPTION:" + sInnderException;
                    }
                }
                return oResponse;
            }

            public bool CreateHeadersFromString(string _Headers, string cDeliminator, int[] IndexesToSkip)
            {
                bool bResponse = true;
                try
                {
                    TableHeaders = new List<ColumnHeader>();
                    ColumnHeader oN = new ColumnHeader();
                    _Headers = _Headers.Replace("[", "").Replace("]", "");

                    //,(?=(?:[^']*'[^']*')*[^']*$) -- error - Too many )'s.
                    //or oSeparator + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))" -- error  - Too many )'s.
                    //or (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$) -- error  - Too many )'s.
                    //or (?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\")) -- error  - Too many 's
                    //string[] oHead = Regex.Split(_Headers, "[" + cDeliminator + "]", RegexOptions.Compiled);
                    Regex oReg = new Regex(cDeliminator, RegexOptions.Compiled);
                    bool bUseRegExt = false;

                    string[] oHead = null;
                    if (cDeliminator.Length > 1)
                    {
                        oHead = oReg.Split(_Headers);
                    }
                    else
                    {
                        char[] oD = cDeliminator.ToCharArray();
                        oHead = _Headers.Split(oD[0]);
                    }

                    if (oHead != null)
                    {
                        int iColCount = 0;
                        for (int i = 0; i < oHead.Length; i++)
                        {
                            bool bSkip = false;
                            #region check if we should skip this column
                            try
                            {
                                if (IndexesToSkip != null)
                                {
                                    for (int i2 = 0; i2 < IndexesToSkip.Length; i2++)
                                    {
                                        if (IndexesToSkip[i2] == i)
                                        {
                                            bSkip = true;
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            oN = new ColumnHeader();
                            //oN.Name = oHead[i].Replace("[", "").Replace("]", "");
                            oN.Name = oHead[i].TrimStart('[');
                            oN.Name = oHead[i].TrimEnd(']');
                            oN.Name = oHead[i].Trim(new Char[] { ' ', '"' });
                            oN.Name = oN.Name.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_");
                            oN.ColumnIndex = i;
                            oN.Skip = bSkip;
                            //before adding check for duplicate name
                            #region
                            try
                            {
                                int iCopyCount = 0;
                                string sOriginalHeaderName = oN.Name;
                                for (int iDC = 0; iDC < TableHeaders.Count; iDC++)
                                {
                                    if (TableHeaders[iDC] != null)
                                    {
                                        if(TableHeaders[iDC].Name == oN.Name)
                                        {
                                            iCopyCount++;
                                            if (!String.IsNullOrEmpty(sOriginalHeaderName))
                                            {
                                                oN.Name = sOriginalHeaderName + "_Copy_" + iCopyCount.ToString();
                                            }
                                            else
                                            {
                                                oN.Name = "EmptyColumnName" + "_Copy_" + iCopyCount.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            TableHeaders.Add(oN);
                            iColCount++;
                        }


                        bResponse = true;
                        return bResponse;
                    }
                    else
                    {
                        bResponse = false;
                    }
                }
                catch(Exception s1)
                {
                    bResponse = false;
                }
                return bResponse;
            }

            public bool CreateHeadersFromExcelCells(IRow oRow, int[] IndexesToSkip)
            {
                bool bResponse = true;
                try
                {
                    TableHeaders = new List<ColumnHeader>();
                    ColumnHeader oN = new ColumnHeader();

                    if (oRow != null)
                    {
                        int iColCount = 0;
                        foreach(ICell oCell in oRow.Cells)
                        {
                            bool bSkip = false;
                            #region check if we should skip this column
                            try
                            {
                                if (IndexesToSkip != null)
                                {
                                    for (int i2 = 0; i2 < IndexesToSkip.Length; i2++)
                                    {
                                        if (IndexesToSkip[i2] == iColCount)
                                        {
                                            bSkip = true;
                                        }
                                    }
                                }
                            }
                            catch { }
                            #endregion

                            if (!bSkip)
                            {
                                oN = new ColumnHeader();
                                oN.Name = oCell.StringCellValue.Trim(new Char[] { ' ', '"' });
                                if (String.IsNullOrEmpty(oN.Name))
                                {
                                    oN.Name = "ColumnIndex_" + iColCount.ToString();
                                }
                                oN.Name = oN.Name.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_");
                                oN.Name = oN.Name.Trim('_');
                                oN.ColumnIndex = iColCount;
                                oN.Skip = bSkip;
                                //before adding check for duplicate name
                                #region
                                try
                                {
                                    int iCopyCount = 0;
                                    string sOriginalHeaderName = oN.Name;
                                    for (int iDC = 0; iDC < TableHeaders.Count; iDC++)
                                    {
                                        if (TableHeaders[iDC] != null)
                                        {
                                            if (TableHeaders[iDC].Name.ToLower() == oN.Name.ToLower())
                                            {
                                                iCopyCount++;
                                                oN.Name = sOriginalHeaderName + "_Copy_" + iCopyCount.ToString();
                                            }
                                        }
                                    }
                                }
                                catch { }
                                #endregion

                                TableHeaders.Add(oN);
                            }
                            iColCount++;
                        }




                        bResponse = true;
                        return bResponse;
                    }
                    else
                    {
                        bResponse = false;
                    }
                }
                catch(Exception s)
                {
                    bResponse = false;
                }
                return bResponse;
            }

            public string Build_CreateTable_SQL(string _DefaultDataType)
            {
                string sResponse = "";
                try
                {
                    if(TableHeaders != null)
                    {
                        sResponse += "CREATE TABLE " + TableName + "" + Environment.NewLine;
                        sResponse += "(" + Environment.NewLine;

                        for(int i=0; i < TableHeaders.Count; i++)
                        {
                            if (String.IsNullOrEmpty(TableHeaders[i].Name))
                            {
                                TableHeaders[i].Name = "Column_index_" + i.ToString();
                            }

                            if (i == 0)
                            {
                                sResponse += "[" + TableHeaders[i].Name.Replace(" ", "_") + "]" + " " + _DefaultDataType + Environment.NewLine;
                            }
                            else
                            {
                                sResponse += ", " + "[" + TableHeaders[i].Name.Replace(" ", "_") + "]" + " " + _DefaultDataType + Environment.NewLine;
                            }
                        }


                        //we add our custom log columns to give report on data findings
                        sResponse += ", " + "[" + "ImportLog" + "]" + " " + "text" + Environment.NewLine;
                        sResponse += ", " + "[" + "IMPHasValue" + "]" + " " + "text" + Environment.NewLine;
                        sResponse += ", " + "[" + "NumFillColumns" + "]" + " " + "int" + Environment.NewLine;
                        sResponse += ", " + "[" + "NumEmpytColumns" + "]" + " " + "int" + Environment.NewLine;

                        sResponse += ");" + Environment.NewLine;
                    }
                }
                catch(Exception s)
                {

                }
                return sResponse;
            }
        }

        public class TableRows
        {
            public class TableRowColumn
            {
                public Tables.ColumnHeader Column { get; set; }

                public string Value { get; set; }
            }

            public List<TableRowColumn> DataRow { get; set; }


            public string Last_Build_SQLStatements_Error { get; set; }
            public string Build_SQLStatement(Tables oTable, FileToRDS.SQLStatementType _SQLType = SQLStatementType.Insert, int IndexToUseForExists = 0,
                int[] CheckExistsColumnsIndexs = null, string _CheckExists_Conjunction = "and", int[] UpdateWhereIndexs = null, string _Update_Conjunction = "and")
            {
                //This whole thing is focused on Postgre, the one I'll put togehter needs to be put into AriesConnector to be specific to each data connection type
                string sSQL_Return = "";
                try
                {
                    if (oTable != null)
                    {
                        string sImportLog = "";
                        string sTableName = oTable.TableName;
                        string sColumns = "";
                        string sValues = "";
                        string sUpdateSet = "";
                        string sExistsWhereClause = "";
                        string sWhereClause = "";
                        string sSelectForExists = "";
                        int iWriteCounter = 0;

                        #region Build Base sql components

                        for (int i = 0; i < oTable.TableHeaders.Count; i++)
                        {
                            if (!oTable.TableHeaders[i].Skip)
                            {
                                if (!String.IsNullOrEmpty(oTable.TableHeaders[i].Name))
                                {
                                    if (iWriteCounter == 0)
                                    {
                                        //sColumns = "[" + oTable.TableHeaders[i].Name + "]";
                                        sColumns = "" + "[" + oTable.TableHeaders[i].Name.Replace(" ", "_").Replace("-", "_") + "]" + "";
                                        iWriteCounter++;
                                    }
                                    else
                                    {
                                        //sColumns += ",[" + oTable.TableHeaders[i].Name + "]";
                                        sColumns += "," + "[" + oTable.TableHeaders[i].Name.Replace(" ", "_").Replace("-", "_") + "]" + "";
                                        iWriteCounter++;
                                    }
                                }
                            }
                        }

                        //we add our custom logging columns to help identify any data row possible issues
                        sColumns += ",[ImportLog]";
                        sColumns += ",[IMPHasValue]";
                        sColumns += ",[NumFillColumns]";
                        sColumns += ",[NumEmpytColumns]";

                        int iCountColumnsWithValues = 0;
                        int iCountColumnsEmpty = 0;


                        iWriteCounter = 0;
                        for (int i = 0; i < oTable.TableHeaders.Count; i++)
                        {
                            try
                            {
                                if (!DataRow[i].Column.Skip)
                                {
                                    if (!String.IsNullOrEmpty(DataRow[i].Column.Name))
                                    {
                                        if (iWriteCounter == 0)
                                        {
                                            try
                                            {
                                                int iSpaceCount = DataRow[i].Value.Split(' ').Length;
                                                if (iSpaceCount > 1)
                                                {
                                                    sImportLog += "Column: " + DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " has multiple spaces in value!  " + Environment.NewLine;
                                                }
                                                int iColonCount = DataRow[i].Value.Split(':').Length;
                                                if (iColonCount > 1)
                                                {
                                                    sImportLog += "Column: " + DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " has multiple : in value!  " + Environment.NewLine;
                                                }
                                            }
                                            catch { }
                                            if (String.IsNullOrEmpty(DataRow[i].Value))
                                            {
                                                DataRow[i].Value = "''";
                                            }
                                            sValues = DataRow[i].Value;
                                            try
                                            {
                                                if (ColumnHasValue(DataRow[i].Value) > 0)
                                                {
                                                    iCountColumnsWithValues++;
                                                }
                                                else
                                                {
                                                    iCountColumnsEmpty++;
                                                }
                                            }
                                            catch { }
                                            sUpdateSet = DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[i].Value;
                                            iWriteCounter++;
                                        }
                                        else
                                        {
                                            try
                                            {
                                                int iSpaceCount = DataRow[i].Value.Split(' ').Length;
                                                if (iSpaceCount > 1)
                                                {
                                                    sImportLog += "Column: " + DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " has multiple spaces in value!  " + Environment.NewLine;
                                                }
                                                int iColonCount = DataRow[i].Value.Split(':').Length;
                                                if (iColonCount > 1)
                                                {
                                                    sImportLog += "Column: " + DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " has multiple : in value!  " + Environment.NewLine;
                                                }
                                            }
                                            catch { }
                                            if (String.IsNullOrEmpty(DataRow[i].Value))
                                            {
                                                DataRow[i].Value = "''";
                                            }
                                            sValues += "," + DataRow[i].Value;
                                            try
                                            {
                                                if (ColumnHasValue(DataRow[i].Value) > 0)
                                                {
                                                    iCountColumnsWithValues++;
                                                }
                                                else
                                                {
                                                    iCountColumnsEmpty++;
                                                }
                                            }
                                            catch { }
                                            sUpdateSet += "," + DataRow[i].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[i].Value;
                                            iWriteCounter++;
                                        }
                                    }
                                }
                            }
                            catch { }
                        }


                        //we then populate those custom columns with our log values
                        sValues += "," + "'" + sImportLog + "'";
                        sUpdateSet += "," + "ImportLog" + " = " + "'" + sImportLog + "'";

                        if (!String.IsNullOrEmpty(sImportLog))
                        {
                            sValues += "," + "'0'";
                            sUpdateSet += "," + "IMPHasValue" + " = " + "'0'";
                        }
                        else
                        {
                            sValues += "," + "'1'";
                            sUpdateSet += "," + "IMPHasValue" + " = " + "'1'";
                        }


                        sValues += "," + "'" + iCountColumnsWithValues.ToString() + "'";
                        sUpdateSet += "," + "NumFillColumns" + " = " + "'" + iCountColumnsWithValues.ToString() + "'";

                        sValues += "," + "'" + iCountColumnsEmpty.ToString() + "'";
                        sUpdateSet += "," + "NumEmpytColumns" + " = " + "'" + iCountColumnsEmpty.ToString() + "'";



                        #endregion

                        #region Build Where Clauses
                        if (CheckExistsColumnsIndexs != null)
                        {
                            if (CheckExistsColumnsIndexs.Length > 0)
                            {
                                sSelectForExists = "SELECT " + oTable.TableHeaders[IndexToUseForExists].Name.Replace(" ", "_").Replace("-", "_") + " FROM " + sTableName;
                                for (int i = 0; i < CheckExistsColumnsIndexs.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        sExistsWhereClause = DataRow[CheckExistsColumnsIndexs[i]].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[CheckExistsColumnsIndexs[i]].Value;
                                    }
                                    else
                                    {
                                        sExistsWhereClause += " " + _CheckExists_Conjunction + " " + DataRow[CheckExistsColumnsIndexs[i]].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[CheckExistsColumnsIndexs[i]].Value;
                                    }
                                }
                            }
                        }

                        if (UpdateWhereIndexs != null)
                        {
                            if (UpdateWhereIndexs.Length > 0)
                            {
                                for (int i = 0; i < UpdateWhereIndexs.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        sWhereClause = DataRow[UpdateWhereIndexs[i]].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[UpdateWhereIndexs[i]].Value;
                                    }
                                    else
                                    {
                                        sWhereClause += " " + _Update_Conjunction + " " + DataRow[UpdateWhereIndexs[i]].Column.Name.Replace(" ", "_").Replace("-", "_") + " = " + DataRow[UpdateWhereIndexs[i]].Value;
                                    }
                                }
                            }
                        }
                        #endregion

                        //Now we build the final result
                        string sSQL_Insert = "INSERT INTO " + sTableName;
                        sSQL_Insert = sSQL_Insert + " (" + sColumns + ") VALUES (" + sValues + ");   " + Environment.NewLine;

                        string sSQL_Update = "UPDATE " + sTableName + " set " + sUpdateSet + " where " + sWhereClause + ";   " + Environment.NewLine;

                        if (_SQLType == SQLStatementType.Insert)
                        {
                            sSQL_Return = sSQL_Insert + Environment.NewLine;
                        }
                        else if (_SQLType == SQLStatementType.InsertIfNotExists)
                        {
                            sSQL_Return = "IF NOT EXISTS (" + sSelectForExists + " where " + sExistsWhereClause + ") THEN   " + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                            sSQL_Return += sSQL_Insert + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                            sSQL_Return += "END IF;     " + Environment.NewLine + Environment.NewLine;
                        }
                        else if (_SQLType == SQLStatementType.InsertOrUpdate)
                        {
                            sSQL_Return = "IF NOT EXISTS (" + sSelectForExists + " where " + sExistsWhereClause + ") THEN   " + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                            sSQL_Return += sSQL_Insert + Environment.NewLine;
                            sSQL_Return += "ELSE      " + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                            sSQL_Return += sSQL_Update + Environment.NewLine;
                            sSQL_Return += "END IF;    " + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
                catch (Exception s)
                {
                    string sInnderException = "";
                    if (s.InnerException != null)
                    {
                        sInnderException = s.InnerException.ToString();
                    }
                    Last_Build_SQLStatements_Error += "Process Exception :" + s.ToString();
                    if (!String.IsNullOrEmpty(sInnderException))
                    {
                        Last_Build_SQLStatements_Error += " INNER EXCEPTION:" + sInnderException;
                    }
                }
                return sSQL_Return;
            }

            public int ColumnHasValue(string _value)
            {
                int iResponse = 0;
                try
                {
                    //the order of these checks is important to first check if it's empty or if it's null, the value spelled out as null makes the value empty in db but not as a string.
                    if (!String.IsNullOrEmpty(_value))
                    {
                        iResponse = 1;
                    }

                    if (_value.ToLower() == "null")
                    {
                        iResponse = 0;
                    }
                }
                catch
                {

                }
                return iResponse;
            }

            public TableRows()
            {
                DataRow = new List<TableRowColumn>();
            }
        }

        public Tables DataDef { get; set; }
        public bool Set_TableName(string _TableName)
        {
            DataDef.TableName = _TableName;
            return true;
        }
        public string Last_Build_DataDef_Error { get; set; }
        public bool Build_DataDef(string _Headers, string _DataRowSample, string sSeparator)
        {
            Last_Build_DataDef_Error = "";
            bool bResponse = false;
            try
            {
                DataDef = new Tables();
                DataDef.TableName = Guid.NewGuid().ToString();
                DataDef.TableHeaders = new List<Tables.ColumnHeader>();
                string[] oHeaders = System.Text.RegularExpressions.Regex.Split(_Headers, sSeparator);
                if (oHeaders != null)
                {
                    for (int i = 0; i < oHeaders.Length; i++)
                    {
                        Tables.ColumnHeader oColN = new Tables.ColumnHeader();
                        oColN.Name = oHeaders[i].Trim(new Char[] { ' ', '"' });
                        oColN.Name = oColN.Name.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_");
                        oColN.ColumnIndex = i;
                        oColN.IsRequired = false;
                        //for now we are just keeping things simple and setting things to string, later we can use the 
                        //method to discover the data type
                        oColN.DataType = Tables.PossibleDataTypes.String;
                        //before adding check for duplicate name
                        #region
                        try
                        {
                            int iCopyCount = 0;
                            string sOriginalHeaderName = oColN.Name;
                            for (int iDC = 0; iDC < DataDef.TableHeaders.Count; iDC++)
                            {
                                if (DataDef.TableHeaders[iDC] != null)
                                {
                                    if (DataDef.TableHeaders[iDC].Name == oColN.Name)
                                    {
                                        iCopyCount++;
                                        oColN.Name = sOriginalHeaderName + "_Copy_" + iCopyCount.ToString();
                                    }
                                }
                            }
                        }
                        catch { }
                        #endregion
                        DataDef.TableHeaders.Add(oColN);
                    }
                    bResponse = true;
                }
                else
                {
                    Last_Build_DataDef_Error += "Error could not split string into headers.";
                }

            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Build_DataDef_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Build_DataDef_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return bResponse;
        }
        public bool Build_DataDef(IRow oHeaderRow, string sSeparator)
        {
            Last_Build_DataDef_Error = "";
            bool bResponse = false;
            try
            {
                DataDef = new Tables();
                DataDef.TableName = Guid.NewGuid().ToString();
                DataDef.TableHeaders = new List<Tables.ColumnHeader>();
                if (oHeaderRow != null)
                {
                    int iColCount = 0;
                    foreach (ICell oCell in oHeaderRow.Cells)
                    {
                        Tables.ColumnHeader oColN = new Tables.ColumnHeader();
                        oColN.Name = oCell.StringCellValue.Trim(new Char[] { ' ', '"' });
                        oColN.Name = oColN.Name.Replace("\\", "_").Replace("/", "_").Replace("!", "_").Replace("@", "_").Replace("#", "_").Replace("$", "_").Replace("%", "_").Replace("^", "_").Replace("&", "_").Replace("*", "_").Replace("(", "_").Replace(")", "_").Replace("-", "_").Replace("=", "_").Replace("+", "_").Replace(".", "_");
                        oColN.ColumnIndex = iColCount;
                        oColN.IsRequired = false;
                        //for now we are just keeping things simple and setting things to string, later we can use the 
                        //method to discover the data type
                        oColN.DataType = Tables.PossibleDataTypes.String;
                        //before adding check for duplicate name
                        #region
                        try
                        {
                            int iCopyCount = 0;
                            string sOriginalHeaderName = oColN.Name;
                            for (int iDC = 0; iDC < DataDef.TableHeaders.Count; iDC++)
                            {
                                if (DataDef.TableHeaders[iDC] != null)
                                {
                                    if (DataDef.TableHeaders[iDC].Name == oColN.Name)
                                    {
                                        iCopyCount++;
                                        oColN.Name = sOriginalHeaderName + "_Copy_" + iCopyCount.ToString();
                                    }
                                }
                            }
                        }
                        catch { }
                        #endregion
                        DataDef.TableHeaders.Add(oColN);
                        iColCount++;
                    }


                    bResponse = true;
                    return bResponse;
                }
                else
                {
                    Last_Build_DataDef_Error += "Error could not split string into headers.";
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Build_DataDef_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Build_DataDef_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return bResponse;
        }


        public string Last_Build_RowStrucutreFromHeaders_Error { get; set; }
        public List<TableRows.TableRowColumn> Build_RowStrucutreFromHeaders()
        {
            Last_Build_RowStrucutreFromHeaders_Error = "";
            List<TableRows.TableRowColumn> oResposne = null;
            try
            {

                if (DataDef != null)
                {
                    if (DataDef.TableHeaders != null)
                    {
                        oResposne = new List<TableRows.TableRowColumn>();
                        oResposne = DataDef.CopyHeaderToRow();
                        if (oResposne != null)
                        {
                            Last_Build_RowStrucutreFromHeaders_Error += DataDef.Last_CopyHeaderToRow_Error;
                            return oResposne;
                        }
                        else
                        {
                            //Error Occured in sub process
                            Last_Build_RowStrucutreFromHeaders_Error += DataDef.Last_CopyHeaderToRow_Error;
                            return null;
                        }
                    }
                    else
                    {
                        //Error null table headers
                        Last_Build_RowStrucutreFromHeaders_Error += "Error Table Headres are Null!";
                        return null;
                    }
                }
                else
                {
                    //Error Null data def
                    Last_Build_RowStrucutreFromHeaders_Error += "Error Table is Null!";
                    return null;
                }

            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Build_RowStrucutreFromHeaders_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Build_RowStrucutreFromHeaders_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return oResposne;
        }

        public List<TableRows> DataRows { get; set; }

        #region Processing Functions

        public string Last_AddRowError { get; set; }

        public bool Add_Row(string _RawRow, string oSeparator, int[] Indexes_ToSkip = null)
        {
            Last_AddRowError = "";
            bool bRespones = false;
            try
            {
                if (DataRows == null)
                {
                    DataRows = new List<TableRows>();
                }

                if (!String.IsNullOrEmpty(_RawRow))
                {
                    ////,(?=(?:[^']*'[^']*')*[^']*$)
                    ////or oSeparator + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"
                    ////or (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)
                    ////or (?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))
                    //string[] oValues = Regex.Split(_RawRow, "[" + oSeparator + "]");


                    Regex oReg = new Regex(oSeparator, RegexOptions.Compiled);


                    string[] oValues = null;
                    if (oSeparator.Length > 1)
                    {
                        oValues = oReg.Split(_RawRow);
                    }
                    else
                    {
                        char[] oD = oSeparator.ToCharArray();
                        oValues = _RawRow.Split(oD[0]);
                    }

                    if (oValues != null)
                    {
                        TableRows oRow = new TableRows();
                        oRow.DataRow = Build_RowStrucutreFromHeaders();


                        if (oRow.DataRow != null)
                        {
                            bRespones = true;
                            if (oValues.Length != oRow.DataRow.Count)
                            {
                                Last_AddRowError += "Row Column Counts do not match, Error Row: " + _RawRow;
                            }

                            for (int i = 0; i < oValues.Length; i++)
                            {
                                try
                                {
                                    bool bShouldSkip = false;
                                    #region Check to see if we should skip this column
                                    try
                                    {
                                        if (Indexes_ToSkip != null)
                                        {
                                            for (int i2 = 0; i2 < Indexes_ToSkip.Length; i2++)
                                            {
                                                if (i == Indexes_ToSkip[i2])
                                                {
                                                    bShouldSkip = true;
                                                    i2 = Indexes_ToSkip.Length + 1;
                                                }
                                            }
                                        }
                                    }
                                    catch { }
                                    #endregion

                                    if (!bShouldSkip)
                                    {
                                        try
                                        {
                                            oValues[i] = Validate_Value(oValues[i]);
                                            oValues[i] = Fix_ValueErrors(oValues[i]);
                                            oValues[i] = SQLiFy_Values(oValues[i]);
                                            oRow.DataRow[i].Value = oValues[i];
                                            DataRows.Add(oRow);
                                        }
                                        catch(Exception s11)
                                        {
                                            oRow.DataRow[i].Value = "ERROR";
                                        }
                                    }
                                }
                                catch (Exception s12)
                                {
                                }
                            }
                        }
                        else
                        {
                            //Error occured in coping headers into a new row.
                            Last_AddRowError += "Error occured in coping headers into a new row.";
                        }
                    }
                    else
                    {
                        //Error occured in splitting data values out of string
                        Last_AddRowError += "Error occured in splitting data values out of string";
                    }
                }
                else
                {
                    //Error null or empty data string value
                    Last_AddRowError += "Error null or empty data string value.";
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_AddRowError += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_AddRowError += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return bRespones;
        }
        public bool Add_Row(IRow oDataRow, int[] Indexes_ToSkip = null)
        {
            Last_AddRowError = "";
            bool bRespones = false;
            try
            {
                if (DataRows == null)
                {
                    DataRows = new List<TableRows>();
                }

                if(oDataRow != null)
                {
                    TableRows oRow = new TableRows();
                    oRow.DataRow = Build_RowStrucutreFromHeaders();
                    int iColCount = 0;
                    foreach (ICell oCell in oDataRow.Cells)
                    {
                        bool bSkip = false;
                        #region check if we should skip this column
                        try
                        {
                            if (Indexes_ToSkip != null)
                            {
                                for (int i2 = 0; i2 < Indexes_ToSkip.Length; i2++)
                                {
                                    if (Indexes_ToSkip[i2] == iColCount)
                                    {
                                        bSkip = true;
                                    }
                                }
                            }
                        }
                        catch { }
                        #endregion

                        if (!bSkip)
                        {
                            try
                            {
                                string sCellValue = "";

                                #region get cell value
                                try
                                {
                                    switch (oCell.CellType)
                                    {
                                        case CellType.Boolean:
                                            sCellValue = oCell.BooleanCellValue.ToString();
                                            break;
                                        case CellType.Error:
                                            sCellValue = oCell.ErrorCellValue.ToString();
                                            break;
                                        case CellType.Formula:
                                            sCellValue = oCell.CellFormula.ToString();
                                            break;
                                        case CellType.Numeric:
                                            sCellValue = oCell.NumericCellValue.ToString();
                                            break;
                                        case CellType.String:
                                            sCellValue = oCell.StringCellValue;
                                            break;
                                        default:
                                            sCellValue = oCell.StringCellValue.ToString();
                                            break;
                                    }
                                }
                                catch(Exception scellsr)
                                {
                                    sCellValue = scellsr.ToString();
                                }
                                #endregion
                                if (String.IsNullOrEmpty(sCellValue))
                                {
                                    sCellValue = "null";
                                }

                                sCellValue = Validate_Value(sCellValue);
                                sCellValue = Fix_ValueErrors(sCellValue);
                                sCellValue = SQLiFy_Values(sCellValue);
                                //oRow.DataRow[iColCount].Value = sCellValue;
                                oRow.DataRow[oCell.ColumnIndex].Value = sCellValue;
                                DataRows.Add(oRow);
                            }
                            catch(Exception sc1)
                            {
                                oRow.DataRow[iColCount].Value = "ERROR";
                            }
                        }
                        iColCount++;
                    }
                    bRespones = true;
                }
                else
                {
                    //Error null or empty data string value
                    Last_AddRowError += "Error null or empty data string value.";
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_AddRowError += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_AddRowError += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return bRespones;
        }

        public string Last_Validate_Value_Error { get; set; }
        private string Validate_Value(string _Raw_Value)
        {
            _Raw_Value = _Raw_Value.Trim(new Char[] { ' ', '"' });
            string _Response = _Raw_Value;
            try
            {
                if ((string.IsNullOrEmpty(_Raw_Value)) || (_Raw_Value.ToLower() == "null"))
                {
                    _Response = "NULL";
                }

                //if(_Response != "null")
                //{
                //    _Response = "'" + _Raw_Value + "'";
                //}
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Validate_Value_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Validate_Value_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return _Response;
        }
        public string Last_Fix_ValueErrors_Error { get; set; }
        private string Fix_ValueErrors(string _Raw_Value)
        {
            string _Response = _Raw_Value;
            try
            {
                //bool bIsNumber = false;
                //try
                //{
                //    double dtTemp = System.Convert.ToDouble(_Raw_Value);
                //    bIsNumber = true;
                //}
                //catch { }
                //try
                //{
                //    if (!bIsNumber)
                //    {
                //        DateTime dtTemp = System.Convert.ToDateTime(_Raw_Value);

                //        //for sql, if the year is less than 1901 it will error out for the insert
                //        if (dtTemp.Year <= 1901)
                //        {
                //            DateTime dtFix = new DateTime(1901, dtTemp.Month, dtTemp.Day);
                //            _Response = dtFix.ToShortDateString() + " " + dtFix.ToShortTimeString();
                //        }
                //    }
                //}
                //catch { }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Fix_ValueErrors_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Fix_ValueErrors_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return _Response;
        }
        public string Last_SQLiFy_Values_Error { get; set; }
        private string SQLiFy_Values(string _Raw_Value)
        {
            string _Response = _Raw_Value;
            try
            {
                //string sDoubleQuoteReplacer = "\"\"";
                if (_Raw_Value.ToLower() == "null")
                {
                    _Response = _Raw_Value;
                }
                else
                {
                    _Response = _Raw_Value.Replace("'", "''");
                    //_Response = _Raw_Value.Replace("\"", sDoubleQuoteReplacer);
                    _Response = "'" + _Response + "'";
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_SQLiFy_Values_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_SQLiFy_Values_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return _Response;
        }

        private Tables.PossibleDataTypes Get_ValueDataType(string _Raw_Value)
        {
            bool bValueFound = false;
            if (!bValueFound)
            {
                try
                {
                    bool bTemp = System.Convert.ToBoolean(_Raw_Value);
                    bValueFound = true;
                    return Tables.PossibleDataTypes.String;
                }
                catch { }
            }

            if (!bValueFound)
            {
                try
                {
                    decimal dTemp = System.Convert.ToDecimal(_Raw_Value);
                    bValueFound = true;
                    return Tables.PossibleDataTypes.Number;
                }
                catch { }
            }

            if (!bValueFound)
            {
                try
                {
                    int iTemp = System.Convert.ToInt32(_Raw_Value);
                    bValueFound = true;
                    return Tables.PossibleDataTypes.Number;
                }
                catch { }
            }

            if (!bValueFound)
            {
                try
                {
                    DateTime dtTemp = System.Convert.ToDateTime(_Raw_Value);
                    bValueFound = true;
                    return Tables.PossibleDataTypes.DateTime;

                }
                catch { }
            }

            return Tables.PossibleDataTypes.String;
        }


        public string Last_Build_SQLStatements_Error { get; set; }
        public string Build_SQLStatements(FileToRDS.SQLStatementType _SQLType = SQLStatementType.Insert, int IndexToUseForExists = 0,
                int[] CheckExistsColumnsIndexs = null, string _CheckExists_Conjunction = "and", int[] UpdateWhereIndexs = null, string _Update_Conjunction = "and")
        {
            string sSQL_Return = "";

            try
            {
                for (int i = 0; i < DataRows.Count; i++)
                {
                    sSQL_Return += DataRows[i].Build_SQLStatement(DataDef, _SQLType, IndexToUseForExists, CheckExistsColumnsIndexs, _CheckExists_Conjunction, UpdateWhereIndexs, _Update_Conjunction);
                }
            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Build_SQLStatements_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Build_SQLStatements_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return sSQL_Return;
        }

        public string Build_SQL_Last_Row_Statements(FileToRDS.SQLStatementType _SQLType = SQLStatementType.Insert, int IndexToUseForExists = 0,
                int[] CheckExistsColumnsIndexs = null, string _CheckExists_Conjunction = "and", int[] UpdateWhereIndexs = null, string _Update_Conjunction = "and")
        {
            string sSQL_Return = "";

            try
            {
                sSQL_Return += DataRows[DataRows.Count - 1].Build_SQLStatement(DataDef, _SQLType, IndexToUseForExists, CheckExistsColumnsIndexs, _CheckExists_Conjunction, UpdateWhereIndexs, _Update_Conjunction);

            }
            catch (Exception s)
            {
                string sInnderException = "";
                if (s.InnerException != null)
                {
                    sInnderException = s.InnerException.ToString();
                }
                Last_Build_SQLStatements_Error += "Process Exception :" + s.ToString();
                if (!String.IsNullOrEmpty(sInnderException))
                {
                    Last_Build_SQLStatements_Error += " INNER EXCEPTION:" + sInnderException;
                }
            }
            return sSQL_Return;
        }
        #endregion

        public FileToRDS()
        {
            DataRows = new List<TableRows>();
        }
    }
    
}
