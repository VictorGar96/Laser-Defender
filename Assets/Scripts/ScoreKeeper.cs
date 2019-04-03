using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {
    /// <summary>
    /// Variable estática score, que será la puntuación.
    /// La hacemos estática para que el valor permanezca en el Template aun que el objeto sea destruido
    /// Lo que nos permite guardar su valor 
    /// </summary>
    
    public static int score = 0;
    //Variable texto donde guardaremos el valor de score y se podrá ver el valor de score en el cmabas como un objeto de texto
    private Text myText;

    private void Start()
    {
       //al empezar reseteamos el valor de score a 0
       myText =  GetComponent<Text>();
       Reset();
    }


    public void Score(int points) {

        score += points;
        myText.text = score.ToString();

    }

    public static void Reset()
    {
        score = 0;
       
    }

}
