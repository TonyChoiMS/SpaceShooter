using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor 
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;

        // calculate start position vector
        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        // Circle color
        Handles.color = Color.white;

        // circle outline view
        Handles.DrawWireDisc(fov.transform.position // Circle position
                             , Vector3.up   // normal Vector
                             , fov.viewRange);    // Circle Radius

        // Sector color
        Handles.color = new Color(1, 1, 1, 0.2f);

        // Draw Sector
        Handles.DrawSolidArc(fov.transform.position // Sector position
                            , Vector3.up    // normal Vector
                             , fromAnglePos // sector start position
                             , fov.viewAngle    // sector angle
                             , fov.viewRange);  // sector radius

        // text view range
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f)
                      , fov.viewAngle.ToString());
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
