using System;

namespace Assets.Scripts.StateEffects
{
    public enum ElementalState
    {
        Fire,
        Lightning,
        Water,
        Air,
        Earth,
        X,
    }

    public class ElementalStateMachine
    {
        private readonly int FIRE_BASE_DAMAGE_OVER_TIME = 20;

        public ElementalState CurrentState;
        private int FireDuration = 0;
        private int FireIntensity = 1;

        public ElementalStateMachine(ElementalState initialState = ElementalState.X)
        {
            CurrentState = initialState;
        }

        public ElementalState ApplyStateTransition(ElementalState action)
        {
            switch (CurrentState)
            {
                case ElementalState.Fire:
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // One more duration of fire
                            CurrentState = ElementalState.Fire;
                            ContinueFire(numTurns: 1);
                            break;
                        case ElementalState.Lightning:
                            // Explode for damage
                            CurrentState = ElementalState.X;
                            ResetFire();
                            break;
                        case ElementalState.Water:
                            // Dries off, resetting the fire
                            CurrentState = ElementalState.X;
                            ResetFire();
                            break;
                        case ElementalState.Air:
                            // Fans the fire, increasing damage-over-time
                            CurrentState = ElementalState.Fire;
                            ContinueFire(numTurns: 1, increaseIntensityBy: 1);
                            break;
                        case ElementalState.Earth:
                        case ElementalState.X:
                            // Does nothing, fire burns down
                            if (FireDuration <= 1)
                            {
                                ResetFire();
                            }
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                case ElementalState.Lightning:
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // Just starts a fire
                            CurrentState = ElementalState.Fire;
                            StartFire();
                            break;
                        case ElementalState.Lightning:
                            // Re-stun
                            CurrentState = ElementalState.Lightning;
                            break;
                        case ElementalState.Water:
                        case ElementalState.Air:
                        case ElementalState.Earth:
                        case ElementalState.X:
                            // Does nothing
                            CurrentState = ElementalState.X;
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                case ElementalState.Water:
                    // TODO: does anything make water wear off on a square or enemy?
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // Dried off by the fire
                            CurrentState = ElementalState.X;
                            ResetFire();
                            break;
                        case ElementalState.Lightning:
                            // Electrocute (apply nice damage)
                            // TODO: apply the damage
                            CurrentState = ElementalState.Water;
                            break;
                        case ElementalState.Water:
                            // Remain slowed
                            CurrentState = ElementalState.Water;
                            break;
                        case ElementalState.Air:
                            // Dried off by the air
                            CurrentState = ElementalState.X;
                            break;
                        case ElementalState.Earth:
                            // Further movement reduction
                            // TODO: Add a muddy state, or keep state about the slow factor?
                            CurrentState = ElementalState.Water;
                            break;
                        case ElementalState.X:
                            // Remain slowed by the water
                            CurrentState = ElementalState.Water;
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                case ElementalState.Air:
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // Just starts a fire
                            CurrentState = ElementalState.Fire;
                            StartFire();
                            break;
                        case ElementalState.Lightning:
                        case ElementalState.Water:
                        case ElementalState.Air:
                        case ElementalState.Earth:
                        case ElementalState.X:
                            // Applies the next state based on action
                            CurrentState = action;
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                case ElementalState.Earth:
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // Just starts a fire
                            CurrentState = ElementalState.Fire;
                            StartFire();
                            break;
                        case ElementalState.Lightning:
                            // Negated by the earth, stay earthy
                            CurrentState = ElementalState.Earth;
                            break;
                        case ElementalState.Water:
                            // Further movement reduction
                            // TODO: Add a muddy state, or keep state about the slow factor?
                            CurrentState = ElementalState.Water;
                            break;
                        case ElementalState.Air:
                            // TODO: Enemy should lose turn if pushed into terrain
                            // Stay earthy?
                            CurrentState = ElementalState.Earth;
                            break;
                        case ElementalState.Earth:
                        case ElementalState.X:
                            // Continue earth effect
                            CurrentState = ElementalState.Earth;
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                case ElementalState.X:
                    switch (action)
                    {
                        case ElementalState.Fire:
                            // Just starts a fire
                            CurrentState = ElementalState.Fire;
                            StartFire();
                            break;
                        case ElementalState.Lightning:
                            // Just stuns
                            CurrentState = ElementalState.Lightning;
                            break;
                        case ElementalState.Water:
                            // Just applies slow
                            CurrentState = ElementalState.Water;
                            break;
                        case ElementalState.Air:
                            // Just pushes enemy back, without applying an effect
                            CurrentState = ElementalState.X;
                            break;
                        case ElementalState.Earth:
                            // Makes enemy dusty
                            CurrentState = ElementalState.Earth;
                            break;
                        case ElementalState.X:
                            // Do nothing
                            CurrentState = ElementalState.X;
                            break;
                        default:
                            HandleUnrecognizedAction(action);
                            break;
                    }
                    break;
                default:
                    throw new NotImplementedException($"Unhandled current state: {CurrentState}");
            }

            // Return updated state
            return CurrentState;
        }

        private void StartFire(int numTurns = 1)
        {
            FireDuration = numTurns;
            FireIntensity = 1;
        }

        private void ContinueFire(int numTurns, int increaseIntensityBy = 0)
        {
            FireDuration += numTurns;
            FireIntensity += increaseIntensityBy;
        }

        private void ResetFire()
        {
            FireDuration = 0;
            FireIntensity = 1;
        }

        private void HandleUnrecognizedAction(ElementalState action)
        {
            throw new NotImplementedException($"Unhandled state applied to {CurrentState}: {action}");
        }

        // Ideas for additional behaviors
        // onTurnStartEffect() -- e.g. puff of smoke or lightning strike
    }
}