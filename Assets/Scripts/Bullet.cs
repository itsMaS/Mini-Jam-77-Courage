using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(GameManager.Instance.config.player.baseBulletSpeed * Time.deltaTime, 0, 0);
    }
}
