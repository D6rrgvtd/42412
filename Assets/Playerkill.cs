using UnityEngine;

public class Playerkill : MonoBehaviour
{
    bool isDead = false;
    [SerializeField]
    float rotateSpeed = 999f;
    [SerializeField]
    float lifetime = 1.5f;
    float timer = 0f;

    void Update()
    {
        if (!isDead && transform.position.y < -10f)
        {
            isDead = true;
        }

        if (isDead)
        {
            transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            if (timer > lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}
