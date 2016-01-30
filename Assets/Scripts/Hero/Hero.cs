﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class Hero : MonoBehaviour {

    public enum HERO_POSE {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT,
        POWER_UP,
    }

    public enum Side
    {
        LEFT,
        RIGHT
    }

    public Sprite[] spriteList;
    
    private SpriteRenderer _spriteRenderer;
	private int _queuePosition;    
    private int _powerLevel;
    private int _health;
    private float _maxQueue;
    private float _maxScale;
    private float _initXPos;
    private float _maxXPos;

    public Side side;
    public bool fighting;
    public Hero target;
    public bool moving;
    public bool dead;

    private float _lastHitTime;
    private float _attackCooldown;
        
    void Start()
    {
        _lastHitTime = Time.time;
        moving = false;
        fighting = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

	public void Init(int queuePosition){
		_queuePosition = queuePosition;
		_powerLevel = -1;
        _maxQueue = HeroManager.HERO_LIMIT;
        _maxScale = 1f;
        _initXPos = -2.5f;
        _maxXPos = 20f;
        ScaleTo(0.5f * _maxScale);
    }

    public void UpdatePose(ComboManager.Direction poseDirection) {
        Debug.Log("Updating psoe");
        switch (poseDirection) {
            case ComboManager.Direction.UP:
                _spriteRenderer.sprite = spriteList[(int)HERO_POSE.UP];
                break;
            case ComboManager.Direction.DOWN:
                _spriteRenderer.sprite = spriteList[(int)HERO_POSE.DOWN];
                break;
            case ComboManager.Direction.LEFT:
                _spriteRenderer.sprite = spriteList[(int)HERO_POSE.LEFT];
                break;
            case ComboManager.Direction.RIGHT:
                _spriteRenderer.sprite = spriteList[(int)HERO_POSE.RIGHT];
                break;
            default:
                break;         
        }
    }

    public void attack()
    {
        if (!dead && _lastHitTime + _attackCooldown < Time.time)
        {
            _lastHitTime = Time.time;
            target.takeDamage(this, _powerLevel);
        }
    }

    public void takeDamage(Hero attacker, int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            dead = true;
            attacker.fighting = false;
            attacker.target = null;
            flyOff();
        }
    }

    private void flyOff()
    {
        if (side.Equals(Side.LEFT))
        {
            
        }
        else
        {

        }
    }

    //IEnumerator

    public void move()
    {
        if (!moving && !fighting)
        {
            if (side.Equals(Side.RIGHT))
            {
                transform.position += 0.02f * Vector3.left;
            }
            else
            {
                transform.position += 0.02f * Vector3.right;
            }
        }
    }

    public void PowerUp(int powerLevel) {
        Debug.Log("Powered Up");
        _powerLevel = powerLevel;
        _health = powerLevel;
        _attackCooldown = 100.0f / powerLevel;
        
        if(_powerLevel < 0) {
            // If poop level, show transformation to poop
        }else{
            // Show transformation to super saiyan
        }
    }

    public void moveToPlayingField(Side s)
    {
        side = s;
        moving = true;
        StartCoroutine(move(transform.position + Vector3.up * 3));
    }

    IEnumerator move(Vector3 final)
    {
        while ((transform.position - final).magnitude > 0.1f)
        {
            transform.localScale *= 0.95f;
            transform.position = Vector3.MoveTowards(transform.position, final, 0.1f);
            yield return null;
        }
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        moving = false;
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
        MoveToPosition(new Vector3(xPosition, 0, 0));

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
        transform.localPosition = position;
    }
    private void ScaleTo(Vector3 scale) {
        transform.localScale = scale;
    }
    private void ScaleTo(float scale) {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
