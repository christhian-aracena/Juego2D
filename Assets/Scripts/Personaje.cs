using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Personaje : MonoBehaviour
{
    private int countCoins;
    //Almacena la cantidad de monedas recolectadas por el jugador durante el juego. 
    public int life;
    //Representa la cantidad de vida actual del personaje. 
    private int xp;
    //Guarda la cantidad de experiencia acumulada por el jugador. 

    public RectTransform coinIconTransform; 
    // Referencia al RectTransform del icono de moneda en la UI
    public GameObject experienceCoinPrefab; 
    // Prefab de la moneda de experiencia


    public Animator heartAnimator;
    //Referencia al Animator que controla las animaciones de los corazones en la UI. 
    public TextMeshProUGUI coinTextTMP; 
    // Referencia al componente TextMeshProUGUI para mostrar la cantidad de monedas
    public TextMeshProUGUI coinXpTMP;
    //Componente TextMeshProUGUI para mostrar la cantidad de experiencia acumulada.
    public Animator animator;
    //Animator asociado al jugador para controlar las animaciones del mismo. 
    public float invincibilityDuration = 1f; 
    // Duración de la invencibilidad después de recibir daño
    public float damageCooldown = 1f;   
    // Tiempo de enfriamiento antes de poder recibir otro daño
    private bool isInvincible = false; 
    // Indica si el jugador es invencible
    private bool canTakeDamage = true;   
    // Indica si el jugador puede recibir daño

    public string gameOverSceneName = "GameOver";
    // Nombre de la escena GameOver
    private bool isDead = false;       
    // Indica si el jugador ya está muerto

    private Rigidbody2D rb;            
    // Referencia al Rigidbody2D del jugador
    private bool isDying = false;     
    // Indica si el jugador está muriendo
    private PlayerMovement movimientoScript;
    // Referencia al script de movimiento del jugador

    public GameObject heartsContainer; 
    // Contenedor de los corazones en el Canvas
    private List<GameObject> hearts;  
    // Lista para mantener los corazones

    void Start()
    {
        xp = 0;
        rb = GetComponent<Rigidbody2D>();
        // Obtener el Rigidbody2D del jugador
        life = 4; 
        // Inicializar la vida del jugador
        UpdateCoinText(); 
        // Asegurarse de que el texto de monedas se inicializa correctamente
        movimientoScript = GetComponent<PlayerMovement>(); 
        // Obtener referencia al script de movimiento

        // Inicializar la lista de corazones con los hijos del contenedor
        hearts = new List<GameObject>();
        foreach (Transform child in heartsContainer.transform)
        {
            hearts.Add(child.gameObject);
        }

        // Actualizar los corazones visualmente según la vida inicial
        UpdateHearts();
    }

    private void UpdateCoinText()
    {
        //Actualiza el texto que muestra la cantidad de monedas recolectadas en la interfaz de usuario. 
        if (coinTextTMP != null)
        {
            coinTextTMP.text = countCoins.ToString("D2");
        }
    }

    public void setCountCoins()
    {
        countCoins++;
        Debug.Log("Coins: " + countCoins); // Mensaje de depuración
        UpdateCoinText();  // Actualizar el texto de las monedas
    }

    public void setLifeDamage()
    {
        //validamos si puede tomar danio
        if (canTakeDamage && !isInvincible)
        {
            if (life > 0)
            {
                life--;
                canTakeDamage = false; // Activar el enfriamiento de daño
                StartCoroutine(EnableDamageCooldown());

                if (life > 0)
                {
                    StartCoroutine(InvincibilityCoroutine()); 
                    // llamamos al metodo para poner invible al enemigo despues de ser atacado
                }


                UpdateHearts(); // Actualizar los corazones visualmente

                if (life <= 0 && !isDead)
                {
                    isDead = true; // Marcar al jugador como muerto para evitar múltiples activaciones
                    Die();
                }
            }
        }
    }
    public void setExperience(int xp)
    {
        // Incrementa la experiencia del jugador y actualiza el texto correspondiente en la interfaz de usuario. 
        this.xp += xp;

        if(coinXpTMP != null) 
        {
            coinXpTMP.text = getExperience();
        }
    }

    public string getExperience() {  return this.xp.ToString("D2"); }
    //Retorna la cantidad actual de experiencia formateada como cadena. 
    public void setLifeHeal(int amount)
    //Incrementa la vida del jugador cuando recoge un corazón, actualizando visualmente los corazones en la UI. 
    {
        life += amount;
        UpdateHearts();
    }

    private IEnumerator EnableDamageCooldown()
    {
        //Coroutine que gestiona el tiempo de enfriamiento antes de que el jugador pueda recibir otro daño. 
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true; // Permitir recibir daño nuevamente después del tiempo de enfriamiento
    }

    private IEnumerator InvincibilityCoroutine()
    {
        //hace al jugador invencible temporalmente después de recibir daño, haciendo que parpadee visiblemente. 
        isInvincible = true; // El personaje se vuelve invencible

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float blinkInterval = 0.1f; // Intervalo de parpadeo en segundos

        for (float i = 0; i < invincibilityDuration; i += blinkInterval)
        {
            // Alternar entre visible e invisible
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Asegurarse de que el personaje es visible al final de la invencibilidad
        spriteRenderer.enabled = true;
        isInvincible = false; // El personaje deja de ser invencible
    }

    public void Die()
    {
        //Método que activa la animación de muerte del jugador, detiene su movimiento y carga la escena de Game Over. 
        if (!isDying)
        {
            isDying = true; // Indicar que el jugador está muriendo
            animator.SetTrigger("Die"); //trigger llamado "Die" del Animator
            rb.velocity = Vector2.zero; // Detener el movimiento del jugador
            rb.isKinematic = true; // Hacer el Rigidbody2D cinemático para evitar colisiones físicas

            // Bloquear movimiento en los ejes x, y, y z
            rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            // Deshabilitar el script de movimiento del jugador
            if (movimientoScript != null)
            {
                movimientoScript.enabled = false;
            }

            StartCoroutine(LoadGameOverScene());
            //cargamos la escena de game over luego de perder todos los corazones
        }
    }

    private IEnumerator LoadGameOverScene()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos antes de cambiar de escena (ajusta según necesites)
        SceneManager.LoadScene(gameOverSceneName); // Cambiar a la escena GameOver
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Colisión con enemigo detectada");
            setLifeDamage();
        }
        else if (collision.gameObject.CompareTag("xp"))
        {
            Debug.Log("Tocaste una moneda de xp!");

            Debug.Log("Experiencia antes: " + getExperience());
            //suma 1 de experiencia si toca una moneda de experiencia
            setExperience(1);
            Debug.Log("Experiencia total: " + getExperience());



            ExperienceCoinController coinxp = collision.gameObject.GetComponent<ExperienceCoinController>();
            Collider2D coinCollider = coinxp.GetComponent<Collider2D>();
            if (coinCollider != null)
                //hacemos que la moneda recogida se mueva hacia el icono de moneda.
            {
                coinCollider.enabled = false;
            }
            coinxp.MoveToIcon(coinIconTransform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Maneja colisiones por triggers con objetos como monedas y corazones
        if (other.gameObject.CompareTag("Coin"))
        {
            setCountCoins();
            Coin coinScript = other.gameObject.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.MoveToIconAndDestroy();
            }
        }
        
        else if (other.gameObject.CompareTag("Heart"))
        {
            Debug.Log("Colisión con corazón detectada");
            if (life < hearts.Count)
            {
                Debug.Log("Vida antes de recoger corazón: " + life);
                setLifeHeal(1);
                HeartPickup heartScript = other.gameObject.GetComponent<HeartPickup>();
                if (heartScript != null)
                {
                    heartScript.MoveToIconAndDestroy();
                }
                Debug.Log("Vida después de recoger corazón: " + life);
            }
            else if (other.gameObject.CompareTag("Enemy1"))
            {
                SwordLord enemy = other.gameObject.GetComponent<SwordLord>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
            }
            else
            {
                Debug.Log("La vida ya está completa. No se puede recoger más corazones.");
            }
        }
    }

    public void UpdateHearts()
    {
        // Actualizar la visibilidad de los corazones según la vida actual
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < life);
            PlayHeartAnimation();
        }
    }

    private void PlayHeartAnimation()
    {
        // Activa una animación en los corazones visuales para indicar cambios en la cantidad de vida.
        if (heartAnimator != null)
        {
            heartAnimator.SetTrigger("Rotate"); 

        }
    }

}
