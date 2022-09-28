namespace 共通UI
{
    partial class Ucロード中
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelLineTop = new System.Windows.Forms.Label();
            this.labelLineBottom = new System.Windows.Forms.Label();
            this.labelLineLeft = new System.Windows.Forms.Label();
            this.labelLineRight = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(50, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "ロード中です...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GridPager.Properties.Resources.Loading_icon;
            this.pictureBox1.Location = new System.Drawing.Point(248, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 27);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // labelLineTop
            // 
            this.labelLineTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelLineTop.Location = new System.Drawing.Point(0, 0);
            this.labelLineTop.Name = "labelLineTop";
            this.labelLineTop.Size = new System.Drawing.Size(340, 5);
            this.labelLineTop.TabIndex = 2;
            // 
            // labelLineBottom
            // 
            this.labelLineBottom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelLineBottom.Location = new System.Drawing.Point(0, 121);
            this.labelLineBottom.Name = "labelLineBottom";
            this.labelLineBottom.Size = new System.Drawing.Size(340, 5);
            this.labelLineBottom.TabIndex = 3;
            // 
            // labelLineLeft
            // 
            this.labelLineLeft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelLineLeft.Location = new System.Drawing.Point(0, 0);
            this.labelLineLeft.Name = "labelLineLeft";
            this.labelLineLeft.Size = new System.Drawing.Size(5, 125);
            this.labelLineLeft.TabIndex = 4;
            // 
            // labelLineRight
            // 
            this.labelLineRight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelLineRight.Location = new System.Drawing.Point(336, 0);
            this.labelLineRight.Name = "labelLineRight";
            this.labelLineRight.Size = new System.Drawing.Size(5, 125);
            this.labelLineRight.TabIndex = 5;
            // 
            // Ucロード中
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.labelLineRight);
            this.Controls.Add(this.labelLineLeft);
            this.Controls.Add(this.labelLineBottom);
            this.Controls.Add(this.labelLineTop);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Name = "Ucロード中";
            this.Size = new System.Drawing.Size(340, 125);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelLineTop;
        private System.Windows.Forms.Label labelLineBottom;
        private System.Windows.Forms.Label labelLineLeft;
        private System.Windows.Forms.Label labelLineRight;
    }
}
