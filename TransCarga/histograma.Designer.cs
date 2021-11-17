namespace TransCarga
{
    partial class histograma
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
            this.pic_flechH = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_flechH)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_flechH
            // 
            this.pic_flechH.Image = global::TransCarga.Properties.Resources.abajo100T;
            this.pic_flechH.Location = new System.Drawing.Point(290, 4);
            this.pic_flechH.Name = "pic_flechH";
            this.pic_flechH.Size = new System.Drawing.Size(110, 130);
            this.pic_flechH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_flechH.TabIndex = 0;
            this.pic_flechH.TabStop = false;
            this.pic_flechH.Visible = false;
            // 
            // histograma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pic_flechH);
            this.Name = "histograma";
            this.Text = "histograma";
            this.Load += new System.EventHandler(this.histograma_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_flechH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_flechH;
    }
}