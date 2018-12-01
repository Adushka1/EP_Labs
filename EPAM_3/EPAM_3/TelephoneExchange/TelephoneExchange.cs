using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPAM_3.BillingSystem;
using EPAM_3.BillingSystem.Enums;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.BillingSystem.Interfaces;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.TelephoneExchange.Enums;
using EPAM_3.TelephoneExchange.EventArgs;
using EPAM_3.TelephoneExchange.Interfaces;

namespace EPAM_3.TelephoneExchange
{
    public class TelephoneExchange : ITelephoneExchange
    {
        private ISet<IPort> _freePorts;
        private ISet<IPhone> _avaliblePhones;
        private IDictionary<IPhone, IPort> _mappedPorts;

        private Func<IPhone, bool> CheckRingAvalible;

        public event EventHandler<RingEventArgs> AbonentsConnected;
        public event EventHandler<RingEventArgs> AbonentsDisconnected;

        public event EventHandler<PortsConnectedEventArgs> PortsConnected;

        public TelephoneExchange(ISet<IPort> ports, ISet<IPhone> phones, IBillingExchange exchangeBilling)
        {
            this._freePorts = ports;
            this._avaliblePhones = phones;
            _mappedPorts = new Dictionary<IPhone, IPort>();

            this.CheckRingAvalible += exchangeBilling.AbonentIsAvalible;
            this.AbonentsConnected += exchangeBilling.AbonentsConnectedEventHandler;
            this.AbonentsDisconnected += exchangeBilling.AbonentsDisconnectedEventHandler;
        }

        protected void OnAbonentsConnected(RingEventArgs e)
        {
            AbonentsConnected?.Invoke(this, e);
        }

        protected void OnAbonentsDisconnected(RingEventArgs e)
        {
            AbonentsDisconnected?.Invoke(this, e);
        }

        protected void OnPortsConnected(PortsConnectedEventArgs e)
        {
            PortsConnected?.Invoke(this, e);
        }

        public bool AvalibleForServe(IPhone sender, IPhone receiver)
        {
            if (sender == null || receiver == null)
            {
                return false;
            }

            var selfConnection = sender == receiver;
            var phonesAvalible = _avaliblePhones.Contains(sender) && _avaliblePhones.Contains(receiver);
            var connectedToPorts = _mappedPorts.ContainsKey(sender) && _mappedPorts.ContainsKey(receiver);

            return phonesAvalible && connectedToPorts && !selfConnection;
        }

        public CallStatus ConnectAbonents(IPhone sender, IPhone receiver)
        {
            if (!AvalibleForServe(sender, receiver))
            {
                return CallStatus.Error;
            }

            if (!CheckRingAvalible(sender))
            {
                return CallStatus.Locked;
            }

            if (!CheckRingAvalible(receiver))
            {
                return CallStatus.Engaged;
            }

            var callStatus = EstablishConnection(sender, receiver);
            if (callStatus == CallStatus.Connected)
            {
                OnAbonentsConnected(new RingEventArgs(sender, receiver));
            }

            return callStatus;
        }

        private CallStatus EstablishConnection(IPhone sender, IPhone receiver)
        {
            var receiverPort = _mappedPorts[receiver];
            var senderPort = _mappedPorts[sender];

            if (senderPort.Status == PortStatus.Listened && receiverPort.Status == PortStatus.Listened)
            {
                receiverPort.Status = PortStatus.Connected;
                senderPort.Status = PortStatus.Connected;

                OnPortsConnected(new PortsConnectedEventArgs(sender, senderPort, receiver, receiverPort));
                return CallStatus.Connected;
            }

            return CallStatus.Engaged;
        }


        public CallStatus DisconnectAbonents(IPhone sender, IPhone receiver)
        {
            if (!AvalibleForServe(sender, receiver))
            {
                return CallStatus.Error;
            }

            var senderPort = _mappedPorts[sender];
            var receiverPort = _mappedPorts[receiver];

            if (senderPort.Status == PortStatus.Connected && receiverPort.Status == PortStatus.Connected)
            {
                _mappedPorts[sender].Status = PortStatus.Listened;
                _mappedPorts[receiver].Status = PortStatus.Listened;

                OnAbonentsDisconnected(new RingEventArgs(sender, receiver));
                return CallStatus.Disconnected;
            }

            return CallStatus.Error;
        }

        public bool ConnectToExchange(IPhone abonent)
        {
            if (!_avaliblePhones.Contains(abonent))
            {
                return false;
            }

            if (!_mappedPorts.ContainsKey(abonent))
            {
                var isMapped = MapToPort(abonent);
                return isMapped;
            }

            return true;
        }

        private bool MapToPort(IPhone phone)
        {
            if (_freePorts.Count == 0)
            {
                return false;
            }

            var avaliblePort = _freePorts.First();
            _freePorts.Remove(avaliblePort);

            avaliblePort.Status = PortStatus.Listened;
            _mappedPorts.Add(phone, avaliblePort);

            return true;
        }

        public bool MapToPort(IPhone phoneNumber, IPort port)
        {
            if (!_freePorts.Contains(port))
            {
                return false;
            }

            _freePorts.Remove(port);
            _mappedPorts.Add(phoneNumber,port);

            return true;
        }

        public bool DisconnectFromExchange(IPhone phone)
        {
            if (!_mappedPorts.ContainsKey(phone))
            {
                return false;
            }

            var mappedPort = _mappedPorts[phone];
            _mappedPorts.Remove(phone);

            mappedPort.Status = PortStatus.Unused;
            _freePorts.Add(mappedPort);

            return true;
        }
    }
}
