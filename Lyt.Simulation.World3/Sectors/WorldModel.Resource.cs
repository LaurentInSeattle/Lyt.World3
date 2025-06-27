namespace Lyt.Simulation.World3;

public sealed partial class WorldModel
{
    private Level nonrenewableResources;

    private Rate nonrenewableResourceUsageRate;

    private Auxiliary nonrenewableResourceFractionRemaining;
    private Auxiliary nonrenewableResourceUsageFactor;
    private Auxiliary perCapitaResourceUsageMultiplier;

    private Auxiliary fractionOfCapitalAllocatedToObtainingResources;
    private Table fractionOfCapitalAllocatedToObtainingResourcesBefore;
    private Table fractionOfCapitalAllocatedToObtainingResourcesAfter;

    private void CreateResourceSector()
    {
        this.CurrentSector = "Non-renewable Resources";
        this.CurrentSubSector = string.Empty;


        this.nonrenewableResources =
            new Level(this, "nonrenewableResources", 129, "resource units", this.nonrenewableResourcesInitialK)
            {
                UpdateFunction = delegate () { return nonrenewableResources.J + this.DeltaTime * (-nonrenewableResourceUsageRate.J); }
            };


        this.nonrenewableResourceUsageRate = new Rate(this, "nonrenewableResourceUsageRate", 130, "resource units per year")
        {
            UpdateFunction = delegate ()
            {
                return population.K * perCapitaResourceUsageMultiplier.K * nonrenewableResourceUsageFactor.K;
            }
        };


        this.nonrenewableResourceUsageFactor = new Auxiliary(this, "nonrenewableResourceUsageFactor", 131, "dimensionless")
        {
            //nonrenewableResourceUsageFactor.before = 1;
            //nonrenewableResourceUsageFactor.after = 1;
            //nonrenewableResourceUsageFactor.updateFn = function() {
            //    return clip(this.after, this.before, t, policyYear);
            //}
            // Always one ??? Possibly a bug ? 
            UpdateFunction = delegate () { return Clip(1.0, 1.0, this.Time, WorldModel.PolicyYear); }
        };

        this.perCapitaResourceUsageMultiplier =
            new Table(this, "perCapitaResourceUsageMultiplier", 132, "resource units per person-year",
                new double[] { 0, 0.85, 2.6, 4.4, 5.4, 6.2, 6.8, 7, 7 }, 0, 1600, 200)
            {
                UpdateFunction = delegate () { return industrialOutputPerCapita.K; }
            };

        this.nonrenewableResourceFractionRemaining = new Auxiliary(this, "nonrenewableResourceFractionRemaining", 133, "dimensionless")
        {
            UpdateFunction = delegate () { return nonrenewableResources.K / nonrenewableResourcesInitialK; }
        };

        this.fractionOfCapitalAllocatedToObtainingResources =
            new Auxiliary(this, "fractionOfCapitalAllocatedToObtainingResources", 134, "dimensionless")
            {
                UpdateFunction = delegate ()
                {
                    return Clip(
                        fractionOfCapitalAllocatedToObtainingResourcesAfter.K,
                        fractionOfCapitalAllocatedToObtainingResourcesBefore.K,
                        this.Time, WorldModel.PolicyYear);
                }
            };

        // BUG ??? The tables After and Before are identical 
        this.fractionOfCapitalAllocatedToObtainingResourcesBefore =
            new Table(
                this, "fractionOfCapitalAllocatedToObtainingResourcesBefore", 135, "dimensionless",
                new double[] { 1, 0.9, 0.7, 0.5, 0.2, 0.1, 0.05, 0.05, 0.05, 0.05, 0.05 }, 0, 1, 0.1)
            {
                UpdateFunction = delegate () { return nonrenewableResourceFractionRemaining.K; }
            };

        this.fractionOfCapitalAllocatedToObtainingResourcesAfter =
            new Table(
                this, "fractionOfCapitalAllocatedToObtainingResourcesAfter", 136, "dimensionless",
                new double[] { 1, 0.9, 0.7, 0.5, 0.2, 0.1, 0.05, 0.05, 0.05, 0.05, 0.05 }, 0, 1, 0.1)
            {
                UpdateFunction = delegate () { return nonrenewableResourceFractionRemaining.K; }
            };

    }
}
