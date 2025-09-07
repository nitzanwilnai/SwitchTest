# SwitchTest

A Unity project to compare switch and if/else.

## Results:

In the Unity Editor, if/else is significantly faster at 30 states. switch starts winning at around 50 states.

In this new updated version, when compiled using IL2CPP to C++ and run with max optimization, switch is ~4% faster than if/else for up to 50 states. 

At 100 states, they are virtually the same.
