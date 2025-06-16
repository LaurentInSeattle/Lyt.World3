namespace Lyt.World3.Model.Utilities;

public sealed class Smooth
{
}

/*
class Smooth:
    """
    Delay information function of the 1st order for smoothing. Also named
    DLINF1 in Dynamo. Returns a class that is callable as a function (see
    Call parameters) at a given step k.

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

    def __init__(self, in_arr, dt, t, method="euler"):
        self.dt = dt
        self.out_arr = np.zeros((t.size,))
        self.in_arr = in_arr  # use in_arr by reference
        self.method = method

    def __call__(self, k, delay):
        if k == 0:
            self.out_arr[k] = self.in_arr[k]
        else:
            if self.method == "odeint":
                res = odeint(func_delay1, self.out_arr[k-1],
                             [0, self.dt], args=(self.in_arr[k-1], delay))
                self.out_arr[k] = res[1, :]
            elif self.method == "euler":
                dout = self.in_arr[k-1] - self.out_arr[k-1]
                dout *= self.dt/delay
                self.out_arr[k] = self.out_arr[k-1] + dout

        return self.out_arr[k]


DlInf1 = Smooth


def func_delay3(out_, t_, in_, del_):
    """
    Computes the derivative of out_ at time t_, for the 3rd order delay. Used
    in integration by odeint.

    """
    dout_ = np.zeros((3,))
    dout_[0] = in_ - out_[0]
    dout_[1] = out_[0] - out_[1]
    dout_[2] = out_[1] - out_[2]

    return dout_ * 3 / del_

*/