using MythicEmpire.Card;
using MythicEmpire.LocalDatabase;
using MythicEmpire.Networking;
using MythicEmpire.Model;
using MythicEmpire.UI.Menu;
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
            builder.Register<NetworkingRealtime>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UserLogin>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UserRegister>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ForgotPassword>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LocalDatabaseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance<CardManager>(_manager);
            builder.RegisterInstance<UserModel>(_userModel);
        }
    }
}