using System.Collections;
using System.Collections.Generic;
using MythicEmpire.LocalDatabase;
using MythicEmpire.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class Loading : MonoBehaviour
{
    [Inject] private IVerifyUserNetwork _verifyUserNetwork;
    [Inject] private IUserDataLocal _userDataLocal;
    void Start()
    {
        StartCoroutine(LoadingScene());
        
    }

    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(1);
        var id = _userDataLocal.GetOldUserId();
        if (id != null)
        {
            _verifyUserNetwork.LoginRequest(id);
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
        
    }


}
