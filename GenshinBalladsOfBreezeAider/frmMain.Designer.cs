
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
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.groupGenshinProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(161, 218);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(133, 38);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始自动演奏";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(160, 29);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(112, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "未找到原神进程";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Location = new System.Drawing.Point(65, 29);
            this.lblStatusTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(82, 15);
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
            this.groupGenshinProcess.Location = new System.Drawing.Point(29, 66);
            this.groupGenshinProcess.Margin = new System.Windows.Forms.Padding(4);
            this.groupGenshinProcess.Name = "groupGenshinProcess";
            this.groupGenshinProcess.Padding = new System.Windows.Forms.Padding(4);
            this.groupGenshinProcess.Size = new System.Drawing.Size(396, 126);
            this.groupGenshinProcess.TabIndex = 4;
            this.groupGenshinProcess.TabStop = false;
            this.groupGenshinProcess.Text = "原神窗口信息";
            // 
            // lblWindowSize
            // 
            this.lblWindowSize.AutoSize = true;
            this.lblWindowSize.Location = new System.Drawing.Point(149, 89);
            this.lblWindowSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowSize.Name = "lblWindowSize";
            this.lblWindowSize.Size = new System.Drawing.Size(37, 15);
            this.lblWindowSize.TabIndex = 0;
            this.lblWindowSize.Text = "未知";
            // 
            // lblWindowSizeTitle
            // 
            this.lblWindowSizeTitle.AutoSize = true;
            this.lblWindowSizeTitle.Location = new System.Drawing.Point(8, 89);
            this.lblWindowSizeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowSizeTitle.Name = "lblWindowSizeTitle";
            this.lblWindowSizeTitle.Size = new System.Drawing.Size(67, 15);
            this.lblWindowSizeTitle.TabIndex = 0;
            this.lblWindowSizeTitle.Text = "分辨率：";
            // 
            // lblWindowLocation
            // 
            this.lblWindowLocation.AutoSize = true;
            this.lblWindowLocation.Location = new System.Drawing.Point(149, 58);
            this.lblWindowLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowLocation.Name = "lblWindowLocation";
            this.lblWindowLocation.Size = new System.Drawing.Size(37, 15);
            this.lblWindowLocation.TabIndex = 0;
            this.lblWindowLocation.Text = "未知";
            // 
            // lblWindowLocationTitle
            // 
            this.lblWindowLocationTitle.AutoSize = true;
            this.lblWindowLocationTitle.Location = new System.Drawing.Point(8, 58);
            this.lblWindowLocationTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowLocationTitle.Name = "lblWindowLocationTitle";
            this.lblWindowLocationTitle.Size = new System.Drawing.Size(97, 15);
            this.lblWindowLocationTitle.TabIndex = 0;
            this.lblWindowLocationTitle.Text = "左上角坐标：";
            // 
            // lblWindowType
            // 
            this.lblWindowType.AutoSize = true;
            this.lblWindowType.Location = new System.Drawing.Point(149, 26);
            this.lblWindowType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowType.Name = "lblWindowType";
            this.lblWindowType.Size = new System.Drawing.Size(37, 15);
            this.lblWindowType.TabIndex = 0;
            this.lblWindowType.Text = "未知";
            // 
            // lblWindowTypeTitle
            // 
            this.lblWindowTypeTitle.AutoSize = true;
            this.lblWindowTypeTitle.Location = new System.Drawing.Point(9, 26);
            this.lblWindowTypeTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWindowTypeTitle.Name = "lblWindowTypeTitle";
            this.lblWindowTypeTitle.Size = new System.Drawing.Size(82, 15);
            this.lblWindowTypeTitle.TabIndex = 0;
            this.lblWindowTypeTitle.Text = "窗口状态：";
            // 
            // debugTextBox
            // 
            this.debugTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugTextBox.Location = new System.Drawing.Point(12, 263);
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.Size = new System.Drawing.Size(433, 0);
            this.debugTextBox.TabIndex = 5;
            this.debugTextBox.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 267);
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.groupGenshinProcess);
            this.Controls.Add(this.lblStatusTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStart);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.TextBox debugTextBox;
    }
}

