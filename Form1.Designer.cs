namespace geek_downloader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            label1 = new Label();
            label_login = new Label();
            listView_course = new ListView();
            label_name = new Label();
            label_phone = new Label();
            button_download = new Button();
            button_save = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            progressBar_download = new ProgressBar();
            backgroundWorker_downloader = new System.ComponentModel.BackgroundWorker();
            label_download = new Label();
            label_progress = new Label();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(17, 24);
            webView21.Margin = new Padding(2, 2, 2, 2);
            webView21.Name = "webView21";
            webView21.Size = new Size(906, 492);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            webView21.NavigationStarting += webView21_NavigationStarting;
            webView21.NavigationCompleted += webView21_NavigationCompleted;
            webView21.SourceChanged += webView21_SourceChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 4);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(82, 17);
            label1.TabIndex = 3;
            label1.Text = "Login Status:";
            // 
            // label_login
            // 
            label_login.AutoSize = true;
            label_login.ForeColor = Color.Gray;
            label_login.Location = new Point(124, 4);
            label_login.Margin = new Padding(2, 0, 2, 0);
            label_login.Name = "label_login";
            label_login.Size = new Size(30, 17);
            label_login.TabIndex = 4;
            label_login.Text = "OFF";
            // 
            // listView_course
            // 
            listView_course.FullRowSelect = true;
            listView_course.Location = new Point(17, 24);
            listView_course.Margin = new Padding(2, 2, 2, 2);
            listView_course.Name = "listView_course";
            listView_course.ShowItemToolTips = true;
            listView_course.Size = new Size(908, 494);
            listView_course.TabIndex = 5;
            listView_course.UseCompatibleStateImageBehavior = false;
            listView_course.View = View.Details;
            listView_course.ItemSelectionChanged += listView_course_ItemSelectionChanged;
            listView_course.MouseDoubleClick += listView_course_MouseDoubleClick;
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Location = new Point(184, 4);
            label_name.Margin = new Padding(2, 0, 2, 0);
            label_name.Name = "label_name";
            label_name.Size = new Size(0, 17);
            label_name.TabIndex = 6;
            // 
            // label_phone
            // 
            label_phone.AutoSize = true;
            label_phone.Location = new Point(268, 4);
            label_phone.Margin = new Padding(2, 0, 2, 0);
            label_phone.Name = "label_phone";
            label_phone.Size = new Size(0, 17);
            label_phone.TabIndex = 7;
            // 
            // button_download
            // 
            button_download.Location = new Point(658, 518);
            button_download.Margin = new Padding(2, 2, 2, 2);
            button_download.Name = "button_download";
            button_download.Size = new Size(126, 40);
            button_download.TabIndex = 8;
            button_download.Text = "Start Download";
            button_download.UseVisualStyleBackColor = true;
            button_download.Visible = false;
            button_download.Click += button_download_Click;
            // 
            // button_save
            // 
            button_save.Location = new Point(796, 518);
            button_save.Margin = new Padding(2, 2, 2, 2);
            button_save.Name = "button_save";
            button_save.Size = new Size(126, 40);
            button_save.TabIndex = 9;
            button_save.Text = "Set Download Path";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += button_save_Click;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // progressBar_download
            // 
            progressBar_download.Location = new Point(17, 526);
            progressBar_download.Margin = new Padding(2, 2, 2, 2);
            progressBar_download.Name = "progressBar_download";
            progressBar_download.Size = new Size(599, 16);
            progressBar_download.TabIndex = 11;
            progressBar_download.Visible = false;
            // 
            // backgroundWorker_downloader
            // 
            backgroundWorker_downloader.WorkerReportsProgress = true;
            backgroundWorker_downloader.DoWork += backgroundWorker_downloader_DoWork;
            backgroundWorker_downloader.ProgressChanged += backgroundWorker_downloader_ProgressChanged;
            backgroundWorker_downloader.RunWorkerCompleted += backgroundWorker_downloader_RunWorkerCompleted;
            // 
            // label_download
            // 
            label_download.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label_download.AutoSize = true;
            label_download.ForeColor = SystemColors.ControlDarkDark;
            label_download.Location = new Point(75, 544);
            label_download.Margin = new Padding(2, 0, 2, 0);
            label_download.Name = "label_download";
            label_download.Size = new Size(24, 17);
            label_download.TabIndex = 12;
            label_download.Text = "    ";
            label_download.TextAlign = ContentAlignment.MiddleRight;
            label_download.Visible = false;
            // 
            // label_progress
            // 
            label_progress.AutoSize = true;
            label_progress.ForeColor = SystemColors.ControlDarkDark;
            label_progress.Location = new Point(628, 528);
            label_progress.Margin = new Padding(2, 0, 2, 0);
            label_progress.Name = "label_progress";
            label_progress.Size = new Size(26, 17);
            label_progress.TabIndex = 13;
            label_progress.Text = "0%";
            label_progress.TextAlign = ContentAlignment.MiddleRight;
            label_progress.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            ClientSize = new Size(803, 516);
            Controls.Add(label_progress);
            Controls.Add(label_download);
            Controls.Add(progressBar_download);
            Controls.Add(button_save);
            Controls.Add(button_download);
            Controls.Add(label_phone);
            Controls.Add(label_name);
            Controls.Add(label_login);
            Controls.Add(label1);
            Controls.Add(webView21);
            Controls.Add(listView_course);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(2, 2, 2, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "GeekBang Downloader";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_login;
        private System.Windows.Forms.ListView listView_course;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.Label label_phone;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.Button button_save;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar_download;
        private System.ComponentModel.BackgroundWorker backgroundWorker_downloader;
        private System.Windows.Forms.Label label_download;
        private System.Windows.Forms.Label label_progress;
    }
}