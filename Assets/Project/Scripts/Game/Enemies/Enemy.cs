using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class Enemy : MonoBehaviour {
	public int health = 1;
    public bool hidden = false;

	private bool IsGrounded = true;
	private Vector3 initialPosition;

    public virtual string Type { get; set; }

	public static event Action<string, int> InformPlayer;

	public UnityEvent<bool> ActivateDisabledEnemy;

    private void OnEnable()
    {
        initialPosition = transform.position;
    }

    public virtual void Hit(string type, string origin, int multiplier = 1)
    {
		if (multiplier > 1) 
		{ 
			health -= 1 * multiplier;
		} 
		else
		{
			health--;
		}
		if (health <= 0) {
			//EffectManager.Instance.ApplyEffect (transform.position, EffectManager.Instance.killEffectPrefab);
			//Debug.Log("destroy enemy type " + type);

			Destroy (gameObject);
			ActivateDisabledEnemy.Invoke(true);

            if (type == "simple")
			{
				string enemyType = "simple";
				int experienceGain = origin == "arrow" ? 2 : 1;

                InformPlayer?.Invoke(enemyType, experienceGain);
            }

        } else {
			//EffectManager.Instance.ApplyEffect (transform.position, EffectManager.Instance.hitEffectPrefab);
		}
	}



	public void OnTriggerEnter (Collider otherCollider) {
		//Debug.Log("OnTriggerEnter enemy " + otherCollider.name);
		//Debug.Log("OnTriggerEnter enemy tag" + otherCollider.tag);
		//if (otherCollider.GetComponent<Arrow> () != null) {
			//Hit ();
			//Destroy (otherCollider.gameObject);
		//}
	}

    private void Update()
    {
        if (transform.position.y < -10)
        {
            transform.position = new Vector3(4, 6, 9);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
  //      Debug.Log("OnTriggerEnter enemy " + collision.gameObject.name);
		//bool isEnemy = collision.gameObject.GetComponent<Enemy>() != null;

  //      if (isEnemy || collision.gameObject.name == "Floor")
		//{
		//	IsGrounded = true;
		//} else
		//{
		//	IsGrounded = false;
  //      }

        if (collision.gameObject.tag == "Arrow")
		{
            Hit(this.Type, "arrow");
			Destroy(collision.gameObject);
		}

    }

    public void OnTriggerStay (Collider otherCollider) {
		if (otherCollider.GetComponent<Sword> () != null) {
			if (otherCollider.GetComponent<Sword> ().JustAttacked) {
				//Hit(this.Type);
			}
		}
	}
}
