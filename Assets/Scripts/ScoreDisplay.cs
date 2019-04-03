using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    /// <summary>
    /// Añadiremos este script a un UI de texto, score en la escena WinScreen, para que siga guardando el valor
    /// de score que teniamos en la escena Game. Se hacen státicas para poder acceder a ellas 
    /// </summary>
	
	void Start () {
        Text myText = GetComponent<Text>();
        myText.text = ScoreKeeper.score.ToString();
        ScoreKeeper.Reset();
	}
	
}
