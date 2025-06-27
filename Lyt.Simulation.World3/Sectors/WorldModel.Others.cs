namespace Lyt.Simulation.World3;

// Model Variables: Creates Other Sectors 
public sealed partial class WorldModel
{
    private Auxiliary fractionOfOutputInAgriculture;
    private Auxiliary fractionOfOutputInServices;
    private Auxiliary fractionOfOutputInIndustry;

    private Auxiliary population0To44;
    private Auxiliary population0To64;

    private double nonrenewableResourcesInitialK = 1.0e12;  // resource units, used in eqns 129 and 133
    private const double nonrenewableResourcesInitialBase = 1.0e12;  // resource units, used in eqns 129 and 133

    public void SetInitialResources (double multiplier)
    {
        this.nonrenewableResourcesInitialK = nonrenewableResourcesInitialBase * multiplier;
        this.nonrenewableResources.ReInitialize(this.nonrenewableResourcesInitialK);
    }

    private void CreateOtherSectors()
    {
        // Model Variables: SUPPLEMENTARY EQUATIONS

        this.CurrentSector = "Supplementary Equations";
        this.CurrentSubSector = string.Empty;

        this.fractionOfOutputInAgriculture = new Auxiliary(this, "fractionOfOutputInAgriculture", 147)
        {
            UpdateFunction = delegate ()
            {
                return 0.22 * food.K / (0.22 * food.K + serviceOutput.K + industrialOutput.K);
            }
        };

        this.fractionOfOutputInIndustry = new Auxiliary(this, "fractionOfOutputInIndustry", 148)
        {
            UpdateFunction = delegate ()
            {
                return industrialOutput.K / (0.22 * food.K + serviceOutput.K + industrialOutput.K);
            }
        };

        this.fractionOfOutputInServices = new Auxiliary(this, "fractionOfOutputInServices", 149)
        {
            UpdateFunction = delegate ()
            {
                return serviceOutput.K / (0.22 * food.K + serviceOutput.K + industrialOutput.K);
            }
        };

        // Model Variables: NEW EQUATIONS

        this.population0To44 = new Auxiliary(this, "population0To44", 150, "persons")
        {
            CannotBeNegative = true,
            CannotBeZero = true,
            UpdateFunction = delegate ()
            {
                return population0To14.K + population15To44.K;
            }
        };

        this.population0To64 = new Auxiliary(this, "population0To64", 151, "persons")
        {
            CannotBeNegative = true,
            CannotBeZero = true,
            UpdateFunction = delegate ()
            {
                return population0To14.K + population15To44.K + population45To64.K;
            }
        };
    }
}
