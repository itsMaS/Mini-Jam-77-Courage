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
    private float speed => config.baseSpeed;
    private Rigidbody rb;
    private Quaternion lookRotation;
    private Quaternion neckRotation;
    private Tile currentTile;

    [SerializeField] LayerMask miningMask;
    [SerializeField] Transform drillPositionTr;
    [SerializeField] MeshRenderer tracksMr;
    [SerializeField] Animator robotBodyAn;
    [SerializeField] AudioSource source;
    [SerializeField] AnimationCurve pitchOverSpeed;
    [SerializeField] Transform NeckTr;


    private float traveledDistance;

    private GameConfig.PlayerConfig config { get => GameManager.Instance.config.player; }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(rb.transform.forward * config.baseDash, ForceMode.Impulse);
            onDash.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            MapController.Instance.Pulse(currentTile);
        }

        float movementSpeed = rb.velocity.magnitude;
        source.pitch = pitchOverSpeed.Evaluate(movementSpeed);

        Vector3 targetPos = CameraController.Instance.worldPoint;
        targetPos.y = NeckTr.position.y;
        Quaternion target = SmoothDamp(NeckTr.rotation, Quaternion.LookRotation(targetPos - NeckTr.position, Vector3.up), ref neckRotation, 0.01f);

        NeckTr.rotation = target;

        traveledDistance += movementSpeed;
        tracksMr.material.SetFloat("_distance", traveledDistance);
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
            Quaternion target = SmoothDamp(transform.rotation, Quaternion.LookRotation(moveDirection), ref lookRotation, config.baseRotationSpeed);

            rb.MoveRotation(target);
        }
        
        rb.AddForce(speed * moveDirection.magnitude * transform.forward);


        RaycastHit hit;
        if (Physics.Raycast(drillPositionTr.position, drillPositionTr.TransformDirection(Vector3.forward), out hit, config.baseDrillRange, miningMask))
        {
            Debug.DrawRay(drillPositionTr.position, drillPositionTr.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            robotBodyAn.SetBool("Drilling", true);
        }
        else
        {
            Debug.DrawRay(drillPositionTr.position, drillPositionTr.TransformDirection(Vector3.forward) * config.baseDrillRange, Color.white);
            robotBodyAn.SetBool("Drilling", false);
        }
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
