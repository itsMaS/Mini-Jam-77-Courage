using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public UnityEvent onDash;

    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseDash;
    [SerializeField] private float baseRotationSpeed;
    private float speed => baseSpeed;

    private Rigidbody rb;

    Quaternion lookRotation;

    [SerializeField] Color lit;
    [SerializeField] Color unlit;

    Tile currentTile;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();

#if !UNITY_EDITOR
        baseSpeed = ConfigManager.appConfig.GetFloat("base_speed");
        baseDash = ConfigManager.appConfig.GetFloat("base_dash");
#endif
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(rb.transform.forward * baseDash, ForceMode.Impulse);
            onDash.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            MapController.Instance.Pulse(currentTile);
        }
    }

    private void FixedUpdate()
    {
        Vector3 camForward = Vector3.ProjectOnPlane(CameraController.Instance.cam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(CameraController.Instance.cam.transform.right, Vector3.up).normalized;

        Vector3 moveDirection = camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal");
        Debug.DrawRay(transform.position, moveDirection);

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        if(moveDirection.magnitude > 0.5f)
        {
            Quaternion target = SmoothDamp(transform.rotation, Quaternion.LookRotation(moveDirection), ref lookRotation, baseRotationSpeed);

            rb.MoveRotation(target);
        }
        
        //ALTERNATIVE MOVEMENT
        rb.AddForce(speed * moveDirection.magnitude * transform.forward);
        
        //rb.AddForce(speed * moveDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Tile>(out Tile tile))
        {
            currentTile = tile;
        }
    }
    private void OnTriggerExit(Collider other)
    {
    }
    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        if (Time.deltaTime < Mathf.Epsilon) return rot;
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;

        // ensure deriv is tangent
        var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
        deriv.x -= derivError.x;
        deriv.y -= derivError.y;
        deriv.z -= derivError.z;
        deriv.w -= derivError.w;

        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }
}
