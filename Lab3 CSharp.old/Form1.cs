using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Lab3_CSharp
{
    public partial class Form1 : Form
    {
        Socket? s = null;

        //[DllImport("Lab3 DLL.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]

        public Form1()
        {
            InitializeComponent();

            int nPort = 12345;
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), nPort);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(endPoint);

            int k = ReceiveInt(s);
            listBox.Items.Add(k);
        }

        static void SendString(Socket s, string str)
        {
            int n = str.Length * 2;
            s.Send(BitConverter.GetBytes(n), sizeof(int), SocketFlags.None);
            s.Send(Encoding.Unicode.GetBytes(str), n, SocketFlags.None);
        }

        static int ReceiveInt(Socket s)
        {
            byte[] b = new byte[sizeof(int)];
            s.Receive(b, sizeof(int), SocketFlags.None);
            int n = BitConverter.ToInt32(b, 0);
            return n;
        }


        //private void OnProcessExited(object sender, EventArgs e)
        //{
        //    if (listBox.InvokeRequired)
        //    {
        //        listBox.Invoke(new Action(() => listBox.Items.Clear()));
        //        buttonSend.Invoke(new Action(() => buttonSend.Enabled = false));
        //        textBox.Invoke(new Action(() => textBox.Text = string.Empty));
        //        textBox.Invoke(new Action(() => textBox.Enabled = false));
        //    }
        //    else
        //    {
        //        listBox.Items.Clear();
        //        buttonSend.Enabled = false;
        //        textBox.Text = string.Empty;
        //        textBox.Enabled = false;
        //    }
        //}

        private void buttonStart_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

    }
}
