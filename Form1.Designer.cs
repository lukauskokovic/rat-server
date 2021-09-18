
namespace ratserver
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectedClientsBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.shareScreenBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.shareScreenBox)).BeginInit();
            this.SuspendLayout();
            // 
            // connectedClientsBox
            // 
            this.connectedClientsBox.FormattingEnabled = true;
            this.connectedClientsBox.ItemHeight = 15;
            this.connectedClientsBox.Location = new System.Drawing.Point(583, 28);
            this.connectedClientsBox.Name = "connectedClientsBox";
            this.connectedClientsBox.Size = new System.Drawing.Size(205, 409);
            this.connectedClientsBox.TabIndex = 0;
            this.connectedClientsBox.SelectedIndexChanged += new System.EventHandler(this.connectedClientsBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(583, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connected clients:";
            // 
            // shareScreenBox
            // 
            this.shareScreenBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.shareScreenBox.Location = new System.Drawing.Point(12, 12);
            this.shareScreenBox.Name = "shareScreenBox";
            this.shareScreenBox.Size = new System.Drawing.Size(565, 425);
            this.shareScreenBox.TabIndex = 2;
            this.shareScreenBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(188)))), ((int)(((byte)(156)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.shareScreenBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectedClientsBox);
            this.Name = "Form1";
            this.Text = "RAT Fringl Pringl";
            ((System.ComponentModel.ISupportInitialize)(this.shareScreenBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox connectedClientsBox;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.PictureBox shareScreenBox;
    }
}

