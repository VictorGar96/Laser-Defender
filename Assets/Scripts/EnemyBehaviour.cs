using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    #region Inicialización de Términos

    /// <summary>
    /// Prefab del láser enemigo
    /// </summary>
    public GameObject projectile;

    /// <summary>
    /// Velocidad del proyectil 
    /// </summary>
    public float projectileSpeed = 7f;

    /// <summary>
    /// Vida 
    /// </summary>
    public float health = 250;

    /// <summary>
    /// Cadencia de disparo
    /// </summary>
    public float shotsPerSecond = 0.5f;

    /// <summary>
    /// Valor que sumaremos a score al eliminar a un objetivo
    /// </summary>
    public int scoreValue = 150;

    /// <summary>
    /// Llamamos al script ScoreKeeper para acceder a la variable score y poder sumarle la puntución
    /// </summary>
    private ScoreKeeper scorekeeper;

    /// <summary>
    /// Clips de audio
    /// </summary>
    public AudioClip firesound;
    public AudioClip deathsound;

    #endregion

    private void Start()
    {
        //Buscamos el objeto Score en la hererquía y le agregamos el componente ScoreKeeper
        scorekeeper =  GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }


    private void Update()
    {
        //Creamos una variable de probabilidad de disparo para que disparen aleatoriamente
        float probaility = Time.deltaTime * shotsPerSecond;

        //Condición para llamar a la función Fire()
        if (Random.value < probaility) {

            EnemyFire();

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Creamos un a variable proyectile, para acceder al script proyectile y le damos el mismo behaviur que en PlayerController
        Proyectile missile = collision.gameObject.GetComponent<Proyectile>();

        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();

            if (health <= 0)
            {
                EnemyDown();
            }

            Debug.Log("Enemy hit!");
        }
    }

    /// <summary>
    /// función que destruirá el objeto enemigo 
    /// sumará a scorekeeper el valor de scoreValue a score, al matar a un enemigo
    /// y reproducirá el efecto de sonido
    /// </summary>
    public void EnemyDown() {

        Destroy(gameObject);
        //Pasamos el valor a score para que se lo sume en el scrip de ScoreKeeper cada vez que matemos a un enemigo
        scorekeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(deathsound, transform.position);

    }

    /// <summary>
    /// Función que instacia los disparos enemigos. Único detalle, es que al sumarle la velocidad hay que ponersela negativa
    /// para que vayan en dirección contraria. Es decir hacia abajo
    /// </summary>
    public void EnemyFire() {
        
        GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(firesound, transform.position);

    }

}
