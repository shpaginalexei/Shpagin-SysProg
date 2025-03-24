using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Lab1_CSharp
{
    public partial class Form1 : Form
    {
        [DllImport("Lab2 DLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private static extern void MapSendMessage(int addr, string str);

        Process? childProcess = null;
        System.Threading.EventWaitHandle startEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StartEvent");
        System.Threading.EventWaitHandle dataEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "DataEvent");
        System.Threading.EventWaitHandle stopEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StopEvent");
        System.Threading.EventWaitHandle confirmEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "ConfirmEvent");
        System.Threading.EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "ExitEvent");
        public Form1()
        {
            InitializeComponent();
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            if (listBox.InvokeRequired)
            {
                listBox.Invoke(new Action(() => listBox.Items.Clear()));
                buttonSend.Invoke(new Action(() => buttonSend.Enabled = false));
                textBox.Invoke(new Action(() => textBox.Text = string.Empty));
                textBox.Invoke(new Action(() => textBox.Enabled = false));
            }
            else
            {
                listBox.Items.Clear();
                buttonSend.Enabled = false;
                textBox.Text = string.Empty;
                textBox.Enabled = false;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (childProcess == null || childProcess.HasExited)
            {
                childProcess = Process.Start("Lab1 CPP.exe");
                childProcess.EnableRaisingEvents = true;
                childProcess.Exited += OnProcessExited;
                confirmEvent.WaitOne();

                listBox.Items.Add("Все потоки");
                listBox.Items.Add("Главный поток");
                listBox.SelectedIndex = 0;

                buttonSend.Enabled = true;
                textBox.Enabled = true;
            }
            else
            {
                int N = (int)numericUpDown.Value;
                int j = listBox.Items.Count;
                for (int i = 0; i < N; i++)
                {
                    startEvent.Set();
                    confirmEvent.WaitOne();
                    listBox.Items.Add($"Поток {j + i - 2}");
                }
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (!(childProcess == null || childProcess.HasExited))
            {
                stopEvent.Set();
                confirmEvent.WaitOne();
                listBox.Items.RemoveAt(listBox.Items.Count - 1);
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!(childProcess == null || childProcess.HasExited))
            {
                exitEvent.Set();
                confirmEvent.WaitOne();
                childProcess = null;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (!(childProcess == null || childProcess.HasExited))
            {
                string message = textBox.Text;
                int id = listBox.SelectedIndex - 2;

                MapSendMessage(id, message);

                dataEvent.Set();
                confirmEvent.WaitOne();
                textBox.Text = string.Empty;
            }
        }
    }
}
