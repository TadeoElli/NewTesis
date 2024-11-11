using UnityEngine;

public class PaintableSpawn : PaintableObject
{
    [SerializeField] private GameObject prefab, spawnedObject;
    private Color feedbackColor;
    private void Awake()
    {
        feedbackColor = Color.black;
    }
    public override void InteractionWithBrush()
    {
        //base.InteractionWithBrush();
        if(spawnedObject == null)
        {
            spawnedObject = Instantiate(prefab);
            spawnedObject.transform.position = transform.position;
            spawnedObject.SetActive(true);
        }
        else
            spawnedObject.transform.position = transform.position;

    }
    public override void InteractionWithEraser()
    {
        //base.InteractionWithEraser();

    }
    public override void HideFeedback()
    {
        FeedbackManager.Instance.ApplyFeedback(base.objRenderer, feedbackColor);
    }
}
