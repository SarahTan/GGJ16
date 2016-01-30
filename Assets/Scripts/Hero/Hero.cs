using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SpriteRenderer))]
public class Hero : MonoBehaviour {

    public enum HERO_TYPE {
        TYPE_A,
        TYPE_B,
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

    void Start() {
        
    }
	
	public void Init(int queuePosition){
		_queuePosition = queuePosition;
		_powerLevel = -1;
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
    }

    public void MoveQueuePosition() {
        // Move this sprite forward to the next graphical representation
		// Different representation/size for 0-4
		// Reduce current position
		if(_queuePosition > 0){
			_queuePosition--;
		}
    }
}
