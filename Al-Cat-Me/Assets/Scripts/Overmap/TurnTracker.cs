public class TurnTracker
{
    private bool _playerTurn = true;
    private bool _enemyTurn = false;

    public bool IsPlayerTurn() => _playerTurn;
    public void EndPlayerTurn()
    {
        this._playerTurn = false;
        this._enemyTurn = true;
    }

    public bool IsEnemyTurn() => _enemyTurn;
    public void EndEnemyTurn()
    {
        this._enemyTurn = false;
        this._playerTurn = true;
    }

    public void Reset()
    {
        this._playerTurn = true;
        this._enemyTurn = false;
    }
}