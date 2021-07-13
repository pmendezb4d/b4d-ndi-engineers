using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] bool outsideOnAwake = true;

    [Header("Events")]
    [SerializeField] UnityEvent onPortalEnter = default;
    [SerializeField] UnityEvent onPortalExit = default;

    [Header("References")]
    [SerializeField] Renderer portalRenderer = default;
    [SerializeField] Transform insidePortal = default;

    public bool IsInside { get; private set; }

    private Material[] materials;

    private const int defaultVisibleReference = 0;
    private int portalReference;

    private int stencilRefPropertyId;

    private BoxCollider boxCollider;

    private void Awake()
    {
        stencilRefPropertyId = Shader.PropertyToID("_StencilRef");
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        var collider = Camera.main.gameObject.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);
        var cameraCollider = Camera.main.gameObject.AddComponent<BoxCollider>();
        cameraCollider.isTrigger = true;
        cameraCollider.size = Vector3.one * 0.15f;

        portalReference = portalRenderer.sharedMaterial.GetInt(stencilRefPropertyId);
        RefreshInsidePortalMaterials();

        if (outsideOnAwake)
            ActiveOutsidePortal();
    }

    public void ActiveInsidePortal() => SetStencilReferenceInsidePortal(defaultVisibleReference);
    public void ActiveOutsidePortal() => SetStencilReferenceInsidePortal(portalReference);

    private void SetStencilReferenceInsidePortal(int reference)
    {
        IsInside = reference != portalReference;

        for (int i = 0; i < materials.Length; i++)
        {
            if(materials[i] != null)
                materials[i].SetInt(stencilRefPropertyId, reference);
        }

        if (IsInside) onPortalEnter.Invoke();
        else onPortalExit.Invoke();

        //if(IsInside)
        //{
        //    Vector3 boxCenter = boxCollider.center;
        //    boxCenter.z = boxCenter.z > 0 ? boxCenter.z : boxCenter.z * -1f;
        //    boxCollider.center = boxCenter;
        //}
        //else
        //{
        //    Vector3 boxCenter = boxCollider.center;
        //    boxCenter.z = boxCenter.z < 0 ? boxCenter.z : boxCenter.z * -1f;
        //    boxCollider.center = boxCenter;
        //}
    }

    private void RefreshInsidePortalMaterials()
    {
        Renderer[] renderers = insidePortal.GetComponentsInChildren<Renderer>();

        List<Material> materialList = new List<Material>();

        for(int i = 0; i < renderers.Length; i++)
        {
            materialList.AddRange(renderers[i].sharedMaterials);
        }

        materials = materialList.ToArray();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.CompareTag("PortalCamera"))
        {
            if (!IsInside) ActiveInsidePortal();
            else ActiveOutsidePortal();
        }
    }

}
