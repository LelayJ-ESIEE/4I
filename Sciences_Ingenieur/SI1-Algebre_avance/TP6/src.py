import numpy as np
from matplotlib import pyplot as plt
from numpy.core.function_base import linspace

##########
# Script #
##########

# Hermite's base
def phi0(t) :
    if (t >= 0 and t <= 1) :
        return (t-1)**2*(2*t+1)
    return 0

def phi1(t) :
    if (t >= 0 and t <= 1) :
        return t**2*(-2*t+3)
    return 0

def phi2(t) :
    if (t >= 0 and t <= 1) :
        return (t-1)**2*t
    return 0

def phi3(t) :
    if (t >= 0 and t <= 1) :
        return (t-1)*t**2
    return 0

# Compute matrix
def matrice(n):
    M = np.zeros((n,n))
    for i in range(n-1):
        M[i][i] = 4
        M[i+1][i] = 1
        M[i][i+1] = 1
    M[0][0] = 2
    M[-1][-1] = 2
    return M

# Compute Z vector where Z = 3/d * {y1-y0, y2-y0, ... yk+1 - yk-1... yn - yn-1}
def VectZ(Y, d):
    n = len(Y)
    v = [Y[1]-Y[0]]
    for i in range(1, len(Y)-1):
        v.append(Y[i+1]-Y[i-1])
    v.append(Y[n-1]-Y[n-2])
    Z = [3/d * t for t in v]
    return Z

# Compute Hermite's polynomes to get P(x)
def foncHermite(X, Y, V, x):
    S = 0
    for i in range(len(X)-1):
        di = X[i+1] - X[i]
        t = (x - X[i]) / di
        S += (Y[i] * phi0(t) + Y[i+1] * phi1(t) + di * (V[i] * phi2(t) + V[i+1] * phi3(t)))
    return S

###########
# Results #
###########

def main_func(a, b, n, f):
    # Generate n points from f(x) for x in [a;b]
    X = np.linspace(a,b,n)
    Y = f(X)

    d = (b-a)/n
    Z = VectZ(Y, n)

    M = matrice(n)

    Yp = np.linalg.solve(M, Z)
    x_aff = np.linspace(a, b, 1000)
    y_est = []
    for i in range(len(x_aff)):
        y_est.append(foncHermite(X, Y, Yp, x_aff[i]))

    plt.plot(x_aff, y_est)
    plt.show()

def main_param(X,Y):
    n = len(X)
    T = range(1,n+1)

    d = 1

    Zx = VectZ(X, d)
    Zy = VectZ(Y, d)

    M = matrice(n)

    Ypx = np.linalg.solve(M, Zx)
    Ypy = np.linalg.solve(M, Zy)

    Taff = linspace(1, n-0.001, 1000)
    Xaff = [foncHermite(T, X, Ypx, x) for x in Taff]
    Yaff = [foncHermite(T, Y, Ypy, x) for x in Taff]

    plt.plot(Xaff,Yaff)
    plt.show()

if __name__ == "__main__":
    ##########
    #Entries #
    ##########
    
    # For main_func
    n = ...
    a = ...
    b = ...
    f = ...
    
    # (example for sin)
    # n = 500
    # a = 0
    # b = 2*np.pi
    # f = np.sin

    # For main_param
    X = ...
    Y = ...

    # Example : Fusée
    # X = [9,0,-3,-2,-3,0,9]
    # Y = [0,0,-1,0,1,0,0]

    # Example : Poisson
    # X = [7,0,-8,-8,0,7]
    # Y = [0,4,-3,3,-4,0]

    main_func(a, b, n, f)
    main_param(X, Y)