using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class JogadorController : MonoBehaviour
{
    public float velocidadeFrontal = 5f;
    public float velocidadeLateral = 10f;
    public float forcaPulo = 8f;
    public float gravidade = 20f;

    private CharacterController controller;
    private Vector3 direcao;
    private float faixaAtual = 0f; // -1 = esquerda, 0 = centro, 1 = direita
    private float larguraFaixa = 2f;
    private int pontuacao = 0;
    public TextMeshProUGUI textoPontuacao;

    public AudioClip somMoeda;
    private AudioSource audioSource;

    public Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            gameObject.AddComponent<CharacterController>();
            controller = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        // Movimento contínuo para frente
        direcao.z = velocidadeFrontal;

        // Movimento lateral por faixas
        if (Input.GetKeyDown(KeyCode.A) && faixaAtual > -1)
        {
            faixaAtual -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.D) && faixaAtual < 1)
        {
            faixaAtual += 1;
        }

        float alvoX = faixaAtual * larguraFaixa;
        float deltaX = alvoX - transform.position.x;
        direcao.x = deltaX * velocidadeLateral;

        // Pulo
        if (controller.isGrounded)
        {
            direcao.y = -1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                direcao.y = forcaPulo;
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            direcao.y -= gravidade * Time.deltaTime;
            animator.SetBool("isJumping", true);
        }

        controller.Move(direcao * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstaculo"))
        {
            Debug.Log("Colidiu com obstáculo!");
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuInicial");
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Aqui você pode parar o jogo, reiniciar a cena, reduzir vida etc.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Moeda"))
        {
            pontuacao++;
            Debug.Log("Moeda coletada! Pontuação: " + pontuacao);

            // Tocar som da moeda, se houver
            if (somMoeda != null && audioSource != null)
            {
                audioSource.PlayOneShot(somMoeda);
            }

            Destroy(other.gameObject);
            textoPontuacao.text = "Pontuação: " + pontuacao;
        }
    }
}
