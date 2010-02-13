namespace TibiaEzBot.View
{
    partial class MiniMapWindow
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
            this.miniMap = new TibiaEzBot.View.Controls.MiniMap();
            this.miniMap1 = new TibiaEzBot.View.Controls.MiniMap();
            this.SuspendLayout();
            // 
            // miniMap
            // 
            this.miniMap.BackColor = System.Drawing.Color.White;
            this.miniMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.miniMap.Location = new System.Drawing.Point(0, 0);
            this.miniMap.Name = "miniMap";
            this.miniMap.Size = new System.Drawing.Size(284, 262);
            this.miniMap.TabIndex = 0;
            // 
            // miniMap1
            // 
            this.miniMap1.BackColor = System.Drawing.Color.White;
            this.miniMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.miniMap1.Location = new System.Drawing.Point(0, 0);
            this.miniMap1.Name = "miniMap1";
            this.miniMap1.Size = new System.Drawing.Size(284, 262);
            this.miniMap1.TabIndex = 0;
            // 
            // MiniMapWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.miniMap);
            this.Name = "MiniMapWindow";
            this.ShowInTaskbar = false;
            this.Text = "MiniMap";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MiniMapWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TibiaEzBot.View.Controls.MiniMap miniMap;
        public TibiaEzBot.View.Controls.MiniMap miniMap1;
    }
}