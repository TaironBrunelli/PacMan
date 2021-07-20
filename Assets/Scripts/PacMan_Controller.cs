/// <summary>
/// This is the PacMan Base Controller:
/// - Life, Speed; 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CircleCollider2D))] //-- Unity will create the required components
[RequireComponent(typeof(Rigidbody2D))] //-- Unity will create the required components

public class PacMan_Controller : MonoBehaviour
{

    [Header("Movement")]
    //-- Movement
    private Vector2 m_Move;
    private float moveSpeed;
    [SerializeField]
    private float speed;

    //-- Character State 
    private bool isInvulnerable; //-- call if the character has to be invulnerable/ will not receive damage;
    private bool isPowerfull; //-- Toogle if player eat Energyzer;
    private bool isDead; //-- Toogle if HP <= 0;
    public static bool ghostEater; //-- Toogle if the player can eat Ghost;

    //-- Others
    private SpriteRenderer m_SpriteRenderer;
    private int playerLife;
    private int dotsEaten;
    private Renderer m_Renderer;

    //-- UI
    private int playerScore;
    public Text playeScoreUI;
    public Text playeHighScoreUI;
    public GameObject gameOverUI;
    public GameObject[] PlayerLivesUI;

    // -- Audio FX
    [Header("Audio FX")]
    public AudioClip[] eat_Sound;
    public AudioClip powerUp_Sound;
    public AudioClip deathInGame_Sound;
    private AudioSource m_Source;


    void Start()
    {
        playerLife = 3;
        playerScore = PlayerPrefs.GetInt("PlayerPoints", 0); //-- Load Score
        playeScoreUI.text = playerScore.ToString();
        playeHighScoreUI.text = PlayerPrefs.GetInt("PlayerHighPoints", 0).ToString(); //-- Load HighScore
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Renderer = GetComponent<Renderer>();
        m_Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
            if (m_Move.sqrMagnitude > 0.01)
                Move(m_Move);
        }
    }

    //-- Move -- Player
    public void OnMove(InputAction.CallbackContext context)
    {
        m_Move = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 direction)
    {
        float xDirection = direction.x;
        float yDirection = direction.y;

        //-- Animation
        if (xDirection >= 0)
            m_SpriteRenderer.flipX = false;
        if (xDirection < 0)
            m_SpriteRenderer.flipX = true;
        if (yDirection > 0)
            transform.eulerAngles = new Vector3(0, 0, 90);
        if (yDirection < 0)
            transform.eulerAngles = new Vector3(0, 0, -90);
        if (yDirection == 0)
            transform.eulerAngles = new Vector3(0, 0, 0);

        moveSpeed = speed * Time.fixedDeltaTime;

        transform.position = new Vector2(Mathf.Lerp(this.transform.position.x, this.transform.position.x + xDirection, moveSpeed),
                Mathf.Lerp(this.transform.position.y, this.transform.position.y + yDirection, moveSpeed));
    }

    //Called when the Trigger entered
    void OnTriggerEnter2D(Collider2D coll)
    {
        //-- Ghost collider
        if (coll.gameObject.CompareTag("Enemy")) //-- Kill the Player, Add score if Powerfull
        {
            if (!isPowerfull && !isInvulnerable) //-- Player can be killed
            {
                PlayDeathSound();
                PlayerKilled(); //-- Kill the Player
            }
            if (isPowerfull)
            {
                PlayEatSound(2);
                ResetGhost(coll.gameObject);
                ScoreUp(coll.gameObject, 100); //-- Disable the ghost and Add Score
            }
        }

        if (coll.gameObject.CompareTag("Dot")) //-- Add Score
        {
            PlayEatSound(0);
            ScoreUp(coll.gameObject, 10);
            dotsEaten++;
            if (dotsEaten == 240) //-- Stage Clear
                NextStage();
        }

        if (coll.gameObject.CompareTag("Energyzer")) //-- Add Score, PowerUp
        {
            PlayEatSound(1);
            PlayPowerUpSound();
            ScoreUp(coll.gameObject, 50);
            PowerUp();
        }
    }

    private void PlayerKilled()
    {
        if (playerLife > 0)
        {
            transform.position = new Vector3(11, -23, 0); //-- Reset Player position
            playerLife--;
            PlayerLivesUI[playerLife].SetActive(false); //-- Remove the Icon 'life' from UI;
            StartCoroutine(Invulnerability(1.2f)); //-- Player become invulnerable for the time;
        }
        else
        {
            PlayerPrefs.SetInt("PlayerPoints", 0); //-- Reset the save points
            if (playerScore > PlayerPrefs.GetInt("PlayerHighPoints", 0))
                PlayerPrefs.SetInt("PlayerHighPoints", playerScore); //-- New Highscore
            gameOverUI.SetActive(true);
            Destroy(gameObject); //-- Remove the Player
        }
    }

    //-- Called if Player eat the Ghost -> Revive after time

    private void ResetGhost(GameObject Ghost)
    {
        StartCoroutine(RespawnGhost(Ghost, 3f));
    }

    private IEnumerator RespawnGhost(GameObject Ghost, float t)
    {
        yield return new WaitForSeconds(t);
        Ghost.transform.position = new Vector3(11, 1, 0);
        Ghost.gameObject.SetActive(true);
    }

    private void ScoreUp(GameObject dot, int score)
    {
        dot.SetActive(false);
        playerScore += score;
        playeScoreUI.text = playerScore.ToString();
    }

    private void PowerUp()
    {
        StartCoroutine(Invulnerability(2.1f)); //-- Player become invulnerable for the time;
        StartCoroutine(Powefull(2.1f)); //-- Player become powerfull for the time;
    }

    private IEnumerator Invulnerability(float t)
    {
        StartCoroutine(Blink(t));
        isInvulnerable = true;
        yield return new WaitForSeconds(t);
        isInvulnerable = false;
    }

    private IEnumerator Powefull(float t)
    {
        ghostEater = true;
        isPowerfull = true;
        m_SpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(t);
        ghostEater = false;
        isPowerfull = false;
        m_SpriteRenderer.color = Color.white;
    }

    //-- Blink the Player -> Visual 'Invulnerable'
    IEnumerator Blink(float t)
    {
        float endTime = Time.time + t;
        while (Time.time < endTime)
        {
            m_Renderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            m_Renderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
    }

    //-- Audio Function
    private void PlayEatSound(int i)
    {
        m_Source.PlayOneShot(eat_Sound[i]);
    }

    private void PlayPowerUpSound()
    {
        m_Source.PlayOneShot(powerUp_Sound);
    }

    private void PlayDeathSound()
    {
        m_Source.PlayOneShot(deathInGame_Sound);
    }

    //-- Reload the scene
    private void NextStage()
    {
        dotsEaten = 0; //-- Reset counter
        PlayerPrefs.SetInt("PlayerPoints", playerScore);
        SceneManager.LoadScene("Game");
    }
}