using MythicEmpire.Card;
using MythicEmpire.Model;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using Unity.Mathematics;
using UnityEngine;
using VContainer;

namespace MythicEmpire.InGame
{
    public class GameController_v2 : MonoBehaviour
    {
        public static GameController_v2 Instance;
        [SerializeField]private MapService_v2 _mapService;
        [Inject] private IRealtimeCommunication _realtimeCommunication;
        [Inject] private UserModel _userModel;
        [Inject] private CardManager _cardManager;
        public void Awake()
        {
            Instance = this;
        }

        public void BuildTower()
        {
            
        }

        public void CreateMonster(MonsterModel model)
        {
            var cardInfo = _cardManager.GetCardById(model.cardId);
            Instantiate(cardInfo.GameObjectPrefab, new Vector3(model.XLogicPosition, 1, model.YLogicPosition),
                quaternion.identity);
        }

        public void CreateSpell()
        {
            
        }
    }
}