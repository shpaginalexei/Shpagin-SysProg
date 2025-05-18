namespace Client
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
            usersListBox = new ListBox();
            messagesListBox = new ListBox();
            textBox = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            buttonSend = new Button();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // usersListBox
            // 
            usersListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            usersListBox.BorderStyle = BorderStyle.FixedSingle;
            usersListBox.FormattingEnabled = true;
            usersListBox.Location = new Point(0, 0);
            usersListBox.Name = "usersListBox";
            usersListBox.Size = new Size(260, 674);
            usersListBox.TabIndex = 0;
            // 
            // messagesListBox
            // 
            messagesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            messagesListBox.BorderStyle = BorderStyle.FixedSingle;
            messagesListBox.FormattingEnabled = true;
            messagesListBox.Location = new Point(260, 0);
            messagesListBox.Name = "messagesListBox";
            messagesListBox.Size = new Size(752, 610);
            messagesListBox.TabIndex = 1;
            // 
            // textBox
            // 
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox.Location = new Point(13, 13);
            textBox.Margin = new Padding(13, 13, 13, 13);
            textBox.MaxLength = 250;
            textBox.Name = "textBox";
            textBox.Size = new Size(547, 39);
            textBox.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(textBox);
            flowLayoutPanel1.Controls.Add(buttonSend);
            flowLayoutPanel1.Location = new Point(260, 613);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(751, 62);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // buttonSend
            // 
            buttonSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            buttonSend.Font = new Font("Segoe UI", 9F);
            buttonSend.Location = new Point(586, 13);
            buttonSend.Margin = new Padding(13, 13, 13, 13);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(146, 45);
            buttonSend.TabIndex = 3;
            buttonSend.Text = "Отправить";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1011, 693);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(messagesListBox);
            Controls.Add(usersListBox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "Form1";
            Text = "Шпагин А.Ю, АС-22-05 ЛР5";
            FormClosed += Form1_FormClosed;
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox usersListBox;
        private ListBox messagesListBox;
        private TextBox textBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button buttonSend;
    }
}
