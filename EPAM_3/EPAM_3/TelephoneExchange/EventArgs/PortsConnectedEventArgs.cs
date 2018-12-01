using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.TelephoneExchange.Interfaces;

namespace EPAM_3.TelephoneExchange.EventArgs
{
    public class PortsConnectedEventArgs : System.EventArgs
    {
        public IPhone SenderPhone { get; private set; }
        public IPort SenderPort { get; private set; }

        public IPhone ReciverPhone { get; private set; }
        public IPort ReciverPort { get; private set; }

        public PortsConnectedEventArgs(IPhone senderPhone, IPort senderPort, IPhone reciverPhone, IPort reciverPort)
        {
            SenderPhone = senderPhone;
            SenderPort = senderPort;

            ReciverPhone = reciverPhone;
            ReciverPort = reciverPort;
        }
    }
}
