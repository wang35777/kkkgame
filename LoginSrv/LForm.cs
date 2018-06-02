using Server.MirNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using S = ServerPackets;

namespace LoginSrv
{
    public partial class LForm : Form
    {
        private TcpListener _listener;
        public List<MirConnection> Connections = new List<MirConnection>();
        private int _sessionID;

        public LForm()
        {
            InitializeComponent();

            StartTCPListener();
        }

        private void InterfaceTimer_Tick(object sender, EventArgs e)
        {
               lock (Connections)
                {
                    for (int i = Connections.Count - 1; i >= 0; i--)
                        Connections[i].Process();
                }
        }

        private void StartTCPListener()
        {
            Packet.IsServer = true;
            _listener = new TcpListener(IPAddress.Parse("0.0.0.0"), 6500);
            _listener.Start();
            _listener.BeginAcceptTcpClient(Connection, null);
        }

        private void Connection(IAsyncResult result)
        {
            if (!_listener.Server.IsBound) return;

            try
            {
                TcpClient tempTcpClient = _listener.EndAcceptTcpClient(result);
                lock (Connections)
                    Connections.Add(new MirConnection(++_sessionID, tempTcpClient));
            }
            catch (Exception ex)
            {
                File.AppendAllText("Error Log (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                              String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, ex.ToString()));
            }
            finally
            {
                while (Connections.Count >= 100)
                    Thread.Sleep(1);

                if (_listener.Server.IsBound)
                    _listener.BeginAcceptTcpClient(Connection, null);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
