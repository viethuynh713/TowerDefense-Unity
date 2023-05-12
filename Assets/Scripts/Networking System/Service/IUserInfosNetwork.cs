using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace MythicEmpire.Networking
{
    public interface IUserInfosNetwork
    {
        Task UpdateInfosRequest(string newNickName);
        Task UpdateRankRequest(string rankAdded);
        Task UpdateGoldRequest(string goldAdded);
        [ItemCanBeNull] Task<List<string>> GetListCardRequest();
    }
    
}