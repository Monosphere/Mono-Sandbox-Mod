using ComputerInterface.Interfaces;
using Zenject;

namespace MonoSandbox.ComputerInterface
{
    class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            base.Container.Bind<IComputerModEntry>().To<MonoSandboxEntry>().AsSingle();
        }
    }
}