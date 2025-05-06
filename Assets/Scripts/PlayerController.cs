using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private GameObject slashEffect1;
    [SerializeField] private GameObject slashEffect2;

    private Vector3 currentVelocity = Vector3.zero;

    private bool isAllowMove = true;
    private bool isAttack1 = false;

    Coroutine attackCoroutine;
    Coroutine endAttackCoroutine;

    private void Update()
    {
        if (isAllowMove)
        {
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("IsRun", true);
                MoveForward();
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                animator.SetBool("IsRun", false);
                currentVelocity = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * 50, ForceMode.Impulse);
            }

            transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

            _rigidbody.linearVelocity = new Vector3(currentVelocity.x, _rigidbody.linearVelocity.y, currentVelocity.z);
        }
        else
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAllowMove = false;

            if(endAttackCoroutine != null) StopCoroutine(endAttackCoroutine);
            endAttackCoroutine = StartCoroutine(EndAttack());
            
            if(!isAttack1)
            {
                isAttack1 = true;
                animator.SetTrigger("OnAttack");
                attackCoroutine = StartCoroutine(AttackDelay());

                StartCoroutine(ProcessByDelay(0.3f, () => {
                    GameObject effect = Instantiate(slashEffect1);
                    effect.transform.position = transform.position;
                    effect.transform.rotation = transform.rotation;
                }));
            }
            else
            {
                animator.SetTrigger("OnAttack2");
                isAttack1 = false;
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);

                StartCoroutine(ProcessByDelay(0.3f, () => {
                    GameObject effect = Instantiate(slashEffect2);
                    effect.transform.position = transform.position;
                    effect.transform.rotation = transform.rotation;
                }));
            }
        }
    }

    private IEnumerator ProcessByDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.75f);
        isAttack1 = false;
    }

    private void MoveForward()
    {
        Vector3 velocity = _rigidbody.linearVelocity;
        
        velocity.x = Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.y - 90f)) * 30;
        velocity.z = Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.y - 90f)) * 30;

        currentVelocity = velocity;
    }

    private void OnEndAttack()
    {
        isAllowMove = true;
        _rigidbody.useGravity = true;
        currentVelocity = Vector3.zero;
        animator.SetBool("IsRun", false);
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.5f);
        OnEndAttack();
    }
}
