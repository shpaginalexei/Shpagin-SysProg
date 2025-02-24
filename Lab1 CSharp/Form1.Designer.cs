namespace Lab1_CSharp
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
            listBox = new ListBox();
            numericUpDown = new NumericUpDown();
            buttonStart = new Button();
            buttonStop = new Button();
            textBox = new TextBox();
            buttonSend = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.Dock = DockStyle.Left;
            listBox.FormattingEnabled = true;
            listBox.Location = new Point(0, 0);
            listBox.Name = "listBox";
            listBox.Size = new Size(187, 433);
            listBox.TabIndex = 0;
            // 
            // numericUpDown
            // 
            numericUpDown.Location = new Point(193, 12);
            numericUpDown.Name = "numericUpDown";
            numericUpDown.Size = new Size(58, 27);
            numericUpDown.TabIndex = 1;
            numericUpDown.TextAlign = HorizontalAlignment.Right;
            numericUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(193, 45);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(94, 29);
            buttonStart.TabIndex = 2;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // buttonStop
            // 
            buttonStop.Location = new Point(293, 45);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(94, 29);
            buttonStop.TabIndex = 3;
            buttonStop.Text = "Stop";
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // textBox
            // 
            textBox.Enabled = false;
            textBox.Location = new Point(262, 12);
            textBox.Name = "textBox";
            textBox.Size = new Size(225, 27);
            textBox.TabIndex = 4;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(393, 45);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(94, 29);
            buttonSend.TabIndex = 5;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(622, 433);
            Controls.Add(buttonSend);
            Controls.Add(textBox);
            Controls.Add(buttonStop);
            Controls.Add(buttonStart);
            Controls.Add(numericUpDown);
            Controls.Add(listBox);
            MinimumSize = new Size(640, 480);
            Name = "Form1";
            Text = "Шпагин А.Ю. АС-22-05 ЛР1";
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)numericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox;
        private NumericUpDown numericUpDown;
        private Button buttonStart;
        private Button buttonStop;
        private TextBox textBox;
        private Button buttonSend;
    }
}
