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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip zombieGrowl;
    [SerializeField] private float growlVolume = 1.0f;
    [SerializeField] private float timeBetweenGrowl;

    private float health = 100f;

    private NavMeshAgent agent;
    bool dying = false;

    private float timer = 0f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("Walking", true);
        StartCoroutine(ZombieSpawning());
    }

    private bool spawning = false;
    private IEnumerator ZombieSpawning()
    {
        slider.gameObject.SetActive(false);
        spawning = true;
        agent.enabled = false;
        audioSource.PlayOneShot(zombieGrowl, growlVolume);
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        animator.CrossFade("Z_FallingBack", 0.0f);
        yield return new WaitForSeconds(3.0f);
        slider.gameObject.SetActive(true);
        animator.CrossFade("Z_Walk", 0.05f);
        agent.enabled = true;
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        spawning = false;

        timeBetweenGrowl += zombieGrowl.length;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dying || spawning) return;

        timer += Time.deltaTime;
        if (timer >= timeBetweenGrowl)
        {
            audioSource.PlayOneShot(zombieGrowl, growlVolume);
            timer = 0f;
        }
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
