import math
import numpy as np
from matplotlib import pyplot as plt
from matplotlib import collections as mc

##########
# Script #
##########

# Compute C(n,k) (k among n)
def binom(n, k):
    return ( math.factorial(n) / ( math.factorial(n-k) * math.factorial(k) ) )

# Evaluate berntein coefficient
def bernstein(n, i, t):
    return ( binom(n, i) * t**i * (1-t)**(n-i) )


# Evaluate Bezier curve points positions
def bezier(L, N):
    points = []
    for k in range(N):
        S_x = 0
        S_y = 0

        for i in range(n):
            S_x += bernstein(n-1, i, T[k]) * L[i][0]
            S_y += bernstein(n-1, i, T[k]) * L[i][1]
        points.append((S_x, S_y))
    return points

# Plot result
def simple_draw():
    p = bezier(L, N)

    fig, ax = plt.subplots()
    if debug:
        l_lb = [[L[i], L[i+1]] for i in range(n - 1)]
        lb = mc.LineCollection(l_lb, colors="black", linewidths = 1)
        ax.add_collection(lb)

    l_cb = [[p[i], p[i+1]] for i in range(N - 1)]
    cb = mc.LineCollection(l_cb, colors="red", linewidths = 2)
    ax.add_collection(cb)

    ax.autoscale()
    ax.axis('equal')

    plt.show()

# Plot result (multiple curves)
def complex_draw():
    global n
    fig, ax = plt.subplots()
    for l in L:
        n = len(l)
        p = bezier(l, N)
        sn = len(l)
        if debug:
            l_lb = [[l[i], l[i+1]] for i in range(sn - 1)]
            lb = mc.LineCollection(l_lb, colors="black", linewidths = 1)
            ax.add_collection(lb)

        l_cb = [[p[i], p[i+1]] for i in range(N - 1)]
        
        cb = mc.LineCollection(l_cb, colors="red", linewidths = 2)
        ax.add_collection(cb)

    ax.autoscale()
    ax.axis('equal')

    plt.show()

#########
#Entries#
#########

L = []
delta = 4.0/3.0 * math.tan(math.pi / 8) # For circle

# list of points (simple_draw)
# L = [(0,0), (1,1), (2,0)] # Conic
# L = [(0,0), (1,10), (3,-10), (4,0)] # Sin-like
# L = [(1,1), (0.5,0), (0,0), (0,1), (0.5,1), (1,0)] # Alpha
# L = [(-0.5,1), (1,0.5), (0,0), (-1,0.5), (0.5,1)] # Gamma
# L = [(0,2), (1,3), (2,2), (0,0)] # Half heart
# L = [(0, 2), (1, 2), (1, 0), (1, 0), (0, 0), (-1, 0), (-1, 0), (-1, 2), (0, 2)] # Egg
# L = [(0,0), (-0.3, 0), (-0.3, 0.3)] # Quarter of circle
# L = [(0,0), (-0.5, 0), (-0.5, 0.8), (0,0.8), (0.5, 0.8), (0.5, 0), (0,0)] # Approximated circle
# L = [(0, -1), (-delta, -1), (-1, -delta), (-1, 0), (-1, delta), (-delta, 1), (0, 1), (delta, 1), (1, delta), (1, 0), (1, -delta), (delta, -1), (0, -1)] # Another approximated circle (needs delta)

# list of lists of points (complex_draw)
# L = [[(0,0), (1,1), (2,0)]] # Conic
# L = [[(0,2), (1,3), (2,2), (0,0)], [(0,0), (-2,2), (-1,3), (0,2)]] # Heart
# L = [ [(0,0), (0, 0.3), (-0.3, 0.3)], [(-0.3, 0.3), (0, 0.3), (0, 0.6)], [(0,0.6), (0, 0.3), (0.3, 0.3)], [(0.3,0.3), (0, 0.3), (0, 0)] ] # Hypocycloid
# L = [ [(0,0), (-0.3, 0), (-0.3, 0.3)], [(-0.3, 0.3), (-0.3, 0.6), (0, 0.6)], [(0,0.6), (0.3, 0.6), (0.3, 0.3)], [(0.3,0.3), (0.3, 0), (0, 0)] ] # Approximated circle, 4 quarters
# L = [ [(0, -1), (-delta, -1), (-1, -delta), (-1, 0)], [(-1, 0), (-1, delta), (-delta, 1), (0, 1)], [(0, 1), (delta, 1), (1, delta), (1, 0)], [(1, 0), (1, -delta), (delta, -1), (0, -1)] ] # Circle (needs delta)

n = len(L)
N = 500
T = np.linspace(0, 1, N, endpoint=True)
debug = True # set to True to see control points

###########
#Execution#
###########

if __name__ == "__main__":
    if type(L[0]) == tuple:
        simple_draw()
    else:
        complex_draw()