using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void OnLoginButtonClick()
    {
        StartCoroutine(TryLogin());
    } 

    private IEnumerator GetToken()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "Token", "POST");

        // application/x-www-form-urlencoded
        WWWForm dataToSend = new WWWForm();
        dataToSend.AddField("grant_type", "password");
        dataToSend.AddField("username", emailInputField.text);
        dataToSend.AddField("password", passwordInputField.text);

        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend.data);
        httpClient.downloadHandler = new DownloadHandlerBuffer();

        httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.certificateHandler = new ByPassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            string jsonResponse = httpClient.downloadHandler.text;
            AuthorizationToken authToken = JsonUtility.FromJson<AuthorizationToken>(jsonResponse);
            player.Token = authToken.access_token;
        }
        httpClient.Dispose();

    }

    private IEnumerator GetPlayerInfo()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Player/GetPlayerInfo", "GET");

        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.downloadHandler = new DownloadHandlerBuffer();
        httpClient.certificateHandler = new ByPassCertificate();

        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            PlayerModel playerModel = JsonUtility.FromJson<PlayerModel>(httpClient.downloadHandler.text);
            player.Id = playerModel.Id;
            player.Name = playerModel.Name;
            player.DateBirth = DateTime.Parse(playerModel.DateBirth);
        }

        httpClient.Dispose();
    }

    private IEnumerator  TryLogin()
    {
        yield return GetToken();
        yield return GetPlayerInfo();
        SceneManager.LoadScene(2);
    }


}
