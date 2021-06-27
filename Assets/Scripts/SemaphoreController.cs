using System.Collections.Generic;
using UnityEngine;

public class SemaphoreController : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Material[] _meshMaterials;

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    [SerializeField] private Material black;

    [SerializeField] private bool isGreen;

    [SerializeField] private Generator generator;
    [SerializeField] private GameObject lightPole;

    [SerializeField] private List<Waypoint> wayPoints;

    public int CarsOnRilevator { get; set; }
    public int TotalCarRilevated { get; set; }
    public int CarsFromThisSemaphore { get; set; }
    public float SemaphoreTimer { get; set; }

    private void Awake()
    {
        _meshRenderer = lightPole.GetComponent<MeshRenderer>();
        _meshMaterials = _meshRenderer.materials;

        generator.tag = gameObject.tag;
        generator.GettingSettingsFromMenu();

        foreach (var point in wayPoints)
            point.tag = gameObject.tag;
    }

    public void Reset()
    {
        IsGreen = false;

        CarsOnRilevator = 0;
        TotalCarRilevated = 0;
        CarsFromThisSemaphore = 0;

        generator.Reset();
    }

    private void Update()
    {
        if (IsGreen)
        {
            _meshMaterials[0] = black;
            _meshMaterials[1] = black;
            _meshMaterials[2] = green;
        }

        if (!IsGreen)
        {
            _meshMaterials[0] = black;
            _meshMaterials[1] = red;
            _meshMaterials[2] = black;
        }

        _meshRenderer.materials = _meshMaterials;
    }

    //-----PROPERTIES-----

    public bool IsGreen
    {
        get => isGreen;
        set => isGreen = value;
    }
}