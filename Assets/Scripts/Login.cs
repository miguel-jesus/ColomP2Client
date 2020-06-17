using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public Button loginButton;

    private void GetToken()
    {
        UnityWebRequest httpClient = new UnityWebRequest("Token", "POST");

        // application/x-www-form-urlencoded
        WWWForm dataToSend = new WWWForm();
        dataToSend.AddField("grant_type", "password");
        dataToSend.AddField("username", emailInputField.text);
        dataToSend.AddField("password", passwordInputField.text);

        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend.data);
        httpClient.downloadHandler = new DownloadHandlerBuffer();

        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            string jsonResponse = httpClient.downloadHandler.text;
            AuthorizationToken authToken = JsonUtility.FromJson<AuthorizationToken>(jsonResponse);
            
        }
        httpClient.Dispose();
    }

    private void TryLogin()
    {

        UnityWebRequest httpClient = new UnityWebRequest("api/Account/UserId", "GET");

        httpClient.SetRequestHeader("Authorization", "bearer ");
        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.downloadHandler = new DownloadHandlerBuffer();

        httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            // TODO

        }

        httpClient.Dispose();
    }


}
