using System.Linq;

public class MageTower : BaseTower
{
    protected override Unit GetTarget()
    {
        UpdateUnitsInRadius();

        if (_unitsInRadius.Count == 0)
            return null;

        return _unitsInRadius.OrderByDescending(x => x.Health).First();
    }
}
