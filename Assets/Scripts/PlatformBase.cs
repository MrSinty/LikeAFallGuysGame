using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    private GameObject player = null;
    protected GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
    protected Renderer platformRenderer;
    protected bool isReloading = false;
    protected bool isAttacking = false;

    private void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
    }

    protected void GetPlayerObject(Collider other)
    {
        Player = other.transform.parent.gameObject;
    }

    protected void ForgetPlayerObject()
    {
        Player = null;
    }
}
