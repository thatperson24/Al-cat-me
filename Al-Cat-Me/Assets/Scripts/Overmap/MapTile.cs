using Assets.Scripts.StateEffects;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Drawing;
using Color = UnityEngine.Color;
using NUnit.Framework;

public class MapTile : MonoBehaviour, IPointerClickHandler
{
    public static class TileState
    {
        public const char NORMAL = 'N';
        public const char BURNING = 'B';
        public const char TERRAIN = 'T';
        public const char MUDDY = 'M';
        public const char WET = 'W';
        public const char ELECTRIFIED = 'E';

        public static readonly char[] Entries = { NORMAL, BURNING, TERRAIN, MUDDY, WET, ELECTRIFIED };

        public static bool CanBeOccupied(char tileState)
        {
            return tileState switch
            {
                TERRAIN => false,
                _ => true,
            };
        }
    }

    /**
     * pls don't modify me outside of initial creation --> :( <--
     */
    public Point MyPosition;
    private GameObject EnemyEntity = null;
    private readonly ElementalStateMachine Esm = new();

    [SerializeField] private char state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;

    private bool isIndicated;
    private Combat combat;

    private void Awake()
    {
        isIndicated = false;
        combat = GameObject.Find("CombatMap").GetComponent<Combat>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
    }

    public void SetInitialState(char newState)
    {
        SetState(newState);
        Esm.ApplyStateTransition(MapTileStateToElement(newState));
    }

    public void SetState(char newState)
    {
        this.state = newState;
        ReRender();
    }

    public char GetState() => this.state;

    private void ReRender()
    {
        Sprite newSprite = state switch
        {
            TileState.NORMAL => sprites[0],
            TileState.BURNING => sprites[1],
            TileState.TERRAIN => sprites[2],
            TileState.MUDDY => sprites[3],
            TileState.WET => sprites[4],
            TileState.ELECTRIFIED => sprites[5],
            _ => throw new NotImplementedException($"Unrecognized tile state: {state}"),
        };

        this.spriteRenderer.sprite = newSprite;
    }

    public void ApplyElementalAction(ElementalState action)
    {
        Esm.ApplyStateTransition(action);
        SetState(MapElementToTileState(Esm.CurrentState));
    }

    private ElementalState MapTileStateToElement(char aState)
    {
        return aState switch
        {
            TileState.NORMAL => ElementalState.X,
            TileState.BURNING => ElementalState.Fire,
            TileState.TERRAIN => ElementalState.Earth,
            TileState.MUDDY => ElementalState.Earth, // TODO: add Muddy element state
            TileState.WET => ElementalState.Water,
            TileState.ELECTRIFIED => ElementalState.Lightning,
            _ => throw new NotImplementedException($"Unrecognized tile state: {aState}"),
        };
    }

    private char MapElementToTileState(ElementalState action)
    {
        return action switch
        {
            ElementalState.Fire => TileState.BURNING,
            ElementalState.Lightning => TileState.ELECTRIFIED,
            ElementalState.Water => TileState.WET,
            ElementalState.Air => TileState.NORMAL, // TODO: actually return GUSTY or something
            ElementalState.Earth => TileState.TERRAIN,
            ElementalState.X => TileState.NORMAL,
            _ => throw new NotImplementedException($"Unrecognized elemental action: {action}"),
        };
    }

    public void IndicateAttack()
    {
        transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = true;
        isIndicated = true;

    }

    public void RemoveAttack()
    {
        transform.Find("AttackIndicator").GetComponent<SpriteRenderer>().enabled = false;
        isIndicated = false;
    }

    public void Attack()
    {
        if (isIndicated)
        {
            GameObject.Find("CombatMap").GetComponent<EncounterMap>().ClearIndicated();
            GameObject.Find("CombatMap").GetComponent<Combat>().DestroySpellCard();
            GameObject.Find("Character").GetComponent<CharacterControl>().SetIsLocked(false);
            Debug.Log($"{gameObject.name} is attacked");
            Debug.Log($"{combat.GetCurrentSpell().GetSpellName()}");
            if (transform.childCount > 1)
            {
                transform.GetChild(1).GetComponent<Enemy>().TakeDamage(combat.GetCurrentSpell().GetDamage());
            }
            GameObject.Find("Character").GetComponent<CharacterControl>().SpendMovement(combat.GetCurrentSpell().GetCost());
        }
    }

    public void SetEntity(GameObject entity)
    {
        if (entity != null)
        {
            entity.GetComponent<EnemyMovement>().SetCurrentTile(this);
        }
        this.EnemyEntity = entity;
        ReRender();
    }

    public bool CanBeOccupied
    {
        get => TileState.CanBeOccupied(this.state);
    }

    public bool IsOccupied
    {
        get => this.EnemyEntity != null || !TileState.CanBeOccupied(this.state);
    }

    public void MoveEntityToOtherMapTile(MapTile other)
    {
        if (this.EnemyEntity == null)
        {
            throw new Exception("Attempted to move null entity to other tile");
        }
        else if (other.EnemyEntity != null)
        {
            throw new Exception("Attempted to move entity to occupied tile");
        }

        // Perform the swaperoo
        other.SetEntity(this.EnemyEntity);
        this.EnemyEntity = null;
        this.ReRender();
    }
}
