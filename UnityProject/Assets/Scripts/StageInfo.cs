using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum StageState
{
    Stage1, //MoveXY and Drop
    Stage2, //Move,Hold and Swipe
    Stage3, //MoveXZ based on physics 
    Stage4, //Tap
}

public class StageInfo : MonoBehaviour
{
    //Choose the touch state of this stage
    public StageState state;

    [Space(20)]
    [Header("Info About Player")]
    public GameObject shape;
    public float speed = 2.5f;
    public float holdSpeed = 0.5f;
    public bool isPhysicsBased = false;

    [Space(5)]
    [Header("Player Particle Effects")]
    public GameObject[] particles;

    [Space(5)]
    [Header("Interactable Props")]
    public PropsController[] props;

    [Space(10)]
    public Transform camerapoint;
    public Transform spawnPoint;

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    private int counter = 1;

    void Update()
    {
        if (Input.touchCount > 0 && !isPhysicsBased)
        {
            theTouch = Input.GetTouch(0);
            ActivateTouchEvent(state);
        }
    }
    private void FixedUpdate()
    {
            if (Input.touchCount > 0 && isPhysicsBased)
            {
                theTouch = Input.GetTouch(0);
                ActivateTouchEvent(state);
            }
    }

    private void ActivateTouchEvent(StageState state)
    {
        switch (state)
        {
            case StageState.Stage1:
                //Drag and Drop
                if (theTouch.phase == TouchPhase.Began)
                {
                    Ray raycast = Camera.main.ScreenPointToRay(theTouch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(raycast, out hit) && hit.collider.CompareTag("Food"))
                    {
                        shape = hit.collider.gameObject;
                    }
                }
                if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Stationary)
                {
                    if (shape != null)
                    {
                        shape.GetComponent<Rigidbody>().useGravity = false;
                        shape.transform.localPosition = new Vector3(
                            shape.transform.localPosition.x + theTouch.deltaPosition.x * speed,
                            shape.transform.localPosition.y + theTouch.deltaPosition.y * speed,
                            shape.transform.localPosition.z);
                    }
                }
                if(theTouch.phase == TouchPhase.Ended)
                {
                    if(shape != null)
                    {
                        shape.GetComponent<Rigidbody>().useGravity = true;
                        shape = null;
                    }
                }

                break;
            case StageState.Stage2:
                //Move ball, Hold to lift and swip to throw
                if (theTouch.phase == TouchPhase.Moved)
                {
                    //swipe by adding a pulse
                    Vector3 ballDirection = Vector3.forward * theTouch.deltaPosition.y + Vector3.right * theTouch.deltaPosition.x;
                    ballDirection.Normalize();
                    shape.GetComponent<Rigidbody>().AddForce(ballDirection * speed, ForceMode.Impulse);
                }
                if (theTouch.phase == TouchPhase.Stationary)
                {
                    if (shape.transform.position.y <= 2)
                    {
                        shape.GetComponent<Rigidbody>().AddForce(Vector3.up * holdSpeed, ForceMode.Impulse);
                    }
                }
                break;
            case StageState.Stage3:
                //Move Left/Right , Forward/Backward
                Vector3 direction = Vector3.forward * theTouch.deltaPosition.y + Vector3.right * theTouch.deltaPosition.x;
                direction.Normalize();
                shape.GetComponent<Rigidbody>().AddForce(direction * speed);
                break;
            case StageState.Stage4:
                //Tap
                if (theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                }
                else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = theTouch.position;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        Ray raycast = Camera.main.ScreenPointToRay(theTouch.position);
                        RaycastHit hit;
                        if (Physics.Raycast(raycast, out hit))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                //Debug.Log("Ouch!");
                                counter++;
                                GameObject vfx = Instantiate(particles[counter >= 30 ? 1 :0], shape.transform.position, Quaternion.identity, shape.transform);
                                GameManager.instance.ShowScore(transform);
                                if(counter < 30)
                                {
                                    shape.transform.DOPunchRotation(new Vector3(shape.transform.position.x, shape.transform.position.y + 0.25f, shape.transform.position.z), 1);
                                }
                                else
                                {
                                    shape.transform.DOScale(1.2f, .2f).OnComplete(() =>
                                    {
                                        shape.transform.DOScale(1, .2f);
                                    });
                                }
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
}
