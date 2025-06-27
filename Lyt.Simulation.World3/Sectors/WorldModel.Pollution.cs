namespace Lyt.Simulation.World3;

public sealed partial class WorldModel
{
    private Level persistentPollution;

    private Rate persistentPollutionGenerationRate;
    private Rate persistentPollutionAssimilationRate;

    private Auxiliary indexOfPersistentPollution;
    private Auxiliary persistentPollutionGenerationFactor;
    private Auxiliary persistentPollutionGeneratedByIndustrialOutput;
    private Auxiliary persistentPollutionGeneratedByAgriculturalOutput;
    private Auxiliary assimilationHalfLife;

    private Delay persistentPollutionAppearanceRate;

    private Table assimilationHalfLifeMultiplier;

    private void CreatePollutionSector()
    {
        this.CurrentSector = "Persistent Pollution";
        this.CurrentSubSector = string.Empty;

        this.persistentPollutionGenerationRate = new Rate(this, "persistentPollutionGenerationRate", 137, "pollution units per year")
        {
            UpdateFunction = delegate ()
            {
                return
                    (persistentPollutionGeneratedByIndustrialOutput.K + persistentPollutionGeneratedByAgriculturalOutput.K) *
                     persistentPollutionGenerationFactor.K;
            }
        };

        this.persistentPollutionGenerationFactor = new Auxiliary(this, "persistentPollutionGenerationFactor", 138)
        {
            //persistentPollutionGenerationFactor.before = 1;
            //persistentPollutionGenerationFactor.after = 1;
            //persistentPollutionGenerationFactor.updateFn = function() {
            //    return clip(this.after, this.before, t, policyYear); }
            // Always one ??? Possibly a bug ? 
            UpdateFunction = delegate () { return Clip(1.0, 1.0, this.Time, WorldModel.PolicyYear); }
        };

        const double persistentPollutionGeneratedByIndustrialOutputFractionOfResourcesAsPersistentMaterial = 0.02;  // dimensionless
        const double persistentPollutionGeneratedByIndustrialOutputIndustrialMaterialsEmissionFactor = 0.1;  // dimensionless
        const double persistentPollutionGeneratedByIndustrialOutputIndustrialMaterialsToxicityIndex = 10;  // pollution units per resource unit  
        this.persistentPollutionGeneratedByIndustrialOutput =
            new Auxiliary(this, "persistentPollutionGeneratedByIndustrialOutput", 139, "pollution units per year")
            {
                UpdateFunction = delegate ()
                {
                    return
                        perCapitaResourceUsageMultiplier.K * population.K *
                        persistentPollutionGeneratedByIndustrialOutputFractionOfResourcesAsPersistentMaterial *
                        persistentPollutionGeneratedByIndustrialOutputIndustrialMaterialsEmissionFactor *
                        persistentPollutionGeneratedByIndustrialOutputIndustrialMaterialsToxicityIndex;
                }
            };


        const double persistentPollutionGeneratedByAgriculturalOutputFractionOfInputsAsPersistentMaterial = 0.001;  // dimensionless
        const double persistentPollutionGeneratedByAgriculturalOutputAgriculturalMaterialsToxicityIndex = 1;  // pollution units per dollar  
        this.persistentPollutionGeneratedByAgriculturalOutput =
            new Auxiliary(this, "persistentPollutionGeneratedByAgriculturalOutput", 140, "pollution units per year")
            {
                UpdateFunction = delegate ()
                {
                    return
                        agriculturalInputsPerHectare.K * arableLand.K *
                        persistentPollutionGeneratedByAgriculturalOutputFractionOfInputsAsPersistentMaterial *
                        persistentPollutionGeneratedByAgriculturalOutputAgriculturalMaterialsToxicityIndex;
                }
            };

        const double persistentPollutionTransmissionDelayK = 20.0; // years, used in eqn 141  
        // Bug ? WHY ??? 
        // persistenPollutionAppearanceRate.qType = "Rate";
        // rateArray.push(auxArray.pop());   // put this among the Rates, not the Auxes
        // Bug : See if we can rename easily 
        this.persistentPollutionAppearanceRate =
            new Delay(
                this, "persistenPollutionAppearanceRate", 141, "pollution units per year",
                persistentPollutionTransmissionDelayK, "persistentPollutionGenerationRate");

        this.persistentPollution = new Level(this, "persistentPollution", 142, "pollution units", 2.5e7)
        {
            UpdateFunction = delegate ()
            {
                return
                    persistentPollution.J + this.DeltaTime * (persistentPollutionAppearanceRate.J - persistentPollutionAssimilationRate.J);
            }
        };

        const double indexOfPersistentPollutionPollutionValueIn1970 = 1.36e8; // pollution units, used in eqn 143
        this.indexOfPersistentPollution = new Auxiliary(this, "indexOfPersistentPollution", 143)
        {
            UpdateFunction = delegate () { return persistentPollution.K / indexOfPersistentPollutionPollutionValueIn1970; }
        };

        this.persistentPollutionAssimilationRate = new Rate(this, "persistentPollutionAssimilationRate", 144, "pollution units per year")
        {
            UpdateFunction = delegate () { return persistentPollution.K / (assimilationHalfLife.K * 1.4); }
        };

        this.assimilationHalfLifeMultiplier =
            new Table(this, "assimilationHalfLifeMultiplier", 145, "dimensionless",
                new double[] { 1, 11, 21, 31, 41 }, 1, 1001, 250)
            {
                UpdateFunction = delegate () { return indexOfPersistentPollution.K; }
            };

        const double assimilationHalfLifeValueIn1970 = 1.5; // years
        this.assimilationHalfLife = new Auxiliary(this, "assimilationHalfLife", 146, "years")
        {
            UpdateFunction = delegate () { return assimilationHalfLifeMultiplier.K * assimilationHalfLifeValueIn1970; }
        };
    }
}
