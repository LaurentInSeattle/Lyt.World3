namespace Lyt.World3.Model.CapitalSector;

/// <summary> Capital sector. The initial code is defined p.253.  </summary>
public sealed class Capital : Sector
{
    #region Documentation 
    /*
        Attributes
        ----------
        ici : float, optional
            industrial capital initial [dollars]. The default is 2.1e11.
        sci : float, optional
            service capital initial [dollars]. The default is 1.44e11.
        iet : float, optional
            industrial equilibrium time [years]. The default is 4000.
        iopcd : float, optional
            industrial output per capita desired [dollars/person-year]. The
            default is 400.
        lfpf : float, optional
            labor force participation fraction []. The default is 0.75.
        lufdt : float, optional
            labor utilization fraction delay time [years]. The default is 2.
        icor1 : float, optional
            icor, value before time=pyear [years]. The default is 3.
        icor2 : float, optional
            icor, value after time=pyear [years]. The default is 3.
        scor1 : float, optional
            scor, value before time=pyear [years]. The default is 1.
        scor2 : float, optional
            scor, value after time=pyear [years]. The default is 1.
        alic1 : float, optional
            alic, value before time=pyear [years]. The default is 14.
        alic2 : float, optional
            alic, value after time=pyear [years]. The default is 14.
        alsc1 : float, optional
            alsc, value before time=pyear [years]. The default is 20.
        alsc2 : float, optional
            alsc, value after time=pyear [years]. The default is 20.
        fioac1 : float, optional
            fioac, value before time=pyear []. The default is 0.43.
        fioac2 : float, optional
            fioac, value after time=pyear []. The default is 0.43.

        **Industrial subsector**

        ic : numpy.ndarray
            industrial capital [dollars]. It is a state variable.
        io : numpy.ndarray
            industrial output [dollars/year].
        icdr : numpy.ndarray
            industrial capital depreciation rate [dollars/year].
        icir : numpy.ndarray
            industrial capital investment rate [dollars/year].
        icor : numpy.ndarray
            industrial capital-output ratio [years].
        iopc : numpy.ndarray
            industrial output per capita [dollars/person-year].
        alic : numpy.ndarray
            average lifetime of industrial capital [years].
        fioac : numpy.ndarray
            fraction of industrial output allocated to consumption [].
        fioacc : numpy.ndarray
            fioac constant [].
        fioacv : numpy.ndarray
            fioac variable [].
        fioai : numpy.ndarray
            fraction of industrial output allocated to industry [].

        **Service subsector**

        sc : numpy.ndarray
            service capital [dollars]. It is a state variable.
        so : numpy.ndarray
            service output [dollars/year].
        scdr : numpy.ndarray
            service capital depreciation rate [dollars/year].
        scir : numpy.ndarray
            service capital investment rate [dollars/year].
        scor : numpy.ndarray
            service capital-output ratio [years].
        sopc : numpy.ndarray
            service output per capita [dollars/person-year].
        alsc : numpy.ndarray
            average lifetime of service capital [years].
        isopc : numpy.ndarray
            indicated service output per capita [dollars/person-year].
        isopc1 : numpy.ndarray
            isopc, value before time=pyear [dollars/person-year].
        isopc2 : numpy.ndarray
            isopc, value after time=pyear [dollars/person-year].
        fioas : numpy.ndarray
            fraction of industrial output allocated to services [].
        fioas1 : numpy.ndarray
            fioas, value before time=pyear [].
        fioas2 : numpy.ndarray
            fioas, value after time=pyear [].

        **Job subsector**

        j : numpy.ndarray
            jobs [persons].
        jph : numpy.ndarray
            jobs per hectare [persons/hectare].
        jpicu : numpy.ndarray
            jobs per industrial capital unit [persons/dollar].
        jpscu : numpy.ndarray
            jobs per service capital unit [persons/dollar].
        lf : numpy.ndarray
            labor force [persons].
        cuf : numpy.ndarray
            capital utilization fraction [].
        luf : numpy.ndarray
            labor utilization fraction [].
        lufd : numpy.ndarray
            labor utilization fraction delayed [].
        pjas : numpy.ndarray
            potential jobs in agricultural sector [persons].
        pjis : numpy.ndarray
            potential jobs in industrial sector [persons].
        pjss : numpy.ndarray
            potential jobs in service sector [persons].

    */
    #endregion Documentation 

    public Capital(World world) : base(world)
        => Sector.InitializeLists(this, this.N, double.NaN);

    // Only one delay in the Capital Sector : LUF
    protected override void SetDelayFunctions()
        => base.CreateSmooth(new Named(this.Luf));

