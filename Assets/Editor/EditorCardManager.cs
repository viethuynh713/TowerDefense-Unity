using System;
using System.Collections.Generic;
using System.Linq;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class EditorCardManager : EditorWindow
{

    private Sprite _defaultItemIcon;
    private VisualTreeAsset _itemRowTemplate;
    private CardManager _cardManager;
    private VisualElement _containerDetail;
    private VisualElement _cardImage;
    private VisualElement _itemsTab;
    private ListView _itemListView;
    private float _itemHeight = 50;
    private CardInfo _activeCard;
    private VisualElement _towerStats;
    private VisualElement _spellStats;
    private VisualElement _monsterStats;
    private StatsCard _statsCard;
    private List<CardInfo> _listCardRender;
    private RarityCard _filterRarity;
    private string _filterCardName = "";

    [MenuItem("MythicEmpire/Card Manager")]
    public static void ShowListCard()
    {
        EditorCardManager wnd = GetWindow<EditorCardManager>();
        wnd.titleContent = new GUIContent("Card Database");
        Vector2 size = new Vector2(1000, 500);
        wnd.minSize = size;
        wnd.maxSize = size;

    }

    private void CreateGUI()
    {
        // Import the UXML Window
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset> ("Assets/Editor/CardManagerUXML.uxml");
        
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);
        
        // Import the stylesheet    
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/CardManagerUSS.uss");
        rootVisualElement.styleSheets.Add(styleSheet);
        
        //Import the ListView Item Template
        _itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ItemTemplateUXML.uxml");
        _defaultItemIcon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/UI/MenuUI/Register/UserIcon", typeof(Sprite));
        
        //Get references for later
        _containerDetail = rootVisualElement.Q<VisualElement>("ContainerDetail");
        _cardImage = _containerDetail.Q<VisualElement>("Image");
        _towerStats = _containerDetail.Q<VisualElement>("TowerStats");
        _spellStats = _containerDetail.Q<VisualElement>("SpellStats");
        _monsterStats = _containerDetail.Q<VisualElement>("MonsterStats");

        //Load all existing item assets 
        LoadAllItems();
        _listCardRender = _cardManager.ListCards;
        //Populate the listview
        _itemsTab = rootVisualElement.Q<VisualElement>("ItemTab");
        GenerateListView();

        //Hook up button click events
        rootVisualElement.Q<Button>("AddButton").clicked += AddItem_OnClick;
        rootVisualElement.Q<Button>("DeleteButton").clicked += DeleteItem_OnClick;
        // Register Value Changed Callbacks for new items added to the ListView
         _containerDetail.Q<TextField>("CardName").RegisterValueChangedCallback(evt => 
         {
             _itemListView.Rebuild();
         });

         rootVisualElement.Q<DropdownField>("FilterByRarity").RegisterValueChangedCallback(evt =>
         {
             _filterRarity = Enum.Parse<RarityCard>(evt.newValue);
             GetCardRender();
             _itemListView.Rebuild();
         });
         rootVisualElement.Q<TextField>("FilterByName").RegisterValueChangedCallback(evt =>
         {
             _filterCardName = evt.newValue;
             GetCardRender();
             _itemListView.Rebuild();

         });
         _containerDetail.Q<DropdownField>("RarityCard").RegisterValueChangedCallback(evt => 
         {
             _itemListView.Rebuild();
         });         
         _containerDetail.Q<SliderInt>("CardStar").RegisterValueChangedCallback(evt => 
         {
             // Debug.Log("1");
             _itemListView.Rebuild();
         });
        _containerDetail.Q<ObjectField>("CardImage").RegisterValueChangedCallback(evt =>
        {
            Sprite newSprite = evt.newValue as Sprite;
            _activeCard.CardImage = newSprite == null ? _defaultItemIcon : newSprite;
            _cardImage.style.backgroundImage = newSprite == null ? _defaultItemIcon.texture : newSprite.texture;
        
            _itemListView.Rebuild();
            
        });

    }

    private void GetCardRender()
    {
        

        if (_filterRarity != RarityCard.None)
        {
            _listCardRender = _cardManager.ListCards.FindAll(card =>
                card.CardRarity == _filterRarity && card.CardName.Contains(_filterCardName));
        }
        else
        {
            _listCardRender = _cardManager.ListCards.FindAll(card =>
                 card.CardName.Contains(_filterCardName));
        }
        _itemListView.Rebuild();
    }


    private void ChangeStats(string value)
    {
        _monsterStats.style.display =  DisplayStyle.None;
        _spellStats.style.display =   DisplayStyle.None;
        _towerStats.style.display =   DisplayStyle.None;
       
        if (value.Equals("MonsterCard"))
        {
            _monsterStats.style.display = DisplayStyle.Flex;
            _statsCard = _activeCard.CardStats;
            SerializedObject so = new SerializedObject(_statsCard);
            _monsterStats.Bind(so);
        }
        else if (value.Equals("SpellCard"))
        {
            _spellStats.style.display = DisplayStyle.Flex;
            _statsCard = _activeCard.CardStats;
            SerializedObject so = new SerializedObject(_statsCard);
            _spellStats.Bind(so);
        }
        else if (value.Equals("TowerCard"))
        {
            _towerStats.style.display = DisplayStyle.Flex;
            _statsCard = _activeCard.CardStats;
            SerializedObject so = new SerializedObject(_statsCard);
            _towerStats.Bind(so);
        }
        

        
    }

