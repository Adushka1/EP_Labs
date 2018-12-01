using System;
using System.Collections.Generic;
using System.Text;
using EPAM_3.TelephoneExchange.Enums;
using EPAM_3.TelephoneExchange.Interfaces;

namespace EPAM_3.TelephoneExchange
{
    public class Port : IPort
    {
        private PortStatus _status;
        public event EventHandler<PortStatus> PortStatusChanged;
        public int Number { get; }

        public PortStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPortStatusChanged(value);
            }
        }

        public Port(int number)
        {
            Number = number;
            _status = PortStatus.Unused;
        }

        public void OnPortStatusChanged(PortStatus status)
        {
            PortStatusChanged?.Invoke(this, status);
        }

        public int CompareTo(IPort port)
        {
            return Number.CompareTo(port.Number);
        }

        public override bool Equals(object obj)
        {
            return Number.Equals((obj as Port)?.Number);
        }

        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
