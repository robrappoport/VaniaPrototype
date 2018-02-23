using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{

    Text _myText;

    // Use this for initialization
    void Start()
    {
        _myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _myText.text = "Score: " + GameManager.instance.score + " Best: " + GameManager.instance.highScore;
    }
}
