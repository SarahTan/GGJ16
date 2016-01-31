using UnityEngine;
using System.Collections;

public interface Organism {
    void move();
    void attack();
    void attackBuilding();
    bool cooledDown();
    float getLastHitTime();
    int getPowerLevel();
    void setPowerLevel(int powerLevel);
    void takeDamage(Organism target, float amount);
    void hit();
    void decreasePowerLevel();

    Hero.State getState();
    void setState(Hero.State state);
    Hero.Side getSide();
    void setSide(Hero.Side side);
    Organism getTarget();
    void setTarget(Organism target);

}
