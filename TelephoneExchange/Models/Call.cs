using System;
using TelephoneExchange.Interfaces;

namespace TelephoneExchange.Models
{
    public class Call
    {
        private ILogger _logger;

        public Guid Id { get; private set; }
        public int DurationInSec { get; private set; }

        public Call(ILogger logger)
        {
            _logger = logger;
            Id = Guid.NewGuid();
            SetDuration();
        }

        public void Answer(string agentName)
        {
            _logger.Log($"Оператор {agentName} начал разговор.");
        }

        public void HangUp(string agentName)
        {
            _logger.Log($"Оператор {agentName} закончил разговор длиной {DurationInSec}с.");
        }

        private void SetDuration()
        {
            Random r = new Random();
            DurationInSec = r.Next(5, 10);
        }
    }
}
