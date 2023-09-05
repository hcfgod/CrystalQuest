using UnityEngine;

namespace FrustumCullingSpace
{
    public class FrustumCullingObject : MonoBehaviour
    {   
        public Renderer renderer { get; private set; }
        public bool turnedOff { get; private set; }

        public Transform[] edges = new Transform[4];

        [Tooltip("Shows the edges in scene view as yellow wire spheres.")]
        public bool showEdges = true;


        void Start()
        {
            renderer = GetComponent<Renderer>();

            if (renderer) {
                FrustumCulling.instance.Add(gameObject);
            }
            else {
                Debug.LogWarning($"No renderer found on gameobject: {gameObject.name}. You must put this script on an object with a renderer.");
            }
            

            if (edges.Length == 0) {
                print($"No edges set on gameobject {gameObject.name}. You need to Build Edges from the FrustumCullingObject component.");
                return;
            }


	        for(int i = 0; i < edges.Length; i++)
	        {
	        	var edge = edges[i];
	        	
	        	if(edge == null)
	        	{
	        		print($"There seems to be missing edges on the gameobject {gameObject.name}. Make sure to Build Edges from the FrustumCullingObject component.");
	        	}
	        }
        }

        void OnDrawGizmosSelected() 
        {
            if (!showEdges) {
                return;
            }

            // render the built edges as yellow wire spheres
	        for (int i = 0; i < edges.Length; i++) 
	        {
		        if (edges[i] == null)
		        {
                    continue;
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(edges[i].position, 0.1f);
            }
        }

        // disable object
        public void DisableObject(bool disableRoot=false)
        {
            if (turnedOff) {
                return;
            }

            turnedOff = true;

            if (disableRoot) {
                transform.root.gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(false);
        }

        // enable object 
        public void EnableObject(bool disableRoot=false)
        {
            if (!turnedOff) {
                return;
            }
            

            turnedOff = false;

            if (disableRoot) {
                transform.root.gameObject.SetActive(true);
                return;
            }
            
            gameObject.SetActive(true);
        }

        // changes the structure of this gameobject and builds edges to work with culling calculations
        public void BuildEdges()
        {
            // force edges length
            edges = new Transform[4];


            // create 4 edges
            for (int i = 0; i < 4; i++) {
                GameObject go = new GameObject();
                go.transform.parent = transform;

                Vector3 offset = Vector3.zero;
                Vector3 pos = Vector3.zero;

                
                // top edge
                if (i == 0) {
                    offset = -transform.up * (transform.localScale.y / 2f) * -1f;
                    pos = transform.position + offset;

                    go.name = "TopEdge";
                }

                // bottom edge
                if (i == 1) {
                    offset = transform.up * (transform.localScale.y / 2f) * -1f;
                    pos = transform.position + offset;

                    go.name = "BottomEdge";
                }

                // right edge
                if (i == 2) {
                    offset = -transform.right * (transform.localScale.x / 2f) * -1f;
                    pos = transform.position + offset;

                    go.name = "RightEdge";
                }

                // left edge
                if (i == 3) {
                    offset = transform.right * (transform.localScale.x / 2f) * -1f;
                    pos = transform.position + offset;

                    go.name = "LeftEdge";
                }


                go.transform.position = pos;
                edges[i] = go.transform;
                
                
                FrustumCullingEdge edgeScript = go.GetComponent<FrustumCullingEdge>();
                if (edgeScript == null) {
                    go.AddComponent<FrustumCullingEdge>();
                }
            }

        }

        // return the edges transforms
        public Transform[] GetEdges()
        {
            return edges;
        }

        public bool CheckIfEdgesBuilt()
        {
            for (int i=0; i<4; i++) {
                if (edges[i] != null) {
                    return true;
                }
            }

            return false;
        }
    }
}