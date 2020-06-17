using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DetectCollisions : MonoBehaviour
{
    public TextMeshProUGUI cowText;
    public TextMeshProUGUI dogText;
    public TextMeshProUGUI horseText;
    public TextMeshProUGUI breadText;
    Player player;

    int score = 0;
    int cowScore = 0;
    int dogScore = 0;
    int horseScore = 0;
    int breadScore = 0;

    void Start()
    {
        player = FindObjectOfType<Player>();
        cowText = GetComponent<TextMeshProUGUI>();
        dogText = GetComponent<TextMeshProUGUI>();
        horseText = GetComponent<TextMeshProUGUI>();
        breadText = GetComponent<TextMeshProUGUI>();
    }

   
    void Update()
    {
        cowText.text = cowScore.ToString();
        horseText.text = horseScore.ToString();
        dogText.text = cowScore.ToString();
        breadText.text = breadScore.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Cow"))
        {
           
            cowScore++;
            score += 3;
        }
        else if (gameObject.CompareTag("Horse"))
        {
           
            horseScore++;
            score += 8;
        }
        else if (gameObject.CompareTag("Dog"))
        {
           
            dogScore++;
            score += 5;
        }
        else if (gameObject.CompareTag("Bread"))
        {
            
            breadScore++;
        }
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

    private IEnumerator insertGame()
    {

        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Games/InsertGame", "POST");

        GamesModel newGame = new GamesModel();
        newGame.PlayerName = player.Name;

        int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        int birthdate = int.Parse(player.DateBirth.ToString("yyyyMMdd"));
        int age = (now - birthdate) / 1000;
        newGame.Age = age.ToString();
        newGame.HourGame = DateTime.Now.ToString();
        newGame.Score = score.ToString();

        string jsonData = JsonUtility.ToJson(newGame);
        byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
        httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.SetRequestHeader("Content-Type", "application/json");
        httpClient.certificateHandler = new ByPassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("OnRegisterButtonClick: Error > " + httpClient.error);
        }

        httpClient.Dispose();
    }
}
