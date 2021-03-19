
namespace GenshinBalladsOfBreezeAider
{
    partial class frmMain
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.groupGenshinProcess = new System.Windows.Forms.GroupBox();
            this.lblWindowSize = new System.Windows.Forms.Label();
            this.lblWindowSizeTitle = new System.Windows.Forms.Label();
            this.lblWindowLocation = new System.Windows.Forms.Label();
            this.lblWindowLocationTitle = new System.Windows.Forms.Label();
            this.lblWindowType = new System.Windows.Forms.Label();
            this.lblWindowTypeTitle = new System.Windows.Forms.Label();
            this.groupGenshinProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(121, 174);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始自动演奏";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(120, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(89, 12);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "未找到原神进程";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Location = new System.Drawing.Point(49, 23);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(65, 12);
            this.lblStatusTitle.TabIndex = 2;
            this.lblStatusTitle.Text = "程序状态：";
            // 
            // groupGenshinProcess
            // 
            this.groupGenshinProcess.Controls.Add(this.lblWindowSize);
            this.groupGenshinProcess.Controls.Add(this.lblWindowSizeTitle);
            this.groupGenshinProcess.Controls.Add(this.lblWindowLocation);
            this.groupGenshinProcess.Controls.Add(this.lblWindowLocationTitle);
            this.groupGenshinProcess.Controls.Add(this.lblWindowType);
            this.groupGenshinProcess.Controls.Add(this.lblWindowTypeTitle);
            this.groupGenshinProcess.Location = new System.Drawing.Point(22, 53);
            this.groupGenshinProcess.Name = "groupGenshinProcess";
            this.groupGenshinProcess.Size = new System.Drawing.Size(297, 101);
            this.groupGenshinProcess.TabIndex = 4;
            this.groupGenshinProcess.TabStop = false;
            this.groupGenshinProcess.Text = "原神窗口信息";
            // 
            // lblWindowSize
            // 
            this.lblWindowSize.AutoSize = true;
            this.lblWindowSize.Location = new System.Drawing.Point(112, 71);
            this.lblWindowSize.Name = "lblWindowSize";
            this.lblWindowSize.Size = new System.Drawing.Size(29, 12);
            this.lblWindowSize.TabIndex = 0;
            this.lblWindowSize.Text = "未知";
            // 
            // lblWindowSizeTitle
            // 
            this.lblWindowSizeTitle.AutoSize = true;
            this.lblWindowSizeTitle.Location = new System.Drawing.Point(6, 71);
            this.lblWindowSizeTitle.Name = "lblWindowSizeTitle";
            this.lblWindowSizeTitle.Size = new System.Drawing.Size(53, 12);
            this.lblWindowSizeTitle.TabIndex = 0;
            this.lblWindowSizeTitle.Text = "分辨率：";
            // 
            // lblWindowLocation
            // 
            this.lblWindowLocation.AutoSize = true;
            this.lblWindowLocation.Location = new System.Drawing.Point(112, 46);
            this.lblWindowLocation.Name = "lblWindowLocation";
            this.lblWindowLocation.Size = new System.Drawing.Size(29, 12);
            this.lblWindowLocation.TabIndex = 0;
            this.lblWindowLocation.Text = "未知";
            // 
            // lblWindowLocationTitle
            // 
            this.lblWindowLocationTitle.AutoSize = true;
            this.lblWindowLocationTitle.Location = new System.Drawing.Point(6, 46);
            this.lblWindowLocationTitle.Name = "lblWindowLocationTitle";
            this.lblWindowLocationTitle.Size = new System.Drawing.Size(77, 12);
            this.lblWindowLocationTitle.TabIndex = 0;
            this.lblWindowLocationTitle.Text = "左上角坐标：";
            // 
            // lblWindowType
            // 
            this.lblWindowType.AutoSize = true;
            this.lblWindowType.Location = new System.Drawing.Point(112, 21);
            this.lblWindowType.Name = "lblWindowType";
            this.lblWindowType.Size = new System.Drawing.Size(29, 12);
            this.lblWindowType.TabIndex = 0;
            this.lblWindowType.Text = "未知";
            // 
            // lblWindowTypeTitle
            // 
            this.lblWindowTypeTitle.AutoSize = true;
            this.lblWindowTypeTitle.Location = new System.Drawing.Point(7, 21);
            this.lblWindowTypeTitle.Name = "lblWindowTypeTitle";
            this.lblWindowTypeTitle.Size = new System.Drawing.Size(65, 12);
            this.lblWindowTypeTitle.TabIndex = 0;
            this.lblWindowTypeTitle.Text = "窗口状态：";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 216);
            this.Controls.Add(this.groupGenshinProcess);
            this.Controls.Add(this.lblStatusTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "原神风物之歌辅助程序";
            this.groupGenshinProcess.ResumeLayout(false);
            this.groupGenshinProcess.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.GroupBox groupGenshinProcess;
        private System.Windows.Forms.Label lblWindowSize;
        private System.Windows.Forms.Label lblWindowSizeTitle;
        private System.Windows.Forms.Label lblWindowLocation;
        private System.Windows.Forms.Label lblWindowLocationTitle;
        private System.Windows.Forms.Label lblWindowType;
        private System.Windows.Forms.Label lblWindowTypeTitle;
    }
}

