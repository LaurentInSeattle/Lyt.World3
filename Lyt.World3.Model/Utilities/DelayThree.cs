namespace Lyt.World3.Model.Utilities;

public class DelayThree
{
}

/*
class Delay3:
    """
    Delay function of the 3rd order. Returns a class that is callable as a
    function (see Call parameters) at a given step k.

    Computes the delayed vector out_arr from the input in_arr, at the step k.

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
        self.out_arr = np.zeros((t.size, 3))
        self.in_arr = in_arr  # use in_arr as reference
        self.method = method
        if self.method == "euler":
            self.A_norm = np.array([[-1., 0., 0.],
                                    [1., -1., 0.],
                                    [0., 1., -1.]])
            self.B_norm = np.array([1, 0, 0])

    def _init_out_arr(self, delay):
        self.out_arr[0, :] = self.in_arr[0] * 3 / delay

    def __call__(self, k, delay):
        if k == 0:
            self._init_out_arr(delay)
        else:
            if self.method == "odeint":
                res = odeint(func_delay3, self.out_arr[k-1, :],
                             [0, self.dt], args=(self.in_arr[k-1], delay))
                self.out_arr[k, :] = res[1, :]
            elif self.method == "euler":
                dout = (self.A_norm  @ self.out_arr[k-1, :] +
                        self.B_norm * self.in_arr[k-1])
                dout *= self.dt*3/delay
                self.out_arr[k, :] = self.out_arr[k-1, :] + dout

        return self.out_arr[k, 2]

*/

