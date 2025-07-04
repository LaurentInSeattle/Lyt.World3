﻿namespace Lyt.World3.Model.Sectors;

/// <summary>
///     Persistent Pollution sector. The initial code is defined p.478.
/// </summary>
public sealed class Pollution : Sector
{
    #region Documentation 
    /*
        ppoli  
            persistent pollution initial[pollution units]. The default is 2.5e7.
        ppol70  
            persistent pollution in 1970 [pollution units]. The default is 1.36e8.
        ahl70  
            assimilation half-life in 1970 [years]. The default is 1.5.
        amti  
            agricultural materials toxicity index[pollution units / dollar].The
            default is 1.
        imti  
            industrial materials toxicity index[pollution units / resource unit].
            The default is 10.
        imef  
            industrial materials emission factor[]. The default is 0.1.
        fipm  
            fraction of inputs as persistent materials[]. The default is 0.001.
        frpm  
            fraction of resources as persistent materials[]. The default is 0.02.
        ppgf1  
            ppgf value before time=pyear[]. The default is 1.
        ppgf2  
            ppgf value after time=pyear[]. The default is 1.
        ppgf21  
            DESCRIPTION.The default is 1.
        pptd1  
            pptd value before time= pyear[years].The default is 20.
        pptd2  
            pptd value after time= pyear[years].The default is 20.


        ppol : numpy.ndarray
            persistent pollution[pollution units]. It is a state variable.
        ppolx : numpy.ndarray
            index of persistent pollution[].
        ppgao : numpy.ndarray
            persistent pollution generated by agricultural output
            [pollution units / year].
        ppgio : numpy.ndarray
            persistent pollution generated by industrial output
            [pollution units / year].
        ppgf : numpy.ndarray
            persistent pollution generation factor[].
        ppgr : numpy.ndarray
            persistent pollution generation rate[pollution units / year].
        ppapr : numpy.ndarray
            persistent pollution appearance rate[pollution units / year].
        ppasr : numpy.ndarray
            persistent pollution assimilation rate[pollution units / year].
        pptd : numpy.ndarray
            persistent pollution transmission delay[years].
        ahl : numpy.ndarray
            assimilation half-life[years].
        ahlm : numpy.ndarray
            assimilation half-life multiplier [].
    */
    #endregion Documentation 

    public Pollution(World world) : base(world)
        => InitializeLists(this, this.N, double.NaN);

    public override void SetDelayFunctions() 
        => this.CreateDelayThree(new(this.Ppgr));

    #region Constants, State and Rates 

    // Constants 

    // persistent pollution initial[pollution units]. The default is 2.5e7.
    public double Ppoli { get; private set; }

    // persistent pollution in 1970 [pollution units]. The default is 1.36e8.
    public double Ppol70 { get; private set; }

    // assimilation half-life in 1970 [years]. The default is 1.5.
    public double Ahl70 { get; private set; }

    // agricultural materials toxicity index[pollution units / dollar].The default is 1.
    public double Amti { get; private set; }

    // industrial materials toxicity index[pollution units / resource unit]. The default is 10.
    public double Imti { get; private set; }

    // industrial materials emission factor[]. The default is 0.1.
    public double Imef { get; private set; }

    // fraction of inputs as persistent materials[]. The default is 0.001.
    public double Fipm { get; private set; }

    // fraction of resources as persistent materials[]. The default is 0.02.
    public double Frpm { get; private set; }

    // ppgf value before time=pyear[]. The default is 1.
    public double Ppgf1 { get; private set; }

    // ppgf value after time=pyear[]. The default is 1.
    public double Ppgf2 { get; private set; }

    // DESCRIPTION. (?)  The default is 1.
    public double Ppgf21 { get; private set; }

    // pptd value before time= pyear[years].The default is 20.
    public double Pptd1 { get; private set; }

    // pptd value after time= pyear[years].The default is 20.
    public double Pptd2 { get; private set; }

    // State 
    //
    // persistent pollution[pollution units]. It is a state variable.
    public List<double> Ppol { get; private set; } = [];

    // Rates 
    //
    // index of persistent pollution[].
    public List<double> Ppolx { get; private set; } = [];

    // persistent pollution generated by agricultural output [pollution units / year].
    public List<double> Ppgao { get; private set; } = [];

    // persistent pollution generated by industrial output [pollution units / year].
    public List<double> Ppgio { get; private set; } = [];

    // persistent pollution generation factor[].
    public List<double> Ppgf { get; private set; } = [];

