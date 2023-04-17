using System.Collections;
using MythicEmpire.Enums;

namespace MythicEmpire.Networking
{
    public interface ICardServiceNetwork
    {
        IEnumerator PurchaseCardRequest(string userId, string cardId);
        IEnumerator UpgradeCardRequest(string userId, string cardId);
        IEnumerator BuyGachaRequest(string userId, GachaType cardId);
    }
}