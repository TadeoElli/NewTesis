using UnityEngine;

public class PaintableSpawn : PaintableObject
{
    [SerializeField] private GameObject prefab, spawnedObject;
    private Rigidbody spObjectRb;
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
            spObjectRb = spawnedObject.GetComponent<Rigidbody>();
            spawnedObject.SetActive(true);
        }
        else
        {
            spObjectRb.velocity = Vector3.zero;
            spObjectRb.angularVelocity = Vector3.zero;
            spawnedObject.transform.position = transform.position;
            spawnedObject.transform.rotation = transform.rotation;
        }

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
