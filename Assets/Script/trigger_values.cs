using UnityEngine;

public class trigger_values : MonoBehaviour
{
    public int scene_id_next;
    public string trigger_name_next;
    private scene_manager sceneManagerInstance;

    private void Start()
    {
        GameObject managerObj = GameObject.FindGameObjectWithTag("scene_manager");
        if (managerObj != null)
        {
            sceneManagerInstance = managerObj.GetComponent<scene_manager>();
        }
        else
        {
            Debug.LogWarning("Scene manager not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && sceneManagerInstance != null)
        {
            sceneManagerInstance.Geto_sinuAidi_to_turiga(scene_id_next, trigger_name_next);// get scene id and trigger
        }
    }
}
