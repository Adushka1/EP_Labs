using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EPAM_3.BillingSystem;
using EPAM_3.BillingSystem.Enums;
using EPAM_3.BillingSystem.EventArgs;
using EPAM_3.Phones;
using EPAM_3.Phones.Interfaces;
using EPAM_3.TelephoneExchange.Interfaces;
using EPAM_3.Terminals.Interfaces;

namespace EPAM_3.Terminals
{
    public class Terminal : ITerminal
    {
        public IPhone PhoneNumber { get; }
        public IPort CurrentPort { get; private set; }

        private ITelephoneExchange _telephoneExchange;
        private IPhone _currentCollocutor;
        private bool? _isReceiver;
        private readonly TimeSpan _callReceiverDelayMs;
        private CancellationTokenSource _callReceivivngDelayCancellator;
        private IInfoBuilder _infoBuilder;

        public Terminal(IPhone phoneNumber, ITelephoneExchange telephoneExchange, TimeSpan callReceiverDelayMs, IInfoBuilder infoBuilder)
        {
            PhoneNumber = phoneNumber;
            _telephoneExchange = telephoneExchange;
            _callReceiverDelayMs = callReceiverDelayMs;
            _callReceivivngDelayCancellator = new CancellationTokenSource();
            _infoBuilder = infoBuilder;
        }

        public CallStatus MakeCall(IPhone receiverNumber)
        {
            var status = _telephoneExchange.ConnectAbonents(PhoneNumber, receiverNumber);
            return status;
        }

        public CallStatus ReceiveCall()
        {
            if (_currentCollocutor == null)
            {
                return CallStatus.Disconnected;
            }

            _callReceivivngDelayCancellator.Cancel();
            return CallStatus.Connected;
        }

        public CallStatus CloseCall()
        {
            if (this._isReceiver == true)
            {
                return _telephoneExchange.DisconnectAbonents(_currentCollocutor, PhoneNumber);
            }

            if (this._isReceiver == false)
            {
                return _telephoneExchange.DisconnectAbonents(PhoneNumber, _currentCollocutor);
            }
            return CallStatus.Error;
        }


        public bool ConnectToExchange()
        {
            _telephoneExchange.AbonentsConnected += ExchangeCallStartEventHandler;
            _telephoneExchange.AbonentsDisconnected += ExchangeCallEndEventHandler;

            return _telephoneExchange.ConnectToExchange(PhoneNumber);
        }

        private async void ExchangeCallStartEventHandler(object sender, RingEventArgs e)
        {
            if (PhoneNumber == e.Sender)
            {
                this._isReceiver = false;
                _currentCollocutor = e.Receiver;
            }

            if (PhoneNumber == e.Receiver)
            {
                this._isReceiver = true;
                _currentCollocutor = e.Sender;

                await Task.Delay(_callReceiverDelayMs, _callReceivivngDelayCancellator.Token)
                    .ContinueWith((task) =>
                    {
                        if (!task.IsCanceled)
                        {
                            this.CloseCall();
                        }
                    });
            }
        }

        private void ExchangeCallEndEventHandler(object sender, RingEventArgs e)
        {
            if (PhoneNumber == e.Sender || PhoneNumber == e.Receiver)
            {
                _currentCollocutor = null;
                this._isReceiver = false;
            }
        }

        public bool DisconnectFromExchange()
        {
            _telephoneExchange.AbonentsConnected -= ExchangeCallStartEventHandler;
            _telephoneExchange.AbonentsDisconnected -= ExchangeCallEndEventHandler;

            return _telephoneExchange.DisconnectFromExchange(PhoneNumber);
        }

        public bool ChangePort(IPort newPort)
        {
            return _telephoneExchange.MapToPort(PhoneNumber, newPort);
        }

        public IEnumerable<CallInfo> GetCallsHistory()
        {
            var callHistory = _infoBuilder.GetCallHistory(PhoneNumber);

            return callHistory;

        }
    }
}
