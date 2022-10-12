using UnityEngine;
using Unity.Netcode;
using Zenject;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector2 _zMaxBorders;
    [SerializeField] private Vector2 _zMinBorders;
    [SerializeField] private float _sensitivity;

    [Inject] private MenuManager _menuManager;
    [Inject] private Team _team;

    private Vector2 _lastDragPosition;
    private Camera _camera;
    private bool _isInverted;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _camera = GetComponent<Camera>();

        if (_team == Team.Second)
        {
            _isInverted = true;
            transform.localPosition += new Vector3(0, 0, _zMaxBorders.y);
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }

    private void Update()
    {
        if (_menuManager.IsInMenu)
        {
            _lastDragPosition = default;
            return;
        }

        Ray ray = new Ray(Vector3.zero, Vector3.zero);

        if (Input.GetMouseButtonDown(0))
            ray = _camera.ScreenPointToRay(Input.mousePosition);
        else if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Began)
            ray = _camera.ScreenPointToRay(Input.touches[0].position);

        if (ray.direction != Vector3.zero)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out TowerCell towerCell))
                    towerCell.OnClick();
                if (hit.collider.TryGetComponent(out BaseTower baseTower))
                    baseTower.OnClick();
            }
        }

        Vector2 delta = Vector2.zero;

        if (Input.GetMouseButton(1))
        {
            if (_lastDragPosition != default)
                delta = _lastDragPosition - (Vector2)Input.mousePosition;

            _lastDragPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
            _lastDragPosition = default;

        if (Input.touches.Length == 1 && Input.touches[0].phase == TouchPhase.Moved)
        {
            delta = Input.touches[0].deltaPosition;
            delta.y *= -1;
        }

        delta *= _sensitivity * Time.deltaTime;
        transform.localPosition += delta.x * Vector3.right + delta.y * Vector3.forward * (_isInverted ? -1 : 1);
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, 0, 0), transform.localPosition.y,
            Mathf.Clamp(transform.localPosition.z, _team == Team.First ? _zMinBorders.x : _zMinBorders.y, _team == Team.First ? _zMaxBorders.x : _zMaxBorders.y));
    }
}
