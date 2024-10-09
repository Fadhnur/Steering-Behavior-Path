// using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    public List<Transform> path = new List<Transform>();
    public Color rayColor = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        // Ambil semua objek anak
        Transform[] path_objs = GetComponentsInChildren<Transform>();
        path.Clear();

        // Simpan setiap transform (selain transform ini sendiri)
        foreach (Transform path_obj in path_objs)
        {
            if (path_obj != transform)
            {
                path.Add(path_obj);
            }
        }

        // Gambarkan jalur dengan garis dan lingkaran
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = path[i].position;
            if (i > 0)
            {
                Vector3 prev = path[i - 1].position;
                Gizmos.DrawLine(prev, pos);
            }
            Gizmos.DrawWireSphere(pos, 0.3f);
        }
    }
}
