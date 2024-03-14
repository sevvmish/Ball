using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{    
    public GameObject[] Levels;
    public GameObject[] Balls;

    [SerializeField] private GameObject explosionExample;
    private ObjectPool explosionPool;

    private int currentLevelNumber;
    private GameObject currentLevelObject;
    private List<Block> activeBlocks = new List<Block>();

    private void Awake()
    {
        HideAll();
        explosionExample.SetActive(false);
        explosionPool = new ObjectPool(5, explosionExample, transform);
    }

    public void SetData(int level)
    {
        currentLevelNumber = level;
        Levels[currentLevelNumber].SetActive(true);
        currentLevelObject = Levels[currentLevelNumber];

        for (int i = 0; i < currentLevelObject.transform.childCount; i++)
        {
            if (currentLevelObject.transform.GetChild(i).TryGetComponent(out Block b))
            {
                activeBlocks.Add(b);
                
            }
        }
    }

    public void MakeExplosion(Vector3 pos)
    {
        GameObject g = explosionPool.GetObject();
        g.transform.position = pos;
        g.SetActive(true);
        StartCoroutine(playExplosion(g));
    }
    private IEnumerator playExplosion(GameObject g)
    {
        yield return new WaitForSeconds(1);
        explosionPool.ReturnObject(g);
    }

    public void HideAll()
    {
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].SetActive(false);
        }

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].SetActive(false);
            Balls[i].transform.position = new Vector3(0, 100, 0);
        }
    }

    public Rigidbody GetActiveBall()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            if (!Balls[i].activeSelf)
            {
                Balls[i].SetActive(true);
                return Balls[i].GetComponent<Rigidbody>();
            }
                
        }

        return Balls[0].GetComponent<Rigidbody>();
    }

    public bool IsAnyActiveBall()
    {
        for (int i = 0; i < Balls.Length; i++)
        {
            if (Balls[i].activeSelf) return true;            
        }

        return false;
    }

    public void Restart()
    {
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            activeBlocks[i].Restart();
        }

        for (int i = 0; i < Balls.Length; i++)
        {
            Balls[i].SetActive(false);
            Balls[i].transform.position = new Vector3(0, 100, 0);
        }
    }

    public bool IsAllBlocksHit()
    {
        for (int i = 0; i < activeBlocks.Count; i++)
        {
            if (!activeBlocks[i].IsHit) return false;
        }

        return true;
    }
}