    // persistent pollution generation rate[pollution units / year].
    public List<double> Ppgr { get; private set; } = [];

    // persistent pollution appearance rate[pollution units / year].
    public List<double> Ppapr { get; private set; } = [];

    // persistent pollution assimilation rate[pollution units / year].
    public List<double> Ppasr { get; private set; } = [];

    // persistent pollution transmission delay[years].
    public List<double> Pptd { get; private set; } = [];

    // assimilation half-life[years].
    public List<double> Ahl { get; private set; } = [];

    // assimilation half-life multiplier[].
    public List<double> Ahlm { get; private set; } = [];

    #endregion Constants, State and Rates 

    public void InitializeConstants(
        double ppoli = 2.5e7,
        double ppol70 = 1.36e8,
        double ahl70 = 1.5,
        double amti = 1.0,
        double imti = 10.0,
        double imef = 0.1,
        double fipm = 0.001,
        double frpm = 0.02,
        double ppgf1 = 1.0,
        double ppgf2 = 1.0,
        double ppgf21 = 1.0,
        double pptd1 = 20.0,
        double pptd2 = 20.0)
    {
        this.Ppoli = ppoli;
        this.Ppol70 = ppol70;
        this.Ahl70 = ahl70;
        this.Amti = amti;
        this.Imti = imti;
        this.Imef = imef;
        this.Fipm = fipm;
        this.Frpm = frpm;
        this.Ppgf1 = ppgf1;
        this.Ppgf2 = ppgf2;  // if sector is alone, modified as exogeneous
        this.Ppgf21 = ppgf21;
        this.Pptd1 = pptd1;
        this.Pptd2 = pptd2;
    }

    // State variable, requires previous step only
    private void UpdatePpol(int k, int j)
    {
        if (k == 0)
        {
            this.Ppol[0] = this.Ppoli;
        }
        else
        {
            this.Ppol[k] = this.Ppol[j] + this.Dt * (this.Ppapr[j] - this.Ppasr[j]);
        } 
    }

    // From step k requires: PPOL
    [DependsOn("PPOL")]
    private void UpdatePpolx(int k) 
        => this.Ppolx[k] = this.Ppol[k] / this.Ppol70;

    // From step k requires: PCRUM POP
    [DependsOn("PCRUM"), DependsOn("POP")]
    private void UpdatePpgio(int k) 
        => this.Ppgio[k] = 
            this.Resource.Pcrum[k] * this.Population.Pop[k] * this.Frpm * this.Imef * this.Imti;

    // From step k requires: AIPH AL
    [DependsOn("AIPH"), DependsOn("AL")]
    private void UpdatePpgao(int k) 
        => this.Ppgao[k] = 
            this.Agriculture.Aiph[k] * this.Agriculture.Al[k] * this.Fipm * this.Amti;

    // From step k requires: nothing
    private void UpdatePpgf(int k) 
        => this.Ppgf[k] = this.ClipPolicyYear(this.Ppgf2, this.Ppgf1, k);

    // From step k requires: PPGIO PPGAO PPGF
    [DependsOn("PPGIO"), DependsOn("PPGAO"), DependsOn("PPGF")]
    private void UpdatePpgr(int k) 
        => this.Ppgr[k] = (this.Ppgio[k] + this.Ppgao[k]) * this.Ppgf[k];

    // From step k requires: nothing
    private void UpdatePptd(int k) 
        => this.Pptd[k] = this.ClipPolicyYear(this.Pptd2, this.Pptd1, k);

    // From step k=0 requires: PPGR, else nothing
    [DependsOn("PPGR")]
    private void UpdatePpapr(int k)
        // ??? is originally ppgr[jk] rather than ppgr[k]
        => this.Ppapr[k] = this.DelayThree(nameof(this.Ppgr), k, this.Pptd[k]);

    // From step k requires: PPOLX
    [DependsOn("PPOLX")]
    private void UpdateAhlm(int k)
        => this.Ahlm[k] = nameof(this.Ahlm).Interpolate(this.Ppolx[k]);

    // From step k requires: AHLM
    [DependsOn("AHLM")]
    private void UpdateAhl(int k)
        => this.Ahl[k] = this.Ahlm[k] * this.Ahl70;

    // From step k requires: AHL PPOL
    [DependsOn("AHL"), DependsOn("PPOL")]
    private void UpdatePpasr(int k)
        => this.Ppasr[k] = this.Ppol[k] / (this.Ahl[k] * 1.4); 
}
