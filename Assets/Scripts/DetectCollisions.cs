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
    Player player;
    GameManager gameManager;

    void Start()
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
    }

   
    void Update()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Cow"))
        {
            gameManager.updateCowScore();
        }
        else if (gameObject.CompareTag("Horse"))
        {
            gameManager.updateHorseScore();
        }
        else if (gameObject.CompareTag("Dog"))
        {
            gameManager.updateDogScore();
        }
        Destroy(gameObject);
        Destroy(other.gameObject);
        gameManager.updateBreadScore();
    }

}
