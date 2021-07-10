namespace TelephoneExchange.Models
{
    public class Agent
    {
        public string Name { get; set; }

        public bool IsBusy { get; set; }

        public override string ToString() => Name;
    }
}
