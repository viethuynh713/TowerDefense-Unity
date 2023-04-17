using MythicEmpire.Networking;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.Manager
{
    public class RootGameLifetime: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<NetworkingService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}