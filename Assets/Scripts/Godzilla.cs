using UnityEngine;
using System.Collections;

public class Godzilla : MonoBehaviour, Organism {

    private BuildingManager _buildingManager;
    private FightSimulator _fightSimulator;

    public GameObject[] spriteList;
    
    public int powerLevel = 1500;
    public float health = 2000;
    private float cooldownTime = 1f;
    public float lastHitTime;

    private bool dying;

    public int Attack1 = 0;
    public int Attack2 = 1;
    public int Attack3 = 2;
    public int Neutral = 3;
    public int Fall = 4;

    public Organism target;

    public Hero.Side side;

    public Hero.State state;
    public Pose currentPose;

    public enum Pose
    {
        Attack1,
        Attack2,
        Attack3,
        Neutral,
        Fall
    }
    
    public void Awake()
    {
        _buildingManager = BuildingManager.Instance;
        _fightSimulator = FightSimulator.Instance;
        state = Hero.State.Idle;
        dying = false;
    }

    public void place(Vector3 pos, Hero.Side s)
    {
        if (s.Equals(Hero.Side.LEFT))
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        side = s;
        transform.position = pos;
        setSprite(3);
    }

    public void move()
    {
        if (state.Equals(Hero.State.Idle))
        {
            if (side.Equals(Hero.Side.RIGHT))
            {
                transform.position += 0.02f * Vector3.left;
            }
            else
            {
                transform.position += 0.02f * Vector3.right;
            }
        }
    }

    public void attackBuilding()
    {

        if (cooledDown())
        {
            lastHitTime = Time.time;
            if (side.Equals(Hero.Side.LEFT))
            {
                _buildingManager.damageBuildings(1, (int)(powerLevel / 20));
                _fightSimulator.checkBuildingHealth(1);
            }
            else
            {
                _buildingManager.damageBuildings(0, (int)(powerLevel / 20));
                _fightSimulator.checkBuildingHealth(0);
            }
            powerLevel = (int)(powerLevel * 0.8f);
            ToggleAttackPose();
        }
    }

    public void decreasePowerLevel()
    {
        powerLevel -= (int)(powerLevel * 0.02f);
    }

    public void attack()
    {
        Debug.Log("Power Level: " + powerLevel);
        if (target == null || target.getState().Equals(Hero.State.Dead))
        {
            state = Hero.State.Idle;
            return;
        }
        if (cooledDown())
        {
            if (target.cooledDown())
            {
                if (target.getPowerLevel() > powerLevel)
                {
                    target.hit();
                    takeDamage(target, target.getPowerLevel() * Constants.ATTACK_MULTIPLIER);
                    target.decreasePowerLevel();
                }
                else
                {
                    lastHitTime = Time.time;
                    target.takeDamage(this, powerLevel * Constants.ATTACK_MULTIPLIER);
                    decreasePowerLevel();
                }
            }
            else
            {
                lastHitTime = Time.time;
                target.takeDamage(this, powerLevel * Constants.ATTACK_MULTIPLIER);
                decreasePowerLevel();
            }
            ToggleAttackPose();
        }
    }

    private void ToggleAttackPose()
    {
        if (currentPose.Equals(Pose.Attack2))
        {
            setSprite(2);
            currentPose = Pose.Attack3;
        }
        else
        {
            setSprite(1);
            currentPose = Pose.Attack2;
        }
        
    }

    public bool cooledDown()
    {
        return (!state.Equals(Hero.State.Dead) && lastHitTime + cooldownTime < Time.time);
    }

    public void takeDamage(Organism attacker, float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            state = Hero.State.Dead;
            attacker.setState(Hero.State.Idle);
            setSprite(4);
            attacker.setTarget(null);
            if (!dying)
            {
                die();
            }
        }
    }

    private void die()
    {
        dying = true;
        Invoke("kill", 2f);
    }

    private void kill()
    {
        Destroy(this.gameObject);
    }

    void setSprite(int index)
    {
        for (int i = 0; i < spriteList.Length; i++)
        {
            if (i == index)
            {
                spriteList[i].SetActive(true);
            }
            else
            {
                spriteList[i].SetActive(false);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public float getLastHitTime()
    {
        return lastHitTime;
    }

    public int getPowerLevel()
    {
        return powerLevel;
    }

    public void setPowerLevel(int pl)
    {
        powerLevel = pl;
    }

    public void hit()
    {
        lastHitTime = Time.time;
    }

    public Hero.State getState()
    {
        return state;
    }

    public void setState(Hero.State s)
    {
        state = s;
    }

    public Hero.Side getSide()
    {
        return side;
    }

    public void setSide(Hero.Side s)
    {
        side = s;
    }

    public Organism getTarget()
    {
        return target;
    }

    public void setTarget(Organism t)
    {
        target = t;
    }
}
