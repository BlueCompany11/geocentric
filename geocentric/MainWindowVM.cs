using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

namespace geocentric
{
    public class MainWindowVM: INotifyPropertyChanged
    {
        public MainWindowVM()
        {
            amountOfSecForPing = 7;
            ButtonContent = "Wysyłaj zapytania PING przez " + amountOfSecForPing + " sek.";
            IsDestinationEnabled = true;
            canPing = false;
            Progress = 0;
            MinProg = 0;
            MaxProg = TimeSpan.FromSeconds(amountOfSecForPing).TotalMilliseconds;
        }
        string textInfo;
        public string TextInfo
        {
            get
            {
                return textInfo;
            }
            private set
            {
                textInfo = value;
                OnPropertyChanged(nameof(TextInfo));
            }
        }
        string destination;
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
                OnPropertyChanged(nameof(Destination));
                if (value != "")
                {
                    canPing = true;
                }
                else
                {
                    canPing = false;
                }
                PingCommand.CanExecute(canPing);
            }
        }
        bool isDestinationEnabled;
        public bool IsDestinationEnabled
        {
            get
            {
                return isDestinationEnabled;
            }
            set
            {
                isDestinationEnabled = value;
                OnPropertyChanged(nameof(IsDestinationEnabled));
            }
        }
        ICommand ping;
        bool canPing;
        public ICommand PingCommand
        {
            get
            {
                return ping ?? (ping = new CommandHandler(Ping, canPing));
            }
        }
        int amountOfSecForPing;
        async void Ping()
        {
            SetStateBeforePing();
            string blankBreak = "   ";
            Stopwatch s = new Stopwatch();
            s.Start();
            await Task.Run(async () =>
            {
                while (s.Elapsed < TimeSpan.FromSeconds(amountOfSecForPing))
                {
                    Progress = s.Elapsed.TotalMilliseconds;
                    var ping = new Ping();
                    var result = ping.SendPingAsync(Destination);
                    if (await Task.WhenAny(result, Task.Delay(TimeSpan.FromSeconds(5))) == result)
                    {
                        TextInfo += "PING status: " + result.Result.Status + blankBreak + "IP: " + result.Result.Address + blankBreak + "time = " + result.Result.RoundtripTime + " ms\n";
                    }
                    else
                    {
                        TextInfo += "PING trwał dłużej niż 5 sek";
                    }
                }
                Progress = MaxProg;
            });
            s.Stop();
            SetStateAfterPing();
        }
        void SetStateBeforePing()
        {
            TextInfo = "";
            canPing = false;
            PingCommand.CanExecute(canPing);
            TextInfo = "Odpowiedzi PING:\n";
            IsDestinationEnabled = false;
        }
        void SetStateAfterPing()
        {
            canPing = true;
            PingCommand.CanExecute(canPing);
            TextInfo += "Koniec\n";
            IsDestinationEnabled = true;
        }
        double progress;
        public double Progress
        {
            get
            {
                return progress;
            }
            private set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
        public double MinProg { get; }
        public double MaxProg { get; }
        public string ButtonContent { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
