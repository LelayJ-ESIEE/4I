import math
import matplotlib.pyplot as plt
import numpy as np

##########
#Entries #
##########

# Choose a defined function
f = np.sin

# Parameters to generate n points from f(x) for x in [a;b]
a = 0
b = 2*np.pi
n = 10

##########
# Script #
##########

# Generate n points from f(x) for x in [a;b]
x = np.linspace(a,b,n)
y = f(x)

# plt.plot(x,y)

# Generate Vandermonde matrix
v = np.vander(x, increasing=True)

# Generate coefficients matrix solving (S) = V.A=Y
a = np.linalg.solve(v,y)

# Evaluate polynome
def pol(x):
    s=0
    for k in range(n):
        s += a[k]*x**k
    return s


# Generate values
xaff = np.linspace(a,b,501)
yex = f(xaff)
yestim = pol(xaff)

# Print max error
errors = yestim - yex
error_max = np.max(errors)
print("Max error : " + str(error_max))

# Plot graphes
plt.plot(xaff,yex)
plt.plot(xaff,yestim)

plt.show()
