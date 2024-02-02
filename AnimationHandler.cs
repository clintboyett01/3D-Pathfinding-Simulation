using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationHandler: MonoBehaviour {
    public Queue < GameObject > animationQueueIndex = new Queue < GameObject > ();
    public Queue < Material > animationQueueMaterial = new Queue < Material > ();
    public Slider AnimationSpeed;
    private
    const float MaxEdgeDistance = 1;
    private
    const float LogZeroAvoidance = 0.00000001 f;

    public float animationTime = 0.5 f;
    public float timer = 0 f;

    private void dequeueAnimation() {
        if (animationQueueIndex.Count > 0)
            animationQueueIndex.Dequeue().GetComponent < Renderer > ().material = animationQueueMaterial.Dequeue();
    }
    public void enqueueAnimation(GameObject obj, Material material) {
        animationQueueIndex.Enqueue(obj);
        animationQueueMaterial.Enqueue(material);
    }

    void Update() {
        animationTime = -Mathf.Log10(AnimationSpeed.value + LogZeroAvoidance);
        timer += Time.deltaTime;
        while (timer > animationTime) {
            timer -= animationTime;
            dequeueAnimation();
        }
    }
}