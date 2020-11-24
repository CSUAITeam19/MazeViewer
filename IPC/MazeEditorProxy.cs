using System;
using NetMQ;
using NetMQ.Sockets;

namespace IPC
{
    /// <summary>
    /// python端迷宫编辑器的代理
    /// </summary>
    public class MazeEditorProxy:IDisposable
    {
        private const string ipcAddress = ">tcp://localhost:8965";
        private RequestSocket socket;
        public MazeEditorProxy()
        {
            socket = new RequestSocket(ipcAddress);
        }

        // debug
        public void SendFrameString(string str)
        {
            socket.SendFrame(str);
        }

        // debug
        public string ReceiveFrameString()
        {
            return socket.ReceiveFrameString();
        }

        public void Dispose()
        {
            socket?.Dispose();
        }
    }
}
