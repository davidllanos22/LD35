﻿using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour {

    public enum TYPE {
        TREE,
        WATER,
        ROCK,
        SOLID,
        TREASURE
    }

    [Header("Tile Types")]
    public Sprite tree;
    public Sprite rock;
    public Sprite water;
    public Sprite solid;
    public Sprite treasure;

    private int maxHp;
    private int currentHp;

    private TYPE type;

    private SpriteRenderer renderer;

    public void Init(TYPE type){
        this.type = type;
        renderer = GetComponent<SpriteRenderer>(); 

        switch(type){
            case TYPE.TREE:
                maxHp = 6;
                renderer.sprite = tree;
                break;
            case TYPE.WATER:
                maxHp = 10;
                renderer.sprite = water;
                break;
            case TYPE.ROCK:
                maxHp = 13;
                renderer.sprite = rock;
                break;
            case TYPE.SOLID:
                maxHp = 999999;
                renderer.sprite = solid;
                break;
            case TYPE.TREASURE:
                maxHp = 999999;
                renderer.sprite = treasure;
                break;
        }

        currentHp = maxHp;
    }

    public TYPE GetType(){
        return type;
    }

    public void Hurt(){
        if(type == TYPE.TREASURE){
            Debug.Log("WIN");
        }else{
            currentHp--;
            if(currentHp <= 0){
                Die();
            }
        }
    }

    private void Die(){
        Destroy(transform.gameObject);
    }

    // Update is called once per frame
    void Update () {

    }
}
