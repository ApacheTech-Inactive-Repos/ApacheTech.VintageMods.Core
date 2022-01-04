using System;

namespace ApacheTech.VintageMods.Core.Common.InternalSystems
{
    public interface IAsyncActions : IDisposable
    {
        /// <summary>
        ///     Adds an action to the task list for the VintageMods shared thread.
        /// </summary>
        /// <param name="action">The action.</param>
        void EnqueueAsyncTask(Action action);

        /// <summary>
        ///     Adds an action to the task list for the main game thread.
        /// </summary>
        /// <param name="action">The action.</param>
        void EnqueueMainThreadTask(Action action);
    }
}