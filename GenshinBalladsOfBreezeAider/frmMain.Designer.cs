
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
            this.lblWarring = new System.Windows.Forms.Label();
            this.lblDpi = new System.Windows.Forms.Label();
            this.lblRatio = new System.Windows.Forms.Label();
            this.lblDpiTitle = new System.Windows.Forms.Label();
            this.lblRatioTitle = new System.Windows.Forms.Label();
            this.btnDebugScreenshot = new System.Windows.Forms.Button();
            this.lblWindowSize = new System.Windows.Forms.Label();
            this.lblWindowSizeTitle = new System.Windows.Forms.Label();
            this.lblWindowLocation = new System.Windows.Forms.Label();
            this.lblWindowLocationTitle = new System.Windows.Forms.Label();
            this.lblWindowType = new System.Windows.Forms.Label();
            this.lblWindowTypeTitle = new System.Windows.Forms.Label();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.groupGenshinProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(122, 241);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 30);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始自动演奏(F10)";
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
            this.groupGenshinProcess.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupGenshinProcess.Controls.Add(this.lblWarring);
            this.groupGenshinProcess.Controls.Add(this.lblDpi);
            this.groupGenshinProcess.Controls.Add(this.lblRatio);
            this.groupGenshinProcess.Controls.Add(this.lblDpiTitle);
            this.groupGenshinProcess.Controls.Add(this.lblRatioTitle);
            this.groupGenshinProcess.Controls.Add(this.btnDebugScreenshot);
            this.groupGenshinProcess.Controls.Add(this.lblWindowSize);
            this.groupGenshinProcess.Controls.Add(this.lblWindowSizeTitle);
            this.groupGenshinProcess.Controls.Add(this.lblWindowLocation);
            this.groupGenshinProcess.Controls.Add(this.lblWindowLocationTitle);
            this.groupGenshinProcess.Controls.Add(this.lblWindowType);
            this.groupGenshinProcess.Controls.Add(this.lblWindowTypeTitle);
            this.groupGenshinProcess.Location = new System.Drawing.Point(22, 53);
            this.groupGenshinProcess.Name = "groupGenshinProcess";
            this.groupGenshinProcess.Size = new System.Drawing.Size(304, 181);
            this.groupGenshinProcess.TabIndex = 4;
            this.groupGenshinProcess.TabStop = false;
            this.groupGenshinProcess.Text = "原神窗口信息";
            // 
            // lblWarring
            // 
            this.lblWarring.ForeColor = System.Drawing.Color.Red;
            this.lblWarring.Location = new System.Drawing.Point(6, 140);
            this.lblWarring.Name = "lblWarring";
            this.lblWarring.Size = new System.Drawing.Size(283, 38);
            this.lblWarring.TabIndex = 9;
            // 
            // lblDpi
            // 
            this.lblDpi.AutoSize = true;
            this.lblDpi.Location = new System.Drawing.Point(126, 117);
            this.lblDpi.Name = "lblDpi";
            this.lblDpi.Size = new System.Drawing.Size(29, 12);
            this.lblDpi.TabIndex = 8;
            this.lblDpi.Text = "未知";
            // 
            // lblRatio
            // 
            this.lblRatio.AutoSize = true;
            this.lblRatio.Location = new System.Drawing.Point(126, 93);
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.Size = new System.Drawing.Size(29, 12);
            this.lblRatio.TabIndex = 8;
            this.lblRatio.Text = "未知";
            // 
            // lblDpiTitle
            // 
            this.lblDpiTitle.AutoSize = true;
            this.lblDpiTitle.Location = new System.Drawing.Point(7, 117);
            this.lblDpiTitle.Name = "lblDpiTitle";
            this.lblDpiTitle.Size = new System.Drawing.Size(35, 12);
            this.lblDpiTitle.TabIndex = 7;
            this.lblDpiTitle.Text = "DPI：";
            // 
            // lblRatioTitle
            // 
            this.lblRatioTitle.AutoSize = true;
            this.lblRatioTitle.Location = new System.Drawing.Point(6, 93);
            this.lblRatioTitle.Name = "lblRatioTitle";
            this.lblRatioTitle.Size = new System.Drawing.Size(53, 12);
            this.lblRatioTitle.TabIndex = 7;
            this.lblRatioTitle.Text = "长宽比：";
            // 
            // btnDebugScreenshot
            // 
            this.btnDebugScreenshot.Location = new System.Drawing.Point(216, 41);
            this.btnDebugScreenshot.Name = "btnDebugScreenshot";
            this.btnDebugScreenshot.Size = new System.Drawing.Size(75, 23);
            this.btnDebugScreenshot.TabIndex = 6;
            this.btnDebugScreenshot.Text = "显示位置";
            this.btnDebugScreenshot.UseVisualStyleBackColor = true;
            this.btnDebugScreenshot.Visible = false;
            this.btnDebugScreenshot.Click += new System.EventHandler(this.btnDebugScreenshot_Click);
            // 
            // lblWindowSize
            // 
            this.lblWindowSize.AutoSize = true;
            this.lblWindowSize.Location = new System.Drawing.Point(126, 69);
            this.lblWindowSize.Name = "lblWindowSize";
            this.lblWindowSize.Size = new System.Drawing.Size(29, 12);
            this.lblWindowSize.TabIndex = 0;
            this.lblWindowSize.Text = "未知";
            // 
            // lblWindowSizeTitle
            // 
            this.lblWindowSizeTitle.AutoSize = true;
            this.lblWindowSizeTitle.Location = new System.Drawing.Point(6, 69);
            this.lblWindowSizeTitle.Name = "lblWindowSizeTitle";
            this.lblWindowSizeTitle.Size = new System.Drawing.Size(53, 12);
            this.lblWindowSizeTitle.TabIndex = 0;
            this.lblWindowSizeTitle.Text = "分辨率：";
            // 
            // lblWindowLocation
            // 
            this.lblWindowLocation.AutoSize = true;
            this.lblWindowLocation.Location = new System.Drawing.Point(126, 45);
            this.lblWindowLocation.Name = "lblWindowLocation";
            this.lblWindowLocation.Size = new System.Drawing.Size(29, 12);
            this.lblWindowLocation.TabIndex = 0;
            this.lblWindowLocation.Text = "未知";
            // 
            // lblWindowLocationTitle
            // 
            this.lblWindowLocationTitle.AutoSize = true;
            this.lblWindowLocationTitle.Location = new System.Drawing.Point(6, 45);
            this.lblWindowLocationTitle.Name = "lblWindowLocationTitle";
            this.lblWindowLocationTitle.Size = new System.Drawing.Size(77, 12);
            this.lblWindowLocationTitle.TabIndex = 0;
            this.lblWindowLocationTitle.Text = "左上角坐标：";
            // 
            // lblWindowType
            // 
            this.lblWindowType.AutoSize = true;
            this.lblWindowType.Location = new System.Drawing.Point(126, 21);
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
            // debugTextBox
            // 
            this.debugTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugTextBox.Location = new System.Drawing.Point(11, 275);
            this.debugTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTextBox.Size = new System.Drawing.Size(315, 6);
            this.debugTextBox.TabIndex = 5;
            this.debugTextBox.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 283);
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.groupGenshinProcess);
            this.Controls.Add(this.lblStatusTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "原神风物之歌/镜花听世辅助程序";
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
        private System.Windows.Forms.TextBox debugTextBox;
        private System.Windows.Forms.Button btnDebugScreenshot;
        private System.Windows.Forms.Label lblRatio;
        private System.Windows.Forms.Label lblRatioTitle;
        private System.Windows.Forms.Label lblDpi;
        private System.Windows.Forms.Label lblDpiTitle;
        private System.Windows.Forms.Label lblWarring;
    }
}

