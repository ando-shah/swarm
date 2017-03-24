# Swarm Tutorial (built on top of SpatialOS)
---



- *GitHub Repository*: [https://github.com/ananda13/swarm](https://github.com/ananda13/swarm)

---

## Introduction

This is a tutorial on how to setup a swarm simulation using [spatialOS](https://spatialos.improbable.io/), which is created by Improbable.

Disclaimer: I am not a spatialOS expert, but a simulation enthusiast, and the intent of the tutorial is to understand how to build large scale simulations with SpatialOS, with the bulk of the computation (physics) running on the cloud. The additional advantage is that you can have multiple users log into it, to view or interact with the simulation

In this tutorial, we will set up a VR enabled (HTC Vive) simulation of a swarm.


## Contents

### Part I   : Basics & Boids
### Part II  : Plug Into Improbable
### PART III : Get Into VR


## PART I

#### Getting Started

Skills required: 

a. Basic Unity/C#

b. SpatialOS

You will need to familiarize yourself with the basics of SpatialOS, which is a key component of this tutorial.
They have excellent starter tutorials [here](https://spatialos.improbable.io/docs/reference/10.1/tutorials/showcase)

If you have a basic grasp of Unity, and know nothing about SpatialOS, I would suggest jumping in and trying it. I'm not a Unity expert, and it took me a few days to build this; and I'm sure you can too!


SWARMS

So what is a swarm, and why simulate it?

Swarms are really interesting because at a micro level, they follows extremely simple rules, but at a macro level, the effects look very sophisticated and frankly, awe-inspiring!

Just look at these guys:

![Starlings Murmurations](/images/starling-murmurations.gif)

_Above : A 'murmuration' of starlings . A poet probably came up with that name. 
Murmurations are a more complex subset of flocking or swarming behavior._


