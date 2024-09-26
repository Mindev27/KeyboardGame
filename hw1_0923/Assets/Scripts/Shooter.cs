using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Rigidbody greenBullet;
    public Rigidbody blueBullet;
    public Rigidbody yellowBullet;
    public float power = 1500f;
    public float moveSpeed = 2f;
    public float duration = 1.0f;  // ��鸮�� �ð�
    public float magnitude = 0.1f; // ��鸲�� ����
    private float shakeTimeRemaining = 0f; // ��鸲�� ���� �ð�

    void Update()
    {
        Vector3 deltaMove = new Vector3(
            Input.GetKeyDown(KeyCode.A) ? -moveSpeed : (Input.GetKeyDown(KeyCode.D) ? moveSpeed : 0),
            Input.GetKeyDown(KeyCode.E) ? -moveSpeed : (Input.GetKeyDown(KeyCode.Q) ? moveSpeed : 0),
            Input.GetKeyDown(KeyCode.S) ? -moveSpeed : (Input.GetKeyDown(KeyCode.W) ? moveSpeed : 0)
        );

        transform.Translate(deltaMove);

        if (Input.GetButtonUp("Fire1"))
        {
            // �ʷϻ� �Ѿ� �߻�
            Rigidbody instance = Instantiate(greenBullet, transform.position, transform.rotation) as Rigidbody;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            instance.AddForce(fwd * Random.Range(0.0f, power));

            // ��鸲 ����
            shakeTimeRemaining = duration;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            // �Ķ��� �Ѿ� �߻�
            Rigidbody instance = Instantiate(blueBullet, transform.position, transform.rotation) as Rigidbody;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            instance.AddForce(fwd * Random.Range(0.0f, power));

            // ��鸲 ����
            shakeTimeRemaining = duration;
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            // ����� �Ѿ� �߻�
            Rigidbody instance = Instantiate(yellowBullet, transform.position, transform.rotation) as Rigidbody;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            instance.AddForce(fwd * Random.Range(0.0f, power));

            // ��鸲 ����
            shakeTimeRemaining = duration;
        }

        // ī�޶� ��鸲 ó��
        if (shakeTimeRemaining > 0)
        {
            // ��鸲 �ð� ����
            shakeTimeRemaining -= Time.deltaTime;

            // ī�޶� �������� �̵�
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.Translate(new Vector3(x, y, 0));
        }
    }
}
