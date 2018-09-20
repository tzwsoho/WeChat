namespace WeChat
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lvwContacts = new System.Windows.Forms.ListView();
            this.colHead = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgHeads = new System.Windows.Forms.ImageList(this.components);
            this.niTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSwitch = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefreshQRCode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.tmrTimeout = new System.Windows.Forms.Timer(this.components);
            this.tc = new System.Windows.Forms.TabControl();
            this.tpContacts = new System.Windows.Forms.TabPage();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tc.SuspendLayout();
            this.tpContacts.SuspendLayout();
            this.tpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwContacts
            // 
            this.lvwContacts.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvwContacts.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lvwContacts.BackColor = System.Drawing.Color.PaleTurquoise;
            this.lvwContacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwContacts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHead,
            this.colNickname});
            this.lvwContacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwContacts.ForeColor = System.Drawing.Color.Red;
            this.lvwContacts.FullRowSelect = true;
            this.lvwContacts.GridLines = true;
            this.lvwContacts.HideSelection = false;
            this.lvwContacts.LabelWrap = false;
            this.lvwContacts.LargeImageList = this.imgHeads;
            this.lvwContacts.Location = new System.Drawing.Point(0, 0);
            this.lvwContacts.MultiSelect = false;
            this.lvwContacts.Name = "lvwContacts";
            this.lvwContacts.ShowItemToolTips = true;
            this.lvwContacts.Size = new System.Drawing.Size(192, 284);
            this.lvwContacts.SmallImageList = this.imgHeads;
            this.lvwContacts.TabIndex = 0;
            this.lvwContacts.UseCompatibleStateImageBehavior = false;
            this.lvwContacts.View = System.Windows.Forms.View.Details;
            this.lvwContacts.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwContacts_ColumnClick);
            this.lvwContacts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwContacts_MouseDoubleClick);
            // 
            // colHead
            // 
            this.colHead.Text = "";
            this.colHead.Width = 18;
            // 
            // colNickname
            // 
            this.colNickname.Text = "昵称";
            this.colNickname.Width = 150;
            // 
            // imgHeads
            // 
            this.imgHeads.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgHeads.ImageSize = new System.Drawing.Size(16, 16);
            this.imgHeads.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // niTray
            // 
            this.niTray.ContextMenuStrip = this.cms;
            this.niTray.Text = "微信 Demo";
            this.niTray.Visible = true;
            // 
            // cms
            // 
            this.cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSwitch,
            this.miRefreshQRCode,
            this.toolStripSeparator1,
            this.miExit});
            this.cms.Name = "cms";
            this.cms.Size = new System.Drawing.Size(153, 76);
            // 
            // miSwitch
            // 
            this.miSwitch.Enabled = false;
            this.miSwitch.Name = "miSwitch";
            this.miSwitch.Size = new System.Drawing.Size(152, 22);
            this.miSwitch.Text = "切换账号(&S)";
            this.miSwitch.Click += new System.EventHandler(this.miSwitch_Click);
            // 
            // miRefreshQRCode
            // 
            this.miRefreshQRCode.Name = "miRefreshQRCode";
            this.miRefreshQRCode.Size = new System.Drawing.Size(152, 22);
            this.miRefreshQRCode.Text = "刷新二维码(&R)";
            this.miRefreshQRCode.Click += new System.EventHandler(this.miRefreshQRCode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // miExit
            // 
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(152, 22);
            this.miExit.Text = "退出(&X)";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwContacts);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtbContent);
            this.splitContainer1.Size = new System.Drawing.Size(1108, 284);
            this.splitContainer1.SplitterDistance = 192;
            this.splitContainer1.TabIndex = 1;
            // 
            // rtbContent
            // 
            this.rtbContent.BackColor = System.Drawing.Color.PaleTurquoise;
            this.rtbContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContent.Location = new System.Drawing.Point(0, 0);
            this.rtbContent.MaxLength = 0;
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(912, 284);
            this.rtbContent.TabIndex = 0;
            this.rtbContent.Text = "";
            this.rtbContent.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbContent_LinkClicked);
            // 
            // tmrTimeout
            // 
            this.tmrTimeout.Interval = 1000;
            this.tmrTimeout.Tick += new System.EventHandler(this.tmrTimeout_Tick);
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tpContacts);
            this.tc.Controls.Add(this.tpLog);
            this.tc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(1122, 316);
            this.tc.TabIndex = 2;
            // 
            // tpContacts
            // 
            this.tpContacts.Controls.Add(this.splitContainer1);
            this.tpContacts.Location = new System.Drawing.Point(4, 22);
            this.tpContacts.Name = "tpContacts";
            this.tpContacts.Padding = new System.Windows.Forms.Padding(3);
            this.tpContacts.Size = new System.Drawing.Size(1114, 290);
            this.tpContacts.TabIndex = 0;
            this.tpContacts.Text = "联系人";
            this.tpContacts.UseVisualStyleBackColor = true;
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.btnClear);
            this.tpLog.Controls.Add(this.txtLog);
            this.tpLog.Location = new System.Drawing.Point(4, 22);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpLog.Size = new System.Drawing.Size(1114, 290);
            this.tpLog.TabIndex = 1;
            this.tpLog.Text = "日志";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(1014, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.HideSelection = false;
            this.txtLog.Location = new System.Drawing.Point(3, 3);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1108, 284);
            this.txtLog.TabIndex = 0;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 316);
            this.Controls.Add(this.tc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "微信";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.cms.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tc.ResumeLayout(false);
            this.tpContacts.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            this.tpLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwContacts;
        private System.Windows.Forms.NotifyIcon niTray;
        private System.Windows.Forms.ContextMenuStrip cms;
        private System.Windows.Forms.ImageList imgHeads;
        private System.Windows.Forms.ToolStripMenuItem miSwitch;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colHead;
        private System.Windows.Forms.ColumnHeader colNickname;
        private System.Windows.Forms.ToolStripMenuItem miRefreshQRCode;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Timer tmrTimeout;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tpContacts;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtLog;
    }
}

