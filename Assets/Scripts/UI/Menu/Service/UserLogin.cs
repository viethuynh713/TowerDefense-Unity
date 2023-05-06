using MythicEmpire.Manager;
using MythicEmpire.Networking;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.UI.Menu
{
    public class UserLogin :ILoginService,IStartable
    {
        [Inject] private IVerifyUserNetwork _verifyUserNetwork;
        public void Login(string email, string password)
        {
             _verifyUserNetwork.LoginRequest(email, password);

        }

        public void Start()
        {
            if (PlayerPrefs.HasKey("old-UserId"))
            {
                Notification.Instance.NotifyStatus("Login successfully");
                
            }
        }
    }
}