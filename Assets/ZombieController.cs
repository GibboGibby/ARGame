using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float fallingBlendTime = 0.2f;
    bool dying = false;
    void Start()
    {
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

        Vector3 moveDirection = GameManager.Instance.PlayerTransform.position - transform.position;
        moveDirection.Normalize();
        moveDirection.y = 0;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        Vector3 posToLookAt = GameManager.Instance.PlayerTransform.position;
        posToLookAt.y = transform.position.y;
        transform.LookAt(posToLookAt);
    }

    public void EnemyDied()
    {
        StartCoroutine(EnemyDyingCoroutine());
    }

    IEnumerator EnemyDyingCoroutine()
    {
        dying = true;
        //animator.SetBool("Walking", false);
        //animator.SetBool("TakingDamage", true);

        //animator.Play("Z_FallingForward");
        animator.CrossFade("Z_FallingForward", 0.2f);
        GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
