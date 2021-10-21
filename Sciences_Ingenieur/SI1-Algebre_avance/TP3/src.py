import math
import matplotlib.pyplot as plt
import numpy as np

##########
#Entries #
##########

# Choose a defined function
f = lambda x : 1/(1+8*x**2)

# Parameters to generate n points from f(x) for x in [a;b]
a = -5
b = 5
n = 71

##########
# Script #
##########

# Generate n points from f(x) for x in [a;b]
X = np.linspace(a,b,n)
Y = f(X)

# plt.plot(x,y)

# Compute Newton k base
def Newton(X, k, x):
    N = 1
    for i in range(k):
        N *= (x-X[i])
    return N

# Evaluate divided diff
def diff_divise(X, Y):
    yc = np.copy(Y)
    for ref in range(n-1):
        for k in range(ref+1, n):
            yc[k] = (yc[k]-yc[ref])/(X[k]-X[ref])
    return yc


# Evaluate Newton polynome
def polNewton(X, Y, x):
    yc = diff_divise(X,Y)
    s=0
    for k in range(n):
        s += yc[k]*Newton(X, k, x)
    return s


# Generate values
xaff = np.linspace(a,b,501)
yex = f(xaff)
yestim = polNewton(X, Y, xaff)

# Plot graphes
plt.ylim(-1.1, 1.1)
plt.plot(xaff,yex)
plt.plot(xaff,yestim)

plt.show()
