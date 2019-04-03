using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    //Box collider creado para eliminar los disparos sobrantes reliazos tanto por la nave, como por los enemigos
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);

    }
}
