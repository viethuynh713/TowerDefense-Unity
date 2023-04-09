using System.Collections;
using System.Collections.Generic;
using MythicEmpire.UI.Menu;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MenuLifeTime : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<UserLogin>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<UserRegister>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<ForgotPassword>(Lifetime.Singleton).AsImplementedInterfaces();
        
        
    }
}
