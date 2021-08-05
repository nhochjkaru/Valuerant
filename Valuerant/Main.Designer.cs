
namespace Valuerant
{
    partial class Main
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
            this.StartBtt = new System.Windows.Forms.Button();
            this.FireRateNum = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.FireRateNum)).BeginInit();
            this.SuspendLayout();
            // 
            // StartBtt
            // 
            this.StartBtt.Location = new System.Drawing.Point(12, 12);
            this.StartBtt.Name = "StartBtt";
            this.StartBtt.Size = new System.Drawing.Size(75, 23);
            this.StartBtt.TabIndex = 0;
            this.StartBtt.Text = "Start";
            this.StartBtt.UseVisualStyleBackColor = true;
            this.StartBtt.Click += new System.EventHandler(this.Start_click);
            // 
            // FireRateNum
            // 
            this.FireRateNum.Location = new System.Drawing.Point(12, 41);
            this.FireRateNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.FireRateNum.Name = "FireRateNum";
            this.FireRateNum.Size = new System.Drawing.Size(120, 23);
            this.FireRateNum.TabIndex = 1;
            this.FireRateNum.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(142, 76);
            this.Controls.Add(this.FireRateNum);
            this.Controls.Add(this.StartBtt);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_load);
            ((System.ComponentModel.ISupportInitialize)(this.FireRateNum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartBtt;
        private System.Windows.Forms.NumericUpDown FireRateNum;
    }
}