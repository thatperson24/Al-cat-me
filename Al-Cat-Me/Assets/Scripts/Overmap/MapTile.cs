using Assets.Scripts.StateEffects;
using UnityEngine;
using System;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Drawing;
using Color = UnityEngine.Color;

public class MapTile : MonoBehaviour, IPointerClickHandler
{
    public static class TileState
    {
        public const char UNOCCUPIED = 'U';
        public const char OCCUPIED = 'O';
        public const char BURNING = 'B';
        public const char TERRAIN = 'T';
        public const char MUDDY = 'M';
        public const char WET = 'W';
        public const char ELECTRIFIED = 'E';

        public static readonly char[] Entries = { UNOCCUPIED, OCCUPIED, BURNING, TERRAIN, MUDDY, WET, ELECTRIFIED };

        public static bool CanBeOccupied(char tileState)
        {
            return tileState switch
            {
                TERRAIN or OCCUPIED => false,
                _ => true,
            };
        }
    }

    /**
     * pls don't modify me outside of initial creation --> :( <--
     */
    public Point MyPosition;
    private OccupyingEntity Entity = null;
    private readonly ElementalStateMachine Esm = new();

    [SerializeField] private char state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
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
        Color newColor = state switch
        {
            TileState.UNOCCUPIED => Color.green,
            TileState.OCCUPIED => Color.yellow,
            TileState.BURNING => Color.red,
            TileState.TERRAIN => Color.brown,
            TileState.MUDDY => Color.black,
            TileState.WET => Color.blue,
            TileState.ELECTRIFIED => Color.purple,
            _ => throw new NotImplementedException($"Unrecognized tile state: {state}"),
        };

        if (this.IsOccupied && this.state == TileState.TERRAIN)
        {
            this.spriteRenderer.color = Color.brown;
        }

        else if (this.IsOccupied)
        {
            // TODO: render enemy on the square
            this.spriteRenderer.color = Color.hotPink;
        }
        else
        {
            this.spriteRenderer.color = newColor;
        }
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
            TileState.UNOCCUPIED or TileState.OCCUPIED => ElementalState.X,
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
            ElementalState.Air => TileState.UNOCCUPIED, // TODO: actually return GUSTY or something
            ElementalState.Earth => TileState.TERRAIN,
            ElementalState.X => TileState.UNOCCUPIED,
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
        }
    }
    
    public void SetEntity(OccupyingEntity entity)
    {
        entity.CurrentTile = this;
        this.Entity = entity;
        ReRender();
    }

    public bool CanBeOccupied
    {
        get => TileState.CanBeOccupied(this.state);
    }

    public bool IsOccupied
    {
        // TODO: Remove OCCUPIED state if we don't really need it
        get => this.Entity != null || this.state == TileState.OCCUPIED || this.state == TileState.TERRAIN;
    }

    public void MoveEntityToOtherMapTile(MapTile other)
    {
        if (this.Entity == null)
        {
            throw new Exception("Attempted to move null entity to other tile");
        }
        else if (other.Entity != null)
        {
            throw new Exception("Attempted to move entity to occupied tile");
        }

        // Perform the swaperoo
        other.SetEntity(this.Entity);
        this.Entity = null;
    }
}
