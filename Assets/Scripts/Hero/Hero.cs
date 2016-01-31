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
        POO
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
    private BuildingManager _buildingManager;
    private FightSimulator _fightSimulator;
	private int _queuePosition;
    public int powerLevel { get; private set; }
    public int totalPowerLevel { get; private set; }
    private float _health;
    private float _maxQueue;
    private float _maxScale;
    private float _scaleRatio;
    private float _initXPos;
    private float _maxXPos;
    private bool _poweredUp;
    private bool _isReadyToSend;
    private bool _isWalking;
    private bool _togglePose;
    private HERO_POSE _currentPose;

    public static float buildingDamageMultiplier;

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
        _isReadyToSend = false;
        _isWalking = false;
        _togglePose = false;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _buildingManager = BuildingManager.Instance;
        _fightSimulator = FightSimulator.Instance;        
    }

	public void Init(int queuePosition){
		_queuePosition = queuePosition;
		powerLevel = -1;
        _maxQueue = HeroManager.HERO_LIMIT;
        _maxScale = 1f;
        _scaleRatio = 1f;
        _initXPos = -1.5f;
        _maxXPos = 15f;
        ScaleTo(0.5f * _maxScale);
    }

    public void UpdatePose(ComboManager.Direction poseDirection, int playerNum) {
        if(!_isReadyToSend) {
            ScaleTo(2f);
            switch (poseDirection) {
                case ComboManager.Direction.UP:
                    SetSprite(HERO_POSE.UP);
                    break;
                case ComboManager.Direction.DOWN:
                    SetSprite(HERO_POSE.DOWN);
                    break;
                case ComboManager.Direction.LEFT:
                    if(playerNum == 0) {
                        SetSprite(HERO_POSE.LEFT);
                    }
                    else{
                        SetSprite(HERO_POSE.RIGHT);
                    }
                    break;
                case ComboManager.Direction.RIGHT:
                    if(playerNum == 0) {
                        SetSprite(HERO_POSE.LEFT);
                    }
                    else{
                        SetSprite(HERO_POSE.RIGHT);
                    }
                    break;
                default:
                    break;         
            }
            ScaleTo(1f);
        }
    }

    public void attackBuilding()
    {

        if (cooledDown())
        {
            lastHitTime = Time.time;
            if (side.Equals(Side.LEFT))
            {
                _buildingManager.damageBuildings(0, (int)(totalPowerLevel * buildingDamageMultiplier));
                _fightSimulator.checkBuildingHealth(1);
            }
            else
            {
                _buildingManager.damageBuildings(0, (int)(totalPowerLevel * buildingDamageMultiplier));
                _fightSimulator.checkBuildingHealth(0);
            }
            TogglePunchPose();
        }
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
                    powerLevel -= (int)(target.powerLevel * Constants.POWER_DECREASE_MULTIPLIER);
                }
                else
                {
                    lastHitTime = Time.time;
                    target.takeDamage(this, powerLevel * 0.4f);
                    powerLevel -= (int)(powerLevel * Constants.POWER_DECREASE_MULTIPLIER);
                }
            }
            else
            {
                lastHitTime = Time.time;
                target.takeDamage(this, powerLevel * 0.4f);
                powerLevel -= (int)(powerLevel * Constants.POWER_DECREASE_MULTIPLIER);
            }
            TogglePunchPose();
        }
    }

    private void TogglePunchPose() {             
        if (_poweredUp) {
            if (_currentPose.Equals(HERO_POSE.PUNCH_LEFT1)) {
                SetSprite(HERO_POSE.PUNCH_LEFT2);
                _currentPose = HERO_POSE.PUNCH_LEFT2;
            }
            else {
                SetSprite(HERO_POSE.PUNCH_LEFT1);
                _currentPose = HERO_POSE.PUNCH_LEFT1;
            }
        }
        else {
            SetSprite(HERO_POSE.POO);
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
            StartCoroutine(flyOff(new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 0)));
        }
        else
        {
            StartCoroutine(flyOff(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0)));
        }
        if(_poweredUp) {
            SetSprite(HERO_POSE.FLY_LEFT);
        }
        else {
            SetSprite(HERO_POSE.POO);
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

    public void PowerUp(int pl, float scaleRatio) {
        powerLevel = pl;
        _health = powerLevel;
        totalPowerLevel = powerLevel;
        _attackCooldown = 40.0f / powerLevel;
        _scaleRatio = scaleRatio;
        _isReadyToSend = true; 

        if (powerLevel < (Constants.HERO_POWER_SHIT*1.2)) {
            // If poop level, show transformation to poop
            SetSprite(HERO_POSE.POO);
        }else{
            // Show transformation to super saiyan         
            _poweredUp = true;
            auraAnimatorController.SetBool("PowerUp", true);
            ScaleTo(1.0f + 0.3f * scaleRatio);
            SetSprite(HERO_POSE.POWER_UP);
        }
    }

    public void moveToPlayingField(Side s)
    {
        side = s;
        state = State.Moving;
        if(_poweredUp) {
            auraAnimatorController.SetBool("PowerUp", false);
            SetSprite(HERO_POSE.PUNCH_LEFT1);
        }
        else {
            SetSprite(HERO_POSE.POO);
        }
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
        //ScaleTo(0.4f + 0.2f*_scaleRatio);
        ScaleTo(0.4f);
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
        SetSprite(HERO_POSE.DEFAULT);
    }
    private IEnumerator StartWalking() {
        while(_isWalking) {
            SetSprite(HERO_POSE.LEFT);
            yield return new WaitForSeconds(0.2f);
            SetSprite(HERO_POSE.RIGHT);
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
    private void SetSprite(HERO_POSE newPose) {
        if(_currentPose != HERO_POSE.POO) {
            _currentPose = newPose;
            _spriteRenderer.sprite = spriteList[(int)_currentPose];
        }
    }
}
