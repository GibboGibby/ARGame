using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float fallingBlendTime = 0.2f;
    [SerializeField] private Slider slider;

    [SerializeField] private List<Collider> colliders = new List<Collider>();

    private float health = 100f;

    private NavMeshAgent agent;
    bool dying = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (dying) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnemyDied();
        }

        if (!GameManager.planesFound)
        {
            //Application.Quit();
            Vector3 moveDirection = GameManager.Instance.PlayerTransform.position - transform.position;
            moveDirection.Normalize();
            moveDirection.y = 0;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            Vector3 posToLookAt = GameManager.Instance.PlayerTransform.position;
            posToLookAt.y = transform.position.y;
            transform.LookAt(posToLookAt);
        }
        else
        {
            agent.SetDestination(GameManager.Instance.PlayerTransform.position);
        }
        

        
    }

    public void EnemyDied()
    {
        StartCoroutine(EnemyDyingCoroutine());
    }

    public void EnemyHit(float val)
    {
        if (dying) return;
        health -= val;
        slider.value = health;
        if (health <= 0)
            EnemyDied();
    }

    IEnumerator EnemyDyingCoroutine()
    {
        dying = true;
        GameManager.Instance.AddKilledPoints();
        //animator.SetBool("Walking", false);
        //animator.SetBool("TakingDamage", true);

        //animator.Play("Z_FallingForward");
        animator.CrossFade("Z_FallingForward", fallingBlendTime);
        slider.transform.parent.gameObject.SetActive(false);
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        //GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
