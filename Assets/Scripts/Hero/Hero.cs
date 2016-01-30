using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

    public enum HERO_POSE {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT,
        POWER_UP,
        FLY_LEFT,
        FLY_RIGHT,
        PUNCH_LEFT1,
        PUNCH_LEFT2,
        PUNCH_RIGHT1,
        PUNCH_RIGHT2
    }

    public enum Side
    {
        LEFT,
        RIGHT
    }

    public enum State
    {
        Moving,
        Attacking,
        Fighting,
        Idle,
        Dead

    }

    public Animator animatorController;
    public Animator auraAnimatorController;
    public Sprite[] spriteList;
    
    private SpriteRenderer _spriteRenderer;
	private int _queuePosition;
    public int powerLevel { get; private set; }
    private float _health;
    private float _maxQueue;
    private float _maxScale;
    private float _initXPos;
    private float _maxXPos;
    private bool _poweredUp;
    private bool _isWalking;

    public State state;
    public Side side;
    public Hero target;
    private bool _flyingOff;

    public float lastHitTime;
    private float _attackCooldown;
        
    void Start()
    {
        state = State.Idle;
        _flyingOff = false;
        lastHitTime = Time.time;
        _poweredUp = false;
        _isWalking = false;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

	public void Init(int queuePosition){
		_queuePosition = queuePosition;
		powerLevel = -1;
        _maxQueue = HeroManager.HERO_LIMIT;
        _maxScale = 1f;
        _initXPos = -1.5f;
        _maxXPos = 15f;
        ScaleTo(0.5f * _maxScale);
    }

    public void UpdatePose(ComboManager.Direction poseDirection, int playerNum) {
        if(!_poweredUp) {
            switch (poseDirection) {
                case ComboManager.Direction.UP:
                    _spriteRenderer.sprite = spriteList[(int)HERO_POSE.UP];
                    break;
                case ComboManager.Direction.DOWN:
                    _spriteRenderer.sprite = spriteList[(int)HERO_POSE.DOWN];
                    break;
                case ComboManager.Direction.LEFT:
                    if(playerNum == 0) {
                        _spriteRenderer.sprite = spriteList[(int)HERO_POSE.LEFT];
                    }else{
                        _spriteRenderer.sprite = spriteList[(int)HERO_POSE.RIGHT];
                    }
                    break;
                case ComboManager.Direction.RIGHT:
                    if (playerNum == 0) {
                        _spriteRenderer.sprite = spriteList[(int)HERO_POSE.RIGHT];
                    }
                    else {
                        _spriteRenderer.sprite = spriteList[(int)HERO_POSE.LEFT];
                    }
                    break;
                default:
                    break;         
            }
        }
    }

    public void attackBuilding()
    {

    }

    public void attack()
    {
        if (cooledDown())
        {
            if (target.cooledDown())
            {
                if (target.powerLevel > powerLevel)
                {
                    target.lastHitTime = Time.time;
                    takeDamage(target, target.powerLevel * 0.4f);
                }
                else
                {
                    lastHitTime = Time.time;
                    target.takeDamage(this, powerLevel * 0.4f);
                }
            }
            else
            {
                lastHitTime = Time.time;
                target.takeDamage(this, powerLevel * 0.4f);
            }
        }
    }

    public bool cooledDown()
    {
        return (!state.Equals(State.Dead) && lastHitTime + _attackCooldown < Time.time);
    }

    public void takeDamage(Hero attacker, float amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            state = State.Dead;
            attacker.state = State.Idle;
            attacker.target = null;
            if (!_flyingOff)
            {
                flyOff();
            }
        }
    }

    private void flyOff()
    {
        _flyingOff = true;
        float angle = Random.Range(30, 70) * Mathf.PI / 180;
        if (side.Equals(Side.LEFT))
        {
            _spriteRenderer.sprite = spriteList[(int)HERO_POSE.FLY_LEFT];
            StartCoroutine(flyOff(new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 0)));
        }
        else
        {
            _spriteRenderer.sprite = spriteList[(int)HERO_POSE.FLY_RIGHT];
            StartCoroutine(flyOff(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)));
        }
    }

    IEnumerator flyOff(Vector3 direction)
    {
        float totalDistance = 10f;
        float step = 0.2f;
        float distance = 0;
        while (distance < totalDistance)
        {
            distance += step;
            transform.position += direction * step;
            yield return null;
        }
        Destroy(this.gameObject);
    }

    public void move()
    {
        if (state.Equals(State.Idle))
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

    public void PowerUp(int pl) {
        powerLevel = pl;
        _health = powerLevel;
        _attackCooldown = 40.0f / powerLevel;

        _poweredUp = true;

        if (powerLevel < 0) {
            // If poop level, show transformation to poop
        }else{
            // Show transformation to super saiyan
            Debug.Log(spriteList[(int)HERO_POSE.POWER_UP]);
            _spriteRenderer.sprite = spriteList[(int)HERO_POSE.POWER_UP];
            auraAnimatorController.SetBool("PowerUp", true);
        }
    }

    public void moveToPlayingField(Side s)
    {
        side = s;
        state = State.Moving;
        auraAnimatorController.SetBool("PowerUp", false);
        StartCoroutine(move(transform.position + Vector3.up * 1.5f));
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
        state = State.Idle;
    }

    // Snap to position
    public void SetToCenter() {
        MoveToPosition(Vector3.zero);
        ScaleTo(_maxScale);
    }    
    public void SetQueuePosition() {
        // Move this sprite forward to the next graphical representation
        // Different representation/size for 0-x
        float ratio = (_queuePosition / _maxQueue);
        float xPosition = _initXPos - ratio * _maxXPos;
        transform.localPosition = new Vector3(xPosition, 0, 0);
    }
    // Move to position
    public void MoveToCenter() {
        StartCoroutine(MoveToCenterQueue());
    }
    public void MoveQueuePosition() {
        // Move this sprite forward to the next graphical representation
        // Different representation/size for 0-x        
        // Reduce current position
        if (_queuePosition > 0) {
            _queuePosition--;
        }
        float ratio = (_queuePosition / _maxQueue);
        float xPosition = _initXPos - ratio * _maxXPos;
        StartCoroutine(MoveToNextQueue(new Vector3(xPosition, 0, 0)));
    }    
    IEnumerator MoveToNextQueue(Vector3 final) {
        // Start walking animation
        StartWalkingAnimation();
        while ((transform.localPosition - final).magnitude > 0.1f) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, final, 0.1f);
            yield return null;
        }
        // Stop walking animation
        StopWalkingAnimation();
    }
    IEnumerator MoveToCenterQueue() {
        // Start walking animation
        StartWalkingAnimation();
        while ((transform.localPosition - Vector3.zero).magnitude > 0.1f) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, 0.1f);
            Vector3 scale = transform.localScale * 1.05f;
            if(scale.x > _maxScale) {
                scale.x = scale.y = scale.z = _maxScale;
            }
            transform.localScale = scale;
            yield return null;
        }
        // Stop walking animation
        StopWalkingAnimation();
    }
    private void StartWalkingAnimation() {
        _isWalking = true;
        StartCoroutine(StartWalking());
    }
    private void StopWalkingAnimation() {
        _isWalking = false;
        StopCoroutine(StartWalking());
    }
    private IEnumerator StartWalking() {
        while(_isWalking) {
            _spriteRenderer.sprite = spriteList[(int)HERO_POSE.LEFT];
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.sprite = spriteList[(int)HERO_POSE.RIGHT];
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
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
