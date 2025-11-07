using Assets.Scripts.StateEffects;
using UnityEngine;
using System;

public class MapTile : MonoBehaviour
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
                // TODO: Can a MUDDY tile be occupied?
                TERRAIN or MUDDY => false,
                _ => true,
            };
        }
    }

    private OccupyingEntity Entity = null;
    private readonly ElementalStateMachine Esm = new();

    [SerializeField] private char state;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        if (this.IsOccupied)
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

    public void SetEntity(OccupyingEntity entity)
    {
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
        get => this.Entity != null || this.state == TileState.OCCUPIED;
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
        other.Entity = this.Entity;
        this.Entity = null;
    }
}
