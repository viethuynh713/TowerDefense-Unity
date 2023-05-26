
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Model
{
    [CreateAssetMenu(fileName = "UserInfo", menuName = "MythicEmpire Data/ User")]

    public class UserModel : ScriptableObject
    {
        public string email ;
        public string password ;
        public string userId ;
        public string nickName;
        public int rank;
        public int gold;
        public List<string> cardListID;
        public List<string> friendListID;
    }
}