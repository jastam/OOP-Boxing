using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : MonoBehaviour, Weapon
{
    [SerializeField] private float range = 1;
    [SerializeField] private float damage = 3;
    [SerializeField] private float extendDuration = 1;
    [SerializeField] private float retractDuration = 1;

    private FistData rightFist;
    private FistData leftFist;
    private bool useRightFist = true;
    private State state = State.Rest;
    private float moveStarted;

    public virtual string Name => "Fists";

    public bool IsActive { get => gameObject.activeInHierarchy; }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        if (state != State.Rest)
        {
            return;
        }
        state = State.Extending;
        moveStarted = Time.time;
    }

    // Start is called before the first frame update
    void Start()
    {
        rightFist = new FistData(transform.Find("Right fist").gameObject, range);
        leftFist = new FistData(transform.Find("Left fist").gameObject, range);
    }

    // Update is called once per frame
    void Update()
    {
        FistData fistData = useRightFist ? rightFist : leftFist;

        if (state == State.Retracting)
        {
            float fracComplete = GetAnimationFractionComplete(retractDuration);
            fistData.Fist.transform.localPosition = Vector3.Slerp(fistData.ExtendedPos, fistData.RestingPos, fracComplete);
            if (fracComplete == 1)
            {
                state = State.Rest;
                useRightFist = !useRightFist;
            }
        }

        if (state == State.Extending)
        {
            float fracComplete = GetAnimationFractionComplete(extendDuration);
            fistData.Fist.transform.localPosition = Vector3.Slerp(fistData.RestingPos, fistData.ExtendedPos, fracComplete);
            if (fracComplete == 1)
            {
                state = State.Retracting;
                moveStarted = Time.time;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != State.Rest)
        {
            Debug.Log("Hit!"); 
        }
    }

    private float GetAnimationFractionComplete(float duration)
    {
        float fracComplete = (Time.time - moveStarted) / duration;
        if (fracComplete > 1)
        {
            fracComplete = 1;
        }

        return fracComplete;
    }

    private enum State { Rest, Extending, Retracting }

    private struct FistData
    {
        public FistData(GameObject fist, float range)
        {
            Fist = fist;
            Collider = fist.GetComponent<Collider>();
            RestingPos = fist.transform.localPosition;
            ExtendedPos = new Vector3(RestingPos.x, RestingPos.y, RestingPos.z + range);
        }

        public GameObject Fist;
        public Collider Collider;
        public Vector3 RestingPos;
        public Vector3 ExtendedPos;
    }
}
