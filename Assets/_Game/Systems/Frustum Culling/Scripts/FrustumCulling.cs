using UnityEngine;
using System.Collections.Generic;
using FrustumCullingSpace;

[DisallowMultipleComponent]
public class FrustumCulling : MonoBehaviour
{
    public bool autoCatchCamera = true;                                    
    public Camera mainCam;
    [Range(0, 1)]
    public float activationDirection = 0.7f;    
    [Range(0, 10)]                 
    public int runEveryFrames = 5;
    public bool cullInScene = true;        
    
    public bool disableRootObject;                                 
    
    public bool distanceCulling;                                  
    public float distanceToCull = 0f;                                     
    public bool prioritizeDistanceCulling;
    public bool distanceCullingOnly;                                       


    public static FrustumCulling instance;
    List<FrustumCullingObject> objectsList = new List<FrustumCullingObject>();
    
    int frames = 0;


    void Awake()
    {
        if (autoCatchCamera) mainCam = Camera.main;

        if (!FrustumCulling.instance) {
            instance = this;
        }
    }

    void Update()
    {   
        // run every 5 frames (for performance)
        if (frames < runEveryFrames) {
            frames++;
            return;
        }

        frames = 0;
        

        // catch camera during runtime if auto catch is set
        if (autoCatchCamera && mainCam == null) {
            mainCam = Camera.main;
            
            if (mainCam == null) {
                print("No camera found.");
                return;
            }

            print("Caught camera during runtime.");
        }


        // if no auto catch and no set camera
        if (mainCam == null) {
            print("No camera set or found.");
            return;
        }


        // if distance culling only set
        if (distanceCulling && distanceCullingOnly) {
            DistanceCulling();
            return;
        }


        // Frustum and distance culling
        CameraCulling();
    }
    
    #region MAIN METHODS
    
    // cull the objects when they're out of view
    void CameraCulling()
    {
	    for (int i = 0; i < objectsList.Count; i++) 
	    {
            FrustumCullingObject script = objectsList[i];
            bool distanceOk = false;


            // for distance culling
		    if (distanceCulling) 
		    {
			    float sqrDistanceToCull = distanceToCull * distanceToCull;  // Precompute this value once if distanceToCull is constant
			    Vector3 diff = script.transform.position - mainCam.transform.position;
			    float sqrDistance = Vector3.SqrMagnitude(diff);
			    
			    if (sqrDistance > sqrDistanceToCull) distanceOk = false;
			    else distanceOk = true;

			    if (prioritizeDistanceCulling) 
			    {
				    if (!distanceOk) 
				    {
                        script.DisableObject(disableRootObject);
                        continue;
                    }
			    }
            }
		    else
		    {
                distanceOk = true;
            }


            // increase precision by not disabling object if renderer is visible (in game build OR if option set)
            if (!Application.isEditor || !cullInScene) {
                if (script.renderer.isVisible) {
                    continue;
                }
            }


            Transform[] edges = script.GetEdges();

            
            // check if any edge is nearing camera view port
            for (int x=0; x<4; x++) {
                if (edges[x] == null) {
                    continue;
                }

                // change edge to view port position and check whether it's about to enter camera padding or not
                Vector3 screenPoint = mainCam.WorldToViewportPoint(edges[x].position).normalized;
                
                if (screenPoint.z >= activationDirection && distanceOk) {
                    script.EnableObject(disableRootObject);
                    break;
                }

                script.DisableObject(disableRootObject);
            }
        }
    }

    // cull the objects when they're too far from set distance
    void DistanceCulling()
    {
        for (int i=0; i<objectsList.Count; i++) {
            FrustumCullingObject script = objectsList[i];
            float distance = Vector3.Distance(script.transform.position, mainCam.transform.position);

            if (distance > distanceToCull) {
                script.DisableObject(disableRootObject);
                continue;
            }

            script.EnableObject(disableRootObject);
        }
    }

    #endregion
    
    #region PUBLIC APIS
    
    // add a gameobject to the culling -> must have the FrustumCullingObject script
    public void Add(GameObject go)
    {
        FrustumCullingObject objectScript = go.GetComponent<FrustumCullingObject>();

        // check gameobject has script
        if (!objectScript) {
            print($"Gameobject: {go.name} doesn't have FrustumCullingObject script.");
            return;
        }

        // check if script is already added to list
        if (objectsList.Contains(objectScript)) {
            return;
        }

        // add to list
        objectsList.Add(objectScript);
    }

    // remove certain game object from culling list -> must have the FrustumCullingObject script
    public void Remove(GameObject go) 
    {
        FrustumCullingObject script = go.GetComponent<FrustumCullingObject>();

        if (script == null) {
            print($"The passed object {go.name} doesn't have a FrustumCullingObject component attached thus nothing can be removed from the Frustum Culling system.");
            return;
        }

        if (objectsList.Contains(script)) {
            objectsList.Remove(script);
        }
    }
    
    #endregion
}
