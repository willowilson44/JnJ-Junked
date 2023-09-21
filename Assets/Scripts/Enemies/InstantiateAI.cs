using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit info))
            {
                Debug.Log("Hit at world position " + info.point + " on object " + info.collider.gameObject.name);
                InstantiateEnemy(info.point);
            }
        }
    }

    void InstantiateEnemy(Vector3 position)
    {
        //Vector3 position = new Vector3(1, 1, 1);
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        GameObject enemy = Resources.Load<GameObject>("EnemyGuard");
        GameObject newObj = Instantiate(enemy, position, rotation);
        GuardBehaviour guard = newObj.GetComponent<GuardBehaviour>();
        guard.player = this.gameObject;
    }
}
