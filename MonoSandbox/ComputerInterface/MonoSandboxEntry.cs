using System;
using ComputerInterface.Interfaces;

namespace MonoSandbox.ComputerInterface
{
    class MonoSandboxEntry : IComputerModEntry
    {
        public string EntryName => "Mono Sandbox Mod";

        // This is the first view that is going to be shown if the user select you mod
        // The Computer Interface mod will instantiate your view 
        public Type EntryViewType => typeof(MonoSandboxView);
    }
}