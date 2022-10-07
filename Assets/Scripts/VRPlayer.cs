using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour
{
	public enum GRIP_STATE { OPEN, OBJECT, AIR}

	public float gripThresholdActivate;
	public float gripThresholdDeactivate;

	public float[] gripValues = new float[2] { 0, 0 };
	public GRIP_STATE[] gripStates = new GRIP_STATE[2] { GRIP_STATE.OPEN, GRIP_STATE.OPEN };
	public Vector3[] gripLocations = new Vector3[2];
	public VRHand[] hands = new VRHand[2];
	Vector3[] cameraRigGripLocation = new Vector3[2];
	public VRGrabbable[] grabbedObjects = new VRGrabbable[2] { null, null };
	
	public Transform head; //the vr camera
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        //get values for controller grips
        gripValues[0] = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        gripValues[1] = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);

        Vector3[] displacements = new Vector3[2];
        for(int i = 0; i < 2; i++)
		{
			displacements[i] = Vector3.zero; //used for grab locomotion
			//begin grip finite state machine
			if (gripStates[i] == GRIP_STATE.AIR) //gripping the air, so move player
			{

                if(gripValues[i] < gripThresholdDeactivate) //user has let go
				{
                    gripStates[i] = GRIP_STATE.OPEN;
				}
                else 
				{
					//calculate player position based on grip location displacement
                    Vector3 handInTracking = transform.worldToLocalMatrix.MultiplyPoint(hands[i].transform.position);
                    Vector3 between = handInTracking - gripLocations[i];

                    displacements[i] = - between;
				}
			} 
            else if (gripStates[i] == GRIP_STATE.OBJECT) //gripping an object
			{
                if (gripValues[i] < gripThresholdDeactivate) //user has let go
                {
                    gripStates[i] = GRIP_STATE.OPEN;
                }
                else
                {
                    //move the object in relation to controller movement
                    VRGrabbable g = grabbedObjects[i];
                    Rigidbody rb = g.GetComponent<Rigidbody>();

                    Vector3 between = hands[i].grabOffset.position - g.transform.position;
                    Vector3 direction = between.normalized;

                    rb.velocity = between / Time.deltaTime;

                    //also handle controller, object rotation
                    Quaternion betweenRot = hands[i].grabOffset.rotation * Quaternion.Inverse(g.transform.rotation);
                    Vector3 axis;
                    float angle;
                    betweenRot.ToAngleAxis(out angle, out axis);

                    rb.angularVelocity = angle * Mathf.Deg2Rad * axis / Time.deltaTime;
                }
			}
            else //user is not gripping, set either AIR or OBJECT states
			{
                if(gripValues[i] > gripThresholdActivate)
                    if(hands[i].grabbables.Count == 0) //nothing to grab in area
				    {
                        gripStates[i] = GRIP_STATE.AIR;
                        Vector3 handInTracking = transform.worldToLocalMatrix.MultiplyPoint(hands[i].transform.position);

                        gripLocations[i] = handInTracking;
                        cameraRigGripLocation[i] = this.transform.position;
					}
					else //something in area to grab
					{
                        gripStates[i] = GRIP_STATE.OBJECT;
                        grabbedObjects[i] = hands[i].grabbables[0]; //grab first object in list
                        hands[i].grabOffset.transform.position = grabbedObjects[i].transform.position;
                    }
			}
		}

        //move player based on grip states
        if (gripStates[0] == GRIP_STATE.AIR && gripStates[1] == GRIP_STATE.AIR) //both controllers
		{
            this.transform.position = (cameraRigGripLocation[0] + displacements[0] + cameraRigGripLocation[1] + displacements[1]) / 2.0f;
		}
        else if (gripStates[0] == GRIP_STATE.AIR) //left controller
		{
            this.transform.position = cameraRigGripLocation[0] + displacements[0];

		}
		else if (gripStates[1] == GRIP_STATE.AIR) //right controller
		{
            this.transform.position = cameraRigGripLocation[1] + displacements[1];
        }
    }
}
