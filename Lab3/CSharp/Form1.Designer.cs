namespace CSharp
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
            buttonSend = new Button();
            textBox = new TextBox();
            buttonStop = new Button();
            buttonStart = new Button();
            numericUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown).BeginInit();
            SuspendLayout();
            // 
            // listBox
            // 
            listBox.Dock = DockStyle.Left;
            listBox.FormattingEnabled = true;
            listBox.Location = new Point(0, 0);
            listBox.MinimumSize = new Size(350, 0);
            listBox.Name = "listBox";
            listBox.Size = new Size(350, 801);
            listBox.TabIndex = 0;
            // 
            // buttonSend
            // 
            buttonSend.Location = new Point(751, 76);
            buttonSend.Margin = new Padding(6);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(177, 53);
            buttonSend.TabIndex = 10;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // textBox
            // 
            textBox.Location = new Point(503, 15);
            textBox.Margin = new Padding(6);
            textBox.Name = "textBox";
            textBox.Size = new Size(425, 43);
            textBox.TabIndex = 9;
            // 
            // buttonStop
            // 
            buttonStop.Location = new Point(562, 76);
            buttonStop.Margin = new Padding(6);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new Size(177, 53);
            buttonStop.TabIndex = 8;
            buttonStop.Text = "Stop";
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // buttonStart
            // 
            buttonStart.Location = new Point(373, 76);
            buttonStart.Margin = new Padding(6);
            buttonStart.Name = "buttonStart";
            buttonStart.Size = new Size(177, 53);
            buttonStart.TabIndex = 7;
            buttonStart.Text = "Start";
            buttonStart.UseVisualStyleBackColor = true;
            buttonStart.Click += buttonStart_Click;
            // 
            // numericUpDown
            // 
            numericUpDown.Location = new Point(373, 15);
            numericUpDown.Margin = new Padding(6);
            numericUpDown.Name = "numericUpDown";
            numericUpDown.Size = new Size(108, 43);
            numericUpDown.TabIndex = 6;
            numericUpDown.TextAlign = HorizontalAlignment.Right;
            numericUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(15F, 37F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 801);
            Controls.Add(buttonSend);
            Controls.Add(textBox);
            Controls.Add(buttonStop);
            Controls.Add(buttonStart);
            Controls.Add(numericUpDown);
            Controls.Add(listBox);
            Name = "Form1";
            Text = "Шпагин А.Ю. АС-22-05 ЛР3";
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)numericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox;
        private Button buttonSend;
        private TextBox textBox;
        private Button buttonStop;
        private Button buttonStart;
        private NumericUpDown numericUpDown;
    }
}
