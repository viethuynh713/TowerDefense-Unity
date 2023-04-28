using MythicEmpire.Card;
using MythicEmpire.Networking;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.Manager
{
    public class RootGameLifetime: LifetimeScope
    {
        [SerializeField] private CardManager _manager; 
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<NetworkingService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance<CardManager>(_manager);
        }
    }
}