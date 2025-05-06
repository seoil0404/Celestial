using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Slider slider;
    [SerializeField] private float health;

    [SerializeField] private GameObject effect1;
    [SerializeField] private GameObject effect2;

    private void Awake()
    {
        slider.value = 1;

        StartCoroutine(AttackByDelay());
    }

    private IEnumerator AttackByDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            int random = Random.Range(0, 2);
            if (random == 0) StartCoroutine(Attack1());
            else StartCoroutine(Attack2());
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator Attack1()
    {
        for(int index = 0; index < 6; index ++)
        {
            yield return new WaitForSeconds(0.5f);
            
            Vector3 pos = playerController.transform.position;

            Vector3 addPos = new Vector3(Mathf.Cos(Mathf.Deg2Rad * (playerController.transform.eulerAngles.y - 90)),0 ,Mathf.Sin(Mathf.Deg2Rad * (playerController.transform.eulerAngles.y-90)));
            addPos.Normalize();

            addPos *= 8;

            pos += addPos;

            Instantiate(effect2).transform.position = pos;
        }
    }

    private IEnumerator Attack2()
    {
        for (int index = 0; index < 6; index++)
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(effect1).transform.position = playerController.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hurt");
        health -= 10;
        slider.value = health / 100;
    }
}
