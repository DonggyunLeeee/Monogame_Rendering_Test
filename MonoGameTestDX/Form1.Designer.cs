namespace Test_Layer_Points
{
    partial class Main_Form
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
            this.but_Test = new System.Windows.Forms.Button();
            this.H_WindowMain = new HalconDotNet.HSmartWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // but_Test
            // 
            this.but_Test.Location = new System.Drawing.Point(542, 933);
            this.but_Test.Name = "but_Test";
            this.but_Test.Size = new System.Drawing.Size(75, 23);
            this.but_Test.TabIndex = 0;
            this.but_Test.Text = "Test";
            this.but_Test.UseVisualStyleBackColor = true;
            this.but_Test.Click += new System.EventHandler(this.but_Test_Click);
            // 
            // H_WindowMain
            // 
            this.H_WindowMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.H_WindowMain.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.H_WindowMain.HDoubleClickToFitContent = true;
            this.H_WindowMain.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            this.H_WindowMain.HImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.H_WindowMain.HKeepAspectRatio = true;
            this.H_WindowMain.HMoveContent = true;
            this.H_WindowMain.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            this.H_WindowMain.Location = new System.Drawing.Point(624, 409);
            this.H_WindowMain.Margin = new System.Windows.Forms.Padding(0);
            this.H_WindowMain.Name = "H_WindowMain";
            this.H_WindowMain.Size = new System.Drawing.Size(418, 410);
            this.H_WindowMain.TabIndex = 1;
            this.H_WindowMain.WindowSize = new System.Drawing.Size(418, 410);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(170, 236);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 583);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(687, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(270, 206);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 982);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.H_WindowMain);
            this.Controls.Add(this.but_Test);
            this.Name = "Main_Form";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button but_Test;
        private HalconDotNet.HSmartWindowControl H_WindowMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

