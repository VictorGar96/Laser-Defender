using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour {

    //Valor del daño causado por un proyectil
    public float damage = 100f;

    public float GetDamage() {

        return damage;
    }

    public void Hit() {

        Destroy(gameObject);

    }
}