private void DeleteItem_OnClick()
{

    //Get the path of the fie and delete it through AssetDatabase
    string path = AssetDatabase.GetAssetPath(_activeCard);
    string statsPath = AssetDatabase.GetAssetPath(_activeCard.CardStats);
    AssetDatabase.DeleteAsset(statsPath);
    AssetDatabase.DeleteAsset(path);
    
    //Purge the reference from the list and refresh the ListView
    _cardManager.ListCards.Remove(_activeCard);
    _listCardRender.Remove(_activeCard);
    _itemListView.Rebuild();
    UnityEditor.EditorUtility.SetDirty(_cardManager);
    UnityEditor.AssetDatabase.SaveAssets();
    UnityEditor.AssetDatabase.Refresh();
    //Nothing is selected, so hide the details section
    _containerDetail.style.visibility = Visibility.Hidden;
    _monsterStats.style.display =  DisplayStyle.None;
    _spellStats.style.display =   DisplayStyle.None;
    _towerStats.style.display =   DisplayStyle.None;

}

    /// <summary>
    /// Add a new Item asset to the Asset/Data folder
    /// </summary>
    private void AddItem_OnClick()
    {
        EditorAddCard.AddCard();
        Close();
    }
    

    /// <summary>
    /// Look through all items located in Assets/Data and load them into memory
    /// </summary>
    private void LoadAllItems()
    {
        _cardManager = AssetDatabase.LoadAssetAtPath<CardManager>("Assets/Data/CardManager.asset");
    }   
    
    /// <summary>
    /// Create the list view based on the asset data
    /// </summary>
    private void GenerateListView()
    {
        if (_listCardRender.Count == 0) return;
        //Defining what each item will visually look like. In this case, the makeItem function is creating a clone of the ItemRowTemplate.
        Func<VisualElement> makeItem = () => _itemRowTemplate.CloneTree();

        //Define the binding of each individual Item that is created. Specifically, 
        //it binds the Icon visual element to the scriptable object�s Icon property and the 
        //Name label to the FriendlyName property.
        Action<VisualElement, int> bindItem = (e, i) =>
        {
            e.Q<VisualElement>("CardImage").style.backgroundImage = _listCardRender[i].CardImage == null ? _defaultItemIcon.texture :  _listCardRender[i].CardImage.texture;
            e.Q<Label>("Name").text = _listCardRender[i].CardName;
            e.Q<Label>("Type").text = _listCardRender[i].TypeOfCard.ToString();
            e.Q<Label>("Rarity").text = _listCardRender[i].CardRarity.ToString();
            e.Q<Label>("Star").text = _listCardRender[i].CardStar.ToString();
        };

        //Create the listview and set various properties
        _itemListView = new ListView(_listCardRender, _itemHeight, makeItem, bindItem);
        _itemListView.selectionType = SelectionType.Single;
        _itemListView.style.height = 450;
        _itemsTab.Add(_itemListView);

        _itemListView.onSelectionChange += ListView_onSelectionChange;
        _itemListView.SetSelection(0);
        ChangeStats(_activeCard.TypeOfCard.ToString());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="selectedItems"></param>
    private void ListView_onSelectionChange(IEnumerable<object> selectedItems)
    {
        //Get the first item in the selectedItems list. 
        //There will only ever be one because SelectionType is set to Single
        _activeCard = (CardInfo)selectedItems.First();
        ChangeStats(_activeCard.TypeOfCard.ToString());

        //Create a new SerializedObject and bind the Details VE to it. 
        //This cascades the binding to the children
        SerializedObject so = new SerializedObject(_activeCard);
        _containerDetail.Bind(so);

        //Set the icon if it exists
        if (_activeCard.CardImage != null)
        {
            _cardImage.style.backgroundImage = _activeCard.CardImage.texture;
        }

        //Make sure the detail section is visible. This can turn off when you delete an item
        _containerDetail.style.visibility = Visibility.Visible;
    }

}

