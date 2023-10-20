using System.Collections;
using UnityEngine;

public class AttackPlatformLogic : PlatformBase
{
    [SerializeField] private float waitBeforeAttack = 1f;
    [SerializeField] private float waitAttacking = 0.5f;
    [SerializeField] private float waitReloading = 5f;
    [SerializeField] private float waitIdle = 0.5f;

    private readonly Color32 idleColor = new Color32(74, 102, 153, 255);
    private readonly Color32 prepareColor = new Color32(198, 96, 34, 255);
    private readonly Color32 attackColor = new Color32(186, 39, 36, 255);
    private readonly Color32 reloadColor = new Color32(122, 122, 122, 255);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetPlayerObject(other);
            if (!isReloading && !isAttacking)
                StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ForgetPlayerObject();
        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        platformRenderer.material.color = prepareColor;
        yield return new WaitForSeconds(waitBeforeAttack);

        platformRenderer.material.color = attackColor;
        DealingDamage();
        yield return new WaitForSeconds(waitAttacking);

        isAttacking = false;
        platformRenderer.material.color = reloadColor;
        isReloading = true;
        yield return new WaitForSeconds(waitReloading);
        isReloading = false;

        platformRenderer.material.color = idleColor;
        yield return new WaitForSeconds(waitIdle);
        if (Player != null)
        {
            StartCoroutine(AttackPlayer());
        }
    }
    void DealingDamage()
    {
        if (Player != null)
        {
            Player.GetComponent<PlayerHP>().PlayerHealth -= 10;
        }
    }
}
