using MythicEmpire.Card;
using MythicEmpire.Networking;
using MythicEmpire.PlayerInfos;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.Manager
{
    public class RootGameLifetime: LifetimeScope
    {
        [SerializeField] private CardManager _manager; 
        [SerializeField] private UserModel _userModel; 
        protected override void Configure(IContainerBuilder builder)
        {
       
            builder.Register<NetworkingService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<NetworkingConfig>(Lifetime.Singleton).AsSelf();
            builder.RegisterInstance<CardManager>(_manager);
            builder.RegisterInstance<UserModel>(_userModel);
        }
    }
}