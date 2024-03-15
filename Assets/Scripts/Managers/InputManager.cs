using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform startPointer;

    private Ray ray;
    private RaycastHit hit;
    private float cameraRayCast = 20f;

    private LayerMask ignoreMask;
    private LayerMask mask;
    private GameManager gm;
    private LevelManager lm;
    private UISound sounds;
    private bool isStretchingSound;

    private bool isPressed, isUnpressed;
    private Vector3 pointToHit;
    private Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        lm = gm.GetLevelManager;
        sounds = UISound.Instance;
        arrow.gameObject.SetActive(false);
        startPointer.gameObject.SetActive(false);
        ignoreMask = LayerMask.GetMask(new string[] { "block", "death" });
        mask = LayerMask.GetMask(new string[] { "back" });
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButton(0) && gm.IsPlaying && gm.PointerClickedCount <= 0)
        {
            ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, cameraRayCast, ~ignoreMask))
            {
                if (hit.collider != null)
                {                    
                    if (!isPressed && !isUnpressed)
                    {
                        isPressed = true;                        
                        startPoint = new Vector3(hit.point.x, 0, hit.point.z);
                        startPointer.gameObject.SetActive(true);
                        startPointer.position = startPoint;
                    }
                    else if (isPressed && !isUnpressed)
                    {
                        pointToHit = new Vector3(hit.point.x, 0, hit.point.z);
                        makeArrow(startPoint, pointToHit);
                        
                        if (!isStretchingSound)
                        {
                            isStretchingSound = true;
                            sounds.PlaySound(SoundsUI.stretching);
                        }
                        
                    }

                    
                }
            }
        }
        else if (!Input.GetMouseButton(0) && isPressed && !isUnpressed)
        {
            if ((pointToHit - startPoint).magnitude < 0.5f)
            {
                Restart();
                sounds.PlaySound(SoundsUI.error);
            }
            else
            {
                isPressed = false;
                arrow.gameObject.SetActive(false);
                isUnpressed = true;
                shoot(pointToHit, lm.GetActiveBall(), startPoint);
                sounds.PlaySound(SoundsUI.impact);
                isStretchingSound = false;
            }

            startPointer.gameObject.SetActive(false);
        }
    }

    private void makeArrow(Vector3 from, Vector3 to)
    {
        Vector3 center = Vector3.Lerp(from, to, 0.5f);
        arrow.gameObject.SetActive(true); 
        arrow.position = center + Vector3.up*0.51f;
        arrow.LookAt(new Vector3(to.x, arrow.position.y, to.z));
        arrow.localScale = new Vector3(1, 1, (from - to).magnitude);
    }

    private void shoot(Vector3 aim, Rigidbody ballRb, Vector3 startPos)
    {
        ballRb.transform.position = startPos;
        Vector3 dir = (aim - ballRb.transform.position).normalized;
        ballRb.velocity = dir * Globals.BALL_SPEED;
    }

    public void Restart()
    {
        arrow.gameObject.SetActive(false);
        isPressed = false;
        isUnpressed = false;
        isStretchingSound = false;
        startPointer.gameObject.SetActive(false);
    }
}
