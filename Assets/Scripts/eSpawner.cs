using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eSpawner : MonoBehaviour {

    #region Inicialización de términos

    /// <summary>
    /// Prefab del enemigo a introducir en el inspector
    /// </summary>
    public GameObject enemyPrefab;
    
    /// <summary>
    /// Valores por defecto del ancho y alto
    /// </summary>
    public float width = 10f;
    public float heght = 5f;
    
    /// <summary>
    /// Velocidad de movimiento de los enemigos
    /// </summary>          
    public float speed = 13f;

    /// <summary>
    /// Ancho y alto máximos
    /// </summary>
    private float xmin;
    private float xmax;

    /// <summary>
    /// Bool, retorna si se esta moviendo o no
    /// </summary>
    private bool isMooving = true;

    /// <summary>
    /// Delay entre la instancia de cada nave
    /// </summary>
    public float spawnDelay = 0.5f;

    #endregion

    void Start()
    {
        //Distancia entre la cámara y el objeto
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;

        //Límite del juego por la parte izquierda, se controla con el tamaño de la cámara
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        //Límite del juego por la derecha
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        
        //Igualamos la posición del vector en X a las variables máximo y mínimo
        xmax = rightEdge.x;
        xmin = leftEdge.x;
        
        //Llamamos a la función SpawnUntilFull()
        SpawnUntilFull();
        
    }

    void SpawnUntilFull() {

        //Creamos un variable de tipo transform (freePosition) la cual igualamos al transform de la posición 
        // libre de la función NextFreePosition()
        Transform freePosition = NextFreePosition();
        //Si se cumple que freePosition es cierto
        if (freePosition)
        {
            //Instanciamos un enemigo, dandole la posición de freePosition, como un GameObject
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            //La hacemos hija de freePosition
            enemy.transform.parent = freePosition;
        }

        //Si childPositionGameObject == 0, llamamos a la función SpawnUntilFull, con un spawnDelay, que será la variable creada anteriormente
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);

        }
    }

    //Esta función se utiliza para visualizar la zona donde estarán los enemigos
    public void OnDrawGizmos(){

        Gizmos.DrawWireCube(transform.position, new Vector3(width, heght, 0));

    }

    // Update is called once per frame
    void Update()
    {
        //si isMooving es true 
        if (isMooving){
            //Damos un vector de movimiento al padre en la hererquía hacia la derecha
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else{
            //Vector de movimiento hacia la izquierda
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xmin){
            isMooving = true;
        }

        else if (rightEdgeOfFormation > xmax)
            isMooving = false;

        //Comprueba si hay enemigos, si no hay llama a la función SpawnUntilFull() para que genere más
        if (AllMembersDead()) {

            Debug.Log("Empty formation");
            SpawnUntilFull();

        }     
                 
    }

    Transform NextFreePosition() {

        //Accedemos a todas las posicíones de los enemigos instanciados
        foreach (Transform childPositionGameObject in transform)
        {
            //Si el número de enemigos es igual a 0, devuelve el transform de estos para que se vuelvan a instanciar
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }

        }
        //Devuelve null si no se cumple la función
        return null;

    }

    public bool AllMembersDead(){

        //Función booleana que comprueba los enemigos que hay, si hay enemigos sera true, else = false
        //Devuelve true o false
        foreach (Transform childPositionGameObject in transform) {

            if (childPositionGameObject.childCount > 0) {

                return false;
            }

        }

        return true;
    }

    public void SpawnEnemies() {

        //Función que crea los enemigos, se llamará a esta función si los enemigos == 0
        foreach (Transform child in transform)
        {
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }


    }
    
}
