using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    #region MonoBehaviour

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    #endregion


    /// <summary>
    /// Shot a bullet.
    /// </summary>
    public void Shot(Vector3 direction, float power)
    {
        GetComponent<Rigidbody>().AddForce(direction.normalized * power, ForceMode.Impulse);
    }
}
