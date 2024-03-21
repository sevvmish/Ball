using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool IsHit { get; private set; }
    [SerializeField] private Material before;
    [SerializeField] private Material after;
    [SerializeField] private bool isGreen = false;

    [SerializeField] private MeshRenderer[] mr;

    private Vector3 position;
    private Vector3 rotation;
    private Vector3 scale;
    private BoxCollider _collider;

    private void Start()
    {       
        position = transform.position;
        rotation = transform.eulerAngles;
        scale = transform.localScale;
        _collider = GetComponent<BoxCollider>();
        Restart();
    }

    public void SetHit()
    {
        if (IsHit) return;
        IsHit = true;

        if (!isGreen)
        {
            for (int i = 0; i < mr.Length; i++)
            {
                mr[i].material = after;
            }
        }
        else
        {
            transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InOutQuad);
            _collider.enabled = false;
        }
        
    }

    public void Restart()
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            //rb.velocity = Vector3.zero;
            RigidbodyConstraints current = rb.constraints;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints = current;
        }

        transform.DOKill();
        transform.position = position;
        transform.eulerAngles = rotation;

        IsHit = false;

        for (int i = 0; i < mr.Length; i++)
        {
            mr[i].material = before;
        }
                
        transform.localScale = Vector3.zero;
        
        transform.DOScale(scale, 0.15f).SetEase(Ease.InOutQuad).OnComplete(() => { transform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(-0.2f, 0.2f), 0, UnityEngine.Random.Range(-0.2f, 0.2f)), 0.1f, 30).SetEase(Ease.OutQuad);});
        _collider.enabled = true;
    }
}