    #region Constants, State and Rates 

    // Constants 
    //
    //   industrial capital initial[dollars]. The default is 2.1e11.
    public double Ici { get; private set; }

    //       service capital initial[dollars]. The default is 1.44e11.
    public double Sci { get; private set; }

    //       industrial equilibrium time[years]. The default is 4000.
    public double Iet { get; private set; }

    //        industrial output per capita desired[dollars / person - year]. The default is 400.
    public double Iopcd { get; private set; }

    //        labor force participation fraction[]. The default is 0.75.
    public double Lfpf { get; private set; }

    //        labor utilization fraction delay time[years]. The default is 2.
    public double Lufdt { get; private set; }

    //        icor1, value before time = pyear[years]. The default is 3.
    public double Icor1 { get; private set; }

    //        icor2, value after time = pyear[years]. The default is 3.
    public double Icor2 { get; private set; }

    //        scor1, value before time = pyear[years].The default is 1.
    public double Scor1 { get; private set; }

    //        scor2, value after time = pyear[years].The default is 1.
    public double Scor2 { get; private set; }

    //        alic1, value before time = pyear[years].The default is 14.
    public double Alic1 { get; private set; }

    //        alic2, value after time = pyear[years].The default is 14.
    public double Alic2 { get; private set; }

    //        alsc, value before time = pyear[years].The default is 20.
    public double Alsc1 { get; private set; }

    //        alsc, value after time = pyear[years].The default is 20.
    public double Alsc2 { get; private set; }

    //         fioac, value before time = pyear[].The default is 0.43.
    public double Fioac1 { get; private set; }

    //         fioac, value after time = pyear[].The default is 0.43.
    public double Fioac2 { get; private set; }

    //     Industrial subsector
    //
    // industrial capital[dollars]. It is a state variable.
    public List<double> Ic { get; private set; } = [];

    // industrial output[dollars / year].
    public List<double> Io { get; private set; } = [];

    // industrial capital depreciation rate[dollars / year].
    public List<double> Icdr { get; private set; } = [];

    // industrial capital investment rate[dollars / year].
    public List<double> Icir { get; private set; } = [];

    //  industrial capital-output ratio[years].
    public List<double> Icor { get; private set; } = [];

    //  industrial output per capita[dollars / person - year].
    public List<double> Iopc { get; private set; } = [];

    //  average lifetime of industrial capital[years].
    public List<double> Alic { get; private set; } = [];

    //  fraction of industrial output allocated to consumption[].
    public List<double> Fioac { get; private set; } = [];

    //  fioac constant[].
    public List<double> Fioacc { get; private set; } = [];

    //  fioac variable[].
    public List<double> Fioacv { get; private set; } = [];

    //  fraction of industrial output allocated to industry[].
    public List<double> Fioai { get; private set; } = [];

    //     Services subsector
    //
    //  service capital[dollars]. It is a state variable.
    public List<double> Sc { get; private set; } = [];

    //         service output[dollars / year].
    public List<double> So { get; private set; } = [];

    //         service capital depreciation rate[dollars / year].
    public List<double> Scdr { get; private set; } = [];

    //         service capital investment rate[dollars / year].
    public List<double> Scir { get; private set; } = [];

    //         service capital-output ratio[years].
    public List<double> Scor { get; private set; } = [];

    //         service output per capita[dollars / person - year].
    public List<double> Sopc { get; private set; } = [];

    //         average lifetime of service capital[years].
    public List<double> Alsc { get; private set; } = [];

    //         indicated service output per capita[dollars / person - year].
    public List<double> Isopc { get; private set; } = [];

    //        isopc, value before time = pyear[dollars / person - year].
    public List<double> Isopc1 { get; private set; } = [];

    //         isopc, value after time = pyear[dollars / person - year].
    public List<double> Isopc2 { get; private set; } = [];

    //        fraction of industrial output allocated to services[].    
    public List<double> Fioas { get; private set; } = [];

    //         fioas, value before time = pyear[].
    public List<double> Fioas1 { get; private set; } = [];

    //        fioas, value after time = pyear[].
    public List<double> Fioas2 { get; private set; } = [];


    //     Jobs subsector

    //         jobs[persons].
    public List<double> J { get; private set; } = [];

    //         jobs per hectare[persons / hectare].
    public List<double> Jph { get; private set; } = [];

    //         jobs per industrial capital unit[persons / dollar].
    public List<double> Jpicu { get; private set; } = [];

    //         jobs per service capital unit[persons / dollar].
    public List<double> Jpscu { get; private set; } = [];

