using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using SocketUtil;

namespace KChat
{
	public partial class Form1 : Form, IClient
	{
        protected internal string ip = string.Empty;
        protected internal int port = 0;
        protected internal Socket socket = null;
        protected internal byte[] buffer = new byte[1024 * 1024 * 2];
        public Form1()
		{
			InitializeComponent();
            this.StartSocket();
		}
        private void button1_Click(object sender, EventArgs e)
        {
			this.SendMessage(this.richTextBox1.Text);
			this.richTextBox1.Text = "";
		}

        private void button2_Click(object sender, EventArgs e)
        {
			this.richTextBox1.Text = "";
        }
        public void StartSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(address, 1234);
            socket.Connect(endPoint);
            Console.WriteLine("connect successfully");
            Thread thread = new Thread(ReceiveMessage);
            thread.IsBackground = true;
            thread.Start();
        }
        public void SendMessage(string msg)
        {
            socket.Send(Encoding.UTF8.GetBytes(msg));
        }
        public void ReceiveMessage()
        {
            while (true)
            {
                int length = socket.Receive(buffer);
                string msg = Encoding.UTF8.GetString(buffer, 0, length);
                HandleMessage(msg);
            }
        }
        public void HandleMessage(string msg)
        {
            switch (msg.Substring(0, 5))
            {
                case "[MSG]":
                    this.Invoke(new Action(() => {
                        this.webBrowser1.DocumentText += $"<p>{msg.Substring(6)}</p>";
                    }));
                    break;
                case "[ADD]":
                    this.Invoke(new Action(() => {
                        this.listBox1.Items.Add(msg.Substring(6));
                    }));
                    break;
                case "[DEL]":
                    this.Invoke(new Action(() => {
                        this.listBox1.Items.Remove(msg.Substring(6));
                    }));
                    break;
                case "[INF]":
                    this.Invoke(new Action(() => {
                        foreach (string i in msg.Substring(6).Split('|')) listBox1.Items.Add(i);
                    }));
                    break;
                default:
                    break;
            }
        }
    }
}
