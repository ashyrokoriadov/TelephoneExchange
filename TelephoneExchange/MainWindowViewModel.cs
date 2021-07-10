using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using TelephoneExchange.Implementations;
using TelephoneExchange.Models;
using TelephoneExchange.UserControls.ViewModels;

namespace TelephoneExchange
{
    public class MainWindowViewModel
    {
        public ConcurrentQueue<Call> Calls { get; set; } = new ConcurrentQueue<Call>();
        public AgentListViewModel Agents { get; set; }
        public WpfLogger LogConsole { get; set; } = new WpfLogger();
        public ICommand AddCall { get; set; }

        public MainWindowViewModel()
        {
            AddCall = new CommandHandler()
            {
                Method = AddCallMethod,
                CanExecuteMethod = CanAddCall
            };

            Agents = new AgentListViewModel(LogConsole);
            ThreadPool.QueueUserWorkItem(AddNewCall);
            ThreadPool.QueueUserWorkItem(HandleCalls);
        }

        private void AddCallMethod(object parameter)
        {
            Call newCall = new Call(LogConsole);
            Calls.Enqueue(newCall);
            LogConsole.Log($"Новое соединение {newCall.Id} было поставлено в очередь.");
            LogConsole.Log($"Количество соединений в очереди: { Calls.Count()}.");
        }

        private bool CanAddCall(object parameter) => true;

        private void AddNewCall(object stateInfo)
        {
            while (true)
            {
                AddCallMethod(null);

                Random r = new Random();
                int sleepTime = r.Next(1, 5);
                Thread.Sleep(sleepTime * 1000);
            }
        }

        private void HandleCalls(object stateInfo)
        {
            while (true)
            {
                foreach (Agent agent in Agents.Agents)
                    ThreadPool.QueueUserWorkItem(HandleCall);

                Thread.Sleep(3 * 1000);
            }
        }

        private void HandleCall(object stateInfo)
        {
            Agent agent = Agents.Agents.FirstOrDefault(x => !x.IsBusy);

            if (agent != null && Calls.TryDequeue(out Call call))
            {
                lock (agent)
                {
                    call.Answer(agent.Name);
                    agent.IsBusy = true;
                    LogConsole.Log($"Количество соединений в очереди: { Calls.Count()}.");
                    Thread.Sleep(call.DurationInSec * 1000);
                    call.HangUp(agent.Name);
                    agent.IsBusy = false;
                }
            }
        }

        
    }
}