    //         labor force[persons].
    public List<double> Lf { get; private set; } = [];

    //         capital utilization fraction[].
    public List<double> Cuf { get; private set; } = [];

    //         labor utilization fraction[].
    public List<double> Luf { get; private set; } = [];

    //         labor utilization fraction delayed[].
    public List<double> Lufd { get; private set; } = [];

    //         potential jobs in agricultural sector[persons].
    public List<double> Pjas { get; private set; } = [];

    //         potential jobs in industrial sector[persons].
    public List<double> Pjis { get; private set; } = [];

    //         potential jobs in service sector[persons].
    public List<double> Pjss { get; private set; } = [];

    #endregion Constants, State and Rates 

    public void InitializeConstants(
        double ici = 2.1e11,
        double sci = 1.44e11,
        double iet = 4000,
        double iopcd = 400,
        double lfpf = 0.75,
        double lufdt = 2,
        double icor1 = 3,
        double icor2 = 3,
        double scor1 = 1,
        double scor2 = 1,
        double alic1 = 14,
        double alic2 = 14,
        double alsc1 = 20,
        double alsc2 = 20,
        double fioac1 = 0.43,
        double fioac2 = 0.43)
    {
        this.Ici = ici;
        this.Sci = sci;
        this.Iet = iet;
        this.Iopcd = iopcd;
        this.Lfpf = lfpf;
        this.Lufdt = lufdt;
        this.Icor1 = icor1;
        this.Icor2 = icor2;
        this.Scor1 = scor1;
        this.Scor2 = scor2;
        this.Alic1 = alic1;
        this.Alic2 = alic2;
        this.Alsc1 = alsc1;
        this.Alsc2 = alsc2;
        this.Fioac1 = fioac1;
        this.Fioac2 = fioac2;
    }

