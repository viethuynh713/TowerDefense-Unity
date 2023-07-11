using System.Collections;
using System.Text;
using MythicEmpire.Card;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class PushAllCardToDatabase : MonoBehaviour
{
    public CardManager Manager;
    public string url = "https://localhost:7097/api/CardControl/addcard";
    private string test;
    public void Start()
    {
        foreach (var card in Manager.GetMultiCard())
        {
            StartCoroutine(Push(card));
            // Debug.Log(JsonConvert.SerializeObject(card));
        }
        // var data = Manager.GetCardById("0e47c4ae-ff6d-4785-b98f-002a537055f2");
        // Debug.Log(data);
    }

    IEnumerator Push(CardInfo data)
    {
        
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(ConvertData(data));
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    private string ConvertData(CardInfo data)
    {
        JObject jsonObj = new JObject(
            new JProperty("CardId", data.CardId),
            new JProperty("CardName", data.CardName),
            new JProperty("CardStar", data.CardStar),
            new JProperty("TypeOfCard", (int)data.TypeOfCard),
            new JProperty("CardRarity", (int)data.CardRarity)
            
        );
        Debug.Log(jsonObj.ToString());
        return jsonObj.ToString();
    }
}
