using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SampleHostedApplication.Models;

namespace SampleHostedApplication.ViewModels
{
    public sealed partial class MainViewModel : ViewModelBase, IRecipient<LogMessageAdded>
    {
        public MainViewModel([FromKeyedServices("logs")] IMessenger logMessenger)
        {
            logMessenger.Register(this);
        }
        readonly StringBuilder _sb = new();
        Queue<string> _logs = new();
        public void Receive(LogMessageAdded message)
        {
            Dispatcher.UIThread.Post(() =>
            {
                while (_logs.Count > 3)
                {
                    _logs.Dequeue();
                }

                _logs.Enqueue(message.Message);

                _sb.Clear();
                foreach (var log in _logs)
                {
                    _sb.AppendLine(log);
                }

                LastLogMessages = _sb.ToString();
            });
        }

        [ObservableProperty]
        public partial string LastLogMessages { get; set; } = "";
    }
}
