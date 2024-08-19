using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CollectToiletPaper : MonoBehaviour
{
    [SerializeField] List<Transform> toiletPaper;
    [SerializeField] ToiletPaperWork toiletPaperWork;
    private bool hasPaper => toiletPaper[0].gameObject.activeSelf;
    private int currentIndex;

    private void Awake()
    {
        currentIndex = -1;
    }

    private void Start()
    {
        toiletPaperWork.OnWorkDone.Subscribe(_ =>
        {
            toiletPaper[currentIndex+1].gameObject.SetActive(true);
            currentIndex++;

            if (currentIndex < 2)
            {
                toiletPaperWork.WorkDone(false);
                toiletPaperWork.Available(true);
            }
            else
            {
                toiletPaperWork.Available(false);
            }
        });
    }

    public void DepositPaper()
    {
        if (hasPaper)
        {
            toiletPaper[currentIndex].gameObject.SetActive(false);
            if(currentIndex>=0)
                currentIndex--;

            toiletPaperWork.Available(true);
        }
    }
}
