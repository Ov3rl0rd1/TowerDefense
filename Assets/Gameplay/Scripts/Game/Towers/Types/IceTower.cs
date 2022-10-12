using System.Linq;

public class IceTower : BaseTower
{
    protected override Unit GetTarget()
    {
        UpdateUnitsInRadius();

        if (_unitsInRadius.Count == 0)
            return null;

        return _unitsInRadius.First(x => x.IsSlowedDown == false);
    }
}
