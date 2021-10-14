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
n = 499

##########
# Script #
##########

# Generate n points from f(x) for x in [a;b]
X = np.linspace(a,b,n)
Y = f(X)

# plt.plot(x,y)

# Compute Lagrange k base
def Lagrange(X, x, k):
    res = 1
    for i in range(n):
        if i != k:
            res *= (x-X[i])/(X[k]-X[i])
    return res


# Evaluate Lagrange polynome
def polLagrange(X, Y, x):
    s=0
    for k in range(n):
        s += Y[k]*Lagrange(X, x, k)
    return s


# Generate values
xaff = np.linspace(a,b,501)
yex = f(xaff)
yestim = polLagrange(X, Y, xaff)

# Plot graphes
plt.ylim(-0.1, 1.1)
plt.plot(xaff,yex)
plt.plot(xaff,yestim)

plt.show()
