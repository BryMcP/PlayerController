using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : System.Object { }

public class PlayerController : MonoBehaviour
{

    [SerializeField] AudioClip rifleSound;
    
    [SerializeField] GameObject bulletPreFab;
    [SerializeField] Transform firePoint;
    public float money { get; set; } = 0;
    [SerializeField] int health = 100;

    List<Weapon> WeaponList = new List<Weapon>();
    Weapon currentWeapon;
    public int ammo
    {
        get { return currentWeapon.ammo; }
        set { currentWeapon.ammo = value; }
    }
    PlayerAttack currentAttack;
    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = new Rifle(GetComponent<AudioSource>(), rifleSound, 30, bulletPreFab, firePoint);
        WeaponList.Add(currentWeapon);
        var nextWeapon = new Shotgun(GetComponent<AudioSource>(), rifleSound, 8,
            bulletPreFab, firePoint);
        WeaponList.Add(nextWeapon);
        currentAttack = currentWeapon.Attack;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0, 0.5f);
            this.enabled = false;
            Application.Quit(0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("slimeFire"))
        {
            TakeDamage(20);
            Destroy(collision.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
            gameObject.transform.Rotate(0, 0, 4);
        if (Input.GetKey(KeyCode.D))
            gameObject.transform.Rotate(0, 0, -4);
        if (Input.GetKey(KeyCode.W))
        {
            Rigidbody2D b = GetComponent<Rigidbody2D>();
            float angle = gameObject.transform.rotation.eulerAngles.z;
            angle *= Mathf.Deg2Rad; // we need radians for sine and cosine
            float speed = 5;
            b.velocity = speed *
                new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        if (Input.GetKeyUp(KeyCode.W))
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if(Input.GetKeyDown(KeyCode.Space) && ammo >0)
        {
            currentAttack();
            //GetComponent<AudioSource>().clip = rifleSound;
            //GetComponent<AudioSource>().Play();
            //ammo--;

            //var bullet = Instantiate(bulletPreFab);
            //bullet.transform.parent = this.transform.parent;
            //bullet.transform.position = firePoint.transform.position;
            //bullet.transform.rotation = this.transform.rotation;
            //bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 40, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            currentWeapon = WeaponList[0];
            currentAttack = currentWeapon.Attack;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            currentWeapon = WeaponList[1];
            currentAttack = currentWeapon.Attack;
        }
    }
}
