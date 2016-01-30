using UnityEngine;
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
    
    public Sprite[] spriteList;
    
    private SpriteRenderer _spriteRenderer;
	private int _queuePosition;    
    private int _powerLevel;
    private float _maxQueue;
    private float _maxScale;
    private float _initXPos;
    private float _maxXPos;

    void Start() {
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

    public void PowerUp(int powerLevel) {
        _powerLevel = powerLevel;
        
        if(_powerLevel < 0) {
            // If poop level, show transformation to poop
        }else{
            // Show transformation to super saiyan
        }
    }

    public void moveToPlayingField()
    {
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
