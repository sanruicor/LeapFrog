using UnityEngine;
using UnityEngine.InputSystem;

public class FrogController : MonoBehaviour
{
    private Vector3 startPosition;
    private int stoneIndex;
    private Vector3 midTargetPossition;
    private Vector3 endTargetPossition;
    private bool isGrounded;
    private float jumpHeight = 2f;
    private float speed = 5f;
    private Vector3 movementDirection;
    private Animator animator;

    void Start()
    {
        stoneIndex = -1;
        startPosition = new Vector3(GameManager.instance.CoordenadaX(stoneIndex), -1.5f, 0f);
        transform.position = startPosition;
        isGrounded = true;
        animator = GetComponent<Animator>();
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
            // Movimiento de piedra en piedra
            stoneIndex = Random.Range(stoneIndex + 1, 11);
            endTargetPossition = transform.position;
            endTargetPossition.x = GameManager.instance.CoordenadaX(stoneIndex);
            midTargetPossition = new Vector3((transform.position.x + endTargetPossition.x) / 2, transform.position.y + jumpHeight, transform.position.z);
            isGrounded = false;
            // para saber el vector de magnitud uno de dirección de movimiento
            movementDirection = (midTargetPossition - transform.position).normalized;
            GameManager.instance.FrogJump();
        }

        if (!isGrounded)
        {
            if (transform.position.x < midTargetPossition.x)
            {
                // Estoy subiendo
                transform.position += movementDirection * speed * Time.deltaTime;
                animator.SetBool("rising", true);
            }
            else
            {
                // Estoy bajando
                // Solo hay que recalcular la dirección de movimiento si esta es hacia arriba
                if (movementDirection.y > 0)
                {
                    movementDirection = (endTargetPossition - midTargetPossition).normalized;
                    animator.SetBool("rising", false);
                    animator.SetBool("falling", true);
                }
                transform.position += movementDirection * speed * Time.deltaTime;

                if (transform.position.x > endTargetPossition.x)
                {
                    isGrounded = true;
                    transform.position = endTargetPossition;
                    animator.SetBool("rising", false);
                    animator.SetBool("falling", false);
                }
            }
        }
    }

    private void StartNewTrial()
    {
        stoneIndex = -1;
        transform.position = startPosition;
        GameManager.instance.StartNewTrial();
    }
}
