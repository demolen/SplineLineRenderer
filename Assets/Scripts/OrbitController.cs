using UnityEngine;
using System.Collections.Generic;

public class OrbitController : MonoBehaviour
{
    public GameObject sun; // The Sun GameObject
    private List<GameObject> planets; // List of planets with the "Planet" tag
    private Dictionary<GameObject, float> orbitSpeeds; // Dictionary to store individual orbit speeds
    private Dictionary<GameObject, float> rotationSpeeds; // Dictionary to store individual rotation speeds
    private Vector3 orbitAxis; // The axis around which the planet will orbit

    // Start is called before the first frame update
    private void Start()
    {
        orbitAxis = new Vector3(0, 0, 1); // Set the orbit axis to the Z-axis for a 2D top-down view

        // Find the Sun GameObject if not assigned in the editor
        if (sun == null)
        {
            sun = GameObject.FindGameObjectWithTag("Sun");
        }

        // Find all GameObjects with the "Planet" tag
        planets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Planet"));

        // Initialize dictionaries for orbit and rotation speeds
        orbitSpeeds = new Dictionary<GameObject, float>();
        rotationSpeeds = new Dictionary<GameObject, float>();

        // Assign random orbit and rotation speeds for each planet
        foreach (GameObject planet in planets)
        {
            orbitSpeeds[planet] = Random.Range(10f, 80f);
            rotationSpeeds[planet] = Random.Range(10f, 50f);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        OrbitAroundSun();
        RotatePlanets();
    }

    private void OrbitAroundSun()
    {
        // Rotate each planet around the Sun at its individual speed
        foreach (GameObject planet in planets)
        {
            planet.transform.RotateAround(sun.transform.position, orbitAxis, orbitSpeeds[planet] * Time.deltaTime);
        }
    }

    private void RotatePlanets()
    {
        // Rotate each planet around its own axis at its individual speed
        foreach (GameObject planet in planets)
        {
            planet.transform.Rotate(orbitAxis, rotationSpeeds[planet] * Time.deltaTime);
        }
    }
}