    // Initialize the Capital sector ( == initial loop with k=0).
    public override void Initialize()
    {
        try
        {
            //  Set initial conditions
            this.Ic[0] = this.Ici;
            this.Sc[0] = this.Sci;
            this.Cuf[0] = 1.0;

            // industrial subsector
            this.UpdateAlic(0);
            this.UpdateIcdr(0, 0);
            this.UpdateIcor(0);
            this.UpdateIo(0);
            this.UpdateIopc(0);
            this.UpdateFioac(0);
            ;
            // service subsector 
            this.UpdateIsopc(0);
            this.UpdateAlsc(0);
            this.UpdateScdr(0, 0);
            this.UpdateScor(0);
            this.UpdateSo(0);
            this.UpdateSopc(0);
            this.UpdateFioas(0);
            this.UpdateScir(0, 0);

            // back to industrial sector 
            this.UpdateFioai(0);
            this.UpdateIcir(0, 0);

            // job subsector     
            this.UpdateJpicu(0);
            this.UpdatePjis(0);
            this.UpdateJpscu(0);
            this.UpdatePjss(0);
            this.UpdateJph(0);
            this.UpdatePjas(0);
            this.UpdateJ(0);
            this.UpdateLf(0);
            this.UpdateLuf(0);
            this.UpdateLufd(0);

            // recompute supplementary initial conditions
            this.UpdateCuf(0);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // Update one loop of the Capital sector.
    public override void Update(int k, int j, int jk, int kl)
    {
        try
        {
            // job subsector                 
            this.UpdateLufd(k);
            this.UpdateCuf(k);

            // industrial subsector              
            this.UpdateIc(k, j, jk);
            this.UpdateAlic(k);
            this.UpdateIcdr(k, kl);
            this.UpdateIcor(k);
            this.UpdateIo(k);
            this.UpdateIopc(k);
            this.UpdateFioac(k);
            
            // service subsector         
            this.UpdateSc(k, j, jk);
            this.UpdateIsopc(k);
            this.UpdateAlsc(k);
            this.UpdateScdr(k, kl);
            this.UpdateScor(k);
            this.UpdateSo(k);
            this.UpdateSopc(k);
            this.UpdateFioas(k);
            this.UpdateScir(k, kl);
            
            // back to industrial sector 
            this.UpdateFioai(k);
            this.UpdateIcir(k, kl);
            
            // back to job subsector             
            this.UpdateJpicu(k);
            this.UpdatePjis(k);
            this.UpdateJpscu(k);
            this.UpdatePjss(k);
            this.UpdateJph(k);
            this.UpdatePjas(k);
            this.UpdateJ(k);
            this.UpdateLf(k);
            this.UpdateLuf(k);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
    }

    // job subsector                 
    private void UpdateLufd(int k) => throw new NotImplementedException();
    private void UpdateCuf(int k) => throw new NotImplementedException();
    /*
    @requires(["lufd"], ["luf"], check_after_init=False)
    def _update_lufd(self, k):
        """
        From step k=0 requires: LUF, else nothing
        """
        self.lufd[k] = self.smooth_luf(k, self.lufdt)

    @requires(["cuf"], ["lufd"])
    def _update_cuf(self, k):
        """
        From step k requires: LUFD
        """
        self.cuf[k] = self.cuf_f(self.lufd[k])

    */

    // industrial subsector              
    private void UpdateIc(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdateAlic(int k) => throw new NotImplementedException();
    private void UpdateIcdr(int k, int kl) => throw new NotImplementedException();
    private void UpdateIcor(int k) => throw new NotImplementedException();
    private void UpdateIo(int k) => throw new NotImplementedException();
    private void UpdateIopc(int k) => throw new NotImplementedException();
    private void UpdateFioac(int k) => throw new NotImplementedException();
    /*
    @requires(["ic"])
    def _update_state_ic(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        if k == 0:
            self.ic[k] = self.ici
        else:
            self.ic[k] = self.ic[j] + self.dt * (self.icir[jk] - self.icdr[jk])

    @requires(["alic"])
    def _update_alic(self, k):
        """
        From step k requires: nothing
        """
        self.alic[k] = clip(self.alic2, self.alic1, self.time[k], self.pyear)

    @requires(["icdr"], ["ic", "alic"])
    def _update_icdr(self, k, kl):
        """
        From step k requires: IC ALIC
        """
        self.icdr[kl] = self.ic[k] / self.alic[k]

    @requires(["icor"])
    def _update_icor(self, k):
        """
        From step k requires: nothing
        """
        self.icor[k] = clip(self.icor2, self.icor1, self.time[k], self.pyear)

    @requires(["io"], ["ic", "fcaor", "cuf", "icor"])
    def _update_io(self, k):
        """
        From step k requires: IC FCAOR CUF ICOR
        """
        self.io[k] = (self.ic[k] * (1 - self.fcaor[k]) * self.cuf[k] /
                      self.icor[k])

    @requires(["iopc"], ["io", "pop"])
    def _update_iopc(self, k):
        """
        From step k requires: IO POP
        """
        self.iopc[k] = self.io[k] / self.pop[k]

    @requires(["fioacv", "fioacc", "fioac"], ["iopc"])
    def _update_fioac(self, k):
        """
        From step k requires: IOPC
        """
        self.fioacv[k] = self.fioacv_f(self.iopc[k] / self.iopcd)
        self.fioacc[k] = clip(self.fioac2, self.fioac1, self.time[k],
                              self.pyear)
        self.fioac[k] = clip(self.fioacv[k], self.fioacc[k], self.time[k],
                             self.iet)
    */

    // service subsector         
    private void UpdateSc(int k, int j, int jk) => throw new NotImplementedException();
    private void UpdateIsopc(int k) => throw new NotImplementedException();
    private void UpdateAlsc(int k) => throw new NotImplementedException();
    private void UpdateScdr(int k, int kl) => throw new NotImplementedException();
    private void UpdateScor(int k) => throw new NotImplementedException();
    private void UpdateSo(int k) => throw new NotImplementedException();
    private void UpdateSopc(int k) => throw new NotImplementedException();
    private void UpdateFioas(int k) => throw new NotImplementedException();
    private void UpdateScir(int k, int kl) => throw new NotImplementedException();
    /*
    @requires(["sc"])
    def _update_state_sc(self, k, j, jk):
        """
        State variable, requires previous step only
        """
        if k == 0:
            self.sc[k] = self.sci
        else:
            self.sc[k] = self.sc[j] + self.dt * (self.scir[jk] - self.scdr[jk])

    @requires(["isopc1", "isopc2", "isopc"], ["iopc"])
    def _update_isopc(self, k):
        """
        From step k requires: IOPC
        """
        self.isopc1[k] = self.isopc1_f(self.iopc[k])
        self.isopc2[k] = self.isopc2_f(self.iopc[k])
        self.isopc[k] = clip(self.isopc2[k], self.isopc1[k], self.time[k],
                             self.pyear)

    @requires(["alsc"])
    def _update_alsc(self, k):
        """
        From step k requires: nothing
        """
        self.alsc[k] = clip(self.alsc2, self.alsc1, self.time[k], self.pyear)

    @requires(["scdr"], ["sc", "alsc"])
    def _update_scdr(self, k, kl):
        """
        From step k requires: SC ALSC
        """
        self.scdr[kl] = self.sc[k] / self.alsc[k]

    @requires(["scor"])
    def _update_scor(self, k):
        """
        From step k requires: nothing
        """
        self.scor[k] = clip(self.scor2, self.scor1, self.time[k], self.pyear)

    @requires(["so"], ["sc", "cuf", "scor"])
    def _update_so(self, k):
        """
        From step k requires: SC CUF SCOR
        """
        self.so[k] = self.sc[k] * self.cuf[k] / self.scor[k]

    @requires(["sopc"], ["so", "pop"])
    def _update_sopc(self, k):
        """
        From step k requires: SO POP
        """
        self.sopc[k] = self.so[k] / self.pop[k]

    @requires(["fioas1", "fioas2", "fioas"], ["sopc", "isopc"])
    def _update_fioas(self, k):
        """
        From step k requires: SOPC ISOPC
        """
        self.fioas1[k] = self.fioas1_f(self.sopc[k] / self.isopc[k])
        self.fioas2[k] = self.fioas2_f(self.sopc[k] / self.isopc[k])
        self.fioas[k] = clip(self.fioas2[k], self.fioas1[k], self.time[k],
                             self.pyear)

    @requires(["scir"], ["io", "fioas"])
    def _update_scir(self, k, kl):
        """
        From step k requires: IO FIOAS
        """
        self.scir[kl] = self.io[k] * self.fioas[k]

    */

    // back to industrial sector 
    private void UpdateFioai(int k) => throw new NotImplementedException();
    private void UpdateIcir(int k, int kl) => throw new NotImplementedException();
    /*
    @requires(["fioai"], ["fioaa", "fioas", "fioac"])
    def _update_fioai(self, k):
        """
        From step k requires: FIOAA FIOAS FIOAC
        """
        self.fioai[k] = (1 - self.fioaa[k] - self.fioas[k] - self.fioac[k])

    @requires(["icir"], ["io", "fioai"])
    def _update_icir(self, k, kl):
        """
        From step k requires: IO FIOAI
        """
        self.icir[kl] = self.io[k] * self.fioai[k]

    */

    // back to job subsector             
    private void UpdateJpicu(int k) => throw new NotImplementedException();
    private void UpdatePjis(int k) => throw new NotImplementedException();
    private void UpdateJpscu(int k) => throw new NotImplementedException();
    private void UpdatePjss(int k) => throw new NotImplementedException();
    private void UpdateJph(int k) => throw new NotImplementedException();
    private void UpdatePjas(int k) => throw new NotImplementedException();
    private void UpdateJ(int k) => throw new NotImplementedException();
    private void UpdateLf(int k) => throw new NotImplementedException();
    private void UpdateLuf(int k) => throw new NotImplementedException();
    /*
    @requires(["jpicu"], ["iopc"])
    def _update_jpicu(self, k):
        """
        From step k requires: IOPC
        """
        self.jpicu[k] = self.jpicu_f(self.iopc[k])

    @requires(["pjis"], ["ic", "jpicu"])
    def _update_pjis(self, k):
        """
        From step k requires: IC JPICU
        """
        self.pjis[k] = self.ic[k] * self.jpicu[k]

    @requires(["jpscu"], ["sopc"])
    def _update_jpscu(self, k):
        """
        From step k requires: SOPC
        """
        self.jpscu[k] = self.jpscu_f(self.sopc[k])

    @requires(["pjss"], ["sc", "jpscu"])
    def _update_pjss(self, k):
        """
        From step k requires: SC JPSCU
        """
        self.pjss[k] = self.sc[k] * self.jpscu[k]

    @requires(["jph"], ["aiph"])
    def _update_jph(self, k):
        """
        From step k requires: AIPH
        """
        self.jph[k] = self.jph_f(self.aiph[k])

    @requires(["pjas"], ["jph", "al"])
    def _update_pjas(self, k):
        """
        From step k requires: JPH AL
        """
        self.pjas[k] = self.jph[k] * self.al[k]

    @requires(["j"], ["pjis", "pjas", "pjss"])
    def _update_j(self, k):
        """
        From step k requires: PJIS PJAS PJSS
        """
        self.j[k] = self.pjis[k] + self.pjas[k] + self.pjss[k]

    @requires(["lf"], ["p2", "p3"])
    def _update_lf(self, k):
        """
        From step k requires: P2 P3
        """
        self.lf[k] = (self.p2[k] + self.p3[k]) * self.lfpf

    @requires(["luf"], ["j", "lf"])
    def _update_luf(self, k):
        """
        From step k requires: J LF
        """
        self.luf[k] = self.j[k] / self.lf[k]
    */
}
