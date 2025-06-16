namespace Lyt.World3.Model.Utilities;

public sealed class DelayInformationThree : DelayThree 
{
}

/*
class Dlinf3(Delay3):
    """
    Delay information function of the 3rd order for smoothing. Returns a class
    that is callable as a function (see Call parameters) at a given step k.

    Computes the smoothed vector out_arr from the input in_arr, at the step k.

    Parameters
    ----------
    in_arr : numpy ndarray
        input vector of the delay function.
    dt : float
        time step.
    t : numpy ndarray
        time vector.
    method : str, optional
        "euler" or "odeint". The default is "euler".

    Call parameters
    ---------------
    k : int
        current loop index.
    delay : float
        delay parameter. Higher delay increases smoothing.

    Call Returns
    ------------
    out_arr[k]

    """

    def _init_out_arr(self, delay):
        self.out_arr[0, :] = self.in_arr[0]
*/