using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Inicialización de Terminos
    
    /// <summary>
    /// Creamos un GameObject a introducir en el inspector, que será el láser 
    /// </summary>
    public GameObject laserPrefab;
    
    /// <summary>
    /// Velocidad de moviento del jugador
    /// </summary>
    public float movementSpeed = 10f;
   
    /// <summary>
    /// Variable a la cual introduciremos el movimiento con las teclas
    /// </summary>
    float movement;

    /// <summary>
    /// Velocidad del laser lanzado por el jugador
    /// </summary>
    public float proyectileSpeed = 25f;

    /// <summary>
    /// Frecuencia de disparo
    /// </summary>
    public float firingRate = 0.2f;

    /// <summary>
    /// vida del jugador
    /// </summary>
    public float health = 500f;

    /// <summary>
    /// Añadimos un padding a cada lado para que no salga la mitad de la nave por cada lado de la pantalla al jugar
    /// </summary>
    public float padding = 1f;

    /// <summary>
    /// Restricciones en el eje x
    /// </summary>
    float xmin;
    float xmax;

    /// <summary>
    /// Posición en la que se va a mover la nave
    /// </summary>
    float newX;

    /// <summary>
    /// Efecto de sonido de la nave al disparar
    /// </summary>
    public AudioClip firesound;

    #endregion

   
    void Start () {

        //Distancia entre la camara y el objeto
        float distance = transform.position.z - Camera.main.transform.position.z;
                                                                       //Leftmost side
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
                                                                        //Abajo a la derecha        
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));

        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;

    }
	
	
	void Update () {

        //Al pulsar la barra espaciadora, llamamos a lafunción Fire, con un delay entre disparo y disparo que le dá 
        //un toque más realista al disparar
        if (Input.GetKeyDown("space"))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }

        if (Input.GetKeyUp("space")) {

            CancelInvoke("Fire");
        }
        
        //rotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        movement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        //transform.Rotate(0, 0, rotation);
        transform.Translate(movement, 0, 0);


        //Restricción del gameSpace igual que en eSpawner
        newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        
    
    }
   
    /// <summary>
    /// Función que creará el láser disparado por la nave
    /// </summary>
    void Fire() {

        #region Offset
        //Offset es un vector3 el cual es creado para que el disparo no se cree exactamente en el transform.position de la nave
        //si no editamos los tags and layers esto sería un problema ya que los disparos se destruirían al chochar con el 
        //transform de la nave. Por eso le añado un offset.
        #endregion

        Vector3 offset = new Vector3(0, 1, 0);
        //Instanciamos los disparos, como gameObjectos y le añado un RigidBody para darles una velocidad y que parezca que estás disparando
        GameObject laser = Instantiate(laserPrefab, transform.position + offset, Quaternion.identity) as GameObject;
        laser.AddComponent<Rigidbody2D>();
        laser.GetComponent<Rigidbody2D>().gravityScale = 0f;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, proyectileSpeed, 0);
        //Al instanciar cada disparo, reproducimos el sonido de desparar
        AudioSource.PlayClipAtPoint(firesound, transform.position);
    }

    /// <summary>
    /// Cuando colisiona un misil lanzado con un objeto, tanto enemigo como aliado 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Llamamos al script Proyectile en el cual hemos creado el proyectil, dandole un daño editable al chocar con un objeto
        //y se lo restará a la vida. 
        Proyectile missile = collision.gameObject.GetComponent<Proyectile>();

        //Si el misil choca con un el jugador o con un enemigo, restará el valor del daño a la vida
        if (missile)
        {
            //Restamos a la variable health, el valor retornado de la función GetDamage() creada en el script Proyectile
            //Esta función retorna el valor del daño que causa un misil.
            //Restamos ese valor a heath
            health -= missile.GetDamage();
            missile.Hit();

            //Llamamos a la función Die() si la vida es igaul o menor a 0
            if (health <= 0)
            {
                Die();
            }

            Debug.Log("Enemy hit!");
        }
    }

    #region Destruye el objeto
    #endregion

    public void Die() {
        
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);

    }


}
