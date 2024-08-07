﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnerPieceData
    {
        public PieceData data;
        public SkinData skin;
    }

    [SerializeField] public Animator _spawnAnimator;

    public float spawnInterval = 2f;

    private float _timeForNextSpawn = 0f;

    private List<SpawnerPieceData> _pieces = new List<SpawnerPieceData>();

    private List<SpawnerPieceData> _temporalSpawnList = new List<SpawnerPieceData>();

    private System.Random _random = new System.Random();

    [HideInInspector] public SkinData defaultSkin = null;

    public void Update()
    {
        if (Time.time >  this._timeForNextSpawn)
        {
            this.Spawn();
        }
    }

    public void ClearSpwnList()
    {
        this._pieces.Clear();
    }

    public void AddPieceToSpawnList(PieceData data, SkinData skin)
    {
        this._pieces.Add(new SpawnerPieceData() {
            data = data,
            skin = skin
        });
    }

    public void ReinitSpawnList()
    {
        this._temporalSpawnList = (List<SpawnerPieceData>) ListUtils.Shuffle(this._random, new List<SpawnerPieceData>(this._pieces));
    }

    public void Spawn()
    {
        this._timeForNextSpawn = Time.time + this.spawnInterval;
        if (this._pieces.Count > 0)
        {
            if (this._temporalSpawnList.Count == 0)
            {
                this.ReinitSpawnList();
                
            }
        }
        if (this._temporalSpawnList.Count == 0) return;

        SpawnerPieceData requestPiece = this._temporalSpawnList[0];
        this._temporalSpawnList.RemoveAt(0);

        GameObject go = Object.Instantiate(requestPiece.data.piecePrefab, this.transform.position, this.transform.rotation);
        Piece piece = go.GetComponent<Piece>();
        if (piece.skin != null)
        {
            piece.skin.data = defaultSkin ?? requestPiece.skin;
        }

        this._spawnAnimator?.Play("Spawn");
    }

}
