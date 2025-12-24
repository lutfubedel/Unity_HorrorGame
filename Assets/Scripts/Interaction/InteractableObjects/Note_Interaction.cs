using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note_Interaction : MonoBehaviour, IInteractable
{
    Note noteScript;

    public int id;

    public string GetInteractionText()
    {
        return "Read The Note";
    }

    private void Start()
    {
        noteScript = FindAnyObjectByType<Note>().GetComponent<Note>();
    }

    public void Interact()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        noteScript.GetComponent<CanvasGroup>().alpha = 1.0f;

        noteScript.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = noteScript.notes[id-1].title;
        noteScript.gameObject.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = noteScript.notes[id-1].message;

        Debug.Log("Note is taken");
        Destroy(this.gameObject);

    }


}
