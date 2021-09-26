using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour
{

    public void ShowSlide1()
    {
        transform.Find("Image1").gameObject.SetActive(true);
        transform.Find("Image2").gameObject.SetActive(false);
        transform.Find("Image3").gameObject.SetActive(false);
        transform.Find("Image4").gameObject.SetActive(false);
        transform.Find("Image5").gameObject.SetActive(false);
    }

    public void ShowSlide2()
    {
        transform.Find("Image1").gameObject.SetActive(false);
        transform.Find("Image2").gameObject.SetActive(true);
        transform.Find("Image3").gameObject.SetActive(false);
        transform.Find("Image4").gameObject.SetActive(false);
        transform.Find("Image5").gameObject.SetActive(false);
    }

    public void ShowSlide3()
    {
        transform.Find("Image1").gameObject.SetActive(false);
        transform.Find("Image2").gameObject.SetActive(false);
        transform.Find("Image3").gameObject.SetActive(true);
        transform.Find("Image4").gameObject.SetActive(false);
        transform.Find("Image5").gameObject.SetActive(false);
    }

    public void ShowSlide4()
    {
        transform.Find("Image1").gameObject.SetActive(false);
        transform.Find("Image2").gameObject.SetActive(false);
        transform.Find("Image3").gameObject.SetActive(false);
        transform.Find("Image4").gameObject.SetActive(true);
        transform.Find("Image5").gameObject.SetActive(false);
    }

    public void ShowSlide5()
    {
        transform.Find("Image1").gameObject.SetActive(false);
        transform.Find("Image2").gameObject.SetActive(false);
        transform.Find("Image3").gameObject.SetActive(false);
        transform.Find("Image4").gameObject.SetActive(false);
        transform.Find("Image5").gameObject.SetActive(true);
    }

    public void HideSlides()
    {
        transform.Find("Image1").gameObject.SetActive(false);
        transform.Find("Image2").gameObject.SetActive(false);
        transform.Find("Image3").gameObject.SetActive(false);
        transform.Find("Image4").gameObject.SetActive(false);
        transform.Find("Image5").gameObject.SetActive(false);
    }

}
