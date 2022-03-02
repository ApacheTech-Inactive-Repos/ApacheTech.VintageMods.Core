using System;
using System.Diagnostics;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Abstractions
{
    public class TimedOperation : ScopedOperation
    {
        private readonly Stopwatch _sw;
        private readonly string _name;

        public event Action<string> OnStart;
        public event Action<string, TimeSpan> OnStop;

        public TimedOperation(string name, Action<string> onStart = null, Action<string, TimeSpan> onStop = null)
        {
            _name = name;
            if (onStart is not null) OnStart += onStart;
            if (onStop is not null) OnStop += onStop;
            OnStart?.Invoke(_name);
            _sw = new Stopwatch();
            _sw.Start();
        }

        public static TimedOperation Profile(string name)
        {
            void OnStart(string operationName)
            {
                ApiEx.Client.Event.EnqueueMainThreadTask(() => { ApiEx.Client.ShowChatMessage($"{operationName} has started."); }, "");
            }

            void OnStop(string operationName, TimeSpan dt)
            {
                ApiEx.Client.Event.EnqueueMainThreadTask(() => { ApiEx.Client.ShowChatMessage($"{operationName} ran for: {dt:g}ms"); }, "");
            }

            return new TimedOperation(name, OnStart, OnStop);
        }

        public override void OnFinally()
        {
            _sw.Stop();
            OnStop?.Invoke(_name, _sw.Elapsed);
        }
    }
}