using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwapper : MonoBehaviour
{
    private FirstPersonEngine fpe;
    private string CurrentLayer;

    void Start()
    {
        fpe = GetComponent<FirstPersonEngine>();
    }

    public void CheckSurface()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            if (hit.transform.GetComponent<SurfaceType>() != null)
            {
                FootstepCollection collection = hit.transform.GetComponent<SurfaceType>().footstepCollection;
                fpe.SwapFootsteps(collection);
            }
        }
    }
}
