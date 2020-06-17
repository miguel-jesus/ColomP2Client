using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public Button registerButton;

    private void RegisterNewUser()
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


        UnityWebRequest httpClient = new UnityWebRequest("api/Account/Register", "POST");

        AspNetUserRegister newUser = new AspNetUserRegister();
        newUser.Email = emailInputField.text;
        newUser.Password = passwordInputField.text;
        newUser.ConfirmPassword = confirmPasswordInputField.text;

        string jsonData = JsonUtility.ToJson(newUser);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);

        httpClient.SetRequestHeader("Content-Type", "application/json");

        httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
        }

        httpClient.Dispose();
    }

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


}
