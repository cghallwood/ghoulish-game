using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public float moveSpeed;

    private float _imageWidth;
    private float _startPos;
    
    void Start()
    {
        _startPos = transform.position.x;
        _imageWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float repeatDistance = cam.transform.position.x * (1 - moveSpeed);
        float offset = cam.transform.position.x * moveSpeed;

        // Move image
        transform.position = new Vector3(_startPos + offset, transform.position.y, transform.position.z);

        // Check camera distance relative to the image width
        if (repeatDistance > _startPos + _imageWidth)
            _startPos += _imageWidth;
        else if (repeatDistance < _startPos - _imageWidth)
            _startPos -= _imageWidth;
    }
}
