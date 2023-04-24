using System;
using System.IO;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using JsonConvert = Unity.Plastic.Newtonsoft.Json.JsonConvert;

public class EditorAddCard : EditorWindow
{
    private CardInfo _newCard;
    private StatsCard _statsCard;
    private CardManager _cardManager;
    private VisualElement _containerDetail;
    private VisualElement _cardImage;
    private VisualElement _towerStats;
    private VisualElement _monsterStats;
    private VisualElement _spellStats;
    private Sprite _defaultItemIcon;

    // [MenuItem("MythicEmpire/Add Card")]
    public static void AddCard()
    {
        EditorAddCard wnd = GetWindow<EditorAddCard>();
        wnd.titleContent = new GUIContent("Add Card");
        Vector2 size = new Vector2(600, 450);
        wnd.minSize = size;
        wnd.maxSize = size;
    }

    private void CreateGUI()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> ("Assets/Editor/AddCardUXML.uxml");
        
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);
        
        _containerDetail = rootVisualElement.Q<VisualElement>("ContainerDetail");
        _cardImage = _containerDetail.Q<VisualElement>("Image");
        _defaultItemIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/UI/Lobby Source UI/SignalIcon.png");
        _towerStats = rootVisualElement.Q<VisualElement>("TowerStats");
        _spellStats = rootVisualElement.Q<VisualElement>("SpellStats");
        _monsterStats = rootVisualElement.Q<VisualElement>("MonsterStats");
        
        LoadCardManager();
        CreateNewCardInfor();
        
        SerializedObject so = new SerializedObject(_newCard);
        _containerDetail.Bind(so);
        
        _containerDetail.Q<ObjectField>("CardImage").RegisterValueChangedCallback(evt =>
        {
            Sprite newSprite = evt.newValue as Sprite;
            _newCard.CardImage = newSprite == null ? _defaultItemIcon : newSprite;
            _cardImage.style.backgroundImage = newSprite == null ? _defaultItemIcon.texture : newSprite.texture;
        });

        _containerDetail.Q<DropdownField>("TypeOfCard").RegisterValueChangedCallback(ChangeStats);
        rootVisualElement.Q<Button>("AddButton").clicked += AddNewCard;

    }

    private void ChangeStats(ChangeEvent<string> evt)
    {
        _monsterStats.style.display =  DisplayStyle.None;
        _spellStats.style.display =   DisplayStyle.None;
        _towerStats.style.display =   DisplayStyle.None;
        if (evt.newValue.Equals("Monster Card"))
        {
            _monsterStats.style.display = DisplayStyle.Flex;
            _statsCard = CreateInstance<MonsterStats>();;
            SerializedObject so = new SerializedObject(_statsCard);
            _monsterStats.Bind(so);
        }
        else if (evt.newValue.Equals("Spell Card"))
        {
            _spellStats.style.display = DisplayStyle.Flex;
            _statsCard = CreateInstance<SpellStats>();;
            SerializedObject so = new SerializedObject(_statsCard);
            _spellStats.Bind(so);
        }
        else if (evt.newValue.Equals("Tower Card"))
        {
            _towerStats.style.display = DisplayStyle.Flex;
            _statsCard = CreateInstance<TowerStats>();
            SerializedObject so = new SerializedObject(_statsCard);
            _towerStats.Bind(so);
        }


        
    }

    private void CreateNewCardInfor()
    {
        _newCard = CreateInstance<CardInfo>();
        _newCard.CardName = "New Card";
        _newCard.CardImage = _defaultItemIcon;
    }

    private void LoadCardManager()
    {
        _cardManager = AssetDatabase.LoadAssetAtPath<CardManager>("Assets/Data/CardManager.asset");
    }

    private void AddNewCard()
    {
        if(_newCard.TypeOfCard== CardType.None)return;
        _newCard.CardStats = _statsCard;
         if (!Directory.Exists($"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}"))
         {
             Directory.CreateDirectory($"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}");
         }
         if(!Directory.Exists($"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}/Stats"))
         {
             Directory.CreateDirectory($"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}/Stats");
         }
         AssetDatabase.CreateAsset(_newCard, $"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}/{_newCard.CardId}.asset");
         AssetDatabase.CreateAsset(_statsCard, $"Assets/Data/{_newCard.TypeOfCard}/{_newCard.CardName}/Stats/{_newCard.CardId}.asset");
         _cardManager.ListCards.Add(_newCard);
         
         Close();
         EditorCardManager.ShowListCard();
         
         
    }
}
