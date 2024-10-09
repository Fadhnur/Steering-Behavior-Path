using UnityEngine;

public class AICarScript : MonoBehaviour
{
    public Vector3 centerOfMass;
    private Transform[] path;
    public Transform pathGroup;
    public float maxSteer = 15.0f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public int currentPathObj;
    public float distFromPath = 20f;
    public float maxTorque = 50f;
    public float currentSpeed;
    public float topSpeed = 150f;
    public float decelerationSpeed = 10f;
    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass;
        GetPath();
    }

    private void Update() {
        GetSteer();
        Move();
    }

    private void GetPath()
    {
        Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
        path = new Transform[path_objs.Length - 1];

        int index = 0;
        for (int i = 0; i < path_objs.Length; i++)
        {
            if (path_objs[i] != pathGroup)
            {
                path[index] = path_objs[i];
                index++;
            }
        }
    }

    private void GetSteer()
    {
        Vector3 steerVector = transform.InverseTransformPoint(path[currentPathObj].position);
        float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
        
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;

        if (steerVector.magnitude <= distFromPath)
        {
            currentPathObj++;
            if (currentPathObj >= path.Length)
                currentPathObj = 0;
        }
    }

    private void Move()
    {
        // Calculate current speed based on wheel rotation
        currentSpeed = 2 * (22f/7f) * wheelRL.radius * wheelRL.rpm * 60f / 1000f;
        currentSpeed = Mathf.Round(currentSpeed);

        if (currentSpeed <= topSpeed)
        {
            // Apply motor torque
            wheelRL.motorTorque = maxTorque;
            wheelRR.motorTorque = maxTorque;
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
        }
        else
        {
            // Apply brake torque
            wheelRL.motorTorque = 0f;
            wheelRR.motorTorque = 0f;
            wheelRL.brakeTorque = decelerationSpeed;
            wheelRR.brakeTorque = decelerationSpeed;
        }
    }
}