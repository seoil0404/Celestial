using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 currentVelocity = Vector3.zero;

    private bool isAllowMove = true;
    private bool isAttack1 = false;

     Coroutine attackCoroutine;

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

            if(Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody.AddForce(Vector3.up * 50, ForceMode.Impulse);
            }

            transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

            _rigidbody.linearVelocity = new Vector3(currentVelocity.x, _rigidbody.linearVelocity.y, currentVelocity.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            if(!isAttack1)
            {
                isAttack1 = true;
                animator.SetTrigger("OnAttack");
                attackCoroutine = StartCoroutine(AttackDelay());
            }
            else
            {
                animator.SetTrigger("OnAttack2");
                isAttack1 = false;
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
            }
        }
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.7f);
        isAttack1 = false;
    }

    private void MoveForward()
    {
        Vector3 velocity = _rigidbody.linearVelocity;
        
        velocity.x = Mathf.Sin(Mathf.Deg2Rad * (transform.eulerAngles.y - 90f)) * 30;
        velocity.z = Mathf.Cos(Mathf.Deg2Rad * (transform.eulerAngles.y - 90f)) * 30;

        currentVelocity = velocity;
    }
}
