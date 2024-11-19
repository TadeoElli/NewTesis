using UnityEngine;

public class PaintableSpawn : PaintableObject
{
    [SerializeField] private GameObject prefab, spawnedObject;
    private Color feedbackColor;
    public override void Awake()
    {
        base.Awake();
        feedbackColor = Color.black;
    }
    private void CreateObject()
    {
        spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = transform.position;
        spawnedObject.transform.rotation = transform.rotation;
        spawnedObject.transform.localScale = transform.localScale;
        spawnedObject.SetActive(true);
    }
    public override void InteractionWithBrush()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
        }
        CreateObject();
    }
    public override void InteractionWithEraser()
    {

    }
    public override void HideFeedback()
    {
        FeedbackManager.Instance.ApplyFeedback(base.objRenderer, feedbackColor);
    }
}
