using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsFrog : MonoBehaviour
{
    private Vector3 startPosition;
    private int stoneIndex;
    private bool isGrounded;
    private float jumpHeight = 2f;
    private float jumpVerticalSpeed;
    private float totalFlightTime;
    private Animator animator;
    private Vector3 velocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        stoneIndex = -1;
        startPosition = new Vector3(GameManager.instance.CoordenadaX(stoneIndex), -1.5f, 0f);
        transform.position = startPosition;
        isGrounded = true;

        // Calculamos el tiempo de vuelo y la velocidad vertical inicial
        // Primero calculamos el tiempo de subida, que es igual al de bajada, dejamos esto en la variable totalFlightTime,
        // que aún no tiene su valor definitivo
        totalFlightTime = Mathf.Sqrt(2 * jumpHeight / Physics.gravity.magnitude);
        // A partir del tiempo de subida se calcula la velocidad vertical inicial
        jumpVerticalSpeed = Physics.gravity.magnitude * totalFlightTime;
        // Ahora ya podemos calcular el verdadero valor de tiempo de vuelo
        totalFlightTime *= 2;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            if (stoneIndex == 10)
            {
                StartNewTrial();
                return;
            }

            stoneIndex = Random.Range(stoneIndex + 1, 11);
            float targetX = GameManager.instance.CoordenadaX(stoneIndex);
            float horizontalDistance = targetX - transform.position.x;
            velocity = new Vector3(horizontalDistance / totalFlightTime, jumpVerticalSpeed, 0f);
            isGrounded = false;
            animator.SetBool("rising", true);
            GameManager.instance.FrogJump();
        }
        else if (transform.position.x > GameManager.instance.CoordenadaX(stoneIndex))
        {
            isGrounded = true;
            velocity = Vector3.zero;
            Vector3 newPosition = transform.position;
            newPosition.y = startPosition.y;
            newPosition.x = GameManager.instance.CoordenadaX(stoneIndex);
            animator.SetBool("rising", false);
            animator.SetBool("falling", false);
        }

        if (!isGrounded)
        {
            transform.position += velocity * Time.deltaTime;
            velocity += Physics.gravity * Time.deltaTime;
        }

        if (animator.GetBool("rising") && velocity.y < 0)
        {
            animator.SetBool("rising", false);
            animator.SetBool("falling", true);
        }
    }

    private void StartNewTrial()
    {
        stoneIndex = -1;
        transform.position = startPosition;
        GameManager.instance.StartNewTrial();
    }
}
