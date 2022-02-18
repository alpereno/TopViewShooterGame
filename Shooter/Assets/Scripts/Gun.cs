using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform chamber;             //shell ejection point
    [SerializeField] private Transform shell;

    [Header("Gun Properties")]
    [SerializeField] private float msBetweenShots = 100;
    [SerializeField] private float muzzleVelocity = 35;
    [SerializeField] int bulletsPerMagazine = 7;
    [SerializeField] private float reloadTime = .8f;

    float nextShotTime;
    int bulletsRemainingInMagazine;
    bool reloading;

    private void Start()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
    }

    private void LateUpdate()
    {
        //animate recoil in late update cause aim method keeps gun in the same rotation each update
        //...

        if (!reloading && bulletsRemainingInMagazine == 0)
        {
            reload();
        }
    }

    public void shoot() {
        if (!reloading && bulletsRemainingInMagazine  > 0 && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.setBulletSpeed(muzzleVelocity);
            bulletsRemainingInMagazine--;

            Instantiate(shell, chamber.position, chamber.rotation);
            //transform.Rotate(25, 0, 0);
        }
    }

    public void aim(Vector3 aimPoint) {
        if (!reloading)
        {
            transform.LookAt(aimPoint);
        }
    }

    public void reload() {
        if (!reloading && bulletsRemainingInMagazine != bulletsPerMagazine)
        {
            StartCoroutine(animateReload());
        }
    }

    IEnumerator animateReload(){
        reloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRotation = transform.localEulerAngles;
        float maxReloadAngle = 45;

        //rotating up and rotating down again 0----> 1 and 1----->0 same thing in the Enemy attacks
        while (percent <= 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = 4 * (-percent * percent + percent);
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;

            yield return null;
        }

        bulletsRemainingInMagazine = bulletsPerMagazine;
        reloading = false;
    }
}
