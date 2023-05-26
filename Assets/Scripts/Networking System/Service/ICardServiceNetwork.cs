using System.Collections;
using System.Threading.Tasks;
using MythicEmpire.Enums;

namespace MythicEmpire.Networking
{
    public interface ICardServiceNetwork
    {
        Task PurchaseCardRequest(string cardId);
        Task UpgradeCardRequest(string cardId);
        Task BuyGachaRequest(GachaType type);
    }
}