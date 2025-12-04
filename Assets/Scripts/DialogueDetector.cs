using UnityEngine;

public class DialogueDetector : MonoBehaviour
{
    public DialogueManager dialogueManager;

    private void OnTriggerEnter(Collider other)
    {
        NPCInteraction npc = other.GetComponent<NPCInteraction>();
        if (npc != null)
        {
            dialogueManager.SetCurrentNPC(npc);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NPCInteraction npc = other.GetComponent<NPCInteraction>();
        if (npc != null)
        {
            dialogueManager.SetCurrentNPC(null);
        }
    }
}
