using System.Diagnostics;

namespace Lab1_CSharp
{
    public partial class Form1 : Form
    {
        Process? childProcess = null;
        System.Threading.EventWaitHandle stopEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StopEvent");
        System.Threading.EventWaitHandle startEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "StartEvent");
        System.Threading.EventWaitHandle confirmEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "ConfirmEvent");
        System.Threading.EventWaitHandle exitEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "ExitEvent");
        public Form1()
        {
            InitializeComponent();
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            listBox.Items.Clear();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (childProcess == null || childProcess.HasExited)
            {
                childProcess = Process.Start("Lab1 CPP.exe");
                childProcess.EnableRaisingEvents = true;
                childProcess.Exited += OnProcessExited;
                confirmEvent.WaitOne();
                listBox.Items.Add($"Все потоки");
                listBox.Items.Add($"Главный поток");
                listBox.SelectedIndex = 0;
            }
            int N = (int)numericUpDown.Value;
            int j = listBox.Items.Count;
            for (int i = 0; i < N; i++)
            {
                startEvent.Set();
                confirmEvent.WaitOne();
                listBox.Items.Add($"Thread {j + i - 1}");
            }
            listBox.SelectedIndex = listBox.Items.Count - 1;
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
    }
}
