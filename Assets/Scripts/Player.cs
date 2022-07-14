

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 9.0f;
    private Rigidbody _playerRb;

    public Vector3 dir;

    public Animator anim;

    public GameObject model;

    private void Awake()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!GameManager.instance.gameOver)
        {
            Movement();
        }

        if (GameManager.instance.freezeTiles)
        {
            _playerRb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.instance.IncrementScore(1);

            if (dir == Vector3.forward)
            {
                dir = Vector3.left;
                model.transform.Rotate(new Vector3(0,-90,0));
            }
            else
            {
                dir = Vector3.forward;
                model.transform.Rotate(new Vector3(0, 90, 0));
            }
        }


        if (!GameManager.instance.gameOver)
        {
           transform.Translate(dir * _speed * Time.deltaTime);
            anim.SetFloat("Forward", 1);
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TopTile" || other.tag == "LeftTile" || other.tag == "StartTile")
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);
            if (!Physics.Raycast(downRay, out hit))
            {
                AudioManager.instance.Play("GameOver");
                anim.SetBool("DeathTrigger", true);
                _playerRb.velocity = new Vector3(0, _playerRb.velocity.y, 0);
                GameManager.instance.EndGame();
            }
        }
    }
}
