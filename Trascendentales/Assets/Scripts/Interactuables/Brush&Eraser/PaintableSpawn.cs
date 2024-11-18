using UnityEngine;

public class PaintableSpawn : PaintableObject
{
    [SerializeField] private GameObject prefab, spawnedObject;
    private Rigidbody spObjectRb;
    private Color feedbackColor;
    public override void Awake()
    {
        base.Awake();
        feedbackColor = Color.black;
    }
    public override void InteractionWithBrush()
    {
        if(spawnedObject == null)
        {
            spawnedObject = Instantiate(prefab);
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = transform.rotation;
            spawnedObject.transform.localScale = transform.localScale;
            spObjectRb = spawnedObject.GetComponent<Rigidbody>();
            spawnedObject.SetActive(true);
        }
        else
        {
            spawnedObject.SetActive(true);
            spObjectRb.velocity = Vector3.zero;
            spObjectRb.angularVelocity = Vector3.zero;
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = transform.rotation;
            spawnedObject.transform.localScale = transform.localScale;
        }

    }
    public override void InteractionWithEraser()
    {

    }
    public override void HideFeedback()
    {
        FeedbackManager.Instance.ApplyFeedback(base.objRenderer, feedbackColor);
    }
}
