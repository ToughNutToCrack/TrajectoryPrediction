using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public TextMeshProUGUI bullets;
    [Space]
    public Sprite starOn;
    public List<Image> stars;

    void Start(){
        LevelManager.instance.currentShoots.propertyUpdated += onShoot;
        LevelManager.instance.currentScore.propertyUpdated += onHit;
    }

    void onShoot(int v){
        bullets.text = v.ToString();
    }

    void onHit(int v){
        stars[v-1].sprite = starOn;
    }

}
