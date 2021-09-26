using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(40));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            LoadMenuFunc();
        }
    }

    private void LoadMenuFunc()
    {
        Debug.Log("Loading scene");
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        LoadMenuFunc();
    }
}
