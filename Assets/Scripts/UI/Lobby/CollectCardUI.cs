using MythicEmpire.Card;
using UnityEngine;

namespace MythicEmpire.UI.Lobby
{
    public class CollectCardUI : MonoBehaviour
    {
        [SerializeField] private CardBaseUI _cardBaseUI;
        public void SetCard(CardInfo cardInfo)
        {
            _cardBaseUI.SetUI(cardInfo);
        }
    }
}