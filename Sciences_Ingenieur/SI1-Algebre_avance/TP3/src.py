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
n = 11

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
    l = [ [x] for x in X]

    for i in range(n):
        l[i].append(round(yc[i],4))

    for ref in range(n-1):
        l[ref] += ["-"]*(n-ref-1)
        for k in range(ref+1, n):
            yc[k] = (yc[k]-yc[ref])/(X[k]-X[ref])
            l[k].append(round(yc[k],4))
    l = [["xk","yk"] + [" "]*n] + [[":-:"]*(n+2)] + l
    s=""
    for i in range(n+2):
        s +="|"
        for j in range(n+1):
            s+= str(l[i][j]) + "|"
        s += "\n"
    print(s)
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
