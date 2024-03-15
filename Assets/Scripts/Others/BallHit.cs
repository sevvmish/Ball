using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHit : MonoBehaviour
{
    private GameManager gm;
    private LevelManager lm;

    private void Start()
    {
        gm = GameManager.Instance;
        lm = gm.GetLevelManager;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //hit block
        if (collision.gameObject.layer == 6)
        {            
            UISound.Instance.PlayAnyHit();

            if (collision.gameObject.TryGetComponent(out Block b))
            {
                b.SetHit();
                b.transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f)), 0.2f, 30).SetEase(Ease.OutQuad);
                Vector3 point = collision.contacts[0].point;

                lm.MakeExplosion(new Vector3(point.x, 0.4f, point.z));
            }

            gm.CheckWinCondition();
        }
        else if (collision.gameObject.layer == 0)
        {
            collision.gameObject.transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f)), 0.2f, 30).SetEase(Ease.OutQuad);
        }
        else if (collision.gameObject.layer == 9)
        {
            collision.gameObject.transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f)), 0.2f, 30).SetEase(Ease.OutQuad);
            
            Vector3 point = collision.contacts[0].point;
            lm.MakeFireExplosion(new Vector3(point.x, 0.4f, point.z));
            gameObject.SetActive(false);
            gm.CheckWinCondition();
            
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //death
        if (other.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
            gm.CheckWinCondition();
        }
    }
}
