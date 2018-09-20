namespace WeChat
{
    partial class FrmVideo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVideo));
            this.ts = new System.Windows.Forms.ToolStrip();
            this.btnPlay = new System.Windows.Forms.ToolStripButton();
            this.btnPause = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHalf = new System.Windows.Forms.ToolStripButton();
            this.btnOriginal = new System.Windows.Forms.ToolStripButton();
            this.btnTwice = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.pnVideo = new System.Windows.Forms.Panel();
            this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.ts.SuspendLayout();
            this.SuspendLayout();
            // 
            // ts
            // 
            this.ts.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPlay,
            this.btnPause,
            this.btnStop,
            this.toolStripSeparator1,
            this.btnHalf,
            this.btnOriginal,
            this.btnTwice,
            this.toolStripSeparator2,
            this.btnSaveAs});
            this.ts.Location = new System.Drawing.Point(0, 0);
            this.ts.Name = "ts";
            this.ts.Size = new System.Drawing.Size(284, 25);
            this.ts.TabIndex = 0;
            // 
            // btnPlay
            // 
            this.btnPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPlay.Enabled = false;
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(23, 22);
            this.btnPlay.Text = "播放";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(23, 22);
            this.btnPause.Text = "暂停";
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(23, 22);
            this.btnStop.Text = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnHalf
            // 
            this.btnHalf.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHalf.Image = ((System.Drawing.Image)(resources.GetObject("btnHalf.Image")));
            this.btnHalf.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHalf.Name = "btnHalf";
            this.btnHalf.Size = new System.Drawing.Size(23, 22);
            this.btnHalf.Text = "一半大小";
            this.btnHalf.Click += new System.EventHandler(this.btnSize_Click);
            // 
            // btnOriginal
            // 
            this.btnOriginal.Checked = true;
            this.btnOriginal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnOriginal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOriginal.Image = ((System.Drawing.Image)(resources.GetObject("btnOriginal.Image")));
            this.btnOriginal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOriginal.Name = "btnOriginal";
            this.btnOriginal.Size = new System.Drawing.Size(23, 22);
            this.btnOriginal.Text = "原始大小";
            this.btnOriginal.Click += new System.EventHandler(this.btnSize_Click);
            // 
            // btnTwice
            // 
            this.btnTwice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTwice.Image = ((System.Drawing.Image)(resources.GetObject("btnTwice.Image")));
            this.btnTwice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTwice.Name = "btnTwice";
            this.btnTwice.Size = new System.Drawing.Size(23, 22);
            this.btnTwice.Text = "两倍大小";
            this.btnTwice.Click += new System.EventHandler(this.btnSize_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // pnVideo
            // 
            this.pnVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnVideo.Location = new System.Drawing.Point(0, 25);
            this.pnVideo.Name = "pnVideo";
            this.pnVideo.Size = new System.Drawing.Size(284, 236);
            this.pnVideo.TabIndex = 1;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(23, 22);
            this.btnSaveAs.Text = "另存为...";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // sfd
            // 
            this.sfd.DefaultExt = "mp4";
            this.sfd.Filter = "MP4 文件|*.mp4|所有文件|*.*";
            // 
            // FrmVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.pnVideo);
            this.Controls.Add(this.ts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "微信视频展示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVideo_FormClosing);
            this.Shown += new System.EventHandler(this.FrmVideo_Shown);
            this.Resize += new System.EventHandler(this.FrmVideo_Resize);
            this.ts.ResumeLayout(false);
            this.ts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ts;
        private System.Windows.Forms.ToolStripButton btnPlay;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnHalf;
        private System.Windows.Forms.ToolStripButton btnOriginal;
        private System.Windows.Forms.ToolStripButton btnTwice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel pnVideo;
        private System.Windows.Forms.ToolStripButton btnPause;
        private System.Windows.Forms.ToolStripButton btnSaveAs;
        private System.Windows.Forms.SaveFileDialog sfd;
    }
}