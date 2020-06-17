using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const string _httpServerAdrees = "https://localhost:44338/";

    public string HttpServerAdrees
    {
        get
        {
            return _httpServerAdrees;
        }
    }

    public string _token;
    public string Token
    {
        get { return _token; }
        set { _token = value; }
    }

    public string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public DateTime _dateBirth;
    public DateTime DateBirth
    {
        get { return _dateBirth; }
        set { _dateBirth = value; }
    }

    private void Awake()
    {
        int count = FindObjectsOfType<Player>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }

}

