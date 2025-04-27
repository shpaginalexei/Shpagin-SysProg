using System.Runtime.InteropServices;

namespace CSharp
{
    enum MessageType
    {
        INIT,
        EXIT,
        START,
        SEND,
        STOP,
        CONFIRM,
    };

    public partial class Form1 : Form
    {
        [DllImport("DLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern int SendM_C(int type, int num = 0, int addr = 0, string str = "");


        public Form1()
        {
            InitializeComponent();
            int n = SendM_C((int)MessageType.INIT);
            if (n != -1)
            {
                listBox.Items.Add("Все потоки");
                listBox.Items.Add("Главный поток");
                for (int i = 0; i < n; i++)
                {
                    listBox.Items.Add($"Поток {i}");
                }
                listBox.SelectedIndex = 0;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            int n = SendM_C((int)MessageType.START, (int)numericUpDown.Value);
            if (n != -1)
            {
                if (n == listBox.Items.Count - 1)
                {
                    listBox.Items.Add($"Поток {listBox.Items.Count - 2}");
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
                else if (n > listBox.Items.Count - 1)
                {
                    int j = listBox.Items.Count - 2;
                    for (int i = 0; i < n - j; i++)
                    {
                        listBox.Items.Add($"Поток {j + i}");
                    }
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
                else if (n < listBox.Items.Count - 1)
                {
                    int j = listBox.Items.Count - 2;
                    for (int i = 0; i < j - n; i++)
                    {
                        listBox.Items.RemoveAt(listBox.Items.Count - 1);
                    }
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            int n = SendM_C((int)MessageType.STOP);
            if (n != -1)
            {
                if (n == listBox.Items.Count - 3)
                {
                    listBox.Items.RemoveAt(listBox.Items.Count - 1);
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
                else if (n < listBox.Items.Count - 3)
                {
                    int j = listBox.Items.Count - 2;
                    for (int i = 0; i < j - n; i++)
                    {
                        listBox.Items.RemoveAt(listBox.Items.Count - 1);
                    }
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
                else if (n > listBox.Items.Count - 3)
                {
                    int j = listBox.Items.Count - 2;
                    for (int i = 0; i < n - j; i++)
                    {
                        listBox.Items.Add($"Поток {j + i}");
                    }
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                }
            }
            }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = textBox.Text;
            int id = listBox.SelectedIndex - 2;

            int n = SendM_C((int)MessageType.SEND, 0, id, message);

            if (n != -1)
            {
                textBox.Text = string.Empty;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                SendM_C((int)MessageType.EXIT);
            }
            catch (Exception)
            {

            }
        }
    }
}
