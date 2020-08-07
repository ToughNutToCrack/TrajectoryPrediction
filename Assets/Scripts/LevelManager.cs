using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelManager : Singleton<LevelManager>{
    public int maxScore;
    public int balls;
    public int maxObstacles;
    [Space]
    public GameObject player;
    public GameObject collectablePrefab;
    public GameObject obstaclesRoot;
    public List<GameObject> obstacles;
    [Space]
    [Header("Spanw Area")]
    public Vector3 playerVolume;
    public Vector3 collectablesVolume;
    public Vector3 obstaclesVolume;

    public Observable<int> currentScore = new Observable<int>(0);
    public Observable<int> currentShoots = new Observable<int>(0);

    int maxIterationsSpawn = 10;

    public void addPoint(){
        currentScore.val ++;
    }

    public void shoot(){
        currentShoots.val --;
    }

    void Start(){
        currentShoots.val = balls;
        currentScore.propertyUpdated += onScore;
        currentShoots.propertyUpdated += onShoot;

        movePlayer();
        createObstacles();
        createNewCollectable();
        PredictionManager.instance.copyAllObstacles();
    }

    void Update() {
        if(Input.GetKeyUp(KeyCode.M)){
            createNewCollectable();
        }    
    }

     void onShoot(int v){
        if(v == 0){
            lose();
        }
    }

    void onScore(int v){
        if(v < maxScore){
            createNewCollectable();
        }else{
            win();
        }
    }

    void movePlayer(){
        player.transform.position = calculatePositionInVolume(playerVolume);
    }

    void createNewCollectable(){
        GameObject c = Instantiate(collectablePrefab);
        bool empty = false;
        int iteration = 0;

        Vector3 p = Vector3.zero;

        while(!empty && iteration < maxIterationsSpawn){
            p = calculatePositionInVolume(collectablesVolume);
            var hits = Physics.OverlapSphere(p, .3f);
            empty = hits.Length <= 0;
            iteration ++;
        }

        c.transform.position = p;
    }

    void createObstacles(){
        int currentObs = 0;
        while(currentObs < maxObstacles){
            GameObject o = Instantiate(obstacles[Random.Range(0, obstacles.Count)]);
            Collider oc = o.GetComponent<Collider>();
            bool empty = false;
            int iterations = 0;

            Vector3 p = Vector3.zero;
            Quaternion r = Random.rotation;

            while(!empty && iterations < maxIterationsSpawn){
                p = calculatePositionInVolume(obstaclesVolume);
                var hits = Physics.OverlapBox(p, oc.bounds.extents, r);
                empty = hits.Length <= 0;
                iterations ++;
            }

            o.transform.position = p;
            o.transform.rotation = r;
            o.transform.SetParent(obstaclesRoot.transform);
            currentObs ++;
        }
    }

    Vector3 calculatePositionInVolume(Vector3 vol){
        Vector3 p = new Vector3();
        p.x = Random.Range(-vol.x/2, vol.x/2);
        p.y = Random.Range(-vol.y/2, vol.y/2);
        p.z = Random.Range(-vol.z/2, vol.z/2);
        return transform.position + transform.rotation * p;
    }

    void win(){
        StartCoroutine(waitAndReload());
    }

    void lose(){
        StartCoroutine(waitAndCheckForReload());
    }

    IEnumerator waitAndReload(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator waitAndCheckForReload(){
        yield return new WaitForSeconds(2);
        if(currentScore.val < maxScore){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, collectablesVolume);

        Gizmos.color = Color.green;
		Gizmos.DrawWireCube(Vector3.zero, playerVolume);

        Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(Vector3.zero, obstaclesVolume);
	}
}
