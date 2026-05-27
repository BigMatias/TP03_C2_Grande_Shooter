using System;
using UnityEngine;
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Camera _camera;
    [SerializeField] private WeaponDataSO gun;
    
    private WeaponBase _currentWeapon;

    private void Start()
    {
        EquipWeapon(gun);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryShoot();
    }

    public void EquipWeapon(WeaponDataSO weaponData)
    {
        if (_currentWeapon != null)
            Destroy(_currentWeapon.gameObject);

        GameObject go = Instantiate(weaponData.weaponPrefab, _weaponHolder);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        Debug.Log($"Weapon spawned at: {go.transform.position}, parent: {go.transform.parent.name}");
        _currentWeapon = go.GetComponent<WeaponBase>();
    }

    private void TryShoot()
    {
        if (_currentWeapon == null) return;
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        _currentWeapon.TryShoot(ray);
    }
}