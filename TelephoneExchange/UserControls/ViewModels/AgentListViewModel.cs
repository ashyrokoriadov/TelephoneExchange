using TelephoneExchange.Implementations;
using System.Windows.Input;
using System.Collections.ObjectModel;
using TelephoneExchange.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TelephoneExchange.Interfaces;

namespace TelephoneExchange.UserControls.ViewModels
{
    public class AgentListViewModel : INotifyPropertyChanged
    {
        private ILogger _logger;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddAgentCommand { get; set; }
        public ICommand RemoveAgentCommand { get; set; }
        public ObservableCollection<Agent> Agents { get; set; } = new ObservableCollection<Agent>();

        private Agent _agent;
        public Agent SelectedAgent
        {
            get => _agent;
            set
            {
                _agent = value;
                NotifyPropertyChanged();
            }
        }

        private string _newAgentName;
        public string NewAgentName
        {
            get => _newAgentName;
            set
            {
                _newAgentName = value;
                NotifyPropertyChanged();
            }
        }

        public AgentListViewModel(ILogger logger)
        {
            _logger = logger;

            Agents.Add(new Agent() { Name = "Bill" });
            Agents.Add(new Agent() { Name = "Rick" });
            Agents.Add(new Agent() { Name = "Harry" });

            AddAgentCommand = new CommandHandler()
            {
                Method = AddMethod,
                CanExecuteMethod = CanAdd
            };

            RemoveAgentCommand = new CommandHandler()
            {
                Method = RemoveMethod,
                CanExecuteMethod = CanRemove
            };
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddMethod(object parameter)
        {
            Agent agent = new Agent()
            {
                Name = NewAgentName
            };
            Agents.Add(agent);
            NewAgentName = string.Empty;
            _logger.Log($"Оператор {agent.Name} добавлен.");
        }

        public bool CanAdd(object parameter) => !string.IsNullOrEmpty(NewAgentName);

        public void RemoveMethod(object parameter)
        {
            var agentName = SelectedAgent.Name;
            Agents.Remove(SelectedAgent);
            _logger.Log($"Оператор {agentName} удален.");
        }

        public bool CanRemove(object parameter) => SelectedAgent != null;
    }
}
