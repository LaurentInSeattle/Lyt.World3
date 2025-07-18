﻿namespace Lyt.Simulation.Equations; 

public class Auxiliary : Equation
{
    public Auxiliary(Simulator model, string name, int number, string units = "dimensionless") 
        : base(model, name, number, units) 
        =>  model.OnNewAuxiliary(this);

    public int EvaluationOrder { get; set; }
}
