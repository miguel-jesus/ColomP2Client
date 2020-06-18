using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI cowText;
    public TextMeshProUGUI dogText;
    public TextMeshProUGUI horseText;
    public TextMeshProUGUI breadText;
    public TextMeshProUGUI scoreText;
    Player player;

    int score = 0;
    int cowScore = 0;
    int dogScore = 0;
    int horseScore = 0;
    int breadScore = 0;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        cowText.text = cowScore.ToString();
        horseText.text = horseScore.ToString();
        dogText.text = dogScore.ToString();
        breadText.text = breadScore.ToString();
        scoreText.text = score.ToString();
    }


    public void updateCowScore()
    {
        cowScore++;
        score += 3;
    }
    public void updateDogScore()
    {
        dogScore++;
        score += 5;
    }
    public void updateHorseScore()
    {
        horseScore++;
        score += 8;
    }
    public void updateBreadScore()
    {
        breadScore++;
    }

    public IEnumerator insertGame()
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
