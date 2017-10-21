using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator animator;
    public GameObject game;
    public GameObject gameGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    private AudioSource audioPlayer;
    private float startY;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        bool isGrounded = startY == transform.position.y;

        if (isGrounded && gamePlaying && userAction)
        {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
	}

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            gameGenerator.SendMessage("CancelGenerator", true);
            game.GetComponent<GameController>().SendMessage("ResetTimeScale", 0.5f);

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();

            DustStop();
        } else if (other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }

    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    void DustStart()
    {
        dust.Play();
    }

    void DustStop()
    {
        dust.Stop();
    }
}
