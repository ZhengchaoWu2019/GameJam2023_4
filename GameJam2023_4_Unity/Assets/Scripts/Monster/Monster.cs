using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    enum MonsterState { Patrol, Chase, Attack, Wait, Back, Attract };

    [Header("Components")]
    [SerializeField] MonsterSearchArea searchArea;
    [SerializeField] Animator animator;
    [SerializeField] Transform bound1Tf;
    [SerializeField] Transform bound2Tf;
    [SerializeField] float moveDuration;
    [SerializeField] float attackRadius;
    [SerializeField] float speed;
    [SerializeField] float waitDuration;
    [SerializeField] MonsterAttackArea attackArea;
    [SerializeField] MonsterAttractArea attractArea;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] float attractSpeed;
    [SerializeField] Transform groundCheckTf;
    [SerializeField] float groundCheckRadius;

    [Header("Read Only")]
    [SerializeField] MonsterState monsterState;
    [SerializeField] Transform currentTargetTf;
    [SerializeField] float t;
    [SerializeField] Vector2 fromPos;
    [SerializeField] Vector2 toPos;
    [SerializeField] bool attacking;
    [SerializeField] float? startWaitTime;
    [SerializeField] Vector2? attractPos;
    [SerializeField] Vector2 faceDir;
    [SerializeField] Vector3 originalScale;

    #region Life Cycle
    private void Awake()
    {
        searchArea.OnTargetChanged += ChangeTarget;
        attractArea.OnAttractWoodFiring += ToAttract;
    }

    private void Start()
    {
        monsterState = MonsterState.Patrol;
        SetRandomFromToPosition();
        t = InverseLerp(fromPos, toPos, transform.position);
        faceDir = (toPos - (Vector2)transform.position).normalized;
        attractPos = null;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        switch (monsterState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Wait:
                Wait();
                break;
            case MonsterState.Back:
                Back();
                break;
            case MonsterState.Attract:
                Attract();
                break;
        }

        if(faceDir.x > 0)
        {
            transform.localScale = originalScale;
        }
        else if(faceDir.x < 0)
        {
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        }
    }
    #endregion
    void Patrol()
    {
        if (t <= 1)
        {
            transform.position = Vector2.Lerp(fromPos, toPos, t);

            t += Time.deltaTime / moveDuration;
        }
        else
        {
            Vector2 tempPos = fromPos;
            fromPos = toPos;
            toPos = tempPos;
            faceDir = (toPos - (Vector2)transform.position).normalized;
            t = 0;
        }
    }
    void Chase()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach(var collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Player"))
            {
                monsterState = MonsterState.Attack;

                return;
            }
        }

        float dir = (currentTargetTf.position - transform.position).x;
        float delta = Mathf.Sign(dir) * speed * Time.deltaTime;
        transform.position += new Vector3(delta, 0, 0);

        faceDir = (currentTargetTf.position - transform.position).normalized;
    }

    void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            animator.SetTrigger("toAttack");
        }
    }

    void Wait()
    {
        if(startWaitTime == null)
        {
            startWaitTime = Time.time;
        }
        else
        {
            if(Time.time > startWaitTime + waitDuration)
            {
                monsterState = MonsterState.Back;
                SetRandomFromToPosition();
                t = 0;
                toPos = fromPos;
                fromPos = transform.position;

                startWaitTime = null;

                faceDir = (toPos - (Vector2)transform.position).normalized;
            }
        }
    }

    void Back()
    {
        if (t <= 1)
        {
            transform.position = Vector2.Lerp(fromPos, toPos, t);

            t += Time.deltaTime / moveDuration;
        }
        else
        {
            transform.position = toPos;
            t = 0;
            toPos = bound1Tf.position.Equals(toPos) ? bound2Tf.position : bound1Tf.position;
            fromPos = transform.position;
            monsterState = MonsterState.Patrol;

            faceDir = (toPos - (Vector2)transform.position).normalized;
        }
    }

    void Attract()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(groundCheckTf.position, groundCheckRadius);
        foreach(var collider in collider2Ds)
        {
            if (collider.CompareTag("Ground"))
            {
                return;
            }
            if (collider.CompareTag("Wood"))
            {
                Destroy(transform.parent.gameObject);
            }
        }

        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
    }

    void SetRandomFromToPosition()
    {
        bool is1 = Random.Range(0, 2) < 0.5f;
        fromPos = is1 ? bound1Tf.position : bound2Tf.position;
        toPos = is1 ? bound2Tf.position : bound1Tf.position;
    }

    float InverseLerp(Vector2 aVec, Vector2 bVec, Vector2 currentVec)
    {
        return Mathf.InverseLerp(aVec.x, bVec.x, currentVec.x);
    }

    void ChangeTarget(Transform newTargetTf)
    {
        if(attractPos != null)
        {
            return;
        }

        if (newTargetTf != null)
        {
            startWaitTime = null;
            currentTargetTf = newTargetTf;
            monsterState = MonsterState.Chase;
        }
        else
        {
            currentTargetTf = null;
            monsterState = MonsterState.Wait;
        }
    }

    void ToAttract(Vector2 newAttractPos)
    {
        attractPos = newAttractPos;
        rb2D.isKinematic = false;
        GetComponent<Collider2D>().isTrigger = false;
        Vector2 delta = attractPos.Value - (Vector2)transform.position;
        Vector2 dir = delta.x > 0 ? Vector2.right : -Vector2.right;
        rb2D.velocity = dir * attractSpeed;

        monsterState = MonsterState.Attract;

        faceDir = (attractPos.Value - (Vector2)transform.position).normalized;
    }

    void Animation_OnDamageStart()
    {
        attackArea.gameObject.SetActive(false);
    }

    void Animation_OnDamageEnd()
    {
        attackArea.gameObject.SetActive(true);
    }

    void Animation_OnAttackEnd()
    {
        attacking = false;
        monsterState = MonsterState.Wait;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        if (groundCheckTf != null)
        {
            Gizmos.DrawWireSphere(groundCheckTf.position, groundCheckRadius);
        }
    }
}
