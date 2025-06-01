using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;

public class Hack : MonoBehaviour
{
    public TextMeshProUGUI text_ToCopy;
    public TMP_InputField text_Writing;
    public List<string> passwords;
    private string currentPasswords;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPasswords = passwords[Random.Range(0, passwords.Count)];
        text_ToCopy.text = currentPasswords;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(text_Writing.text);
        if (text_Writing.text == currentPasswords)
        {
            currentPasswords = passwords[Random.Range(0, passwords.Count)];
            text_ToCopy.text = currentPasswords;
            text_Writing.text = "";
        }
    }
}
