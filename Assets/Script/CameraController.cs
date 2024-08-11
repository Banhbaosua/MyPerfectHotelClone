using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Vector3 baseCameraRotate;
    IEnumerator rotateInst;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        baseCameraRotate = new Vector3(
            this.transform.eulerAngles.x,
            this.transform.eulerAngles.y,
            this.transform.eulerAngles.z );
        Debug.Log(baseCameraRotate);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.position;
    }

    public void RotateCamera(Vector3 rotation)
    {
        if (rotateInst != null)
            StopCoroutine(rotateInst);

        rotateInst = RotateBehaviour(rotation);
        StartCoroutine( rotateInst );
    }

    public void ResetCamera()
    {
        if(rotateInst != null)
            StopCoroutine(rotateInst);

        rotateInst = RotateBehaviour(baseCameraRotate);
        StartCoroutine(rotateInst);
        
    }
    IEnumerator RotateBehaviour(Vector3 rotation)
    {
        while (Mathf.Abs(Mathf.DeltaAngle(this.transform.rotation.eulerAngles.x, rotation.x)) > 0.001f * Mathf.Rad2Deg
            || Mathf.Abs(Mathf.DeltaAngle(this.transform.rotation.eulerAngles.y,rotation.y))>0.001f * Mathf.Rad2Deg
            || Mathf.Abs(Mathf.DeltaAngle(this.transform.rotation.eulerAngles.z, rotation.z)) > 0.001f * Mathf.Rad2Deg)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(rotation), 4f*Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }


}
