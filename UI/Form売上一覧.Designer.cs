namespace GridPager
{
    partial class Form売上一覧
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.dtp開始 = new System.Windows.Forms.DateTimePicker();
            this.dtp終了 = new System.Windows.Forms.DateTimePicker();
            this.btn検索 = new System.Windows.Forms.Button();
            this.ucロード中 = new 共通UI.Ucロード中();
            this.ucGridPager = new 共通UI.UcGridPager();
            this.SuspendLayout();
            // 
            // dtp開始
            // 
            this.dtp開始.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dtp開始.Location = new System.Drawing.Point(25, 36);
            this.dtp開始.Name = "dtp開始";
            this.dtp開始.Size = new System.Drawing.Size(178, 28);
            this.dtp開始.TabIndex = 1;
            // 
            // dtp終了
            // 
            this.dtp終了.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dtp終了.Location = new System.Drawing.Point(241, 36);
            this.dtp終了.Name = "dtp終了";
            this.dtp終了.Size = new System.Drawing.Size(178, 28);
            this.dtp終了.TabIndex = 2;
            // 
            // btn検索
            // 
            this.btn検索.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn検索.Location = new System.Drawing.Point(473, 36);
            this.btn検索.Name = "btn検索";
            this.btn検索.Size = new System.Drawing.Size(94, 28);
            this.btn検索.TabIndex = 3;
            this.btn検索.TabStop = false;
            this.btn検索.Text = "検索";
            this.btn検索.UseVisualStyleBackColor = true;
            this.btn検索.Click += new System.EventHandler(this.btn検索_Click);
            // 
            // ucロード中
            // 
            this.ucロード中.BackColor = System.Drawing.Color.Black;
            this.ucロード中.Location = new System.Drawing.Point(315, 250);
            this.ucロード中.Name = "ucロード中";
            this.ucロード中.Size = new System.Drawing.Size(340, 125);
            this.ucロード中.TabIndex = 4;
            // 
            // ucGridPager
            // 
            this.ucGridPager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucGridPager.Location = new System.Drawing.Point(0, 82);
            this.ucGridPager.Name = "ucGridPager";
            this.ucGridPager.RowsInPage = 100;
            this.ucGridPager.Size = new System.Drawing.Size(975, 631);
            this.ucGridPager.TabIndex = 0;
            // 
            // Form売上一覧
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(91)))), ((int)(((byte)(108)))));
            this.ClientSize = new System.Drawing.Size(971, 713);
            this.Controls.Add(this.btn検索);
            this.Controls.Add(this.dtp終了);
            this.Controls.Add(this.dtp開始);
            this.Controls.Add(this.ucロード中);
            this.Controls.Add(this.ucGridPager);
            this.Name = "Form売上一覧";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "売上一覧";
            this.Load += new System.EventHandler(this.Form売上一覧_Load);
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private 共通UI.UcGridPager ucGridPager;
        private System.Windows.Forms.DateTimePicker dtp開始;
        private System.Windows.Forms.DateTimePicker dtp終了;
        private System.Windows.Forms.Button btn検索;
        private 共通UI.Ucロード中 ucロード中;
    }
}

