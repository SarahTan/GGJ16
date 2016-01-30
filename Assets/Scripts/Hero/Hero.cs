﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class Hero : MonoBehaviour {

    public enum HERO_TYPE {
        TYPE_A,
        SIZE
    }
    public enum HERO_POSE {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT,
        POWER_UP,
        POWER_DOWN
    }
    
    public Sprite[] spriteList;
    
    private SpriteRenderer _spriteRenderer;
	private int _queuePosition;    
    private int _powerLevel;
    private float _maxQueue;
    private float _maxScale;
    private float _initXPos;
    private float _maxXPos;

    void Start() {
        
    }
	
	public void Init(int queuePosition){
		_queuePosition = queuePosition;
		_powerLevel = -1;
        _maxQueue = 15f;
        _maxScale = 10f;
        _initXPos = -0.5f;
        _maxXPos = 10f;
        ScaleTo(0.5f * _maxScale);
    }

    public void UpdatePose(HERO_POSE heroPose) {
        switch (heroPose) {
            case HERO_POSE.UP:
                //_spriteRenderer.sprite = spriteList[(int)HERO_POSE.UP];
                break;
            default:
                break;         
        }
    }

    public void PowerUp(int powerLevel) {
        _powerLevel = powerLevel;
        
        if(_powerLevel < 0) {
            // If poop level, show transformation to poop
        }else{
            // Show transformation to super saiyan
        }
        MoveToPosition(new Vector3(5f, 0, 0));
    }
    public void MoveToCenter() {
        MoveToPosition(Vector3.zero);
        ScaleTo(_maxScale);
    }    
    public void MoveQueuePosition() {
        // Move this sprite forward to the next graphical representation
        // Different representation/size for 0-x
        float ratio = (_queuePosition / _maxQueue);
        float xPosition = _initXPos - ratio * _maxXPos;
        Vector3 currentPosition = transform.position;
        currentPosition.x = xPosition;
        MoveToPosition(currentPosition);

        // Reduce current position
        if (_queuePosition > 0){
			_queuePosition--;
		}
    }

    private void SetAlpha(float alpha) {
        Color spriteColor = _spriteRenderer.color;
        spriteColor.a = alpha;
        _spriteRenderer.color = spriteColor;
    }
    private void MoveToPosition(Vector3 position) {
        transform.position = position;
    }
    private void ScaleTo(Vector3 scale) {
        transform.localScale = scale;
    }
    private void ScaleTo(float scale) {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
