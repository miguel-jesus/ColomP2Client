using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetBestScore : MonoBehaviour
{
    public GameObject[] gameScores;
    Player player;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private IEnumerator GetBestScores()
    {
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAdrees + "api/Games/GetGamesInfo", "GET");

        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.downloadHandler = new DownloadHandlerBuffer();
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.certificateHandler = new ByPassCertificate();

        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            Debug.Log(httpClient.error);
        }
        else
        {
            string jsonresponse = httpClient.downloadHandler.text;
            string response = "{\"gameModelList\":" + jsonresponse + "}";
            ScoreModelList gameModelList = JsonUtility.FromJson<ScoreModelList>(response);
            int index = 0;
            foreach (GamesModel gm in gameModelList.gameModelList)
            {
                Instantiate(gameScores[index]);
                index++;
                if (index > 5)
                {
                    break;
                }
            }

        }

        httpClient.Dispose();
    }
}
