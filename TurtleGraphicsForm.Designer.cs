namespace Turtle
{
    internal partial class TurtleGraphicsForm
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
            this.Graph = new System.Windows.Forms.PictureBox();
            this.TurtleHead = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TurtleHead)).BeginInit();
            this.SuspendLayout();
            // 
            // Graph
            // 
            this.Graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Graph.BackColor = System.Drawing.Color.White;
            this.Graph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Graph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Graph.ErrorImage = null;
            this.Graph.Location = new System.Drawing.Point(3, 3);
            this.Graph.Name = "Graph";
            this.Graph.Padding = new System.Windows.Forms.Padding(3);
            this.Graph.Size = new System.Drawing.Size(953, 806);
            this.Graph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Graph.TabIndex = 0;
            this.Graph.TabStop = false;
            this.Graph.Tag = "CurrentPostion; NewPostion";
            this.Graph.WaitOnLoad = true;
            this.Graph.SizeChanged += new System.EventHandler(this.Graph_SizeChanged);
            this.Graph.Paint += new System.Windows.Forms.PaintEventHandler(this.Graph_Paint);
            // 
            // TurtleHead
            // 
            this.TurtleHead.BackColor = System.Drawing.Color.Transparent;
            this.TurtleHead.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.TurtleHead.Image = global::Turtle.Properties.Resources.TurtleHeadIMG;
            this.TurtleHead.Location = new System.Drawing.Point(448, 393);
            this.TurtleHead.Name = "TurtleHead";
            this.TurtleHead.Size = new System.Drawing.Size(16, 16);
            this.TurtleHead.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.TurtleHead.TabIndex = 1;
            this.TurtleHead.TabStop = false;
            // 
            // TurtleGraphicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(959, 812);
            this.Controls.Add(this.TurtleHead);
            this.Controls.Add(this.Graph);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "TurtleGraphicsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Turtle";
            this.Load += new System.EventHandler(this.TurtleGraphicsForm_Load);
            this.Shown += new System.EventHandler(this.TurtleGraphicsForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.Graph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TurtleHead)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.PictureBox Graph;
        internal System.Windows.Forms.PictureBox TurtleHead;
    }
}

