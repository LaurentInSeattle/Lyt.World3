﻿namespace Lyt.Simulation.World3;

//  Limits to Growth: This is a re-implementation in C# of World3, the social-economic-environmental model created by
//  Dennis and Donella Meadows and others circa 1970. The results of the modeling exercise were published in The Limits to Growth
//  in 1972, and the model itself was more fully documented in Dynamics of Growth in a Finite World in 1974. 

#region MIT License and more

/* Original Work by Brian Hayes - under MIT Licence - Can be found in the JavaScript project folder
  
 
    MIT License

    Copyright (c) 2016 Brian Hayes

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

/*
    Original Comments by Brian Hayes, circa 2012 

    The Limits to Growth_ by Meadows et al. (1972) presented a system dynamics model of the global ecosystem and economy, called World3.
    The original simulation was written in a language called DYNAMO. 
    The code in this repository, written in 2012, attempts to translate the DYNAMO World3 program into JavaScript.
    You can run the program in a web browser at [http://bit-player.org/extras/limits/ltg.html](http://bit-player.org/extras/limits/ltg.html).

    For background on the project see:

    * "Computation and the Human Predicament: _The Limits to Growth_ and the limits to computer modeling," by Brian Hayes, _American Scientist_ Volume 100, Number 3, May-June 2012, pp. 186–191. 
    * Available online in [HTML]
    * (http://www.americanscientist.org/issues/pub/computation-and-the-human-predicament) 
    * and [PDF]
    * (http://www.americanscientist.org/libraries/documents/2012491358139046-2012-05Hayes.pdf).
    * [World3, the public beta](http://bit-player.org/2012/world3-the-public-beta) (article posted on bit-player.org).    
 */

/* Derivative Work, this code, by Laurent Yves Testud - under MIT Licence 


   MIT License

   Copyright (c) 2021 Laurent Yves Testud 

   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.
*/

/* Laurent's comments, circa 2021
    This project tries to bring to a larger population of software 'people' the scope of this work done now more than 50 years ago.
    Sadly: It is spot on.

 */
#endregion MIT License 

public sealed partial class WorldModel : Simulator
{
#pragma warning disable 8618 
    // Non-nullable field '...' must contain a non-null value when exiting constructor.
    // Consider adding the 'required' modifier or declaring the field as nullable.
    // For all equations defined in the partial class.
    public WorldModel() : base()
#pragma warning restore 8618 
    {
        this.Parameters = new Parameters(parameters);
        this.Parameters.ToDefaults();
        this.CreatePopulationSector();
        this.CreateCapitalSector();
        this.CreateAgriculturalSector();
        this.CreatePollutionSector();
        this.CreateResourceSector();
        this.CreateOtherSectors();
        this.AdjustForPersistentPollutionAppearanceRate();
        base.FinalizeConstruction(this.auxSequence, this.CustomUpdate);
    }

    public override string TimeUnit => "Year";

    public override double InitialTime() => WorldModel.StartYear;

    public override bool SimulationEnded()
    {
        var durationYears = this.Parameters.FromName("Simulation Duration");
        return (this.Time > this.InitialTime() + (int)durationYears.CurrentValue);
    }

    private void CustomUpdate() => this.persistentPollutionAppearanceRate.Update();

    private void AdjustForPersistentPollutionAppearanceRate() =>
        this.Auxiliaries.Remove("persistentPollutionAppearanceRate");
}
