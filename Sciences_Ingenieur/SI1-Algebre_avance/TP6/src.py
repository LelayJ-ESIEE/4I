import numpy as np
from matplotlib import pyplot as plt
from matplotlib import collections as mc

##########
#Entries #
##########

X = [-5, -2, 0, 3, 6]
Y = [-4, -1, 1, 1, -1]
V = [3, 0, 3, -2, 0]
n = 500

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
if __name__ == "__main__":
    x = np.linspace(X[0], X[len(X)-1], n)
    y = []
    for i in range(n):
        y.append(foncHermite(X, Y, V, x[i]))

    fig, ax = plt.subplots()
    ax.plot(x,y)

    l_t = [[(X[i]-1, Y[i]-V[i]), (X[i]+1, Y[i]+V[i])] for i in range(len(X))]
    t = mc.LineCollection(l_t, colors="black", linewidths = 0.5)
    ax.add_collection(t)

    ax.autoscale()
    ax.axis('equal')
    plt.show()