using MythicEmpire;
using MythicEmpire.Networking;
using MythicEmpire.PlayerInfos;
using UnityEngine.Playables;
using VContainer;
using Notification = MythicEmpire.Manager.Notification;

namespace MythicEmpire.UI.Menu
{

    public class UserRegister : IRegisterService
    {

        [Inject]private IVerifyUserNetwork _verifyUserNetwork;
        public void Register(UserModel infos)
        {
            _verifyUserNetwork.RegisterRequest(infos.nickName, infos.email, infos.password);
        }
    }
}
