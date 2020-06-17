using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public TMP_InputField nameInputField;
    public TMP_InputField dateBirthInputField;
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    public void onRegisterButtonClick()
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterNewUser()
    {
        if (string.IsNullOrEmpty(emailInputField.text))
        {
            throw new NullReferenceException("Email can't be void");
        }
        else if (string.IsNullOrEmpty(passwordInputField.text))
        {
            throw new NullReferenceException("Password can't be void");
        }
        else if (passwordInputField.text != confirmPasswordInputField.text)
        {
            throw new Exception("Passwords don't match");
        }


        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Account/Register", "POST");

        AspNetUserRegister newUser = new AspNetUserRegister();
        newUser.Email = emailInputField.text;
        newUser.Password = passwordInputField.text;
        newUser.ConfirmPassword = confirmPasswordInputField.text;

        string jsonData = JsonUtility.ToJson(newUser);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");
        httpClient.certificateHandler = new ByPassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
        }

        httpClient.Dispose();
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

    public static IEnumerator GetPlayerId()
    {
        Player player = FindObjectOfType<Player>();
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Account/UserId", "GET");

        byte[] bodyRaw = Encoding.UTF8.GetBytes("Nothing");
        httpClient.uploadHandler= new UploadHandlerRaw(bodyRaw);
       
        httpClient.downloadHandler = new DownloadHandlerBuffer();
        httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.SetRequestHeader("Authorization","bearer " + player.Token);
        httpClient.certificateHandler = new ByPassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            player.Id = httpClient.downloadHandler.text.Replace("\"", "");
        }

        httpClient.Dispose();
   
    }

    private IEnumerator RegisterNewPlayer()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Player/InsertPlayer", "POST");

        PlayerModel playerModel = new PlayerModel();
        playerModel.Id = player.Id;
        playerModel.Name = nameInputField.text;
        playerModel.DateBirth = dateBirthInputField.text;

        string jsonData = JsonUtility.ToJson(playerModel);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.certificateHandler = new ByPassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
        }
        else
        {
            player.Name = nameInputField.text;
            player.DateBirth = DateTime.Parse(dateBirthInputField.text);
        }

        httpClient.Dispose();
    }
    private IEnumerator RegisterUser()
    {
        yield return RegisterNewUser();
        yield return GetToken();
        yield return GetPlayerId();
        yield return RegisterNewPlayer();
        SceneManager.LoadScene(2);

    }
}
