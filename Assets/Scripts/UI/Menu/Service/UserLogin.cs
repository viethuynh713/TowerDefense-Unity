using MythicEmpire.LocalDatabase;
using MythicEmpire.Manager;
using MythicEmpire.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.UI.Menu
{
    public class UserLogin :ILoginService
    {
        [Inject] private IVerifyUserNetwork _verifyUserNetwork;
        [Inject] private IUserDataLocal _userDataLocal;
        public void Login(string email, string password)
        {
             _verifyUserNetwork.LoginRequest(email, password);

        }
        
        // public void Start()
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}