using System.Diagnostics;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.Game;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Abstractions
{
    public class TimedOperation : ScopedOperation
    {
        private readonly Stopwatch _sw;
        private readonly string _name;

        public TimedOperation(string name)
        {
            _name = name;
            ApiEx.Client.EnqueueShowChatMessage($"{_name} has begun.");
            _sw = new Stopwatch();
            _sw.Start();
        }

        public static TimedOperation Profile(string name)
        {
            return new TimedOperation(name);
        }

        public override void OnFinally()
        {
            _sw.Stop();
            ApiEx.Client.EnqueueShowChatMessage($"{_name} ran for: {_sw.Elapsed:g}ms");
        }
    }
}